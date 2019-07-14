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

            try
            {
                getCookie();

                Model.Profile getProfile = new Model.Profile();
                getProfile.ProfileID = int.Parse(cookie["ID"].ToString());

                List<Model.TravelLog> ProfileTravelLog = handler.getProfileTravelLog(getProfile);
                List<Model.SP_GetJobExpense_Result> ProfileJobExpenses = handler.getAllJobExpense(getProfile);
                List<Model.SP_GetGeneralExpense_Result> ProfileGeneralExpenses = handler.getGeneralExpenses(getProfile);

                Model.ExpenseViewModel viewModel = new Model.ExpenseViewModel();
                viewModel.GExpense = ProfileGeneralExpenses;
                viewModel.JExpense = ProfileJobExpenses;
                viewModel.TExpense = ProfileTravelLog;


                return View(viewModel);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Expese Page");
                return Redirect("Home/index");
            }
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

                Model.SP_GetJob_Result Job = handler.getJob(getJob);
                ViewBag.JobTitle = Job.JobTitle;
                ViewBag.JobID = Job.JobID;

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

                Model.Job getJob = new Model.Job();
                getJob.JobID = JobExpense.JobID;
                Model.SP_GetJob_Result Job = handler.getJob(getJob);
                ViewBag.JobTitle = Job.JobTitle;

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

        #region General Expense View
        public ActionResult GeneralExpense(string ID)
        {
            try
            {
                getCookie();
                Model.Expense getExpense = new Model.Expense();
                getExpense.ExpenseID = int.Parse(ID);
                Model.SP_GetGeneralExpense_Result GeneralExpense = handler.getGeneralExpense(getExpense);

                return View(GeneralExpense);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding job expense Details");
                return Redirect("/Expense/Expenses?ID=" + ID);
            }
        }
        #endregion

        #region New Travel Expense
        // GET: 
        public ActionResult NewTravelExpense()
        {
            try
            {
                getCookie();
                Model.Profile getProfileVehicles = new Model.Profile();
                getProfileVehicles.ProfileID = int.Parse(cookie["ID"]);
                List<Model.Vehicle> Vehicles = handler.getVehicles(getProfileVehicles);
                ViewBag.Vehicles = new SelectList(Vehicles, "VehicleID", "Name");
                return View();
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loading new Travel Log Expense");
                return RedirectToAction("../Shared/Error");
            }
        }

        // POST:
        [HttpPost]
        public ActionResult NewTravelExpense(FormCollection collection, string ID)
        {
            try
            {
                getCookie();
                Model.Profile getProfileVehicles = new Model.Profile();
                getProfileVehicles.ProfileID = int.Parse(cookie["ID"]);
                List<Model.Vehicle> Vehicles = handler.getVehicles(getProfileVehicles);
                ViewBag.Vehicles = new SelectList(Vehicles, "VehicleID", "Name");

                Model.TravelLog newTravelLogExpense = new Model.TravelLog();

                newTravelLogExpense.From = Request.Form["From"].ToString();
                newTravelLogExpense.To = Request.Form["To"].ToString();
                newTravelLogExpense.Reason = Request.Form["Reason"].ToString();
                newTravelLogExpense.OpeningKMs = double.Parse(Request.Form["OpeningKMs"].ToString());
                newTravelLogExpense.ClosingKMs = double.Parse(Request.Form["ClosingKMs"].ToString());
                newTravelLogExpense.VehicleID = int.Parse(Request.Form["VehicleList"].ToString());
                newTravelLogExpense.JobID = int.Parse(ID);

                bool result = handler.NewTravelExpense(newTravelLogExpense);

                if (result == true)
                {
                    return Redirect("/Expense/JobExpenses?ID=" + ID);
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

        #region New Vehicale Expense
        // GET
        public ActionResult NewVehicle()
        {
            try
            {
                getCookie();
                return View();
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loading new Vehical");
                return RedirectToAction("../Expense/NewTravelExpense");
            }
        }

        // POST
        [HttpPost]
        public ActionResult NewVehicle(FormCollection collection)
        {
            try
            {
                getCookie();
                Model.Vehicle newVehicle = new Model.Vehicle();

                newVehicle.Name = Request.Form["Name"].ToString();
                newVehicle.SARSFuelCost = decimal.Parse(Request.Form["SARSFuelCost"].ToString());
                newVehicle.SARSMaintenceCost = decimal.Parse(Request.Form["SARSMaintenceCost"].ToString());
                newVehicle.SARSFixedCost = decimal.Parse(Request.Form["SARSFixedCost"].ToString());
                newVehicle.ClientCharge = decimal.Parse(Request.Form["ClientCharge"].ToString());
                newVehicle.ProfielID = int.Parse(cookie["ID"]);
                
                bool result = handler.newVehicle(newVehicle);

                if (result == true)
                {
                    return Redirect("/Expense/TravelExpenses");
                }
                else
                {
                    return RedirectToAction("../Shared/Error");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in new vehicle of expense controler");
                return View();
            }
        }
        #endregion

        #region Travel Loge View
        public ActionResult JobTravelLog(string ID)
        {
            try
            {
                getCookie();
                Model.Job getJob = new Model.Job();
                getJob.JobID = int.Parse(ID);
                List <Model.TravelLog> JobTravelLog = handler.getJobTravelLog(getJob);

                Model.SP_GetJob_Result Job = handler.getJob(getJob);
                ViewBag.JobTitle = Job.JobTitle;
                return View(JobTravelLog);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding job travel log List");
                return Redirect("/job/job?ID=" + ID);
            }
        }
        public ActionResult TravleLog()
        {
            try
            {
                getCookie();
                Model.Profile getProfile = new Model.Profile();
                getProfile.ProfileID = int.Parse(cookie["ID"].ToString());
                List <Model.TravelLog> ProfileTravelLog = handler.getProfileTravelLog(getProfile);
                return View(ProfileTravelLog);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Profile travel log List");
                return Redirect("/Expense/Expenses");
            }
        }
        public ActionResult TravleLogItem(string ID)
        {
            try
            {
                getCookie();

                Model.TravelLog getTravelLogItem = new Model.TravelLog();
                getTravelLogItem.ExpenseID = int.Parse(ID);
                Model.TravelLog travelLogItem = handler.getTravelLogItem(getTravelLogItem);

                return View(travelLogItem);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Travle Log Item Details");
                return Redirect("/Expense/Expenses?ID=" + ID);
            }
        }
        #endregion
    }
}