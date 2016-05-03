using System.Collections.Generic;
using System.Web.Mvc;
using DevLunch.Data.Models;

namespace DevLunch.Controllers
{
    public class RestaurantController : Controller
    {
        // GET: Product
        public ViewResult Index()
        {
            return View(new List<Restaurant> { new Restaurant() } );
        }
    }
}