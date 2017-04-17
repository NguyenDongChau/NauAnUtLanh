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

        [Required(ErrorMessage = "Giá thực đơn không thể bỏ trống")]
        public long Price { get; set; }

        [Required(ErrorMessage = "Chưa chọn món ăn cho thực đơn")]
        public Guid FoodId { get; set; }

        public bool Activated { get; set; }
    }
}