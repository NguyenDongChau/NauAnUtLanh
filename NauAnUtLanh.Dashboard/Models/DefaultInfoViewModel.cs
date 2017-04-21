using System;
using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Dashboard.Models
{
    public class DefaultInfoViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public string SiteLogo { get; set; }
        public string SiteIcon { get; set; }

        [Required(ErrorMessage = "Tên công ty / cơ sở / website không thể bỏ trống")]
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhone { get; set; }
        public string Hotline { get; set; }

        [Required(ErrorMessage = "Email liên hệ không thể bỏ trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string CompanyEmail { get; set; }
        public string OnlineSupport { get; set; }
        public string GoogleMapUrl { get; set; }
        public string MetaDescription { get; set; }
        public string MetaImage { get; set; }
        public string MetaKeywords { get; set; }
        public string FacebookPageUrl { get; set; }
        public string GooglePlusPageUrl { get; set; }
        public string TwitterPageUrl { get; set; }
        public string FacebookAppId { get; set; }
    }
}