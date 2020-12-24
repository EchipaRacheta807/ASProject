using KendamaShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KendamaShop.Controllers
{
    [Authorize(Roles = "User,Partner,Admin")]
    public class CartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private Array<Product> ProductList = new Array<Product>();

        // GET: Carts
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                System.Diagnostics.Debug.WriteLine("null branch");
                System.Diagnostics.Debug.WriteLine(ProductList);
                var products = ProductList;
                ViewBag.Products = products;
            }
            else
            {
                ViewBag.Products = new List<Product>();
            }
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }

        public ActionResult RemoveFromCart(int id)
        {

            return RedirectToAction("Index");
        }

        public ActionResult Buy()
        {

            return RedirectToAction("Index");
        }

        [HttpPut]
        public ActionResult AddToCart(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                TempData["message"] = "Inexistent Product!";
                return RedirectToAction("Index", "Products");
            }
            TempData["message"] = "Product added to cart!";
            ProductList.Add(product);
            return RedirectToAction("Show", "Products", new { id = id });
        }

        [NonAction]
        public ICollection<Product> GetProducts()
        {
            return ProductList;
        }
    }
}