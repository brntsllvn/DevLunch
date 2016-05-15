using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevLunch.Data.Models
{
    public class Lunch
    {
        public Lunch()
        {
            Restaurants = new List<Restaurant>();
        }

        [Key]
        public int Id { get; set; }

        public string Host { get; set; }

        [DataType(DataType.Date)]
        public DateTime? MeetingTime { get; set; }

        public List<Restaurant> Restaurants { get; set; }
    }
}
 