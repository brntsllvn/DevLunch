using System.Collections.Generic;
using System.Linq;

namespace GeneralStore.Models
{
    public class ProductRepository
    {
        private IApplicationDbContext _context;

        public ProductRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }
    }
}
