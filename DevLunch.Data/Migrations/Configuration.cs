using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DevLunch.Data.Models;

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

        protected override void Seed(DevLunchDbContext context)
        {
            context.Votes.RemoveRange(context.Votes);
            context.Restaurants.RemoveRange(context.Restaurants);
            context.Lunches.RemoveRange(context.Lunches);

            var restaurants = new List<Restaurant>
            {
                new Restaurant {Name = "Rocco’s"},
                new Restaurant {Name = "Li’l Woody’s"},
                new Restaurant {Name = "Biscuit Bitch Belltown"},
                new Restaurant {Name = "Lunchbox Laboratory"},
                new Restaurant {Name = "Citrus Thai Cuisine"},
                new Restaurant {Name = "Icon Grill"},
                new Restaurant {Name = "FareStart"},
                new Restaurant {Name = "Gordon Biersch"},
                new Restaurant {Name = "Dahlia Lounge"},
                new Restaurant {Name = "Cactus"},
                new Restaurant {Name = "Blue Moon Burgers"},
                new Restaurant {Name = "Serious Pie"},
                new Restaurant {Name = "Brave Horse Tavern"}
            };

            context.Restaurants.AddRange(restaurants);
            context.SaveChanges();
        }
    }
}
