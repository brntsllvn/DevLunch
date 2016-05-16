using DevLunch.Controllers;
using DevLunch.Data;
using DevLunch.Data.Models;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DevLunch.Tests.Controllers
{
    [TestFixture]
    public class VotesControllerTests
    {
        private DevLunchDbContext _context;

        [SetUp]
        public void StuffStartsHere()
        {
            _context = new DevLunchDbContext(Effort.DbConnectionFactory.CreateTransient());
        }

        [Test]
        public void Upvote_Post_CreatesNewRecordAndSavesToDb()
        {
            // Arrange
            var controller = new VotesController(_context);

            var lunch = new Lunch()
            {
                Host = "Brent",
                MeetingTime = new DateTime(1985, 6, 6),
                Restaurants = new List<Restaurant>()
                {
                    new Restaurant { Name = "Linda's", Latitude = 55, Longitude = 60 },
                    new Restaurant { Name = "The Pine Box", Latitude = 55, Longitude = 60 },
                    new Restaurant { Name = "Sizzler", Latitude = 55, Longitude = 60 }
                }
            };

            _context.Lunches.Add(lunch);
            _context.SaveChanges();

            // Act
            var result = controller.Upvote(lunch.Id, lunch.Restaurants.First().Id);

            // Assert
            var vote = _context.Votes.First();
            vote.ShouldNotBeNull();
            vote.Value.ShouldBe(1);
        }

        [Test]
        public void Downvote_Post_CreatesNewRecordAndSavesToDb()
        {
            // Arrange
            var controller = new VotesController(_context);

            var lunch = new Lunch()
            {
                Host = "Brent",
                MeetingTime = new DateTime(1985, 6, 6),
                Restaurants = new List<Restaurant>()
                {
                    new Restaurant { Name = "Linda's", Latitude = 55, Longitude = 60 },
                    new Restaurant { Name = "The Pine Box", Latitude = 55, Longitude = 60 },
                    new Restaurant { Name = "Sizzler", Latitude = 55, Longitude = 60 }
                }
            };

            _context.Lunches.Add(lunch);
            _context.SaveChanges();

            // Act
            var result = controller.Downvote(lunch.Id, lunch.Restaurants.First().Id);

            // Assert
            var vote = _context.Votes.First();
            vote.ShouldNotBeNull();
            vote.Value.ShouldBe(-2);
        }

        [Test]
        public void Vote_Throws_WhenLunchCannotBeFound()
        {
            // Arrange
            var controller = new VotesController(_context);
            var restaurant = new Restaurant {Name = "Brent's"};
            _context.Restaurants.Add(restaurant);
            _context.SaveChanges();

            // Act
            var result = controller.Downvote(999, restaurant.Id);

            // Assert
            result.ShouldBeOfType<HttpStatusCodeResult>();
            var typedResult = (HttpStatusCodeResult) result;
            typedResult.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
        }

        [Test]
        public void Vote_Throws_WhenRestaurantCannotBeFound()
        {
            // Arrange
            var controller = new VotesController(_context);
            var lunch = new Lunch {Host = "Josh"};
            _context.Lunches.Add(lunch);
            _context.SaveChanges();

            // Act
            var result = controller.Downvote(lunch.Id, 999);

            // Assert
            result.ShouldBeOfType<HttpStatusCodeResult>();
            var typedResult = (HttpStatusCodeResult) result;
            typedResult.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
        }

        //[Test]
        //public void Delete_Post_RemovesRestaurantFromDb()
        //{
        //    // Arrange
        //    _context.Restaurants.Add(new Restaurant { Name = "Brave Horse" });
        //    _context.SaveChanges();
        //    var controller = new RestaurantController(_context);

        //    var restaurantId = _context.Restaurants.First().Id;
        //    var restaurantDeleteGetResult = controller.Delete(restaurantId) as ViewResult;

        //    var restaurantToDelete = restaurantDeleteGetResult.Model as Restaurant;

        //    // Act
        //    var result = controller.DeleteConfirmed(restaurantToDelete.Id) as RedirectToRouteResult;

        //    // Assert
        //    _context.Restaurants.Count().ShouldBe(0);
        //    result.RouteValues["action"].ShouldBe("Index");
        //}
    }
}
