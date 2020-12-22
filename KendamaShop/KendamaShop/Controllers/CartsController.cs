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

        private ICollection<Product> ProductList = new List<Product>();

        // GET: Carts
        public ActionResult Index()
        {
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

        [NonAction]
        public void AddToProductList(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
                return;
            ProductList.Add(product);
        }

        [NonAction]
        public ICollection<Product> GetProducts()
        {
            return ProductList;
        }
    }
}