using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using NauAnUtLanh.Dashboard.Models;
using NauAnUtLanh.Database;

namespace NauAnUtLanh.Dashboard.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();

        public async Task<ActionResult> Index()
        {
            var categories = await _db.Categories.OrderByDescending(x => x.CreatedTime).ToListAsync();
            return View(categories);
        }

        public List<SelectListItem> GetList()
        {
            var categoryList = new List<SelectListItem> {new SelectListItem
            {
                Text = "---Chọn chủng loại món ăn---",
                Value = "0"
            } };
            var categories = _db.Categories.Where(x => x.Activated).ToList();
            categoryList.AddRange(categories.Select(category => new SelectListItem
            {
                Text = category.CategoryName, Value = category.Id.ToString()
            }));
            return categoryList;
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var existed = await _db.Categories.AnyAsync(x => x.CategoryName == model.CategoryName);
            if (existed)
            {
                ModelState.AddModelError("", "Chủng loại món ăn đã tồn tại");
                return View(model);
            }
            var category = new Category
            {
                CategoryName = model.CategoryName,
                Activated = model.Activated,
                CreatedTime = DateTime.Now
            };
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return RedirectToAction("index");
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var category = await _db.Categories.FindAsync(id);
            if (category == null) return HttpNotFound();
            var model = new CategoryViewModel
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Activated = category.Activated
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CategoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var category = await _db.Categories.FindAsync(model.Id);
            if (category == null) return HttpNotFound();
            category.CategoryName = model.CategoryName;
            category.Activated = model.Activated;
            _db.Entry(category).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction("index");
        }

        public async Task<int> Add(string name)
        {
            if (string.IsNullOrEmpty(name)) return -1;
            var existed = await _db.Categories.AnyAsync(x => x.CategoryName == name);
            if (existed) return -1;
            var category = new Category
            {
                CategoryName = name,
                Activated = true,
                CreatedTime = DateTime.Now
            };
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return category.Id;
        }

        [HttpPost]
        public async Task<bool> ChangeStatus(int? id)
        {
            if (id == null) return false;
            var category = await _db.Categories.FindAsync(id);
            if (category == null) return false;
            category.Activated = !category.Activated;
            _db.Entry(category).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return true;
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