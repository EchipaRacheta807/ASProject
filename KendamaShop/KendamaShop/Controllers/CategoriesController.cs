using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KendamaShop.Controllers
{
    public class CategoriesController : Controller
    {
        public Models.AppContext db = new Models.AppContext();

        // GET: Categories
        public ActionResult Index()
        {
            return View();
        }
    }
}