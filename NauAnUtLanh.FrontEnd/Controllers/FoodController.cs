﻿using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using NauAnUtLanh.Database;
using PagedList;

namespace NauAnUtLanh.FrontEnd.Controllers
{
    public class FoodController : Controller
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();
        private const int PageSize = 30;

        public async Task<ActionResult> Index(int? page, int? type)
        {
            var pageNumber = page ?? 1;
            var catId = type ?? 0;
            var foods = await _db.Foods.OrderByDescending(x => x.CreatedTime).Where(x => x.Activated).ToListAsync();
            if (catId != 0)
            {
                foods = await _db.Foods.OrderByDescending(x => x.CreatedTime).Where(x => x.Activated & x.CategoryId == catId).ToListAsync();
            }
            return View(foods.ToPagedList(pageNumber, PageSize));
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