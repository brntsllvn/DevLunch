using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
        private DevLunchDbContext _context;

        [SetUp]
        public void StuffStartsHere()
        {
            _context = new DevLunchDbContext(Effort.DbConnectionFactory.CreateTransient());
        }

        [Test]
        public void Details_ReturnsOneRestaurant()
        {
            // Arrange
            _context.Restaurants.Add(new Restaurant { Name = "Brave Horse" });
            _context.SaveChanges();
            var controller = new RestaurantController(_context);

            // Act
            var Id = _context.Restaurants.First().Id;
            var result = controller.Details(Id) as ViewResult;

            // Assert
            var data = result.Model as Restaurant;
            data.ShouldNotBeNull();
            data.Id.ShouldBe(1);
            data.Name.ShouldBe("Brave Horse");
        }

        [Test]
        public void Details_WithoutIdThrows()
        {
            // Arrange
            var controller = new RestaurantController(_context);

            // Act
            var result = controller.Details(null) as HttpStatusCodeResult;

            // Assert
            result.ShouldBeOfType<HttpStatusCodeResult>();
            result.StatusCode.ShouldBe(400);
        }

        [Test]
        public void Detail_ReturnsNullException_WhenRecordDoesNotExist()
        {
            var controller = new RestaurantController(_context);

            // Act 
            var result = controller.Details(999) as HttpStatusCodeResult;

            // Assert
            result.ShouldBeOfType<HttpNotFoundResult>();
            result.StatusCode.ShouldBe(404);
        }

        [Test]
        public void Index_ReturnsAllRestaurants()
        {
            // Arrange
            _context.Restaurants.Add(new Restaurant { Name = "Brave Horse" });
            _context.Restaurants.Add(new Restaurant { Name = "Yard House" });
            _context.SaveChanges();

            var controller = new RestaurantController(_context);

            // Act
            var result = controller.Index() as ViewResult;

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
            var controller = new RestaurantController(_context);

            // Act
            var result = controller.Create();

            // Assert
            result.ShouldNotBeNull();
            result.Model.ShouldBeOfType<Restaurant>();
        }

        [Test]
        public void Create_Post_CreatesNewRestaurantAndSavesToDb()
        {
            // Arrange
            var controller = new RestaurantController(_context);

            // Act
            var result = controller.Create(new Restaurant {Name = "Brent's Pub"}) as RedirectToRouteResult;

            // Assert
            _context.Restaurants.First(r=>r.Name == "Brent's Pub").ShouldNotBeNull();
            result.RouteValues["action"].ShouldBe("Index");
        }

        [Test]
        public void Create_Post_DoesNotSaveToDBAndReturnsCreateViewIfNameIsTooShort()
        {
            // Arrange
            var controller = new RestaurantController(_context);

            // Act
            var restaurant = new Restaurant { Name = "B" };
            var result = controller.Create(restaurant) as ViewResult;

            // Assert
            result.Model.ShouldBe(restaurant);
        }

        [Test]
        public void Edit_Get_ShowsRestaurantInTheView()
        {
            // Arrange
            _context.Restaurants.Add(new Restaurant { Name = "Brave Horse" });
            _context.SaveChanges();
            var controller = new RestaurantController(_context);

            // Act
            var Id = _context.Restaurants.First().Id;
            var result = controller.Edit(Id) as ViewResult;

            // Assert
            result.ShouldNotBeNull();
            result.Model.ShouldBeOfType<Restaurant>();
        }

        [Test]
        public void Edit_Get_ThrowsIfRestaurantIdIsNull()
        {
            var controller = new RestaurantController(_context);

            // Act
            var result = controller.Edit(null) as HttpStatusCodeResult;

            // Assert
            result.ShouldBeOfType<HttpStatusCodeResult>();
            result.StatusCode.ShouldBe(400);
        }

        [Test]
        public void Edit_Get_ThrowsNotFoundIfRestaurantNotInDb()
        {
            var _context = new DevLunchDbContext(Effort.DbConnectionFactory.CreateTransient());
            var controller = new RestaurantController(_context);

            // Act 
            var result = controller.Details(999) as HttpStatusCodeResult;

            // Assert
            result.ShouldBeOfType<HttpNotFoundResult>();
            result.StatusCode.ShouldBe(404);
        }

        [Test]
        public void Edit_Post_EditsRestaurantAndSavesToDb()
        {
            // Arrange
            _context.Restaurants.Add(new Restaurant { Name = "Brave Horse" });
            _context.SaveChanges();
            var controller = new RestaurantController(_context);

            var editableRestaurantId = _context.Restaurants.First().Id;
            var restaurantEditGetResult = controller.Edit(editableRestaurantId) as ViewResult;

            var restaurantToEdit = restaurantEditGetResult.Model as Restaurant;
            restaurantToEdit.Name = "Linda's";
            restaurantToEdit.Longitude = -5;
            restaurantToEdit.Latitude = 5;

            // Act
            var result = controller.Edit(editableRestaurantId, restaurantToEdit) as RedirectToRouteResult;

            // Assert
            _context.Restaurants.First().Name.ShouldBe("Linda's");
            result.RouteValues["action"].ShouldBe("Index");
        }

        [Test]
        public void Delete_Get_ReturnsViewWithRestaurant()
        {
            // Arrange
            _context.Restaurants.Add(new Restaurant { Name = "Brave Horse" });
            _context.SaveChanges();
            var controller = new RestaurantController(_context);

            // Act
            var Id = _context.Restaurants.First().Id;
            var result = controller.Delete(Id) as ViewResult;

            // Assert
            result.Model.ShouldNotBeNull();
            result.Model.ShouldBeOfType<Restaurant>();
        }

        [Test]
        public void Delete_ThrowsWhenIdIsNull()
        {
            // Arrange
            var controller = new RestaurantController(_context);

            // Act
            var result = controller.Delete(null) as HttpStatusCodeResult;

            // Assert
            result.ShouldBeOfType<HttpStatusCodeResult>();
            result.StatusCode.ShouldBe(400);
        }

        [Test]
        public void Delete_ThrowsWhenRestaurantCannotBeFound()
        {
            // Arrange
            var controller = new RestaurantController(_context);

            // Act
            var result = controller.Delete(999) as HttpStatusCodeResult;

            // Assert
            result.ShouldBeOfType<HttpNotFoundResult>();
            result.StatusCode.ShouldBe(404);
        }

        [Test]
        public void Delete_Post_RemovesRestaurantFromDb()
        {
            // Arrange
            _context.Restaurants.Add(new Restaurant { Name = "Brave Horse" });
            _context.SaveChanges();
            var controller = new RestaurantController(_context);

            var restaurantId = _context.Restaurants.First().Id;
            var restaurantDeleteGetResult = controller.Delete(restaurantId) as ViewResult;

            var restaurantToDelete = restaurantDeleteGetResult.Model as Restaurant;

            // Act
            var result = controller.DeleteConfirmed(restaurantToDelete.Id) as RedirectToRouteResult;

            // Assert
            _context.Restaurants.Count().ShouldBe(0);
            result.RouteValues["action"].ShouldBe("Index");
        }
    }
}
