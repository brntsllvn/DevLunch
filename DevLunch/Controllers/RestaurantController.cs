using System;
using System.Linq;
using System.Web.Mvc;
using DevLunch.Data;
using DevLunch.Data.Models;

namespace DevLunch.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly DevLunchDbContext _context;
    
        public RestaurantController() : this(new DevLunchDbContext())
        {
            
        }

        public RestaurantController(DevLunchDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ViewResult Details(int Id)
        {
            if (_context.Restaurants.Find(Id) != null)
            {
                var model = _context.Restaurants.Find(Id);
                return View(model);
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        [HttpGet]
        public ViewResult Index()
        {
            var model = _context.Restaurants.ToList();
            return View(model);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View(new Restaurant());
        }

        [HttpPost]
        public RedirectToRouteResult Create(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Edit(int Id)
        {
            return View(_context.Restaurants.Find(Id));
        }

        [HttpPost]
        public RedirectToRouteResult Edit(int Id, Restaurant edittedRestaurant)
        {
            var originalRestaurant = _context.Restaurants.Find(Id);

            originalRestaurant.Name = edittedRestaurant.Name;
            originalRestaurant.Longitude = edittedRestaurant.Longitude;
            originalRestaurant.Latitude = edittedRestaurant.Latitude;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public RedirectToRouteResult Delete(int Id)
        {
            var restaurant = _context.Restaurants.Find(Id);
            _context.Restaurants.Remove(restaurant);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}