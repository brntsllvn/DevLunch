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
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DevLunchDbContext context)
        {
            var Lunches = new List<Lunch>();

            for (int i = 0; i < 100; i++)
            {
                new Lunch
                {
                    Host = $"Person_{i}",
                    Restaurant = new Restaurant
                    {
                        Name = $"Restaurant_{i}",
                        Latitude = new Random().Next(0, 10),
                        Longitude = new Random().Next(10, 50),
                    },
                    MeetingTime = new DateTime(new Random().Next(1990, 2050), new Random().Next(1, 12), new Random().Next(1, 27))
                };
            }

            context.Lunches.AddRange(Lunches);
            context.SaveChanges();
        }
    }
}
