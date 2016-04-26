using System.Linq;
using GeneralStore.Controllers.api;
using GeneralStore.Data;
using GeneralStore.Data.Models;
using NUnit.Framework;

namespace GeneralStore.Tests.Controllers.api
{
    [TestFixture]
    public class ProductControllerTests
    {
        [Test]
        public void Get_NoParameters_ReturnsAllProduct()
        {
            // Arrange
            var context = new GeneralStoreDbContext(Effort.DbConnectionFactory.CreateTransient());
            context.Products.Add(new Product {Name = "one"});
            context.SaveChanges();

            var controller = new ProductController(context);

            // Act
            var result = controller.Get();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(),Is.EqualTo(1));
            Assert.That(result.First().Name,Is.EqualTo("one"));

        }
    }
}