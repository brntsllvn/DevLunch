using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GeneralStore.Data;
using GeneralStore.Data.Models;

namespace GeneralStore.Controllers.api
{
    public class ProductController : ApiController
    {
        private readonly GeneralStoreDbContext _context;

        public ProductController() : this(new GeneralStoreDbContext())
        {
            
        }

        public ProductController(GeneralStoreDbContext context)
        {
            _context = context;
        }

        // GET: api/Product
        public IEnumerable<Product> Get()
        {
            return _context.Products;
        }

        // GET: api/Product/5
        public Product Get(int id)
        {
            return _context.Products.Find(id);
        }

        // POST: api/Product
        public void Post([FromBody]Product value)
        {
            if (_context.Products.Find(value.Id) != null)
                throw new ArgumentException("Product already exists");

            _context.Products.Add(value);
        }

        // PUT: api/Product/5
        public void Put(int id, [FromBody]Product value)
        {
            if(value == null)
                throw new ArgumentNullException(nameof(value),"Product must be specified");
            var product = _context.Products.Find(id);
            if (product == null)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Product not found");

            product.Name = value.Name;
            _context.SaveChanges();
        }

        // DELETE: api/Product/5
        public void Delete(int id)
        {
            var product = _context.Products.Find(id);
            if(product == null)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Product not found");

            _context.Products.Remove(product);
            _context.SaveChanges();
           

        }
    }
}
