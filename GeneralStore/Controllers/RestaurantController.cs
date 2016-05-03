using System.Linq;
using System.Web.Mvc;
using DevLunch.Data;

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

        public ViewResult Index(int Id)
        {
            var model = _context.Restaurants.Find(Id);
            return View(model);
        }

        public ViewResult Index()
        {
            var model = _context.Restaurants.ToList();
            return View(model);
        }
    }
}