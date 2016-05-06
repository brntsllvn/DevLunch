using System;
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
            var Id = context.Restaurants.First().Id;
            var result = controller.Details(Id);

            // Assert
            var data = result.Model as Restaurant;
            data.ShouldNotBeNull();
            data.Id.ShouldBe(1);
            data.Name.ShouldBe("Brave Horse");
        }

        [Test]
        public void Detail_ReturnsNullException_WhenRecordDoesNotExist()
        {
            var context = new DevLunchDbContext(Effort.DbConnectionFactory.CreateTransient());
            var controller = new RestaurantController(context);

            // Act / Assert
            Should.Throw<NullReferenceException>(() => { controller.Details(999); });
        }

        [Test]
        public void Index_ReturnsAllRestaurants()
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

        [Test]
        public void Create_Get_CreatesDefaultAndShowsItInTheView()
        {
            // Arrange
            var context = new DevLunchDbContext(Effort.DbConnectionFactory.CreateTransient());
            var controller = new RestaurantController(context);

            // Act
            var results = controller.Create();

            // Assert
            results.ShouldNotBeNull();
            results.Model.ShouldBeOfType<Restaurant>();
        }

        [Test]
        public void Create_Post_CreatesNewRestaurantAndSavesToDb()
        {
            // Arrange
            var context = new DevLunchDbContext(Effort.DbConnectionFactory.CreateTransient());
            var controller = new RestaurantController(context);

            // Act
            var results = controller.Create(new Restaurant {Name = "Brent's Pub"});

            // Assert
            context.Restaurants.First(r=>r.Name == "Brent's Pub").ShouldNotBeNull();
            results.RouteValues["action"].ShouldBe("Index");
        }

        [Test]
        public void Edit_Get_ShowsRestaurantInTheView()
        {
            // Arrange
            var context = new DevLunchDbContext(Effort.DbConnectionFactory.CreateTransient());
            context.Restaurants.Add(new Restaurant { Name = "Brave Horse" });
            context.SaveChanges();
            var controller = new RestaurantController(context);

            // Act
            var Id = context.Restaurants.First().Id;
            var results = controller.Edit(Id);

            // Assert
            results.ShouldNotBeNull();
            results.Model.ShouldBeOfType<Restaurant>();
        }

        [Test]
        public void Edit_Post_EditsRestaurantAndSavesToDb()
        {
            // Arrange
            var context = new DevLunchDbContext(Effort.DbConnectionFactory.CreateTransient());
            context.Restaurants.Add(new Restaurant { Name = "Brave Horse" });
            context.SaveChanges();
            var controller = new RestaurantController(context);

            // Act
            var originalRestaurantId = context.Restaurants.First().Id;
            var edittedRestaurant = new Restaurant {Name = "Linda's"};
            var results = controller.Edit(originalRestaurantId, edittedRestaurant);

            // Assert
            context.Restaurants.First().Name.ShouldBe("Linda's");
            results.RouteValues["action"].ShouldBe("Index");
        }

        [Test]
        public void Delete_RemovesRestaurantFromDatabase()
        {
            // Arrange
            var context = new DevLunchDbContext(Effort.DbConnectionFactory.CreateTransient());
            context.Restaurants.Add(new Restaurant { Name = "Brave Horse" });
            context.SaveChanges();
            var controller = new RestaurantController(context);

            // Act
            var Id = context.Restaurants.First().Id;
            var results = controller.Delete(Id);

            // Assert
            context.Restaurants.ShouldBeEmpty();
            results.RouteValues["action"].ShouldBe("Index");
        }
    }
}
