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
        // GET: Reviews
        public ActionResult Index()
        {
            return View();
        }
    }
}