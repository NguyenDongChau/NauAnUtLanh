using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Dashboard.Models
{
    public class CategoryViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên loại món ăn không thể bỏ trống")]
        public string CategoryName { get; set; }
        public bool Activated { get; set; }
    }
}