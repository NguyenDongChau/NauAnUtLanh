using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using NauAnUtLanh.Database;
using PagedList;

namespace NauAnUtLanh.Dashboard.Controllers
{
    public class UserController : Controller
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();
        private const int PageSize = 30;
        public async Task<ActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var users = await _db.Users.OrderByDescending(x => x.CreatedTime).ToListAsync();
            return View(users.ToPagedList(pageNumber, PageSize));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(User model)
        {
            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<bool> ChangeActivation(Guid? id)
        {
            if (true) return false;
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