using System.Collections.Generic;
using GeneralStore.Controllers;
using GeneralStore.Data.Models;
using NUnit.Framework;
using Shouldly;

namespace GeneralStore.Tests.Controllers
{
    [TestFixture]
    public class ProductControllerTests
    {
        [Test]
        public void Index_NoParameters_ReturnsAllProducts()
        {
            // Arrange
            var controller = new ProductController();

            // Act
            var result = controller.Index();

            // Assert
            result.Model.ShouldNotBeNull();
            var data = result.Model as IEnumerable<Product>;
            data.ShouldNotBeEmpty();
        }
    }
}
