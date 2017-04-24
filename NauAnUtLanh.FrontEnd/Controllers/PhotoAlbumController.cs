using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using NauAnUtLanh.Database;
using NauAnUtLanh.FrontEnd.Models;
using PagedList;

namespace NauAnUtLanh.FrontEnd.Controllers
{
    public class PhotoAlbumController : Controller
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();
        private const int PageSize = 30;
        private const string Folder = "~/upload/album";

        public async Task<ActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var albums =
                await _db.PhotoAlbums.OrderByDescending(x => x.CreatedTime).Where(x => x.Activated).ToListAsync();
            return View(albums.ToPagedList(pageNumber , PageSize));
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var album = await _db.PhotoAlbums.FindAsync(id);
            if (album == null) return HttpNotFound();
            return View(album);
        }

        public ActionResult PhotosByAlbum(Guid id)
        {
            var folder = Server.MapPath($"{Folder}/{id}");
            var files = Directory.GetFiles(folder).ToList();
            var imgNameList = new List<string>();
            foreach (var file in files)
            {
                var imgName = new FileInfo(file).Name;
                imgNameList.Add(imgName);
            }
            var imgGallery = new ImageGallery
            {
                FolderName = $"{id}",
                ImageNameList = imgNameList
            };
            return PartialView("_PhotoGallery", imgGallery);
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