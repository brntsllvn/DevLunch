using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DevLunch.Data;
using DevLunch.Data.Models;

namespace DevLunch.Controllers
{
    [Authorize]
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

        public ActionResult Index()
        {
            var model = _context.Restaurants.ToList();
            return View(model);
        }

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
        public ActionResult Edit(int id, [Bind(Include = "Id, Name, Longitude, Latitude")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(restaurant).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Details", restaurant);
        }

        public ActionResult Delete(int? Id)
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

            return View("Delete", restaurant);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int Id)
        {
            var restaurant = _context.Restaurants.Find(Id);
            _context.Restaurants.Remove(restaurant);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}