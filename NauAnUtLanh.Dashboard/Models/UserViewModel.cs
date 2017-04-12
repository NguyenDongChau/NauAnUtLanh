using System;

namespace NauAnUtLanh.Dashboard.Models
{
    public class UserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public bool Gender { get; set; }
        public string BirthDate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}