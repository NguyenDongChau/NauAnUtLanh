using System;
using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Dashboard.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Email không thể bỏ trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = "Họ tên không thể bỏ trống")]
        public string FullName { get; set; }
        public bool Gender { get; set; }
        public string BirthDate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}