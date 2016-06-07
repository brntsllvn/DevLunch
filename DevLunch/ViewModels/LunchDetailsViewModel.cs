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

        public string MeetingTimeDisplay
        {
            get
            {
                if (!MeetingTime.HasValue)
                    return null;

                return this.MeetingTime.Value.ToShortTimeString() + " " + this.MeetingTime.Value.ToLongDateString();
            }
        }
    }

    public static class DateFormattingExtensions
    {
        public static string ToDateTimeLocal(this DateTime? date)
        {
            if (!date.HasValue)
                return null;

            var dateTime = date.Value;
            return String.Format("{0}-{1}-{2}T{3}:{4}:{5}",
                dateTime.Year,
                dateTime.Month.ToString().PadLeft(2, '0'),
                dateTime.Day.ToString().PadLeft(2, '0'),
                dateTime.Hour.ToString().PadLeft(2, '0'),
                dateTime.Minute.ToString().PadLeft(2, '0'),
                dateTime.Second.ToString().PadLeft(2, '0')
                );

            
        }
    }
}