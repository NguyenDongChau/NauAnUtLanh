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
    public class AccountController : Controller
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();
        private const string Path = "~/upload/user/avatar";

        public ActionResult Login(string returnurl)
        {
            if (string.IsNullOrEmpty(returnurl) && Request.UrlReferrer != null)
                returnurl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

            if (Url.IsLocalUrl(returnurl) && !string.IsNullOrEmpty(returnurl))
            {
                ViewBag.returnurl = returnurl;
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model, string returnurl)
        {
            try
            {
                var decodeUrl = string.Empty;
                if (!ModelState.IsValid) return View(model);
                var user = await _db.Users.SingleOrDefaultAsync(x => x.Email.Equals(model.Email));
                if (user == null)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
                    return View(model);
                }
                if (!EncryptDecrypt.CompareTwoMd5String(model.Password, user.Password))
                {
                    ModelState.AddModelError("", "Mật khẩu không chính xác");
                    return View(model);
                }
                if (!user.Activated)
                {
                    ModelState.AddModelError("", "Tài khoản đang tạm khóa");
                    return View(model);
                }
                Session["UserId"] = user.Id;
                if (!string.IsNullOrEmpty(returnurl))
                {
                    if (!string.IsNullOrEmpty(returnurl))
                    {
                        decodeUrl = Server.UrlDecode(returnurl);
                        return Redirect(decodeUrl);
                    }
                }
                return RedirectToAction("index", "home");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public async Task<ActionResult> Manage(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = await _db.Users.FindAsync(id);
            if (user == null) return HttpNotFound();
            return View(user);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("login", "account");
        }

        public async Task<ActionResult> Update(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = await _db.Users.FindAsync(id);
            if (user == null) return HttpNotFound();
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Update(User model)
        {
            if (!ModelState.IsValid) return View(model);
            var avatar = Request.Files["Avatar"];
            if (avatar != null && avatar.ContentLength > 0)
            {
                if (avatar.ContentLength > 2048000)
                {
                    ModelState.AddModelError("", "Dung lượng avatar không lớn hơn 2Mb");
                    return View(model);
                }
                if (System.IO.File.Exists($"{Path}/{model.Avatar}")) System.IO.File.Delete($"{Path}/{model.Avatar}");
                var folder = Server.MapPath(Path);
                var fileName = new FileInfo(avatar.FileName);
                var fileExt = fileName.Extension;
                var newFileName = $"{model.Id}{fileExt}";
                avatar.SaveAs($"{folder}/{newFileName}");
                model.Avatar = newFileName;
            }
            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction("manage");
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