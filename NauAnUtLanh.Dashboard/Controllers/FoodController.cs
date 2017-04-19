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
    public class FoodController : BaseController
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();
        private const int PageSize = 30;
        private const string Path = "~/upload/food";
        private readonly CategoryController _cc = new CategoryController();

        public async Task<ActionResult> Index(int? page, int? categoryid)
        {
            var pageNumber = page ?? 1;
            var catId = categoryid ?? -1;
            var foods = await _db.Foods.OrderByDescending(x => x.CreatedTime).ToListAsync();
            if(catId > 0)
                foods = await _db.Foods.OrderByDescending(x => x.CreatedTime).Where(x=>x.CategoryId == catId).ToListAsync();
            return View(foods.ToPagedList(pageNumber, PageSize));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(FoodViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (model.CategoryId <= 0)
            {
                if (string.IsNullOrEmpty(model.CategoryName))
                {
                    ModelState.AddModelError("", "Chưa chọn chủng loại món ăn hoặc cung cấp tên chủng loại món ăn mới");
                    return View(model);
                }
                model.CategoryId = await _cc.Add(model.CategoryName);
            }
            if (!Directory.Exists(Path)) Directory.CreateDirectory(Server.MapPath(Path));
            var id = Guid.NewGuid();
            var avatar = Request.Files["Avatar"];
            if (avatar != null && avatar.ContentLength > 0)
            {
                if (avatar.ContentLength > 2048000)
                {
                    ModelState.AddModelError("", "Hình ảnh không quá 2Mb");
                    return View(model);
                }
                var file = new FileInfo(avatar.FileName);
                var fileExt = file.Extension;
                var newFileName = $"{id}{fileExt}";
                avatar.SaveAs($"{Server.MapPath(Path)}/{newFileName}");
                model.Avatar = newFileName;
            }
            var food = new Food
            {
                Id = id,
                CategoryId = model.CategoryId,
                FoodName = model.FoodName,
                FoodType = model.FoodType,
                Activated = model.Activated,
                Avatar = model.Avatar,
                CreatedTime = DateTime.Now
            };
            _db.Foods.Add(food);
            await _db.SaveChangesAsync();
            return RedirectToAction("index");
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var food = await _db.Foods.FindAsync(id);
            if (food == null) return HttpNotFound();
            var model = new FoodViewModel
            {
                Id = food.Id,
                CategoryId = food.CategoryId,
                FoodName = food.FoodName,
                FoodType = food.FoodType,
                Activated = food.Activated
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(FoodViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var food = await _db.Foods.FindAsync(model.Id);
            if (food == null) return HttpNotFound();
            var avatar = Request.Files["Avatar"];
            if (avatar != null && avatar.ContentLength > 0)
            {
                if (avatar.ContentLength > 2048000)
                {
                    ModelState.AddModelError("", "Hình ảnh không quá 2Mb");
                    return View(model);
                }
                var file = new FileInfo(avatar.FileName);
                var fileExt = file.Extension;
                var newFileName = $"{model.Id}{fileExt}";
                avatar.SaveAs($"{Server.MapPath(Path)}/{newFileName}");
                food.Avatar = newFileName;
            }
            food.CategoryId = model.CategoryId;
            food.FoodName = model.FoodName;
            food.FoodType = model.FoodType;
            food.Activated = model.Activated;
            _db.Entry(food).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<bool> ChangeStatus(Guid? id)
        {
            if (id == null) return false;
            var food = await _db.Foods.FindAsync(id);
            if (food == null) return false;
            food.Activated = !food.Activated;
            _db.Entry(food).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<JsonResult> GetFoodsByCategory(int id)
        {
            var foods = await _db.Foods.Where(x => x.Activated && x.CategoryId == id).ToListAsync();
            var list = new List<SelectListItem>();
            foreach (var food in foods)
            {
                list.Add(new SelectListItem { Text = food.FoodName, Value = food.Id.ToString()});
            }
            return Json(list);
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