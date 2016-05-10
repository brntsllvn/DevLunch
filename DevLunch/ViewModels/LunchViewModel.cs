using System.Collections.Generic;
using DevLunch.Data.Models;

namespace DevLunch.ViewModels
{
    public class LunchViewModel
    {
        public Lunch Lunch { get; set; }

        public List<Restaurant> RestaurantChoices { get; set; }
    }
}