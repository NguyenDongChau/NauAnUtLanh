using System;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using NauAnUtLanh.Database;
using System.Linq;
using NauAnUtLanh.Dashboard.Models;
using PagedList;

namespace NauAnUtLanh.Dashboard.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();
        private const int PageSize = 30;

        public async Task<ActionResult> Index(int? page, string keyword)
        {
            var pageNumber = page ?? 1;
            var articles = await _db.Articles.OrderByDescending(x => x.CreatedTime).ToListAsync();
            if (!string.IsNullOrEmpty(keyword))
            {
                articles = await _db.Articles.OrderByDescending(x => x.CreatedTime).Where(x=>x.Keywords.Contains(keyword)).ToListAsync();
            }
            var total = articles.Count;
            ViewData["total"] = total;
            return View(articles.ToPagedList(pageNumber, PageSize));
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var article = await _db.Articles.FindAsync(id);
            if (article == null) return HttpNotFound();
            return View(article);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(ArticleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            model.Id = Guid.NewGuid();
            model.CreatedTime = DateTime.Now;
            var file = Request.Files["ArticleAvatar"];
            var folder = Server.MapPath("~/upload/article/");
            if (file == null || file.ContentLength <= 0)
            {
                ModelState.AddModelError("", "Chưa cung cấp hình đại diện cho bài viết");
                return View(model);
            }
            if (file.ContentLength > 2048000)
            {
                ModelState.AddModelError("", "File upload không vượt quá 2Mb");
                return View(model);
            }
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            var fileInfo = new FileInfo(file.FileName);
            var fileExt = fileInfo.Extension;
            var newFileName = $"{DateTime.Now:yyyyMMddhhmmssfff}{fileExt}";
            file.SaveAs($"{folder}/{newFileName}");
            model.ArticleAvatar = newFileName;
            var article = new Article
            {
                Id = model.Id,
                Activated = model.Activated,
                Hot = model.Hot,
                ArticleTitle = model.ArticleTitle,
                ShortDescription = model.ShortDescription,
                ArticleContent = model.ArticleContent,
                Keywords = model.Keywords,
                ArticleAvatar = model.ArticleAvatar,
                CreatedTime = DateTime.Now
            };
            _db.Articles.Add(article);
            await _db.SaveChangesAsync();
            return RedirectToAction("index");
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var article = await _db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            var model = new ArticleViewModel
            {
                Id = article.Id,
                Activated = article.Activated,
                Hot = article.Hot,
                ArticleTitle = article.ArticleTitle,
                ShortDescription = article.ShortDescription,
                ArticleContent = article.ArticleContent,
                Keywords = article.Keywords
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(ArticleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var article = await _db.Articles.FindAsync(model.Id);
            if (article == null) return HttpNotFound();
            var file = Request.Files["ArticleAvatar"];
            var folder = Server.MapPath("~/upload/article/avatar");
            if (file != null && file.ContentLength > 0)
            {
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                var fileInfo = new FileInfo(file.FileName);
                var fileExt = fileInfo.Extension;
                var newFileName = $"{DateTime.Now:yyyyMMddhhmmssfff}{fileExt}";
                file.SaveAs($"{folder}/{newFileName}");
                article.ArticleAvatar = newFileName;
            }

            article.Activated = model.Activated;
            article.Hot = model.Hot;
            article.ArticleTitle = model.ArticleTitle;
            article.ShortDescription = model.ShortDescription;
            article.ArticleContent = model.ArticleContent;
            article.Keywords = model.Keywords;
            _db.Entry(article).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction("index");
        }

        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var article = await _db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            _db.Articles.Remove(article);
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
