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

        [HttpPost]
        public ActionResult New(Review review)
        {
            review.Date = DateTime.Now;
            try
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                TempData["message"] = "The reviews was added!";
                return Redirect("/Products/Show/" + review.ProductId.ToString());
            }
            catch (Exception e)
            {
                return Redirect("/Products/Show/" + review.ProductId.ToString());
            }
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
                        review = requestReview;
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
            TempData["message"] = "The review was edited!";
            return Redirect("/Products/Show/" + review.ProductId.ToString());
        }
    }
}