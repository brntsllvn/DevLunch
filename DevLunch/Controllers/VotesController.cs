using DevLunch.Data;
using DevLunch.Data.Models;
using System.Web.Mvc;

namespace DevLunch.Controllers
{
    public class VotesController : Controller
    {
        private DevLunchDbContext _context;

        public VotesController()
        {

        }

        public VotesController(DevLunchDbContext context)
        {
            _context = context;
        }
        
        // POST: Votes/Create
        [HttpPost]
        public ActionResult Create(Lunch lunch, Restaurant restaurant, int value)
        {
            var vote = new Vote
            {
                Lunch = lunch,
                Restaurant = restaurant,
                Value = value
            };

            _context.Votes.Add(vote);
            _context.SaveChanges();

            return RedirectToAction("Details", "Lunches", new { Id = lunch.Id });
        }

        // POST: Votes/Delete/5
        //[HttpPost]
        //public RedirectResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
