using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using NauAnUtLanh.Dashboard.Models;
using NauAnUtLanh.Database;
using PagedList;

namespace NauAnUtLanh.Dashboard.Controllers
{
    public class PhotoAlbumController : BaseController
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();
        private const string Folder = "~/upload/album";

        public async Task<ActionResult> Index(int? page, string keywords)
        {
            var pageNumber = page ?? 1;
            var albums = await _db.PhotoAlbums
                .OrderByDescending(x => x.CreatedTime)
                .ToListAsync();
            if (!string.IsNullOrEmpty(keywords))
                albums =
                    await
                        _db.PhotoAlbums
                            .Where(x => x.AlbumName.Contains(keywords))
                            .OrderByDescending(x => x.CreatedTime)
                            .ToListAsync();
            return View(albums.ToPagedList(pageNumber, 30));
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var album = await _db.PhotoAlbums.FindAsync(id);
            if (album == null) return HttpNotFound();
            return View(album);
        }

        [HttpPost]
        public ActionResult AddImages(string id)
        {
            var uploadPath = Server.MapPath($"{Folder}/{id}");
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
            var files = Request.Files;
            for (var i = 0; i < files.Count; i++)
            {
                var fileInfo = new FileInfo(files[i].FileName);
                if (fileInfo.Extension.ToLower().Equals(".jpeg") ||
                    fileInfo.Extension.ToLower().Equals(".jpg") ||
                    fileInfo.Extension.ToLower().Equals(".png") ||
                    fileInfo.Extension.ToLower().Equals(".bmp") ||
                    fileInfo.Extension.ToLower().Equals(".gif"))
                {
                    var name = $"{DateTime.Now:yyyyMMddHHmmssfff}";
                    var fileName = $"{name}{fileInfo.Extension}";
                    files[i].SaveAs($"{uploadPath}/{fileName}");
                }
            }
            
            var imgs = Directory.GetFiles(uploadPath).ToList();
            var imgNameList = new List<string>();
            foreach (var file in imgs)
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PhotoAlbumViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var files = Request.Files;
            if (files == null || files.Count <= 0)
            {
                ModelState.AddModelError("", "Chưa cung cấp hình ảnh cho album");
                return View(model);
            }
            var existed = await _db.PhotoAlbums.AnyAsync(x => x.AlbumName.Equals(model.AlbumName));
            if (existed)
            {
                ModelState.AddModelError("", "Tên album đã có, hãy đặt tên khác");
                return View(model);
            }
            var id = Guid.NewGuid();
            var album = new PhotoAlbum
            {
                Id = id,
                AlbumName = model.AlbumName,
                Activated = model.Activated,
                CreatedTime = DateTime.Now
            };
            _db.PhotoAlbums.Add(album);
            await _db.SaveChangesAsync();

            var uploadPath = Server.MapPath($"{Folder}/{id}");
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
            for (var i = 0; i < files.Count; i++)
            {
                var fileInfo = new FileInfo(files[i].FileName);
                if (fileInfo.Extension.ToLower().Equals(".jpeg") ||
                    fileInfo.Extension.ToLower().Equals(".jpg") ||
                    fileInfo.Extension.ToLower().Equals(".png") ||
                    fileInfo.Extension.ToLower().Equals(".bmp") ||
                    fileInfo.Extension.ToLower().Equals("gif"))
                {
                    var name = $"{DateTime.Now:yyyyMMddHHmmssfff}";
                    var fileName = $"{name}{fileInfo.Extension}";
                    files[i].SaveAs($"{uploadPath}/{fileName}");
                }
            }
            return RedirectToAction("Index");
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

        [HttpPost]
        public void Delete(string album, string imgname)
        {
            var folderPath = Server.MapPath($"{Folder}/{album}");
            if(System.IO.File.Exists($"{folderPath}/{imgname}")) System.IO.File.Delete($"{folderPath}/{imgname}");
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
