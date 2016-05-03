using System.Collections.Generic;
using System.Linq;
using DevLunch.Controllers;
using DevLunch.Data;
using DevLunch.Data.Models;
using NUnit.Framework;
using Shouldly;

namespace DevLunch.Tests.Controllers
{
    [TestFixture]
    public class RestaurantControllerTests
    {
        [Test]
        public void Detail_ReturnsOneRestaurant()
        {
            // Arrange
            var context = new DevLunchDbContext(Effort.DbConnectionFactory.CreateTransient());
            context.Restaurants.Add(new Restaurant { Name = "Brave Horse" });
            context.SaveChanges();

            var controller = new RestaurantController(context);

            // Act
            var result = controller.Detail(context.Restaurants.First().Id);

            // Assert
            var data = result.Model;
            data.ShouldNotBeNull();
        }

        [Test]
        public void Index_NoParameters_ReturnsAllRestaurants()
        {
            // Arrange
            var context = new DevLunchDbContext(Effort.DbConnectionFactory.CreateTransient());
            context.Restaurants.Add(new Restaurant { Name = "Brave Horse" });
            context.Restaurants.Add(new Restaurant { Name = "Yard House" });
            context.SaveChanges();

            var controller = new RestaurantController(context);

            // Act
            var result = controller.Index();

            // Assert
            var data = result.Model as IEnumerable<Restaurant>;
            data.ShouldNotBeNull();
            data.Count().ShouldBe(2);
            data.First().Name.ShouldBe("Brave Horse");
            data.Last().Name.ShouldBe("Yard House");
        }
    }
}
