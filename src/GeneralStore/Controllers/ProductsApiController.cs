using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using GeneralStore.Models;

namespace GeneralStore.Controllers
{
    [Produces("application/json")]
    [Route("api/Products")]
    public class ProductsApiController : Controller
    {
        private IApplicationDbContext _context;

        public ProductsApiController(IApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductsApi
        [HttpGet]
        public IEnumerable<Product> Get()
        {
           return _context.Products;
        }

        // GET: api/ProductsApi/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get([FromRoute] int id)
        {
            throw new NotImplementedException();
        }

        // PUT: api/ProductsApi/5
        [HttpPut("{id}")]
        public IActionResult PutProduct(int id, [FromBody] Product product)
        {
            throw new NotImplementedException();
        }

        // POST: api/ProductsApi
        [HttpPost]
        public IActionResult Post(Product product)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/ProductsApi/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }

    }
}