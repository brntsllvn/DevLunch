using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DevLunch.Controllers;
using DevLunch.Data;
using DevLunch.Data.Models;
using DevLunch.Tests.Helpers;
using DevLunch.ViewModels;
using NUnit.Framework;
using Shouldly;

namespace DevLunch.Tests.Controllers
{
    [TestFixture]
    class VoteActionTests
    {
        private DevLunchDbContext _context;
        private LunchesController _controller;
        private Lunch _lunch;
        private int _lunchId;
        private int _restaurantId1;
        private int _restaurantId2;
        private int _restaurantId3;

        [SetUp]
        public void StuffStartsHere()
        {
            _context = new DevLunchDbContext(Effort.DbConnectionFactory.CreateTransient());

            _controller = new LunchesController(_context);
            _controller.WithAuthenticatedUser("Brent", "ImBrent");

            _lunch = new Lunch()
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

            _context.Lunches.Add(_lunch);
            _context.SaveChanges();

            _lunchId = _lunch.Id;
            _restaurantId1 = _lunch.Restaurants.First().Id;
            _restaurantId2 = _lunch.Restaurants.FirstOrDefault(r => r.Name == "The Pine Box").Id;
            _restaurantId3 = _lunch.Restaurants.Last().Id;
        }

        [Test]
        public void Upvote_Post_CreatesNewRecordAndSavesToDb()
        {
            // Act
            var result = _controller.Upvote(_lunch.Id, _lunch.Restaurants.First().Id) as JsonResult;

            // Assert
            var voteViewModel = result.Data as VoteViewModel;
            voteViewModel.ShouldNotBeNull();

            voteViewModel.ShouldSatisfyAllConditions(
                () => voteViewModel.OldLunchRestaurantId.ShouldBeNull(),
                () => voteViewModel.OldLunchRestaurantVoteTotal.ShouldBe(0),
                () => voteViewModel.NewLunchRestaurantId.ShouldBe(1),
                () => voteViewModel.NewLunchRestaurantVoteTotal.ShouldBe(1)
                );

            var vote = _context.Votes.FirstOrDefault();
            vote.ShouldNotBeNull();
            vote.Value.ShouldBe(1);
        }

        [Test]
        public void Upvote_PostWithSameUser_OnlyCreatedOneVoteWhenCalledMultipleTimes()
        {
            // Act
            var result1 = _controller.Upvote(_lunchId, _restaurantId1);
            var result2 = _controller.Upvote(_lunchId, _restaurantId1) as JsonResult;

            // Assert
            var voteViewModel = result2.Data as VoteViewModel;
            voteViewModel.ShouldNotBeNull();

            voteViewModel.ShouldSatisfyAllConditions(
                () => voteViewModel.OldLunchRestaurantId.ShouldBeNull(),
                () => voteViewModel.OldLunchRestaurantVoteTotal.ShouldBe(0),
                () => voteViewModel.NewLunchRestaurantId.ShouldBe(1),
                () => voteViewModel.NewLunchRestaurantVoteTotal.ShouldBe(1)
                );
            var numberOfVotes = _context.Votes.Count();
            numberOfVotes.ShouldBe(1);

            var voteValue = _context.Votes
                .Where(v => v.Restaurant.Id == _restaurantId1)
                .Where(v => v.Lunch.Id == _lunchId)
                .Sum(v => v.Value);

            voteValue.ShouldBe(1);
        }

        [Test]
        public void Downvote_PostWithSameUserDifferentRestaurant_PreviousDownVoteRemovedAndNewDownvoteApplied()
        {
            // Act
            var result1 = _controller.Downvote(_lunchId, _restaurantId1);
            var result2 = _controller.Downvote(_lunchId, _restaurantId2) as JsonResult;

            // Assert
            var voteViewModel = result2.Data as VoteViewModel;
            voteViewModel.ShouldNotBeNull();

            voteViewModel.ShouldSatisfyAllConditions(
                () => voteViewModel.OldLunchRestaurantId.ShouldBe(1),
                () => voteViewModel.OldLunchRestaurantVoteTotal.ShouldBe(0),
                () => voteViewModel.NewLunchRestaurantId.ShouldBe(2),
                () => voteViewModel.NewLunchRestaurantVoteTotal.ShouldBe(-2)
                );

            var numberOfVotes = _context.Votes.Count();
            numberOfVotes.ShouldBe(1);

            var vote = _context.Votes.First();
            vote.ShouldSatisfyAllConditions(
                () => vote.Restaurant.Id.ShouldBe(_restaurantId2),
                () => vote.Lunch.Id.ShouldBe(_lunchId),
                () => vote.UserName.ShouldBe("Brent"),
                () => vote.VoteType.ShouldBe(VoteType.Downvote),
                () => vote.Value.ShouldBe(-2)
                );
        }

        [Test]
        public void Downvote_PostWithSameUserDifferentRestaurant_RecalculatesTotalsForEachRestaurant()
        {
            // Act
            var result1 = _controller.Upvote(_lunchId, _restaurantId1);
            var result2 = _controller.Downvote(_lunchId, _restaurantId2);
            var result3 = _controller.Downvote(_lunchId, _restaurantId3) as JsonResult;

            // Assert
            var voteViewModel = result3.Data as VoteViewModel;
            voteViewModel.ShouldNotBeNull();

            voteViewModel.ShouldSatisfyAllConditions(
                () => voteViewModel.OldLunchRestaurantId.ShouldBe(2),
                () => voteViewModel.OldLunchRestaurantVoteTotal.ShouldBe(0),
                () => voteViewModel.NewLunchRestaurantId.ShouldBe(3),
                () => voteViewModel.NewLunchRestaurantVoteTotal.ShouldBe(-2)
                );

            var numberOfVotes = _context.Votes.Count();
            numberOfVotes.ShouldBe(2);

            AssertLunchRestaurantVoteTotal(_lunchId, _restaurantId1, 1);
            AssertLunchRestaurantVoteTotal(_lunchId, _restaurantId2, 0);
            AssertLunchRestaurantVoteTotal(_lunchId, _restaurantId3, -2);
        }

        [Test]
        public void Downvote_PostWithSameUserDifferentRestaurant_DownvoteOriginallyUpvotedRestaurant()
        {
            var result1 = _controller.Upvote(_lunchId, _restaurantId1);
            var result2 = _controller.Downvote(_lunchId, _restaurantId2);
            var result3 = _controller.Downvote(_lunchId, _restaurantId3);
            var result4 = _controller.Downvote(_lunchId, _restaurantId1) as JsonResult;

            var voteViewModel = result4.Data as VoteViewModel;
            voteViewModel.ShouldNotBeNull();

            voteViewModel.ShouldSatisfyAllConditions(
                () => voteViewModel.OldLunchRestaurantId.ShouldBe(3),
                () => voteViewModel.OldLunchRestaurantVoteTotal.ShouldBe(0),
                () => voteViewModel.NewLunchRestaurantId.ShouldBe(1),
                () => voteViewModel.NewLunchRestaurantVoteTotal.ShouldBe(-2)
                );

            var numberOfVotes = _context.Votes.Count();
            numberOfVotes.ShouldBe(1);

            AssertLunchRestaurantVoteTotal(_lunchId, _restaurantId1, -2);
            AssertLunchRestaurantVoteTotal(_lunchId, _restaurantId2, 0);
            AssertLunchRestaurantVoteTotal(_lunchId, _restaurantId3, -2);
        }

        [Test]
        public void Downvote_PostWithSameUserDifferentRestaurant_DownvoteOriginallyUpvotedRestaurantThenUpvote()
        {
            // Act
            var result1 = _controller.Upvote(_lunchId, _restaurantId1);
            var result2 = _controller.Downvote(_lunchId, _restaurantId2);
            var result3 = _controller.Downvote(_lunchId, _restaurantId3);
            var result4 = _controller.Downvote(_lunchId, _restaurantId1);
            var result5 = _controller.Upvote(_lunchId, _restaurantId1) as JsonResult;

            // Assert
            var voteViewModel = result5.Data as VoteViewModel;
            voteViewModel.ShouldNotBeNull();

            voteViewModel.ShouldSatisfyAllConditions(
                () => voteViewModel.OldLunchRestaurantId.ShouldBeNull(),
                () => voteViewModel.OldLunchRestaurantVoteTotal.ShouldBe(0),
                () => voteViewModel.NewLunchRestaurantId.ShouldBe(1),
                () => voteViewModel.NewLunchRestaurantVoteTotal.ShouldBe(1)
                );

            var numberOfVotes = _context.Votes.Count();
            numberOfVotes.ShouldBe(1);

            AssertLunchRestaurantVoteTotal(_lunchId, _restaurantId1, 1);
            AssertLunchRestaurantVoteTotal(_lunchId, _restaurantId2, 0);
            AssertLunchRestaurantVoteTotal(_lunchId, _restaurantId3, -2);
        }

        [Test]
        public void Downvote_PostWithSameUserDifferentRestaurant_DownvoteOriginallyUpvotedRestaurantThenUpvoteThenDownvoteAgain()
        {
            // Act
            var result1 = _controller.Upvote(_lunchId, _restaurantId1);
            var result2 = _controller.Downvote(_lunchId, _restaurantId2);
            var result3 = _controller.Downvote(_lunchId, _restaurantId3);
            var result4 = _controller.Downvote(_lunchId, _restaurantId1);
            var result5 = _controller.Upvote(_lunchId, _restaurantId1);
            var result6 = _controller.Downvote(_lunchId, _restaurantId1);
            var result7 = _controller.Upvote(_lunchId, _restaurantId2);
            var result8 = _controller.Upvote(_lunchId, _restaurantId3) as JsonResult;

            // Assert
            var voteViewModel = result8.Data as VoteViewModel;
            voteViewModel.ShouldNotBeNull();

            voteViewModel.ShouldSatisfyAllConditions(
                () => voteViewModel.OldLunchRestaurantId.ShouldBe(1),
                () => voteViewModel.OldLunchRestaurantVoteTotal.ShouldBe(-2),
                () => voteViewModel.NewLunchRestaurantId.ShouldBe(3),
                () => voteViewModel.NewLunchRestaurantVoteTotal.ShouldBe(1)
                );

            var numberOfVotes = _context.Votes.Count();
            numberOfVotes.ShouldBe(1);

            AssertLunchRestaurantVoteTotal(_lunchId, _restaurantId1, -2);
            AssertLunchRestaurantVoteTotal(_lunchId, _restaurantId2, 1);
            AssertLunchRestaurantVoteTotal(_lunchId, _restaurantId3, 1);
        }

        [Test]
        public void Upvote_PostWithDifferntUser_CreatedOneVoteWhenCalledMultipleTimes()
        {
            var existingVote = new Vote
            {
                Restaurant = _context.Restaurants.Find(_restaurantId1),
                Lunch = _context.Lunches.Find(_lunchId),
                Value = 1,
                UserName = "Someone Else"
            };
            _context.Votes.Add(existingVote);
            _context.SaveChanges();

            // Act
            var result1 = _controller.Upvote(_lunchId, _restaurantId1);
            var result2 = _controller.Upvote(_lunchId, _restaurantId1);

            // Assert
            var numberOfVotes = _context.Votes.Count();
            numberOfVotes.ShouldBe(2);

            var voteValue = _context.Votes
                .Where(v => v.Restaurant.Id == _restaurantId1)
                .Where(v => v.Lunch.Id == _lunchId)
                .Where(v => v.UserName == "Brent")
                .Sum(v => v.Value);

            voteValue.ShouldBe(1);

            AssertLunchRestaurantVoteTotal(_lunchId, _restaurantId1, 2);
        }

        [Test]
        public void Vote_PostWithDifferntUser_CheckTotalVoteValue()
        {
            var existingVote1 = new Vote
            {
                Restaurant = _context.Restaurants.Find(_restaurantId1),
                Lunch = _context.Lunches.Find(_lunchId),
                Value = 1,
                UserName = "Paul"
            };
            var existingVote2 = new Vote
            {
                Restaurant = _context.Restaurants.Find(_restaurantId3),
                Lunch = _context.Lunches.Find(_lunchId),
                Value = -2,
                UserName = "James"
            };
            _context.Votes.Add(existingVote1);
            _context.Votes.Add(existingVote2);
            _context.SaveChanges();

            // Act
            var result1 = _controller.Upvote(_lunchId, _restaurantId1);
            var result2 = _controller.Downvote(_lunchId, _restaurantId3);

            // Assert
            var numberOfVotes = _context.Votes.Count();
            numberOfVotes.ShouldBe(4);

            AssertLunchRestaurantVoteTotal(_lunchId, _restaurantId1, 2);
            AssertLunchRestaurantVoteTotal(_lunchId, _restaurantId3, -4);
        }

        [Test]
        public void Downvote_Post_CreatesNewRecordAndSavesToDb()
        {
            // Act
            var result = _controller.Downvote(_lunch.Id, _lunch.Restaurants.First().Id);

            // Assert
            var vote = _context.Votes.First();
            vote.ShouldNotBeNull();
            vote.Value.ShouldBe(-2);
        }

        [Test]
        public void Vote_Throws_WhenLunchCannotBeFound()
        {
            // Act
            var result = _controller.Downvote(999, _restaurantId1);

            // Assert
            result.ShouldBeOfType<HttpStatusCodeResult>();
            var typedResult = (HttpStatusCodeResult)result;
            typedResult.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
        }

        [Test]
        public void Vote_Throws_WhenRestaurantCannotBeFound()
        {
            // Act
            var result = _controller.Downvote(_lunchId, 999);

            // Assert
            result.ShouldBeOfType<HttpStatusCodeResult>();
            var typedResult = (HttpStatusCodeResult)result;
            typedResult.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
        }

        private void AssertLunchRestaurantVoteTotal(int lunchId, int restaurantId, int expectedVoteValue)
        {
            var existingRestaurant3Votes = _context.Votes
                .Where(v => v.Lunch.Id == lunchId)
                .Any(v => v.Restaurant.Id == restaurantId);

            if (existingRestaurant3Votes)
            {
                _context.Votes.Where(v => v.Lunch.Id == lunchId)
                    .Where(v => v.Restaurant.Id == restaurantId)
                    .Sum(v => v.Value)
                    .ShouldBe(expectedVoteValue);
            }
        }
    }
}
