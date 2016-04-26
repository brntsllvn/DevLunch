using GeneralStore.Data.Models;

namespace GeneralStore.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GeneralStore.Data.GeneralStoreDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GeneralStore.Data.GeneralStoreDbContext context)
        {
            context.Products.AddOrUpdate(p=>p.Name,new Product {Name = "beef jerky"});
            context.Products.AddOrUpdate(p=>p.Name,new Product {Name = "milk"});
        }
    }
}
