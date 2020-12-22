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
            var products = db.Products.Include("Category").Include("User").Where(prod => prod.Accepted);

            ViewBag.Products = products;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Pending()
        {
            var products = db.Products.Include("Category").Include("User").Where(prod => prod.Accepted == false);
            if (TempData.ContainsKey("message")){
                ViewBag.Message = TempData["message"];
            }

            ViewBag.Products = products;

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Accept(int id)
        {
            var product = db.Products.Find(id);
            if (product.Accepted)
            {
                TempData["message"] = "Tried to perform invalid Accepting command";
                return RedirectToAction("Pending");
            }
            if (TryUpdateModel(product))
            {
                product.Accepted = true;
                db.SaveChanges();
                TempData["message"] = "The product was accepted successfully";
                return RedirectToAction("Pending");
            }

            TempData["message"] = "Could not accept product, try again later";
            return RedirectToAction("Pending");
        }

        // GET
        public ActionResult Show(int id)
        {
            var product = db.Products.Find(id);
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            SetAccessRights();
            return View(product);
        }

        [HttpPost]
        public ActionResult Show(Review review)
        {
            review.Date = DateTime.Now;
            review.UserId = User.Identity.GetUserId();
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
                    SetAccessRights();
                    return View(prod);
                }

            }
            catch (Exception e)
            {
                Product prod = db.Products.Find(review.ProductId);
                SetAccessRights();
                return View(prod);
            }
        }

        [Authorize(Roles = "Partner,Admin")]
        public ActionResult New()
        {
            Product product = new Product();
            product.Categ = GetAllCategories();
            product.UserId = User.Identity.GetUserId();
            return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "Partner,Admin")]
        public ActionResult New(Product product)
        {
            product.Date = DateTime.Now;
            product.UserId = User.Identity.GetUserId();
            product.Accepted = false;
            try
            {
                if (ModelState.IsValid)
                {
                    if (User.IsInRole("Admin"))
                    {
                        product.Accepted = true;
                        TempData["message"] = "The product was added to the database!";
                    }
                    else
                    {
                        TempData["message"] = "The request to add the product has been sent!";
                    }
                    db.Products.Add(product);
                    db.SaveChanges();
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

        [Authorize(Roles = "Partner,Admin")]
        public ActionResult Edit(int id)
        {
            Product product = db.Products.Find(id);
            product.Categ = GetAllCategories();

            if (product.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(product);
            }

            else
            {
                TempData["message"] = "You don't have sufficient rights to modify the product!";
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Partner,Admin")]
        public ActionResult Edit(int id, Product requestProd)
        {
            requestProd.Categ = GetAllCategories();

            try
            {
                if (ModelState.IsValid)
                {
                    Product product = db.Products.Find(id);

                    if (product.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
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
                        TempData["message"] = "Insufficient rights to modify the product!";
                        return RedirectToAction("Index");   
                    }
                    
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
        [Authorize(Roles = "Partner,Admin")]
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            if (product.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Products.Remove(product);
                db.SaveChanges();
                TempData["message"] = "The product was deleted!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Insufficient rights to delete the product!";
                return RedirectToAction("Index");
            }
            
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

        private void SetAccessRights()
        {
            ViewBag.isPartner = User.IsInRole("Partner");
            ViewBag.isAdmin = User.IsInRole("Admin");
            ViewBag.currentUser = User.Identity.GetUserId();
        }
    }
}