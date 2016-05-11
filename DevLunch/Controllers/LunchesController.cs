using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DevLunch.Data;
using DevLunch.Data.Models;

namespace DevLunch.Controllers
{
    public class LunchesController : Controller
    {
        private DevLunchDbContext _context = new DevLunchDbContext();

        public LunchesController() : this(new DevLunchDbContext())
        {

        }

        public LunchesController(DevLunchDbContext context)
        {
            _context = context;
        }


        // GET: Lunches
        public ActionResult Index()
        {
            return View(_context.Lunches.ToList());
        }

        // GET: Lunches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lunch lunch = _context.Lunches.Find(id);
            if (lunch == null)
            {
                return HttpNotFound();
            }
            return View(lunch);
        }

        // GET: Lunches/Create
        public ActionResult Create()
        {
            return View(new Lunch());
        }

        // POST: Lunches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Host,MeetingTime,Restaurant")] Lunch lunch)
        {
            if (ModelState.IsValid)
            {
                _context.Lunches.Add(lunch);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lunch);
        }

        // GET: Lunches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lunch lunch = _context.Lunches.Find(id);
            if (lunch == null)
            {
                return HttpNotFound();
            }
            return View(lunch);
        }

        // POST: Lunches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, [Bind(Include = "Id,Host,MeetingTime")] Lunch lunch)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(lunch).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lunch);
        }

        // GET: Lunches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lunch lunch = _context.Lunches.Find(id);
            if (lunch == null)
            {
                return HttpNotFound();
            }
            return View(lunch);
        }

        // POST: Lunches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lunch lunch = _context.Lunches.Find(id);
            _context.Lunches.Remove(lunch);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
