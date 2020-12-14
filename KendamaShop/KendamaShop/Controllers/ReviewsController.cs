using KendamaShop.Models;
using Microsoft.AspNet.Identity;
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
        [Authorize(Roles = "User,Partner,Admin")]
        public ActionResult Edit(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(review);
            }
            else
            {
                TempData["message"] = "Insufficient rights to modify this review!";
                return RedirectToAction("Index", "Products");
            }
        }

        [HttpPut]
        [Authorize(Roles = "User,Partner,Admin")]
        public ActionResult Edit(int id, Review requestReview)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    Review review = db.Reviews.Find(id);

                    if (review.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
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
                        TempData["message"] = "Insufficient rights to modify this review!";
                        return RedirectToAction("Index", "Products");
                    }
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
        [Authorize(Roles = "User,Partner,Admin")]
        public ActionResult Delete(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Reviews.Remove(review);
                db.SaveChanges();
                TempData["message"] = "The review was deleted!";
                return Redirect("/Products/Show/" + review.ProductId.ToString());
            }
            else
            {
                TempData["message"] = "Insufficient rights to detele this review!";
                return RedirectToAction("Index", "Products");
            }
        }
    }
}