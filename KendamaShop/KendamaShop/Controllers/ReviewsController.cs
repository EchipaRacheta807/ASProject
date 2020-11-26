using KendamaShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KendamaShop.Controllers
{
    public class ReviewsController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reviews
        public ActionResult Index()
        {
            return View();
        }

        // GET
        public ActionResult Edit(int id)
        {
            Review review = db.Reviews.Find(id);
            return View(review);
        }

        [HttpPut]
        public ActionResult Edit(int id, Review requestReview)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    Review review = db.Reviews.Find(id);

                    if (TryUpdateModel(review))
                    {
                        // Make sure the edit form contains all model properties
                        review.Content = requestReview.Content;
                        review.Stars = requestReview.Stars;
                        db.SaveChanges();
                        TempData["message"] = "The review was edited!";
                    }
                    return Redirect("/Products/Show/" + review.ProductId.ToString());
                }
                else
                {
                    return View(requestReview);
                }
            }
            catch (Exception e)
            {
                return View(requestReview);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
            db.SaveChanges();
            TempData["message"] = "The review was deleted!";
            return Redirect("/Products/Show/" + review.ProductId.ToString());
        }
    }
}