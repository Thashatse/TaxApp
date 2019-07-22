using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaxApp.Controllers
{
    public class TaxController : Controller
    {
        // GET: Tax
        public ActionResult TaxCenter()
        {
            return View();
        }
    }
}