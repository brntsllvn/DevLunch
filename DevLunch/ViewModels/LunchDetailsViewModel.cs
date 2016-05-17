using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DevLunch.Data.Models;

namespace DevLunch.ViewModels
{
    public class LunchDetailsViewModel
    {
        public LunchDetailsViewModel()
        {
            Restaurants = new List<Restaurant>();
            Votes = new List<Vote>();
        }

        public int Id { get; set; }

        public string Host { get; set; }

        [DataType(DataType.Date)]
        public DateTime? MeetingTime { get; set; }

        public Lunch Lunch { get; set; }

        public ICollection<Restaurant> Restaurants { get; set; }

        public ICollection<Vote> Votes { get; set; }
    }
}