using System.Web.Mvc;

namespace NauAnUtLanh.Dashboard.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}