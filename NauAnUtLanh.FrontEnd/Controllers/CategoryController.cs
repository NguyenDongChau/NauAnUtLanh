using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using NauAnUtLanh.Database;

namespace NauAnUtLanh.FrontEnd.Controllers
{
    public class CategoryController : Controller
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();

        public async Task<ActionResult> GetCategories()
        {
            var categories = await _db.Categories.Where(x=>x.Activated).ToListAsync();
            return PartialView("_CategoryList", categories);
        }

        public async Task<ActionResult> GetCategoriesForSmall()
        {
            var categories = await _db.Categories.Where(x => x.Activated).ToListAsync();
            return PartialView("_SelectCategoryList", categories);
        }
    }
}