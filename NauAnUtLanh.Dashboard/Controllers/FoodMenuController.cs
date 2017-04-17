using System;
using System.Data.Entity;
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
                FoodId = model.FoodId,
                Activated = model.Activated,
                CreateTime = DateTime.Now
            };
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
                MenuName = menu.MenuName,
                Price = menu.Price,
                FoodId = menu.FoodId,
                Activated = menu.Activated
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
            menu.FoodId = model.FoodId;
            menu.Activated = model.Activated;
            _db.Entry(menu).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<bool> RemoveItem(Guid? menuid, Guid? foodid)
        {
            if (menuid == null || foodid == null) return false;
            var menu = await _db.FoodMenus.FirstOrDefaultAsync(x=>x.Id == menuid.Value && x.FoodId == foodid.Value);
            if (menu == null) return false;
            _db.Entry(menu).State = EntityState.Deleted;
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