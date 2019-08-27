using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaxApp.Controllers
{
    public class SharedController : Controller
    {
        public ActionResult Error(string Err)
        {
            ViewBag.Details = Err;
            return View();
        }
        public ActionResult Help()
        {
            return View();
        }
    }
}
