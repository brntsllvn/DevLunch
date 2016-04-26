using GeneralStore.Controllers.api;
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
            var controller = new ProductController();

            // Act
            var result = controller.Get();

            // Assert
            Assert.NotNull(result);

        }
    }
}