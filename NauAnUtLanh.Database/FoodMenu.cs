using System;
using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Database
{
    public class FoodMenu
    {
        [Key]
        public Guid Id { get; set; }

        public string MenuName { get; set; }
        public string Avatar { get; set; }
        public long Price { get; set; }
        public string FoodIdList { get; set; }
        public bool Activated { get; set; }
        public bool Feature { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
