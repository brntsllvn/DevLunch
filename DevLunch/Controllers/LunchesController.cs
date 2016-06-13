using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DevLunch.Data;
using DevLunch.Data.Models;
using DevLunch.ViewModels;

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
            var lunchData = _context
                .Lunches
                .Include(lunch => lunch.Restaurants)
                .ToList();

            var lunches = lunchData
                .Select(lunch=>MapLunch(lunch,new List<Vote>()))
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

            var lunchDetailsViewModel = MapLunch(lunch, votes);

            return View(lunchDetailsViewModel);
        }

        private static LunchDetailsViewModel MapLunch(Lunch lunch, List<Vote> votes)
        {
            var lunchDetailsViewModel = new LunchDetailsViewModel
            {
                Id = lunch.Id,
                Host = lunch.Host,
                MeetingTime = lunch.MeetingTime,
                Restaurants = lunch.Restaurants,
                Votes = votes
            };
            return lunchDetailsViewModel;
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
            })
            .OrderBy(r => r.Display)
            .ToList();

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

            var votesForLunch = _context.Votes.Where(v => v.Lunch.Id == lunch.Id)
                .ToList();
            _context.Votes.RemoveRange(votesForLunch);

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

            var hasExistingUpvote = existingUpvoteOnSameRestaurant != null;
            var hasExistingDownvote = existingDownvoteOnSameRestaurant != null;
            var hasDownvoteOnOtherRestaurant = existingDownvoteOnDifferentRestaurant != null;

            if (hasExistingUpvote)
            {
                AddDownvoteToSameRestaurant(lunchId, type, existingUpvoteOnSameRestaurant, voteValue, existingDownvoteOnDifferentRestaurant);
            }
            else if (hasExistingDownvote)
            {
                AddUpvoteToSameRestaurant(type, existingDownvoteOnSameRestaurant, voteValue);
            }
            else if (hasDownvoteOnOtherRestaurant)
            {
                TransferDownvoteToOtherRestaurant(type, existingDownvoteOnDifferentRestaurant, restaurant);
            }
            else
            {
                AddUpvoteOrDownvoteToRestaurant(type, lunch, restaurant, userName, voteValue);
            }

            _context.SaveChanges();

            var voteViewModel = new VoteViewModel
            {
                NewLunchRestaurantId = restaurantId,
                NewLunchRestaurantVoteTotal = CalculateNewRestaurantVoteTotal(lunchId, restaurantId, true, voteValue),
                OldLunchRestaurantId = existingDownvoteRestaurantId,
                OldLunchRestaurantVoteTotal = CalculateNewRestaurantVoteTotal(lunchId, existingDownvoteRestaurantId, false, voteValue)
            };

            return Json(voteViewModel, JsonRequestBehavior.AllowGet);
        }

        private void AddDownvoteToSameRestaurant(int lunchId, VoteType type, Vote existingUpvoteOnSameRestaurant, int voteValue, Vote existingDownvoteOnDifferentRestaurant)
        {
            if (type == VoteType.Downvote)
            {
                existingUpvoteOnSameRestaurant.VoteType = VoteType.Downvote;
                existingUpvoteOnSameRestaurant.Value = voteValue;

                if (existingDownvoteOnDifferentRestaurant != null)
                {
                    RemoveExistingDownvote(lunchId);
                }
            }
        }

        private static void TransferDownvoteToOtherRestaurant(VoteType type, Vote existingDownvoteOnDifferentRestaurant,
            Restaurant restaurant)
        {
            if (type == VoteType.Downvote)
            {
                existingDownvoteOnDifferentRestaurant.Restaurant = restaurant;
            }
        }

        private static void AddUpvoteToSameRestaurant(VoteType type, Vote existingDownvoteOnSameRestaurant, int voteValue)
        {
            if (type == VoteType.Upvote)
            {
                existingDownvoteOnSameRestaurant.VoteType = VoteType.Upvote;
                existingDownvoteOnSameRestaurant.Value = voteValue;
            }
        }

        private void AddUpvoteOrDownvoteToRestaurant(VoteType type, Lunch lunch, Restaurant restaurant, string userName, int voteValue)
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

        private void RemoveExistingDownvote(int lunchId)
        {
            var existingDownvote = _context.Votes
                .Where(v => v.Lunch.Id == lunchId)
                .SingleOrDefault(v => v.VoteType == VoteType.Downvote);
            _context.Votes.Remove(existingDownvote);
        }

        private int CalculateNewRestaurantVoteTotal(int lunchId, int? restaurantId, bool newRestaurant, int voteValue)
        {
            if (restaurantId == null)
                return 0;

            // todo: ask Josh if there is a better way
            // make sure there are votes to sum over or the query breaks
            var existingVotesOnSameRestaurant = _context.Votes
                .Where(v => v.Lunch.Id == lunchId)
                .Any(v => v.Restaurant.Id == restaurantId);

            Int32 voteTotal;
            if (!existingVotesOnSameRestaurant)
            {
                if (newRestaurant)
                {
                    voteTotal = voteValue;
                }
                else
                {
                    voteTotal = 0;
                }
                
            }
            else
            {
                voteTotal = _context.Votes
                    .Where(v => v.Lunch.Id == lunchId)
                    .Where(v => v.Restaurant.Id == restaurantId)
                    .Sum(v => v.Value);
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
