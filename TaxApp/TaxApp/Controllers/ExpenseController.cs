using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaxApp.Controllers
{
    public class ExpenseController : Controller
    {
        IDBHandler handler = new DBHandler();
        HttpCookie cookie;
        Functions function = new Functions();

        public void getCookie()
        {
            try
            {
                //check if the user is loged in
                cookie = Request.Cookies["TaxAppUserID"];

                if (cookie != null)
                {
                    //show the nav tabs menue only for customers
                    if (cookie["ID"] != null || cookie["ID"] != "")
                    {
                        Model.Profile checkProfile = new Model.Profile();

                        checkProfile.ProfileID = int.Parse(cookie["ID"].ToString());
                        checkProfile.EmailAddress = "";
                        checkProfile.Username = "";

                        if (handler.getProfile(checkProfile) == null)
                        {
                            Response.Redirect("/Landing/Welcome");
                        }
                    }
                    else
                    {
                        Response.Redirect("/Landing/Welcome");
                    }
                }
                else
                {
                    Response.Redirect("/Landing/Welcome");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in welcome method of LandingControles");
                Redirect("/Shared/Error");
            }
        }

        // GET: Expense
        public ActionResult Expenses()
        {
            getCookie();
            return View();
        }

        #region New General Expense
        // GET: Landing/NewProfile
        public ActionResult NewGeneralExpense()
        {
            try { 
            getCookie();
            List<Model.ExpenseCategory> cats = handler.getExpenseCatagories();
            ViewBag.CategoryList = new SelectList(cats, "CategoryID", "Name");
            return View();
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loading new general expense");
                return Redirect("/Expense/Expenses");
            }
        }

        // POST: Landing/NewProfile
        [HttpPost]
        public ActionResult NewGeneralExpense(FormCollection collection)
        {
            try
            {
                getCookie();
                List<Model.ExpenseCategory> cats = handler.getExpenseCatagories();
                ViewBag.CategoryList = new SelectList(cats, "CategoryID", "Name");

                Model.SP_GetGeneralExpense_Result newExpense = new Model.SP_GetGeneralExpense_Result();

                newExpense.CategoryID = int.Parse(Request.Form["CategoryList"].ToString());
                newExpense.Name = Request.Form["Name"].ToString();
                newExpense.Description = Request.Form["Description"].ToString();
                newExpense.ProfileID = int.Parse(cookie["ID"].ToString());
                newExpense.Date = DateTime.Parse(Request.Form["Date"].ToString());
                newExpense.Amount = Decimal.Parse(Request.Form["Amount"].ToString());
                newExpense.Repeat = bool.Parse(Request.Form["Repeat"].ToString().Split(',')[0]);
                //newExpense.Invoice_ReceiptCopy = DBNull.Value;

                bool result = handler.newGeneralExpense(newExpense);

                if (result == true)
                {
                    return Redirect("/Expense/Expenses");
                }
                else
                {
                    return RedirectToAction("../Shared/Error");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in new general expense of expense controler");
                return View();
            }
        }
        #endregion

        #region New Job Expense
        // GET: Landing/NewProfile
        public ActionResult NewJobExpense()
        {
            try { 
            getCookie();
            List<Model.ExpenseCategory> cats = handler.getExpenseCatagories();
            ViewBag.CategoryList = new SelectList(cats, "CategoryID", "Name");
            return View();
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loading new Job Expense");
                return RedirectToAction("../Shared/Error");
            }
        }

        // POST: Landing/NewProfile
        [HttpPost]
        public ActionResult NewJobExpense(FormCollection collection, string ID)
        {
            try
            {
                getCookie();
                List<Model.ExpenseCategory> cats = handler.getExpenseCatagories();
                ViewBag.CategoryList = new SelectList(cats, "CategoryID", "Name");

                Model.SP_GetJobExpense_Result newExpense = new Model.SP_GetJobExpense_Result();

                newExpense.CategoryID = int.Parse(Request.Form["CategoryList"].ToString());
                newExpense.Name = Request.Form["Name"].ToString();
                newExpense.Description = Request.Form["Description"].ToString();
                newExpense.JobID = int.Parse(ID);
                newExpense.Date = DateTime.Parse(Request.Form["Date"].ToString());
                newExpense.Amount = Convert.ToDecimal(double.Parse(Request.Form["Amount"].ToString()));
                //newExpense.Invoice_ReceiptCopy = DBNull.Value;

                bool result = handler.newJobExpense(newExpense);

                if (result == true)
                {
                    return Redirect("/Job/Job?ID="+ID);
                }
                else
                {
                    return RedirectToAction("../Shared/Error");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in new general expense of expense controler");
                return View();
            }
        }
        #endregion

        #region ExpenseCatagories
        public ActionResult ExpenseCatagories()
        {
            try
            {
                getCookie();
            List<Model.ExpenseCategory> cats = handler.getExpenseCatagories();
            ViewBag.CategoryList = new SelectList(cats, "CategoryID", "Name");
            return View(cats);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loading expense catagories");
                return Redirect("/Expense/Expenses");
            }
        }
        #endregion

        #region Job Expense View

        public ActionResult JobExpenses(string ID)
        {
            try
            {
                getCookie();
                Model.Job getJob = new Model.Job();
                getJob.JobID = int.Parse(ID);
                List<Model.SP_GetJobExpense_Result> JobExpenses = handler.getJobExpenses(getJob);
                return View(JobExpenses);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding job expense List");
                return Redirect("/job/job?ID="+ID);
            }
        }
        public ActionResult JobExpense(string ID)
        {
            try
            {
                getCookie();
                Model.Expense getExpense = new Model.Expense();
                getExpense.ExpenseID = int.Parse(ID);
                Model.SP_GetJobExpense_Result JobExpense = handler.getJobExpense(getExpense);
                return View(JobExpense);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding job expense Details");
                return Redirect("/Expense/Expenses?ID=" + ID);
            }
        }
        #endregion
    }
}