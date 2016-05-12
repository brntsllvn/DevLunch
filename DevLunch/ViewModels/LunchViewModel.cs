using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DevLunch.ViewModels
{
    public class LunchViewModel
    {
        [Required]
        public string Host { get; set; }

        [Required]
        public DateTime? MeetingTime { get; set; }

        [Required]
        [DisplayName("Restaurant")]
        public int SelectedRestaurantId { get; set; }

        public IEnumerable<SelectListItem> Restaurants { get; set; }
    }
}