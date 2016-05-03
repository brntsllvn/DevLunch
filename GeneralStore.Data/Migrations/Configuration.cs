using DevLunch.Data.Models;

namespace DevLunch.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GeneralStoreDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GeneralStoreDbContext context)
        {
            context.Products.AddOrUpdate(p=>p.Name,new Product {Name = "beef jerky"});
            context.Products.AddOrUpdate(p=>p.Name,new Product {Name = "milk"});
        }
    }
}
