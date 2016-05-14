using DevLunch.Controllers;
using DevLunch.Data;
using DevLunch.Data.Models;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DevLunch.Tests.Controllers
{
    [TestFixture]
    class LunchCandidateControllerTests
    {
        private DevLunchDbContext _context;

        [SetUp]
        public void StuffStartsHere()
        {
            _context = new DevLunchDbContext(Effort.DbConnectionFactory.CreateTransient());

            var restaurantList = new List<Restaurant>
            {
                new Restaurant {Name = "Joe's Tavern", Latitude = 55, Longitude = 65 },
                new Restaurant {Name = "Linda's", Latitude = 35, Longitude = 18 },
                new Restaurant {Name = "The Pine Box", Latitude = 55, Longitude = 75 },
                new Restaurant {Name = "Dick's", Latitude = 28, Longitude = 20 },
                new Restaurant {Name = "Plum Bistro", Latitude = 55, Longitude = 50 }
            };

            _context.Restaurants.AddRange(restaurantList);
            _context.SaveChanges();
        }

        [Test]
        public void Create_Get_ShowsTheLunchCandidateForm()
        {
            // Arrange
            var controller = new LunchCandidateController(_context);

            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            result.ShouldNotBeNull();
            result.Model.ShouldBeOfType<LunchCandidate>();
            result.ViewName.ShouldBe("Create");
        }
    }
}
