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

        public ViewResult Detail(int Id)
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
    }
}