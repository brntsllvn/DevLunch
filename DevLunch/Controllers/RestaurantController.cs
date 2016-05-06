using System;
using System.Linq;
using System.Net;
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
        public ActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var restaurant = _context.Restaurants.Find(Id);

            if (restaurant == null)
            {
                return new HttpNotFoundResult();
            }
            return View(restaurant);
        }

        [HttpGet]
        public ActionResult Index()
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
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name, Longitude, Latitude")]Restaurant restaurant)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Restaurants.Add(restaurant);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
            }
            return View(restaurant);
        }

        [HttpGet]
        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var restaurant = _context.Restaurants.Find(Id);

            if (restaurant == null)
            {
                return new HttpNotFoundResult();
            }
            return View(restaurant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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