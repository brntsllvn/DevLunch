using System.Collections.Generic;
using DevLunch.Controllers;
using DevLunch.Data.Models;
using NUnit.Framework;
using Shouldly;

namespace DevLunch.Tests.Controllers
{
    [TestFixture]
    public class RestaurantControllerTests
    {
        [Test]
        public void Index_NoParameters_ReturnsAllProducts()
        {
            // Arrange
            var controller = new RestaurantController();

            // Act
            var result = controller.Index();

            // Assert
            result.Model.ShouldNotBeNull();
            var data = result.Model as IEnumerable<Restaurant>;
            data.ShouldNotBeEmpty();
        }
    }
}
