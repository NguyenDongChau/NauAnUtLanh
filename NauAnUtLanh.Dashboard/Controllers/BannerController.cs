using System;
using System.Linq;
using System.Data.Entity;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using NauAnUtLanh.Database;

namespace NauAnUtLanh.Dashboard.Controllers
{
    public class BannerController : BaseController
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();

        public async Task<ActionResult> Index()
        {
            var banners = await _db.Banners.OrderByDescending(x=>x.MenuOrder).ToListAsync();
            return View(banners);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Guid id, bool activated, int orderno)
        {
            var banner = await _db.Banners.FindAsync(id);
            banner.Activated = activated;
            banner.MenuOrder = orderno;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Upload()
        {
            var files = Request.Files;
            var folder = Server.MapPath("~/upload/banner");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            for (var i = 0; i < files.Count; i++)
            {
                if (files[i] != null && files[i].ContentLength > 0)
                {
                    var fileInfo = new FileInfo(files[i].FileName);
                    var fileExt = fileInfo.Extension;
                    var id = Guid.NewGuid();
                    var newFileName = $"{id}{fileExt}";
                    var filePath = $"{folder}/{newFileName}";
                    files[i].SaveAs(filePath);
                    var banner = new Banner
                    {
                        Id = id,
                        MenuOrder = 0,
                        BannerImage = newFileName,
                        Activated = false
                    };
                    _db.Banners.Add(banner);
                }
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<int> Remove(Guid id)
        {
            var banner = await _db.Banners.FindAsync(id);
            if (banner == null) return -1;
            var folder = Server.MapPath("~/upload/banner");
            if(System.IO.File.Exists($"{folder}/{banner.BannerImage}")) 
                System.IO.File.Delete($"{folder}/{banner.BannerImage}");
            _db.Entry(banner).State = EntityState.Deleted;
            return await _db.SaveChangesAsync();
        }

        [HttpPost]
        public async Task<int> ChangeStatus(Guid id)
        {
            var banner = await _db.Banners.FindAsync(id);
            if (banner == null) return 0;
            banner.Activated = !banner.Activated;
            await _db.SaveChangesAsync();
            return 1;
        }

        [HttpPost]
        public async Task<int> UpdateOrder(Guid id, int order)
        {
            var banner = await _db.Banners.FindAsync(id);
            if (banner == null) return 0;
            banner.MenuOrder = order;
            _db.Entry(banner).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return 1;
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