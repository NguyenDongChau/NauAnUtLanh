using System;
using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Dashboard.Models
{
    public class PersonalInfoViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public bool Gender { get; set; }
        public string BirthDate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
    }
}