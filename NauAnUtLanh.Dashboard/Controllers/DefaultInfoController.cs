using System;
using System.Data.Entity;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Mvc;
using NauAnUtLanh.Dashboard.Models;
using NauAnUtLanh.Database;

namespace NauAnUtLanh.Dashboard.Controllers
{
    public class DefaultInfoController : BaseController
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();
        private const string Path = "~/upload";

        public async Task<ActionResult> Index()
        {
            var info = await _db.DefaultInfos.FindAsync(Guid.Empty);
            if (info == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var model = new DefaultInfoViewModel
            {
                SiteLogo = info.SiteLogo,
                SiteIcon = info.SiteIcon,
                CompanyName = info.CompanyName,
                CompanyAddress = info.CompanyAddress,
                CompanyPhone = info.CompanyPhone,
                Hotline = info.Hotline,
                CompanyEmail = info.CompanyEmail,
                OnlineSupport = info.OnlineSupport,
                GoogleMapUrl = info.GoogleMapUrl,
                MetaDescription = info.MetaDescription,
                MetaImage = info.MetaImage,
                MetaKeywords = info.MetaKeywords,
                FacebookPageUrl = info.FacebookPageUrl,
                GooglePlusPageUrl = info.GooglePlusPageUrl,
                TwitterPageUrl = info.TwitterPageUrl
            };
            return View(model);
        }

        public async Task<ActionResult> Update()
        {
            var info = await _db.DefaultInfos.FindAsync(Guid.Empty);
            if (info == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var model = new DefaultInfoViewModel
            {
                SiteLogo = info.SiteLogo,
                SiteIcon = info.SiteIcon,
                CompanyName = info.CompanyName,
                CompanyAddress = info.CompanyAddress,
                CompanyPhone = info.CompanyPhone,
                Hotline = info.Hotline,
                CompanyEmail = info.CompanyEmail,
                OnlineSupport = info.OnlineSupport,
                GoogleMapUrl = info.GoogleMapUrl,
                MetaDescription = info.MetaDescription,
                MetaImage = info.MetaImage,
                MetaKeywords = info.MetaKeywords,
                FacebookPageUrl = info.FacebookPageUrl,
                GooglePlusPageUrl = info.GooglePlusPageUrl,
                TwitterPageUrl = info.TwitterPageUrl
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Update(DefaultInfoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var info = await _db.DefaultInfos.FindAsync(Guid.Empty);
            if (info == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (System.IO.File.Exists($"{Path}")) System.IO.File.Delete($"{Path}");
            var logo = Request.Files["logo"];
            if (logo != null && logo.ContentLength > 0)
            {
                if (logo.ContentLength > 2048000)
                {
                    ModelState.AddModelError("", "Hình ảnh upload không vượt quá 2Mb");
                    return View(model);
                }
                var folder = Server.MapPath(Path);
                var fileName = new System.IO.FileInfo(logo.FileName);
                var fileExt = fileName.Extension;
                var newFileName = $"logo{fileExt}";
                logo.SaveAs($"{folder}/{newFileName}");
                info.SiteLogo = newFileName;
            }
            var icon = Request.Files["icon"];
            if (icon != null && icon.ContentLength > 0)
            {
                if (icon.ContentLength > 2048000)
                {
                    ModelState.AddModelError("", "Hình ảnh upload không vượt quá 2Mb");
                    return View(model);
                }
                var folder = Server.MapPath(Path);
                var fileName = new System.IO.FileInfo(icon.FileName);
                var fileExt = fileName.Extension;
                var newFileName = $"favicon{fileExt}";
                icon.SaveAs($"{folder}/{newFileName}");
                info.SiteIcon = newFileName;
            }
            var image = Request.Files["image"];
            if (image != null && image.ContentLength > 0)
            {
                if (image.ContentLength > 2048000)
                {
                    ModelState.AddModelError("", "Hình ảnh upload không vượt quá 2Mb");
                    return View(model);
                }
                var folder = Server.MapPath(Path);
                var fileName = new System.IO.FileInfo(image.FileName);
                var fileExt = fileName.Extension;
                var newFileName = $"{DateTime.Now:yyyyMMddhhmmssfff}{fileExt}";
                image.SaveAs($"{folder}/{newFileName}");
                info.MetaImage = newFileName;
            }
            info.CompanyName = model.CompanyName;
            info.CompanyAddress = model.CompanyAddress;
            info.CompanyPhone = model.CompanyPhone;
            info.Hotline = model.Hotline;
            info.CompanyEmail = model.CompanyEmail;
            info.OnlineSupport = model.OnlineSupport;
            info.GoogleMapUrl = model.GoogleMapUrl;
            info.MetaDescription = model.MetaDescription;
            info.MetaKeywords = model.MetaKeywords;
            info.FacebookPageUrl = model.FacebookPageUrl;
            info.GooglePlusPageUrl = model.GooglePlusPageUrl;
            info.TwitterPageUrl = model.TwitterPageUrl;
            _db.Entry(info).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction("index");
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