using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Dashboard.Models
{
    public class ChangePassViewModel
    {
        [Required(ErrorMessage = "Chưa nhập mật khẩu hiện tại")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu không thể bỏ trống")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Chưa xác nhận mật khẩu")]
        public string ConfirmNewPassword { get; set; }
    }
}