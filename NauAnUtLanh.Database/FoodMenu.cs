using System;
using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Database
{
    public class FoodMenu
    {
        [Key]
        public Guid Id { get; set; }

        public string MenuName { get; set; }
        public long Price { get; set; }
        public Guid FoodId { get; set; }
        public bool Activated { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
