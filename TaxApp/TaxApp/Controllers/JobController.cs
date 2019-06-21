using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaxApp.Controllers
{
    public class JobController : Controller
    {
        // GET: Job
        public ActionResult Jobs()
        {
            return View();
        }
    }
}