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
            var orders = db.Orders.Include("OrderProducts").Where(order => order.UserId == currentUser);
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
                //var products = db.Orders.Include("Products").Include("Category").Include("User").Where(o => o.OrderId == id).SelectMany(o => o.Products);
                var products = db.OrderProducts.Include("Product").Include("Category").Include("User").Where(op => op.OrderId == id).Select(op => op.Product);
                var orderPCount = db.OrderProducts.Where(op => op.OrderId == id);
                IDictionary<int, int> PCount = new Dictionary<int, int>();
                foreach (var op in orderPCount)
                {
                    PCount.Add(op.ProductId, op.Quantity);
                }
                ViewBag.Order = order;
                ViewBag.Products = products;
                ViewBag.ProductCount = PCount;
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
            db.Orders.Add(order);
            //order.Products = GetProducts();
            //
            //order.ProductCount = ProductCount;
            foreach(KeyValuePair<int, int> entry in ProductCount)
            {
                OrderProducts op = new OrderProducts(order.OrderId, entry.Key, entry.Value);
                db.OrderProducts.Add(op);
            }
            //
            ProductCount.Clear();
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