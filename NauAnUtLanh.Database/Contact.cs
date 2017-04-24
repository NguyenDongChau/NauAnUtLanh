using System;
using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Database
{
    public class Contact
    {
        [Key]
        public Guid Id { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
