using System;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using NauAnUtLanh.Dashboard.Models;
using NauAnUtLanh.Database;

namespace NauAnUtLanh.Dashboard.Controllers
{
    public class PersonalController : BaseController
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();
        private const string Path = "~/upload/user";

        public async Task<ActionResult> Manage(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = await _db.Users.FindAsync(id);
            if (user == null) return HttpNotFound();
            var model = new PersonalInfoViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                Address = user.Address,
                Phone = user.Phone,
                Avatar = user.Avatar
            };
            return View(model);
        }

        public async Task<ActionResult> Update(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = await _db.Users.FindAsync(id);
            if (user == null) return HttpNotFound();
            var model = new PersonalInfoViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                Address = user.Address,
                Phone = user.Phone,
                Avatar = user.Avatar
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Update(PersonalInfoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _db.Users.FindAsync(model.Id);
            if (user == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var avatar = Request.Files["Avatar"];
            if (avatar != null && avatar.ContentLength > 0)
            {
                if (avatar.ContentLength > 2048000)
                {
                    ModelState.AddModelError("", "Dung lượng hình ảnh không lớn hơn 2Mb");
                    return View(model);
                }
                if (System.IO.File.Exists($"{Path}/{model.Avatar}")) System.IO.File.Delete($"{Path}/{model.Avatar}");
                var folder = Server.MapPath(Path);
                var fileName = new FileInfo(avatar.FileName);
                var fileExt = fileName.Extension;
                var newFileName = $"{model.Id}{fileExt}";
                avatar.SaveAs($"{folder}/{newFileName}");
                user.Avatar = newFileName;
            }
            user.FullName = model.FullName;
            user.Gender = model.Gender;
            user.BirthDate = model.BirthDate;
            user.Address = model.Address;
            user.Phone = model.Phone;
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction("manage");
        }
    }
}