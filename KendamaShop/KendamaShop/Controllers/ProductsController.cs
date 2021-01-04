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

        private int _perPage = 3;
        private static string sortingCriterium = "date_asc";

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include("Category").Include("User").Where(prod => prod.Accepted).OrderBy(prod => prod.Date);
            var search = "";

            if (Request.Params.Get("search") != null)
            {
                search = Request.Params.Get("search").Trim();

                // Search in product title and description
                List<int> productIds = db.Products.Where(
                    prod => prod.Title.Contains(search) || prod.Description.Contains(search)
                    ).Select(prod => prod.ProductId).ToList();

                // Search in reviews
                List<int> reviewIds = db.Reviews.Where(
                    rev => rev.Content.Contains(search)
                    ).Select(rev => rev.ProductId).ToList();

                List<int> mergedIds = productIds.Union(reviewIds).ToList();

                products = db.Products.Include("Category").Include("User").Where(prod => mergedIds.Contains(prod.ProductId)).OrderBy(prod => prod.Date);
            }

            // sort the list of products
            if (sortingCriterium == "price_asc")
            {
                products = products.OrderBy(prod => prod.Price);
            }
            else if (sortingCriterium == "price_desc")
            {
                products = products.OrderByDescending(prod => prod.Price);
            }
            else if (sortingCriterium == "rating_asc")
            {
                products = products.OrderBy(prod => prod.Rating);
            }
            else if (sortingCriterium == "rating_desc")
            {
                products = products.OrderByDescending(prod => prod.Rating);
            }
            else if (sortingCriterium == "date_asc")
            {
                products = products.OrderBy(prod => prod.Date);
            }
            else if (sortingCriterium == "date_desc")
            {
                products = products.OrderByDescending(prod => prod.Date);
            }

            var totalItems = products.Count();
            var currentPage = Convert.ToInt32(Request.Params.Get("page"));
            var offset = 0;

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * this._perPage;
            }

            var paginatedProducts = products.Skip(offset).Take(this._perPage);

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            ViewBag.total = totalItems;
            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.Products = paginatedProducts;
            ViewBag.SearchString = search;

            return View();
        }

        public ActionResult SetSortingCriterium(string sortParam)
        {
            sortingCriterium = sortParam;
            return RedirectToAction("Index");
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

        [Authorize(Roles = "Admin")]
        public ActionResult Decline(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            TempData["message"] = "The product was declined!";
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
            product.PartnerId = User.Identity.GetUserId();
            return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "Partner,Admin")]
        public ActionResult New(Product product)
        {
            product.Date = DateTime.Now;
            product.PartnerId = User.Identity.GetUserId();
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

            if (product.PartnerId == User.Identity.GetUserId() || User.IsInRole("Admin"))
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

                    if (product.PartnerId == User.Identity.GetUserId() || User.IsInRole("Admin"))
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
        [Authorize(Roles = "Partner, Admin")]
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            if (product.PartnerId == User.Identity.GetUserId() || User.IsInRole("Admin"))
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