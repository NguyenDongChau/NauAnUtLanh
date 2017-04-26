using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using NauAnUtLanh.Database;
using PagedList;

namespace NauAnUtLanh.Dashboard.Controllers
{
    public class ContactController : Controller
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();
        private const int PageSize = 30;

        public async Task<ActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var contacts = await _db.Contacts.OrderByDescending(x => x.CreatedTime).ToListAsync();
            return View(contacts.ToPagedList(pageNumber, PageSize));
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if(id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var contact = await _db.Contacts.FindAsync(id);
            if (contact == null) return HttpNotFound();
            contact.Read = true;
            _db.Entry(contact).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return View(contact);
        }
    }
}