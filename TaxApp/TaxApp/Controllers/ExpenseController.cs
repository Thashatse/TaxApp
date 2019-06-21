using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaxApp.Controllers
{
    public class ExpenseController : Controller
    {
        // GET: Expense
        public ActionResult Expenses()
        {
            return View();
        }
    }
}