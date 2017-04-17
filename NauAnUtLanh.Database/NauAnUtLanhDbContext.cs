using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace NauAnUtLanh.Database
{
    public class NauAnUtLanhDbContext:DbContext
    {
        public NauAnUtLanhDbContext():base("name=NauAnUtLanh.ConnectionString")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<DefaultInfo> DefaultInfos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodMenu> FoodMenus { get; set; }
        public DbSet<PhotoAlbum> PhotoAlbums { get; set; }
        public DbSet<Article> Articles { get; set; }
    }
}
