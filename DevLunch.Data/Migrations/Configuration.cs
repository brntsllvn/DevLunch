using System;
using System.Collections.Generic;
using DevLunch.Data.Models;

namespace DevLunch.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DevLunchDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DevLunchDbContext context)
        {
            context.Restaurants.AddOrUpdate(r => r.Name, new Restaurant { Name = "Lunch Box Lab" });
            context.Restaurants.AddOrUpdate(r => r.Name, new Restaurant { Name = "Yard House" });

            var Lunches = new List<Lunch>
            {
                new Lunch
                {
                     Host = "Brent",
                     Restaurant = new Restaurant
                     {
                         Name = "Lunchbox Labs",
                         Latitude = 55,
                         Longitude = 99
                     },
                     MeetingTime = new DateTime(1999,12,31)
                },
                new Lunch
                {
                     Host = "Brent",
                     Restaurant = new Restaurant
                    {
                        Name = "Lunchbox Labs",
                        Latitude = 55,
                        Longitude = 99
                    },
                    MeetingTime = new DateTime(1999,12,31)
                }
            };

            context.Lunches.AddRange(Lunches);
            context.SaveChanges();
        }
    }
}
