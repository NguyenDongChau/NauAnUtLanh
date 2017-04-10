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

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("login", "account");
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