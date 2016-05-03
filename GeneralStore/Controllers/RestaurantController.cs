using System.Collections.Generic;
using System.Web.Mvc;
using GeneralStore.Data.Models;

namespace GeneralStore.Controllers
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