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

        // todo: not working...
        protected override void Seed(DevLunchDbContext context)
        {
            var Lunches = new List<Lunch>();
            var restaurants = new List<Restaurant>
            {
                new Restaurant
                {
                    Name = string.Format("Restaurant_{0}", new Random().Next(0, 1000)),
                    Latitude = new Random().Next(0, 10),
                    Longitude = new Random().Next(10, 50)
                },
                new Restaurant
                {
                    Name = string.Format("Restaurant_{0}", new Random().Next(0, 1000)),
                    Latitude = new Random().Next(0, 10),
                    Longitude = new Random().Next(10, 50)
                },
                new Restaurant
                {
                    Name = string.Format("Restaurant_{0}", new Random().Next(0, 1000)),
                    Latitude = new Random().Next(0, 10),
                    Longitude = new Random().Next(10, 50)
                }
            };

            for (int i = 0; i < 100; i++)
            {
                new Lunch
                {
                    Host = $"Person_{i}",
                    Restaurants = restaurants,
                    MeetingTime = new DateTime(new Random().Next(1990, 2050), new Random().Next(1, 12), new Random().Next(1, 27))
                };
            }

            context.Lunches.AddRange(Lunches);
            context.SaveChanges();
        }
    }
}
