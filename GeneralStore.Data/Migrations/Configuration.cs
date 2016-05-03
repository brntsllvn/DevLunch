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
            context.Restaurants.AddOrUpdate( r => r.Name,new Restaurant {Name = "Lunch Box Lab"});
            context.Restaurants.AddOrUpdate( r => r.Name,new Restaurant { Name = "Yard House"});
        }
    }
}
