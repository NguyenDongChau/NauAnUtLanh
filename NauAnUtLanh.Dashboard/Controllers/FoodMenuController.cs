using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        private List<string> _foodIdList;

        public async Task<ActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var menus = await _db.FoodMenus.OrderByDescending(x => x.CreateTime).ToListAsync();
            return View(menus.ToPagedList(pageNumber, PageSize));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(FoodMenuViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
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
                avatar.SaveAs(Server.MapPath($"{Server.MapPath(Path)}/{newFileName}"));
                menu.Avatar = newFileName;
            }
            _db.FoodMenus.Add(menu);
            await _db.SaveChangesAsync();
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
                FoodIdList = menu.FoodIdList,
                Avatar = menu.Avatar
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(FoodMenuViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var menu = await _db.FoodMenus.FindAsync(model.Id);
            if (menu == null) return HttpNotFound();
            menu.MenuName = model.MenuName;
            menu.Price = model.Price;
            menu.Activated = model.Activated;
            menu.FoodIdList = model.FoodIdList;
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
            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<bool> RemoveItem(Guid? menuid, string foodid)
        {
            if (menuid == null || foodid == null) return false;
            var menu = await _db.FoodMenus.FirstOrDefaultAsync(x=>x.Id == menuid.Value);
            if (menu == null) return false;
            var foodIds = menu.FoodIdList.Split(';').ToList();
            foodIds.Remove(foodid);
            menu.FoodIdList = String.Join(";", foodIds);
            _db.Entry(menu).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            return true;
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
        public string AddOrRemoveFood(string id)
        {
            if (_foodIdList.Contains(id))
            {
                _foodIdList.Remove(id);
            }
            else
            {
                _foodIdList.Add(id);
            }
            return string.Join(";", _foodIdList.ToArray());
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