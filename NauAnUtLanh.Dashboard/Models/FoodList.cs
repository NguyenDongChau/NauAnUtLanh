using System.Collections.Generic;
using System.Web.Mvc;

namespace NauAnUtLanh.Dashboard.Models
{
    public class FoodList
    {
        public IList<string> SelectedFoods { get; set; }
        public IList<SelectListItem> AvailableFoods { get; set; }

        public FoodList()
        {
            SelectedFoods = new List<string>();
            AvailableFoods = new List<SelectListItem>();
        }
    }
}