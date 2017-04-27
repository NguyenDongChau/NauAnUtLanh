using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using NauAnUtLanh.Database;
using PagedList;

namespace NauAnUtLanh.FrontEnd.Controllers
{
    public class NewsController : Controller
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();
        private const int PageSize = 20;

        public async Task<ActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var articles = await _db.Articles.OrderByDescending(x => x.CreatedTime)
                .Where(x => x.Activated)
                .ToListAsync();
            return View(articles.ToPagedList(pageNumber, PageSize));
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var article = await _db.Articles.FindAsync(id);
            if (article == null) return HttpNotFound();
            return View(article);
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