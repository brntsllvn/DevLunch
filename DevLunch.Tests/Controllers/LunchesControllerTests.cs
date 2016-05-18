using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DevLunch.Data;
using DevLunch.Data.Models;
using NUnit.Framework;
using DevLunch.Controllers;
using DevLunch.ViewModels;
using Shouldly;

namespace DevLunch.Tests.Controllers
{
    [TestFixture]
    public class LunchesControllerTests
    {
        private DevLunchDbContext _context;

        [SetUp]
        public void StuffStartsHere()
        {
            _context = new DevLunchDbContext(Effort.DbConnectionFactory.CreateTransient());
        }

        [Test]
        public void Details_ReturnsOneLunchWithRestaurantsWithVotes()
        {
            // Arrange
            _context.Lunches.Add(new Lunch
            {
                Host = "Brent",
                Restaurants = new List<Restaurant>
                {
                    new Restaurant
                    {
                    Name = "Lunchbox Labs",
                    Longitude = 55,
                    Latitude = 42
                    }
                },
                MeetingTime = new DateTime(1999, 12, 31)
            });

            _context.SaveChanges();

            _context.Votes.AddRange(
                new List<Vote>
                {
                    new Vote { Lunch = _context.Lunches.First(), Restaurant = _context.Restaurants.First(), Value = 2},
                    new Vote { Lunch = _context.Lunches.First(), Restaurant = _context.Restaurants.First(), Value = 3},
                    new Vote { Lunch = _context.Lunches.First(), Restaurant = _context.Restaurants.First(), Value = -8}
                }
                );

            _context.SaveChanges();

            var controller = new LunchesController(_context);

            // Act
            var id = _context.Lunches.First().Id;
            var result = controller.Details(id) as ViewResult;

            // Assert
            var data = result.Model as LunchDetailsViewModel;
            data.ShouldNotBeNull();
            data.Id.ShouldBe(1);
            data.Restaurants.Count.ShouldBe(1);
            data.Votes.Count.ShouldBe(3);
            data.Votes.Sum(v => v.Value).ShouldBe(-3);
            data.Host.ShouldBe("Brent");
        }

        [Test]
        public void Details_WithoutIdThrows()
        {
            // Arrange
            var controller = new LunchesController(_context);

            // Act
            var result = controller.Details(null) as HttpStatusCodeResult;

            // Assert
            result.ShouldBeOfType<HttpStatusCodeResult>();
            result.StatusCode.ShouldBe(400);
        }

        [Test]
        public void Details_ReturnsHttpNotFound_WhenRecordDoesNotExist()
        {
            var controller = new LunchesController(_context);

            // Act 
            var result = controller.Details(999) as HttpStatusCodeResult;

            // Assert
            result.ShouldBeOfType<HttpNotFoundResult>();
            result.StatusCode.ShouldBe(404);
        }

        [Test]
        public void Index_ReturnsAllLunchesInDescendingOrder()
        {
            // Arrange
            var Lunches = new List<Lunch>
            {
                new Lunch
                {
                     Host = "Brent",
                     Restaurants = new List<Restaurant>
                     {
                         new Restaurant
                         {
                         Name = "Lunchbox Labs",
                         Latitude = 55,
                         Longitude = 99
                         }
                     },
                     MeetingTime = new DateTime(1999,12,31)
                },
                new Lunch
                {
                     Host = "Drew",
                     Restaurants = new List<Restaurant>
                     {
                         new Restaurant
                         {
                         Name = "The Pine Box",
                         Latitude = 55,
                         Longitude = 99
                         }
                     },
                     MeetingTime = new DateTime(2005,12,31)
                },
            };

            _context.Lunches.AddRange(Lunches);
            _context.SaveChanges();

            var controller = new LunchesController(_context);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            var data = result.Model as IEnumerable<Lunch>;
            data.ShouldNotBeNull();
            data.Count().ShouldBe(2);
            data.First().Host.ShouldBe("Drew");
            data.Last().Host.ShouldBe("Brent");
        }

        [Test]
        public void Create_Get_CreatesDefaultAndShowsItInTheView()
        {
            // Arrange
            var controller = new LunchesController(_context);

            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            result.ShouldNotBeNull();
            result.Model.ShouldBeOfType<LunchCreateEditViewModel>();
        }

        [Test]
        public void Create_Post_CreatesNewLunchAndSavesToDb()
        {
            // Arrange
            var controller = new LunchesController(_context);

            // Act
            var result = controller.Create(new LunchCreateEditViewModel 
            {
                Host = "Brent",
                MeetingTime = new DateTime(1999, 12, 31)
            }) as RedirectToRouteResult;

            // Assert
            _context.Lunches.First().ShouldNotBeNull();
            result.RouteValues["action"].ShouldBe("Index");
        }

        [Test]
        public void Edit_Get_ShowsRestaurantInTheView()
        {
            // Arrange
            _context.Lunches.Add(new Lunch
            {
                Host = "Brent",
                Restaurants = new List<Restaurant>
                     {
                         new Restaurant
                         {
                         Name = "Lunchbox Labs",
                         Latitude = 55,
                         Longitude = 99
                         }
                     },
                MeetingTime = new DateTime(1999, 12, 31)
            });
            _context.SaveChanges();
            var controller = new LunchesController(_context);

            // Act
            var Id = _context.Lunches.First().Id;
            var result = controller.Edit(Id) as ViewResult;

            // Assert
            result.ShouldNotBeNull();
            result.Model.ShouldBeOfType<LunchCreateEditViewModel>();
        }

        [Test]
        public void Edit_Get_ThrowsIfRecordIdIsNull()
        {
            var controller = new LunchesController(_context);

            // Act
            var result = controller.Edit(null) as HttpStatusCodeResult;

            // Assert
            result.ShouldBeOfType<HttpStatusCodeResult>();
            result.StatusCode.ShouldBe(400);
        }

        [Test]
        public void Edit_Get_ThrowsNotFoundIfRecordNotInDb()
        {
            var _context = new DevLunchDbContext(Effort.DbConnectionFactory.CreateTransient());
            var controller = new LunchesController(_context);

            // Act 
            var result = controller.Details(999) as HttpStatusCodeResult;

            // Assert
            result.ShouldBeOfType<HttpNotFoundResult>();
            result.StatusCode.ShouldBe(404);
        }

        [Test]
        public void Edit_Post_EditsRecordAndSavesToDb()
        {
            // Arrange
            _context.Lunches.Add(new Lunch
            {
                Host = "Brent",
                Restaurants = new List<Restaurant>
                     {
                         new Restaurant
                         {
                         Name = "Lunchbox Labs",
                         Latitude = 55,
                         Longitude = 99
                         }
                     },
                MeetingTime = new DateTime(1999, 12, 31)
            });
            _context.SaveChanges();
            var controller = new LunchesController(_context);

            var editableRecordId = _context.Restaurants.First().Id;
            var recordEditGetResult = controller.Edit(editableRecordId) as ViewResult;

            var recordToEdit = recordEditGetResult.Model as LunchCreateEditViewModel;
            recordToEdit.Host = "Bob";

            // Act
            var result = controller.Edit(editableRecordId, recordToEdit) as RedirectToRouteResult;

            // Assert
            _context.Lunches.First().Host.ShouldBe("Bob");
            result.RouteValues["action"].ShouldBe("Index");
        }

        [Test]
        public void Delete_Get_ReturnsViewWithRecord()
        {
            // Arrange
            _context.Lunches.Add(new Lunch
            {
                Host = "Brent",
                Restaurants = new List<Restaurant>
                     {
                         new Restaurant
                         {
                         Name = "Lunchbox Labs",
                         Latitude = 55,
                         Longitude = 99
                         }
                     },
                MeetingTime = new DateTime(1999, 12, 31)
            });
            _context.SaveChanges();
            var controller = new LunchesController(_context);

            // Act
            var Id = _context.Lunches.First().Id;
            var result = controller.Delete(Id) as ViewResult;

            // Assert
            result.Model.ShouldNotBeNull();
            result.Model.ShouldBeOfType<Lunch>();
        }

        [Test]
        public void Delete_ThrowsWhenIdIsNull()
        {
            // Arrange
            var controller = new LunchesController(_context);

            // Act
            var result = controller.Delete(null) as HttpStatusCodeResult;

            // Assert
            result.ShouldBeOfType<HttpStatusCodeResult>();
            result.StatusCode.ShouldBe(400);
        }

        [Test]
        public void Delete_ThrowsWhenRecordCannotBeFound()
        {
            // Arrange
            var controller = new LunchesController(_context);

            // Act
            var result = controller.Delete(999) as HttpStatusCodeResult;

            // Assert
            result.ShouldBeOfType<HttpNotFoundResult>();
            result.StatusCode.ShouldBe(404);
        }

        [Test]
        public void Delete_Post_RemovesRecordFromDb()
        {
            // Arrange
            _context.Lunches.Add(new Lunch
            {
                Host = "Brent",
                Restaurants = new List<Restaurant>
                     {
                         new Restaurant
                         {
                         Name = "Lunchbox Labs",
                         Latitude = 55,
                         Longitude = 99
                         }
                     },
                MeetingTime = new DateTime(1999, 12, 31)
            });
            _context.SaveChanges();
            var controller = new LunchesController(_context);

            var recordId = _context.Lunches.First().Id;
            var recordDeleteGetResult = controller.Delete(recordId) as ViewResult;

            var recordToDelete = recordDeleteGetResult.Model as Lunch;

            // Act
            var result = controller.DeleteConfirmed(recordToDelete.Id) as RedirectToRouteResult;

            // Assert
            _context.Lunches.Count().ShouldBe(0);
            result.RouteValues["action"].ShouldBe("Index");
        }

        [Test]
        public void Upvote_Post_CreatesNewRecordAndSavesToDb()
        {
            // Arrange
            var controller = new LunchesController(_context);

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
            var controller = new LunchesController(_context);

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
            var controller = new LunchesController(_context);
            var restaurant = new Restaurant { Name = "Brent's" };
            _context.Restaurants.Add(restaurant);
            _context.SaveChanges();

            // Act
            var result = controller.Downvote(999, restaurant.Id);

            // Assert
            result.ShouldBeOfType<HttpStatusCodeResult>();
            var typedResult = (HttpStatusCodeResult)result;
            typedResult.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
        }

        [Test]
        public void Vote_Throws_WhenRestaurantCannotBeFound()
        {
            // Arrange
            var controller = new LunchesController(_context);
            var lunch = new Lunch { Host = "Josh" };
            _context.Lunches.Add(lunch);
            _context.SaveChanges();

            // Act
            var result = controller.Downvote(lunch.Id, 999);

            // Assert
            result.ShouldBeOfType<HttpStatusCodeResult>();
            var typedResult = (HttpStatusCodeResult)result;
            typedResult.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
        }

    }
}
