using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using NauAnUtLanh.Database;

namespace NauAnUtLanh.Dashboard.Controllers
{
    public class CategoryController : Controller
    {
        private readonly NauAnUtLanhDbContext _db = new NauAnUtLanhDbContext();

        public async Task<int> Create(Category model)
        {
            if (!ModelState.IsValid) return -1;
            var existed = await _db.Categories.AnyAsync(x => x.CategoryName == model.CategoryName);
            if (existed)
            {
                var category = await _db.Categories.FirstOrDefaultAsync(x => x.CategoryName == model.CategoryName);
                return category.Id;
            }
            model.CreatedTime = DateTime.Now;
            model.Activated = true;
            _db.Entry(model).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return model.Id;
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