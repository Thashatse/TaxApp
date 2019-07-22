using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaxApp.Controllers
{
    public class VatController : Controller
    {
        // GET: VAT
        public ActionResult VatCenter()
        {
            return View();
        }
    }
}