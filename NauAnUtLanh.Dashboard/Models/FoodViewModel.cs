using System;
using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Dashboard.Models
{
    public class FoodViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Tên món ăn không thể bỏ trống")]
        public string FoodName { get; set; }

        public string FoodType { get; set; }
        public bool Activated { get; set; }
        public string Avatar { get; set; }
    }
}