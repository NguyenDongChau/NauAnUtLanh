using System.Linq;
using System.Web.Mvc;
using NauAnUtLanh.Database;

namespace NauAnUtLanh.FrontEnd.Controllers
{
    public class BannerController : Controller
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();
        private const int PageSize = 5;

        public ActionResult ShowBanners()
        {
            var banners = _db.Banners.OrderBy(x => x.MenuOrder).Where(x => x.Activated).Take(PageSize).ToList();
            return PartialView("_Banners", banners);
        }
    }
}