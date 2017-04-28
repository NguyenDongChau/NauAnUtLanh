using System;
using System.Data.Entity.Migrations;

namespace NauAnUtLanh.Database.Migrations
{
    

    internal sealed class Configuration : DbMigrationsConfiguration<NauAnUtLanhDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(NauAnUtLanhDbContext context)
        {
            //context.DefaultInfos.AddOrUpdate(
            //  new DefaultInfo
            //  {
            //      Id = Guid.Empty,
            //      SiteLogo = "logo.png",
            //      SiteIcon = "favicon.ico",
            //      CompanyName = "Nấu ăn Út Lanh",
            //      CompanyAddress = "Tổ 4, Ấp Láng Cát, Xã Tân Phú Trung, Huyện Củ Chi, TP.HCM",
            //      CompanyPhone = "08.38.922.934",
            //      Hotline = "0933.55.7773",
            //      CompanyEmail = "nauanutlanh@gmail.com",
            //      MetaDescription = "Nấu ăn Út Lanh",
            //      MetaImage = "nauanutlanh.jpg",
            //      MetaKeywords = "nấu ăn, cơ sở nấu ăn, nấu ăn Út Lanh, cơ sở nấu ăn Củ Chi"
            //  }
            //);
            //context.SaveChanges();

            //context.Users.AddOrUpdate(
            //    new User
            //    {
            //        Id = Guid.NewGuid(),
            //        Email = "chau.nguyendong@ndc.net.vn",
            //        Password = "e10adc3949ba59abbe56e057f20f883e",
            //        FullName = "Nguyễn Đông Châu",
            //        Gender = true,
            //        BirthDate = "1982/01/21",
            //        Address = "147/1 Tô Ngọc Vân, Phường Thạnh Xuân, Quận 12, Tp. Hồ Chí Minh",
            //        Phone = "0939.248.449",
            //        CreatedTime = DateTime.Now,
            //        Activated = true
            //    },
            //    new User
            //    {
            //        Id = Guid.NewGuid(),
            //        Email = "minhhau.le0710@gmail.com",
            //        Password = "e10adc3949ba59abbe56e057f20f883e",
            //        FullName = "Lê Minh Hậu",
            //        Gender = true,
            //        BirthDate = "1987/10/07",
            //        Phone = "0937.400.710",
            //        CreatedTime = DateTime.Now,
            //        Activated = true
            //    },
            //    new User
            //    {
            //        Id = Guid.NewGuid(),
            //        Email = "nauanutlanh@gmail.com",
            //        Password = "e10adc3949ba59abbe56e057f20f883e",
            //        FullName = "Nấu ăn Út Lanh",
            //        Gender = true,
            //        Phone = "0933.55.7773",
            //        CreatedTime = DateTime.Now,
            //        Activated = true
            //    }
            //);
            //context.SaveChanges();
        }
    }
}
