using System;
using System.Collections.Generic;
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
            
        public List<CheckBoxListItem> Restaurants { get; set; }
    }
}