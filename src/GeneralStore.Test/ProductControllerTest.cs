using GeneralStore.Controllers;
using GeneralStore.Models;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GeneralStore.Test
{
    public class ProductControllerTest
    {
        [Fact]
        public void IndexReturnsAllProducts()
        {
            var data = new List<Product>
            {
                new Product { Name = "Box" },
                new Product { Name = "Cutting Board" },
                new Product { Name = "Hat" },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Product>>();
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockDbContext = new Mock<IApplicationDbContext>();
            mockDbContext.Setup(m => m.Products).Returns(mockDbSet.Object);

            var repo = new ProductRepository(mockDbContext.Object);
            var productController = new ProductController(repo);

            var result = productController.Index() as ViewResult;

            Assert.Equal(data.ToList(), result.ViewData.Model);
        }
    }
}
