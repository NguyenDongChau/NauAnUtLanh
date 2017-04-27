using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using NauAnUtLanh.Dashboard.Models;
using NauAnUtLanh.Database;
using PagedList;

namespace NauAnUtLanh.Dashboard.Controllers
{
    public class FoodMenuController : BaseController
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();
        private const int PageSize = 30;
        private const string Path = "~/upload/foodmenu";
        
        public async Task<ActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var menus = await _db.FoodMenus.OrderByDescending(x => x.CreateTime).ToListAsync();
            return View(menus.ToPagedList(pageNumber, PageSize));
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if(id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var menu = await _db.FoodMenus.FindAsync(id);
            if (menu == null) return HttpNotFound();
            var model = new FoodMenuViewModel
            {
                Id = menu.Id,
                MenuName = menu.MenuName,
                Avatar = menu.Avatar,
                Price = menu.Price,
                FoodIdList = menu.FoodIdList
            };
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(FoodMenuViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var existed = await _db.FoodMenus.AnyAsync(x => x.MenuName == model.MenuName);
            if (existed)
            {
                ModelState.AddModelError("", "Tên thực đơn đã tồn tại hãy cung cấp tên khác");
                return View(model);
            }
            var foodIdList = (List<string>) Session["FoodIdList"];
            if (foodIdList == null || !foodIdList.Any())
            {
                ModelState.AddModelError("", "Chưa thêm món ăn vào thực đơn");
                return View(model);
            }
            model.FoodIdList = string.Join(";", foodIdList.ToArray());
            var menu = new FoodMenu
            {
                Id = Guid.NewGuid(),
                MenuName = model.MenuName,
                Price = model.Price,
                FoodIdList = model.FoodIdList,
                Activated = model.Activated,
                CreateTime = DateTime.Now
            };
            if (!Directory.Exists(Server.MapPath(Path))) Directory.CreateDirectory(Server.MapPath(Path));
            var avatar = Request.Files["Avatar"];
            if (avatar != null && avatar.ContentLength > 0)
            {
                if (avatar.ContentLength > 2048000)
                {
                    ModelState.AddModelError("", "Hình ảnh có dung lượng không quá 2Mb");
                    return View(model);
                }
                var fileInfo = new FileInfo(avatar.FileName);
                var fileExt = fileInfo.Extension;
                var newFileName = $"{menu.Id}{fileExt}";
                avatar.SaveAs($"{Server.MapPath(Path)}/{newFileName}");
                menu.Avatar = newFileName;
            }
            _db.FoodMenus.Add(menu);
            await _db.SaveChangesAsync();
            Session["FoodIdList"] = null;
            return RedirectToAction("index");
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var menu = await _db.FoodMenus.FindAsync(id);
            if (menu == null) return HttpNotFound();
            var model = new FoodMenuViewModel
            {
                Id = menu.Id,
                MenuName = menu.MenuName,
                Price = menu.Price,
                Activated = menu.Activated,
                Avatar = menu.Avatar
            };
            Session["FoodIdList"] = menu.FoodIdList.Split(';').ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(FoodMenuViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var menu = await _db.FoodMenus.FindAsync(model.Id);
            if (menu == null) return HttpNotFound();
            var foodIdList = (List<string>) Session["FoodIdList"];
            if (foodIdList == null || !foodIdList.Any())
            {
                ModelState.AddModelError("", "Chưa thêm món ăn vào thực đơn");
                return View(model);
            }
            menu.MenuName = model.MenuName;
            menu.Price = model.Price;
            menu.Activated = model.Activated;
            menu.FoodIdList = string.Join(";", foodIdList.ToArray());
            var avatar = Request.Files["Avatar"];
            if (avatar != null && avatar.ContentLength > 0)
            {
                if (avatar.ContentLength > 2048000)
                {
                    ModelState.AddModelError("", "Hình ảnh có dung lượng không quá 2Mb");
                    return View(model);
                }
                var fileInfo = new FileInfo(avatar.FileName);
                var fileExt = fileInfo.Extension;
                var newFileName = $"{menu.Id}{fileExt}";
                avatar.SaveAs(Server.MapPath($"{Server.MapPath(Path)}/{newFileName}"));
                menu.Avatar = newFileName;
            }
            _db.Entry(menu).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            Session["FoodIdList"] = null;
            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<bool> ChangeStatus(Guid? id)
        {
            if (id == null) return false;
            var menu = await _db.FoodMenus.FindAsync(id);
            if (menu == null) return false;
            menu.Activated = !menu.Activated;
            _db.Entry(menu).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return true;
        }

        [HttpPost]
        public async Task<bool> ChangeFeature(Guid? id)
        {
            if (id == null) return false;
            var menu = await _db.FoodMenus.FindAsync(id);
            if (menu == null) return false;
            menu.Feature = !menu.Feature;
            _db.Entry(menu).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return true;
        }

        [HttpPost]
        public void AddFood(string id)
        {
            var foodIdList = (List<string>)Session["FoodIdList"];
            if (foodIdList == null || foodIdList.Count <= 0)
            {
                foodIdList = new List<string> { id };
            }
            else
            {
                if (!foodIdList.Contains(id))
                {
                    foodIdList.Add(id);
                }
                else
                {
                    foodIdList.Remove(id);
                }
            }
            Session["FoodIdList"] = foodIdList;
        }

        [HttpPost]
        public async Task Delete(Guid id)
        {
            var menu = await _db.FoodMenus.FindAsync(id);
            if (menu == null) return;
            _db.Entry(menu).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}