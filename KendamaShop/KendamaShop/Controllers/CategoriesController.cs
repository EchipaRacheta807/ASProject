using KendamaShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KendamaShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Category
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            var categories = from category in db.Categories
                             orderby category.CategoryName
                             select category;
            ViewBag.Categories = categories;
            return View();
        }

        // GET
        public ActionResult Show(int id)
        {
            Category category = db.Categories.Find(id);
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View(category);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(Category cat)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Categories.Add(cat);
                    db.SaveChanges();
                    TempData["message"] = "New category was added!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(cat);
                }
            }
            catch (Exception e)
            {
                return View(cat);
            }
        }

        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }

        [HttpPut]
        public ActionResult Edit(int id, Category requestCategory)
        {
            try
            {
                Category category = db.Categories.Find(id);

                if (TryUpdateModel(category))
                {
                    category.CategoryName = requestCategory.CategoryName;
                    db.SaveChanges();
                    TempData["message"] = "The category was modified!";
                    return RedirectToAction("Index");
                }

                return View(requestCategory);
            }
            catch (Exception e)
            {
                return View(requestCategory);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            TempData["message"] = "Category was deleted!";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}