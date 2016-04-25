using System.Collections.Generic;
using System.Linq;
using GeneralStore.Controllers;
using GeneralStore.Models;
using GeneralStore.Test.Fakes;
using Xunit;

namespace GeneralStore.Test
{
    public class ProductApiControllerTests
    {
        [Fact]
        public void Get_NoArguments_ReturnsEmptyList_WhenDbEmpty()
        {
            // Arrange
            var emptyList = new List<Product>();
            var dbContext = new FakeApplicationDbContext();
            var controller = new ProductsApiController(dbContext);

            // Act
            var result = controller.Get().ToList();

            // Assert
            Assert.NotNull(result);

            var resultCount = result.Count();
            Assert.Equal(0, resultCount);

            Assert.Equal(emptyList, result);
        }

        [Fact]
        public void Get_NoArguments_ReturnsAll()
        {
            // Arrange
            var data = new List<Product>
            {
                new Product { Name = "Carrot", Description = "A Root" },
                new Product { Name = "Hat", Description = "For keeping head warm" },
                new Product { Name = "Dog", Description = "Friendly companion" }
            };

            var dbContext = new FakeApplicationDbContext();
            dbContext.Products.AddRange(data);
            dbContext.SaveChanges();

            var controller = new ProductsApiController(dbContext);

            // Act
            var result = controller.Get().ToList();

            // Assert
            Assert.NotNull(result);

            var resultCount = result.Count();
            Assert.Equal(3, resultCount);

            Assert.Equal(data, result);
        }
    }
}