using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralStore.Models
{
    public class ProductSeedData
    {
        private ApplicationDbContext _context;

        public ProductSeedData(ApplicationDbContext context)
        {
            _context = context;
        }

        public void EnsureSeedData()
        {
            if (!_context.Products.Any())
            {
                var products = new List<Product>();

                for (int i = 0; i < 100; i++)
                {
                    products.Add(new Product { Name = $"box_{i}", Description = $"Someth{i}ng" });
                }

                _context.Products.AddRange(products);
                _context.SaveChanges();
            }
        }
    }
}
