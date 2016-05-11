using System;
using System.ComponentModel.DataAnnotations;

namespace DevLunch.Data.Models
{
    public class Lunch
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Host { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? MeetingTime { get; set; }

        public Restaurant Restaurant { get; set; }
    }
}
 