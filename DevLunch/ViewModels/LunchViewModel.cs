using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevLunch.ViewModels
{
    public class LunchViewModel
    {
        public LunchViewModel()
        {
            Restaurants = new List<CheckBoxListItem>();
        }

        [Required]
        public string Host { get; set; }

        [Required]
        public DateTime? MeetingTime { get; set; }

        [Required]
        [DisplayName("Restaurant")]
        public int SelectedRestaurantId { get; set; }
            
        public List<CheckBoxListItem> Restaurants { get; set; }

        //public IEnumerable<SelectListItem> Restaurants { get; set; }
    }
}