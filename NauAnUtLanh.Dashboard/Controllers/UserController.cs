using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using NauAnUtLanh.Dashboard.Models;
using NauAnUtLanh.Database;
using PagedList;

namespace NauAnUtLanh.Dashboard.Controllers
{
    public class UserController : BaseController
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
        public async Task<ActionResult> Create(UserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (await CheckEmailExisted(model.Email))
            {
                ModelState.AddModelError("", "Email đã tồn tại");
                return View(model);
            }
            if (string.IsNullOrEmpty(model.Password)) model.Password = "123456";
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = model.Email,
                Password = EncryptDecrypt.GetMd5(model.Password),
                FullName = model.FullName,
                Gender = model.Gender,
                BirthDate = model.BirthDate.ToString(),
                Address = model.Address,
                Phone = model.Address,
                CreatedTime = DateTime.Now,
                Activated = true
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return RedirectToAction("index");
        }

        public async Task<bool> CheckEmailExisted(string email)
        {
            return await _db.Users.AnyAsync(x => x.Email == email);
        }

        [HttpPost]
        public async Task ChangeActivation(Guid id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return;
            user.Activated = !user.Activated;
            _db.Entry(user).State = EntityState.Modified;
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