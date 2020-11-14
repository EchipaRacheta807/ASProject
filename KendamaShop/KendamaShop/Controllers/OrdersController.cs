using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KendamaShop.Controllers
{
    public class OrdersController : Controller
    {
        public Models.AppContext db = new Models.AppContext();

        // GET: Orders
        public ActionResult Index()
        {
            return View();
        }
    }
}