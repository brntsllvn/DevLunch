using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DevLunch.Data;
using DevLunch.Data.Models;
using DevLunch.ViewModels;
using Microsoft.Ajax.Utilities;

namespace DevLunch.Controllers
{
    public class LunchesController : Controller
    {
        private readonly DevLunchDbContext _context;

        public LunchesController() : this(new DevLunchDbContext())
        { }

        public LunchesController(DevLunchDbContext context)
        {
            _context = context;
        }

        // GET: Lunches
        public ActionResult Index()
        {
            var lunches = _context
                .Lunches
                .Include(lunch => lunch.Restaurants)
                .OrderByDescending(lunch => lunch.MeetingTime)
                .ToList();

            return View(lunches);
        }

        // GET: Lunches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var lunch = _context.Lunches.Find(id);

            if (lunch == null)
                return new HttpNotFoundResult();

            lunch = _context.Lunches.Include(l => l.Restaurants).First(l => l.Id == id);

            var votes = _context.Votes.Where(v => v.Lunch.Id == id).ToList();

            var lunchDetailsViewModel = new LunchDetailsViewModel
            {
                Id = lunch.Id,
                Host = lunch.Host,
                MeetingTime = lunch.MeetingTime,
                Restaurants = lunch.Restaurants,
                Votes = votes
            };

            return View(lunchDetailsViewModel);
        }

        // GET: Lunches/Create
        public ActionResult Create()
        {
            // todo: validate only signed-in PPA user can create a lunch
            var lunchCreateEditViewModel = new LunchCreateEditViewModel();

            var allRestaurants = _context.Restaurants.ToList();

            var checkBoxListItems = allRestaurants.Select(restaurant => new CheckBoxListItem()
            {
                ID = restaurant.Id,
                Display = restaurant.Name,
                IsChecked = false
            }).ToList();

            lunchCreateEditViewModel.Restaurants = checkBoxListItems;

            return View("Create", lunchCreateEditViewModel);
        }

        // POST: Lunches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(LunchCreateEditViewModel lunchCreateEditViewModel)
        {

            var selectedRestaurants = lunchCreateEditViewModel.Restaurants.Where(r => r.IsChecked).Select(r => r.ID).ToList();

            if (ModelState.IsValid)
            {
                var lunch = new Lunch()
                {

                    Host = lunchCreateEditViewModel.Host,
                    MeetingTime = lunchCreateEditViewModel.MeetingTime
                };

                foreach (var restaurantId in selectedRestaurants)
                {
                    var restaurant = _context.Restaurants.Find(restaurantId);
                    lunch.Restaurants.Add(restaurant);
                }

                _context.Lunches.Add(lunch);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }


            return View("Create", lunchCreateEditViewModel);
        }

        // GET: Lunches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var lunch = _context.Lunches.Find(id);

            if (lunch == null)
                return new HttpNotFoundResult();

            lunch = _context
                    .Lunches
                    .Include(l => l.Restaurants)
                    .First(l => l.Id == id);


            var lunchCreateEditViewModel = new LunchCreateEditViewModel
            {
                Host = lunch.Host,
                MeetingTime = lunch.MeetingTime,
            };

            var lunchRestaurants = lunch.Restaurants;

            var allRestaurants = _context.Restaurants.ToList();
            var checkBoxListItems = allRestaurants.Select(restaurant => new CheckBoxListItem
            {
                ID = restaurant.Id,
                Display = restaurant.Name,
                IsChecked = lunchRestaurants.Any(r => r.Id == restaurant.Id)
            }).ToList();


            lunchCreateEditViewModel.Restaurants = checkBoxListItems;
            return View("Edit", lunchCreateEditViewModel);
        }

        //POST: Lunches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(int id, LunchCreateEditViewModel lunchCredteEditViewModel)
        {
            // todo: validate lunch-creator can edit a lunch
            var selectedRestaurants = lunchCredteEditViewModel.Restaurants.Where(r => r.IsChecked).Select(r => r.ID).ToList();

            var lunch = _context
                .Lunches
                .Include(l => l.Restaurants)
                .First(l => l.Id == id);

            if (ModelState.IsValid)
            {

                lunch.Host = lunchCredteEditViewModel.Host;
                lunch.MeetingTime = lunchCredteEditViewModel.MeetingTime;

                foreach (var restaurantId in selectedRestaurants)
                {
                    var restaurant = _context.Restaurants.Find(restaurantId);
                    lunch.Restaurants.Add(restaurant);
                }

                _context.Entry(lunch).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }


            return View("Edit", lunchCredteEditViewModel);
        }

        // GET: Lunches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lunch lunch = _context.Lunches.Find(id);
            if (lunch == null)
            {
                return new HttpNotFoundResult();
            }
            return View(lunch);
        }

        // POST: Lunches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lunch lunch = _context.Lunches.Find(id);
            _context.Lunches.Remove(lunch);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Upvote(int lunchId, int restaurantId)
        {
            return CreateVote(lunchId, restaurantId, VoteType.Upvote);
        }

        [HttpPost]
        public ActionResult Downvote(int lunchId, int restaurantId)
        {
            return CreateVote(lunchId, restaurantId, VoteType.Downvote);
        }

        private ActionResult CreateVote(int lunchId, int restaurantId, VoteType type)
        {
            var lunch = _context.Lunches.Find(lunchId);
            if (lunch == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, $"Specified lunch '{lunchId}' does not exist");

            var restaurant = _context.Restaurants.Find(restaurantId);
            if (restaurant == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, $"Specified restaurant '{restaurantId}' does not exist");

            if (User?.Identity == null)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            var userName = User.Identity.Name;

            var voteValue = GetVoteValue(type);

            var existingUpvoteOnSameRestaurant = _context.Votes
                .Where(v => v.Lunch.Id == lunchId)
                .Where(v=> v.Restaurant.Id == restaurantId)
                .Where(v => v.VoteType == VoteType.Upvote)
                .SingleOrDefault(v => v.UserName == userName);

            var existingDownvoteOnSameRestaurant = _context.Votes
                .Where(v => v.Lunch.Id == lunchId)
                .Where(v => v.Restaurant.Id == restaurantId)
                .Where(v=>v.VoteType == VoteType.Downvote)
                .SingleOrDefault(v => v.UserName == userName);

            var existingDownvoteOnDifferentRestaurant = _context.Votes
                .Include(v => v.Restaurant)
                .Where(v => v.Lunch.Id == lunchId)
                .Where(v => v.Restaurant.Id != restaurantId)
                .Where(v => v.VoteType == VoteType.Downvote)
                .SingleOrDefault(v => v.UserName == userName);

            var existingDownvoteRestaurantId = existingDownvoteOnDifferentRestaurant?.Restaurant.Id;

            if (existingUpvoteOnSameRestaurant != null)
            {
                if (type == VoteType.Downvote)
                {
                    existingUpvoteOnSameRestaurant.VoteType = VoteType.Downvote;
                    existingUpvoteOnSameRestaurant.Value = voteValue;

                    if (existingDownvoteOnDifferentRestaurant != null)
                    {
                        RemoveExistingDownvote();
                    }
                }
            }
            else if (existingDownvoteOnSameRestaurant != null)
            {
                if (type == VoteType.Upvote)
                {
                    existingDownvoteOnSameRestaurant.VoteType = VoteType.Upvote;
                    existingDownvoteOnSameRestaurant.Value = voteValue;
                }
            }
            else if (existingDownvoteOnDifferentRestaurant != null)
            {
                if (type == VoteType.Downvote)
                {
                    existingDownvoteOnDifferentRestaurant.Restaurant = restaurant;
                }
            }
            else
            {
                var newVote = new Vote
                {
                    Lunch = lunch,
                    Restaurant = restaurant,
                    UserName = userName,
                    Value = voteValue,
                    VoteType = type
                };
                _context.Votes.Add(newVote);
            }

            _context.SaveChanges();

            var voteViewModel = new VoteViewModel
            {
                newLunchRestaurantId = restaurantId,
                newLunchRestaurantVotetotal = CalculateVoteTotal(lunchId, restaurantId),
                oldLunchRestaurantId = existingDownvoteRestaurantId,
                oldLunchRestaurantVoteTotal = CalculateVoteTotal(lunchId, existingDownvoteRestaurantId)
            };

            return Json(voteViewModel, JsonRequestBehavior.AllowGet);
        }

        private void RemoveExistingDownvote()
        {
            var existingDownvote = _context.Votes.SingleOrDefault(v => v.VoteType == VoteType.Downvote);
            _context.Votes.Remove(existingDownvote);
        }

        private int CalculateVoteTotal(int lunchId, int? restaurantId)
        {
            if (restaurantId == null)
                return 0;

            var existingLunchRestaurantVotes = _context.Votes
                .Where(v => v.Lunch.Id == lunchId)
                .Any(v => v.Restaurant.Id == restaurantId);

            Int32 voteTotal;
            if (existingLunchRestaurantVotes)
            {
                voteTotal = _context.Votes
                    .Where(v => v.Lunch.Id == lunchId)
                    .Where(v => v.Restaurant.Id == restaurantId)
                    .Sum(v => v.Value);
            }
            else
            {
                voteTotal = 0;
            }
            return voteTotal;
        }

        private static int GetVoteValue(VoteType type)
        {
            switch (type)
            {
                case VoteType.Upvote:
                    return 1;
                case VoteType.Downvote:
                    return -2;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }
    }
}
