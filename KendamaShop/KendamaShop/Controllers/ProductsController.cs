using KendamaShop.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KendamaShop.Controllers
{
    public class ProductsController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include("Category").Include("User");
            ViewBag.Products = products;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }

        // GET
        public ActionResult Show(int id)
        {
            var product = db.Products.Find(id);
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View(product);
        }

        [HttpPost]
        public ActionResult Show(Review review)
        {
            review.Date = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Reviews.Add(review);
                    db.SaveChanges();
                    TempData["message"] = "The review was added!";
                    return Redirect("/Products/Show/" + review.ProductId.ToString());
                }
                else
                {
                    Product prod = db.Products.Find(review.ProductId);
                    return View(prod);
                }

            }
            catch (Exception e)
            {
                Product prod = db.Products.Find(review.ProductId);
                return View(prod);
            }
        }

        public ActionResult New()
        {
            Product product = new Product();
            product.Categ = GetAllCategories();
            product.UserId = User.Identity.GetUserId();
            return View(product);
        }

        [HttpPost]
        public ActionResult New(Product product)
        {
            product.Date = DateTime.Now;
            product.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    TempData["message"] = "The product was added to the database";
                    return RedirectToAction("Index");
                }
                else
                {
                    product.Categ = GetAllCategories();
                    return View(product);
                }                
            }
            catch (Exception e)
            {
                product.Categ = GetAllCategories();
                return View(product);
            }
        }

        public ActionResult Edit(int id)
        {
            Product product = db.Products.Find(id);
            product.Categ = GetAllCategories();
            return View(product);
        }

        [HttpPut]
        public ActionResult Edit(int id, Product requestProd)
        {
            requestProd.Categ = GetAllCategories();

            try
            {
                if (ModelState.IsValid)
                {
                    Product product = db.Products.Find(id);

                    if (TryUpdateModel(product))
                    {
                        // Make sure the edit form contains all model properties
                        product.Title = requestProd.Title;
                        product.Description = requestProd.Description;
                        product.Price = requestProd.Price;
                        product.Rating = requestProd.Rating;
                        product.Categ = requestProd.Categ;
                        db.SaveChanges();
                        TempData["message"] = "The product info was modified!";
                    }
                    return Redirect("/Products/Show/" + product.ProductId);
                }
                else
                {
                    return View(requestProd);
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
            TempData["message"] = "The product was deleted!";
            return RedirectToAction("Index");
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();

            // extragem toate categoriile din baza de date
            var categories = from cat in db.Categories
                             select cat;

            // iteram prin categorii
            foreach (var category in categories)
            {
                // adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName.ToString()
                });
            }
            /*
            foreach (var category in categories)
            {
                var listItem = new SelectListItem();
                listItem.Value = category.CategoryId.ToString();
                listItem.Text = category.CategoryName.ToString();

                selectList.Add(listItem);
            }*/

            // returnam lista de categorii
            return selectList;
        }
    }
}