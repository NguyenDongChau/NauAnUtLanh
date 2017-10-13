using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using NauAnUtLanh.Database;
using NauAnUtLanh.FrontEnd.Models;

namespace NauAnUtLanh.FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();
        
        public async Task<ActionResult> Index()
        {
            var info = await _db.DefaultInfos.FindAsync(Guid.Empty);
            ViewData["defaultinfo"] = info;
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Contact(ContactViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Subject = model.Subject,
                Content = model.Content,
                Read = false,
                CreatedTime = DateTime.Now
            };
            _db.Contacts.Add(contact);
            await _db.SaveChangesAsync();
            ViewData["message"] = "Bạn đã gửi thành công! Chúng tôi sẽ hồi âm lại trong thời gian sớm nhất! Xin cám ơn";
            return View();
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