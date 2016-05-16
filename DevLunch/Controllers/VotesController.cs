using System.Net;
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
        public ActionResult Upvote(int lunchId, int restaurantId)
        {
            return CreateVote(lunchId, restaurantId,1);
 
        }

        [HttpPost]
        public ActionResult Downvote(int lunchId, int restaurantId)
        {
           return CreateVote(lunchId, restaurantId, -2);

        }

        private ActionResult CreateVote(int lunchId, int restaurantId, int value)
        {
            var lunch = _context.Lunches.Find(lunchId);
            if (lunch == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, $"Specified lunch '{lunchId}' does not exist");

            var restaurant = _context.Restaurants.Find(restaurantId);
            if (restaurant == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, $"Specified restaurant '{restaurantId}' does not exist");

            var vote = new Vote
            {
                Lunch = lunch,
                Restaurant = restaurant,
                Value = value
            };

            _context.Votes.Add(vote);
            _context.SaveChanges();

            return RedirectToAction("Details", "Lunches", new { Id = lunchId });
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
