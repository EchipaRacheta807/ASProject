using KendamaShop.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KendamaShop.Controllers
{
    [Authorize(Roles = "User,Partner,Admin")]
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private static IDictionary<int, int> ProductCount = new Dictionary<int, int>();

        // GET: Orders
        public ActionResult Index()
        {
            ViewBag.currentUser = User.Identity.GetUserId();
            string currentUser = User.Identity.GetUserId();
            var orders = db.Orders.Include("Products").Where(order => order.UserId == currentUser);
            ViewBag.Orders = orders;
            return View();
        }

        // GET: Orders
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                System.Diagnostics.Debug.WriteLine("null branch");
                var products = db.Products.Include("Category").Include("User").Where(a => ProductCount.Keys.Contains(a.ProductId));
                

                ViewBag.Products = products;
                ViewBag.ToBuy = (ProductCount.Count() > 0);
                ViewBag.ProductCount = ProductCount;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("not null branch");
                var order = db.Orders.Find(id);
                var products = db.Orders.Include("Products").Include("Category").Include("User").Where(o => o.OrderId == id).SelectMany(o => o.Products);
                //var order = from order in db.OrderProducts
                //            where order.OrderId == id
                //            select order.ProductId;
                //List < Product > products = new List<Product>();
                //foreach(var product in order.Products)
                //{
                //    products.Add(product);
                //}
                //Product prod;
                //var products = from order in db.Orders
                //               where order.OrderId == id
                //               select order
                ViewBag.Order = order;
                ViewBag.Products = products;
                ViewBag.ProductCount = order.ProductCount;
                ViewBag.ToBuy = false;
            }
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }

        [HttpPut]
        public ActionResult RemoveOneFromCart(int id)
        {
            if (ProductCount.ContainsKey(id))
            {
                if (ProductCount[id] == 1)
                {
                    ProductCount.Remove(id);
                }
                else
                {
                    ProductCount[id]--;
                }
            }
            return RedirectToAction("Show");
        }

        [HttpPut]
        public ActionResult RemoveFromCart(int id)
        {
            if (ProductCount.ContainsKey(id))
            {
                ProductCount.Remove(id);
            }
            return RedirectToAction("Show");
        }

        [HttpPost]
        public ActionResult Buy()
        {
            if (ProductCount.Count == 0)
            {
                TempData["message"] = "There are no products in the cart!";
                return RedirectToAction("Index", "Products");
            }
            Order order = new Order();
            order.UserId = User.Identity.GetUserId();
            order.Date = DateTime.Now;
            order.Products = GetProducts();
            order.ProductCount = ProductCount;
            ProductCount.Clear();
            db.Orders.Add(order);
            db.SaveChanges();
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
            if (ProductCount.ContainsKey(id))
            {
                ProductCount[id]++;
            }
            else
            {
                ProductCount.Add(id, 1);
            }
            return RedirectToAction("Show", "Products", new { id = id });
        }

        [NonAction]
        public ICollection<Product> GetProducts()
        {
            var products = db.Products.Where(product => ProductCount.Keys.Contains(product.ProductId)).ToArray();
            return products;
        }
    }
}