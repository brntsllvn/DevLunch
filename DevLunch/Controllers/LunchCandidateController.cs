using System.Web.Mvc;
using DevLunch.Data;
using DevLunch.Data.Models;
using System.Linq;

namespace DevLunch.Controllers
{
    public class LunchCandidateController : Controller
    {
        private DevLunchDbContext _context;

        public LunchCandidateController(DevLunchDbContext context)
        {
            _context = context;
        }

        public ActionResult Create()
        {
            var model = new LunchCandidate
            {
                Restaurants = _context.Restaurants.ToList()
            };

            return View("Create", model);
        }
    }
}