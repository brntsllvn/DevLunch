using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevLunch.Data.Models
{
    public class LunchCandidate
    {
        [Key]
        public int id { get; set; }

        public string Host { get; set; }

        public DateTime Time { get; set; }

        public IEnumerable<Restaurant> Restaurants { get; set; }
    }
}
