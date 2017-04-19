using System;
using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Dashboard.Models
{
    public class FoodMenuViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Tên thực đơn không thể bỏ trống")]
        public string MenuName { get; set; }

        public string Avatar { get; set; }

        [Required(ErrorMessage = "Giá thực đơn không thể bỏ trống")]
        public long Price { get; set; }

        public string FoodIdList { get; set; }

        public bool Activated { get; set; }
    }
}