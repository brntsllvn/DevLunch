using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DevLunch.Data;
using DevLunch.Data.Models;
using DevLunch.ViewModels;
using System.Collections.Generic;

namespace DevLunch.Controllers
{
    public class LunchesController : Controller
    {
        private readonly DevLunchDbContext _context;

        public LunchesController() : this(new DevLunchDbContext())
        {

        }

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

            return View(_context.Lunches.Include(l => l.Restaurants).First(l => l.Id == id));
        }

        // GET: Lunches/Create
        public ActionResult Create()
        {
            var lunchViewModel = new LunchViewModel();

            var allRestaurants = _context.Restaurants.ToList();

            var checkBoxListItems = new List<CheckBoxListItem>();

            foreach (var restaurant in allRestaurants)
            {
                checkBoxListItems.Add(new CheckBoxListItem()
                {
                    ID = restaurant.Id,
                    Display = restaurant.Name,
                    IsChecked = false
                });
            }

            lunchViewModel.Restaurants = checkBoxListItems;

            return View("Create", lunchViewModel);
        }

        // POST: Lunches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LunchViewModel lunchViewModel)
        {
            var selectedRestaurants = lunchViewModel.Restaurants.Where(r => r.IsChecked).Select(r => r.ID).ToList();

            if (ModelState.IsValid)
            {
                var lunch = new Lunch()
                {
                    Host = lunchViewModel.Host,
                    MeetingTime = lunchViewModel.MeetingTime
                };

                foreach (var restaurantID in selectedRestaurants)
                {
                    var restaurant = _context.Restaurants.Find(restaurantID);
                    lunch.Restaurants.Add(restaurant);
                }

                _context.Lunches.Add(lunch);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("Create", lunchViewModel);
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

            var lunchViewModel = new LunchViewModel
            {
                Host = lunch.Host,
                MeetingTime = lunch.MeetingTime,
            };

            var lunchRestaurants = lunch.Restaurants;

            var allRestaurants = _context.Restaurants.ToList();
            var checkBoxListItems = new List<CheckBoxListItem>();

            foreach (var restaurant in allRestaurants)
            {
                checkBoxListItems.Add(new CheckBoxListItem
                {
                    ID = restaurant.Id,
                    Display = restaurant.Name,
                    IsChecked = lunchRestaurants.Where(r => r.Id == restaurant.Id).Any()
                });
            }

            lunchViewModel.Restaurants = checkBoxListItems;
            return View("Edit", lunchViewModel);
        }

        //POST: Lunches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, LunchViewModel lunchViewModel)
        {
            var lunch = _context
                .Lunches
                .Include(l => l.Restaurants)
                .First(l => l.Id == id);

            if (ModelState.IsValid)
            {
                lunch.Host = lunchViewModel.Host;
                lunch.MeetingTime = lunchViewModel.MeetingTime;

                _context.Entry(lunch).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Edit", lunchViewModel);
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
    }
}
