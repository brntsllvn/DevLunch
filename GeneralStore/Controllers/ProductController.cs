using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeneralStore.Data.Models;

namespace GeneralStore.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ViewResult Index()
        {
            return View(new List<Product> { new Product() } );
        }
    }
}