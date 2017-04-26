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
        private const int PageSize = 10;

        public async Task<ActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var albums =
                await _db.PhotoAlbums.OrderByDescending(x => x.CreatedTime).Where(x => x.Activated).ToListAsync();
            return View(albums.ToPagedList(pageNumber , PageSize));
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