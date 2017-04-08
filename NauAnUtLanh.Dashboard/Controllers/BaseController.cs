using System.Web.Mvc;

namespace NauAnUtLanh.Dashboard.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var returnUrl = Request.Url.AbsoluteUri;
            if (Session["UserId"] == null)
                filterContext.Result = new RedirectResult(Url.Action("login", "account", new { returnurl = returnUrl }));
        }
    }
}