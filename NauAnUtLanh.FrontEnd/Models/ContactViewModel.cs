using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.FrontEnd.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Vui lòng cung cấp họ tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng cung cấp địa chỉ email")]
        [EmailAddress(ErrorMessage = "Địa chỉ mail không đúng định dạng")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng cung cấp tiêu đề")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Vui lòng cung cấp nội dung")]
        public string Content { get; set; }
    }
}