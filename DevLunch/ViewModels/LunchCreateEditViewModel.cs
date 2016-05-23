using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevLunch.ViewModels
{
    public class LunchCreateEditViewModel
    {
        public LunchCreateEditViewModel()
        {
            Restaurants = new List<CheckBoxListItem>();
        }

        public string Host { get; set; }

        [Required]
        public DateTime? MeetingTime { get; set; }
            
        public List<CheckBoxListItem> Restaurants { get; set; }
    }
}