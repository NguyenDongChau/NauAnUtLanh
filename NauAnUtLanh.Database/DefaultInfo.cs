using System;
using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Database
{
    public class DefaultInfo
    {
        [Key]
        public Guid Id { get; set; }

        public string SiteLogo { get; set; }
        public string SiteIcon { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhone { get; set; }
        public string Hotline { get; set; }
        public string CompanyEmail { get; set; }
        public string OnlineSupport { get; set; }
        public string GoogleMapUrl { get; set; }
        public string MetaDescription { get; set; }
        public string MetaImage { get; set; }
        public string MetaKeywords { get; set; }
        public string FacebookPageUrl { get; set; }
        public string GooglePlusPageUrl { get; set; }
        public string TwitterPageUrl { get; set; }
    }
}
