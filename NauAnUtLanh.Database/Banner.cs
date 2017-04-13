using System;
using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Database
{
    public class Banner
    {
        [Key]
        public Guid Id { get; set; }
        public int MenuOrder { get; set; } 

        [Required(ErrorMessage = "Please provide banner image, image size 1920x600")]
        public string BannerImage { get; set; }
        public bool Activated { get; set; }
    }
}
