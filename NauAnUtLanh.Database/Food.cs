using System;
using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Database
{
    public class Food
    {
        [Key]
        public Guid Id { get; set; }

        public int CategoryId { get; set; }
        public string FoodName { get; set; }
        public string FoodType { get; set; }
        public bool Activated { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
