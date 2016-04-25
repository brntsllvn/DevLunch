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
        public void Get_NoArguments_ReturnsAll()
        {
            // Arrange
            var data = new Product { Name = "Carrot", Description = "A Root" };

            var dbContext = new FakeApplicationDbContext();
            dbContext.Products.Add(data);
            dbContext.SaveChanges();

            var controller = new ProductsApiController(dbContext);

            // Act
            var result = controller.Get().ToList();

            // Assert
            Assert.NotNull(result);

            var resultCount = result.Count();
            Assert.Equal(1,resultCount);

            var product = result.First();
            Assert.Equal(data.Name,product.Name);
        }

    }
}