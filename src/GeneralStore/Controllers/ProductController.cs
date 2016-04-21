using GeneralStore.Models;
using Microsoft.AspNet.Mvc;

namespace GeneralStore.Controllers
{
    public class ProductController : Controller
    {
        private ProductRepository _repo;

        public ProductController(ProductRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var model = _repo.GetAllProducts();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}
