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
        public Models.AppContext db = new Models.AppContext();

        [HttpPost]
        public ActionResult New(Review review)
        {
            review.Date = DateTime.Now;
            try
            {
                db.Reviews.Add(review);
                db.SaveChanges();

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
        public ActionResult Edit(int id, Review requestProd)
        {
            try
            {
                Review review = db.Reviews.Find(id);

                if (TryUpdateModel(review))
                {
                    // Make sure the edit form contains all model properties
                    review = requestProd;
                    db.SaveChanges();

                    return Redirect("/Products/Show/" + review.ProductId.ToString());
                }
                else
                {
                    return Redirect("/Products/Show/" + review.ProductId.ToString());
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
            db.SaveChanges();

            return Redirect("/Products/Show/" + review.ProductId.ToString());
        }
    }
}