using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Dashboard.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Chưa nhập email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Chưa nhập mật khẩu")]
        public string Password { get; set; }
    }
}
