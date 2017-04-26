﻿using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using NauAnUtLanh.Database;
using PagedList;

namespace NauAnUtLanh.FrontEnd.Controllers
{
    public class MenuController : Controller
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();
        private const int PageSize = 10;

        public async Task<ActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var menus = await _db.FoodMenus.OrderByDescending(x => x.CreateTime).Where(x => x.Activated).ToListAsync();
            return View(menus.ToPagedList(pageNumber, PageSize));
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