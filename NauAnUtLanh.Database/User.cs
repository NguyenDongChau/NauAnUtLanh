using System;
using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Database
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public bool Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool Activated { get; set; }
    }
}
