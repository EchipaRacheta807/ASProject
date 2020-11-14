using KendamaShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KendamaShop.Controllers
{
    public class ProductsController : Controller
    {
        public Models.AppContext db = new Models.AppContext();

        // GET: Products
        public ActionResult Index()
        {
            var products = from prod in db.Products select prod;
            ViewBag.Products = products;

            return View();
        }

        // GET
        public ActionResult Show(int id)
        {
            var product = db.Products.Find(id);
            return View(product);
        }

        public ActionResult New()
        {
            Product product = new Product();


            return View(product);
        }

        [HttpPost]
        public ActionResult New(Product product)
        {
            try
            {
                db.Products.Add(product);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View(product);
            }
        }

        public ActionResult Edit(int id)
        {
            Product product = db.Products.Find(id);

            return View(product);
        }

        [HttpPut]
        public ActionResult Edit(int id, Product requestProd)
        {
            try
            {
                Product product = db.Products.Find(id);
                
                if (TryUpdateModel(product))
                {
                    // Make sure the edit form contains all model properties
                    product = requestProd;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                return View(requestProd);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}