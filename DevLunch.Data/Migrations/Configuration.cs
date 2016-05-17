using System.Collections.Generic;
using DevLunch.Data.Models;
using FizzWare.NBuilder;

namespace DevLunch.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DevLunch.Data.DevLunchDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DevLunch.Data.DevLunchDbContext context)
        {
            var restaurants = Builder<Restaurant>.CreateListOfSize(30)
                .All()
                .With(r => r.Name = Faker.Company.Name())
                .Build();

            var daysGenerator = new RandomGenerator();

            var lunches = Builder<Lunch>.CreateListOfSize(20)
                .All()
                .With(r => r.Host = Faker.Name.FullName())
                .With(r => r.MeetingTime = DateTime.Now.AddDays(-daysGenerator.Next(1, 100)))
                .With(r => r.Restaurants = new List<Restaurant>
                {
                    Pick<Restaurant>.RandomItemFrom(restaurants),
                    Pick<Restaurant>.RandomItemFrom(restaurants),
                    Pick<Restaurant>.RandomItemFrom(restaurants),
                })
                .Build();

            context.Lunches.AddRange(lunches);
            context.SaveChanges();

        }
    }
}
