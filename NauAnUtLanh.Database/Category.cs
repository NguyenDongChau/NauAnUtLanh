using System;
using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Database
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string CategoryName { get; set; }
        public bool Activated { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
