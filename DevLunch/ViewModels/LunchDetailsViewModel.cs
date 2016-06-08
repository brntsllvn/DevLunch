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
            {
                var today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 11, 30, 00);
                return FormatDateTime(today);
            }

            return FormatDateTime(date.Value);
        }

        private static string FormatDateTime(DateTime today)
        {
            return String.Format("{0}-{1}-{2}T{3}:{4}:{5}",
                today.Year,
                today.Month.ToString().PadLeft(2, '0'),
                today.Day.ToString().PadLeft(2, '0'),
                today.Hour.ToString().PadLeft(2, '0'),
                today.Minute.ToString().PadLeft(2, '0'),
                today.Second.ToString().PadLeft(2, '0')
                );
        }
    }
}