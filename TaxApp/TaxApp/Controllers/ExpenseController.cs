using BLL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;

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

                        checkProfile = handler.getProfile(checkProfile);

                        if (checkProfile == null)
                        {
                            Response.Redirect("/Landing/Welcome");
                        }

                        ViewBag.ProfileName = checkProfile.FirstName + " " + checkProfile.LastName;
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

                SP_GetGeneralExpense_Result defaultData = new SP_GetGeneralExpense_Result();
                defaultData.DefultDate = DateTime.Now.ToString("yyyy-MM-dd");
                defaultData.MaxDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                return View(defaultData);
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

                bool result = handler.newGeneralExpense(newExpense);

                if (result == true)
                {
                    return Redirect("/Expense/GeneralExpense");
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
                return View("../Shared/Error");
            }
        }
        #endregion

        #region New Job Expense
        // GET: Landing/NewProfile
        public ActionResult NewJobExpense(string ID)
        {
            try { 
            getCookie();
            List<Model.ExpenseCategory> cats = handler.getExpenseCatagories();
            ViewBag.CategoryList = new SelectList(cats, "CategoryID", "Name");

                Model.Job getJob = new Model.Job();
                getJob.JobID = int.Parse(ID);
                Model.SP_GetJob_Result Job = handler.getJob(getJob);
                ViewBag.JobTitle = Job.JobTitle;
                ViewBag.JobID = Job.JobID;

                SP_GetJobExpense_Result defaultData = new SP_GetJobExpense_Result();
                defaultData.DefultDate = DateTime.Now.ToString("yyyy-MM-dd");
                defaultData.MaxDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                return View(defaultData);
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
                newExpense.Amount = Convert.ToDecimal(Request.Form["Amount"], CultureInfo.CurrentCulture);
                //newExpense.Invoice_ReceiptCopy = DBNull.Value;

                bool result = handler.newJobExpense(newExpense);

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
        
        #region Edit Job Expense
        public ActionResult EditJobExpense(string ID)
        {
            try { 
                getCookie();

                    Model.Expense getExpense = new Model.Expense();
                    getExpense.ExpenseID = int.Parse(ID);
                    Model.SP_GetJobExpense_Result JobExpense = handler.getJobExpense(getExpense);

                return View(JobExpense);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loading edit Job Expense");
                return RedirectToAction("../Shared/Error");
            }
        }

        [HttpPost]
        public ActionResult EditJobExpense(FormCollection collection, string ID)
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
                newExpense.Amount = Convert.ToDecimal(Request.Form["Amount"], CultureInfo.CurrentCulture);
                //newExpense.Invoice_ReceiptCopy = DBNull.Value;

                bool result = handler.newJobExpense(newExpense);

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

                if (Job.EndDate != null)
                {
                    ViewBag.Complete = "Done";
                }
                else
                {
                    ViewBag.Complete = "NotDone";
                }

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
                ViewBag.Details = JobExpense.Name + " Expense";
                ViewBag.Title = "Invoice or Receipt";

                if (Job.EndDate != null)
                {
                    ViewBag.Complete = "Done";
                }
                else
                {
                    ViewBag.Complete = "NotDone";
                }

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
                ViewBag.Details = GeneralExpense.Name + " Expense";
                ViewBag.Title = "Invoice or Receipt";

                return View(GeneralExpense);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding job expense Details");
                return Redirect("/Expense/Expenses?ID=" + ID);
            }
        }

        public ActionResult GeneralExpenses()
        {
            getCookie();

            try
            {
                getCookie();

                Model.Profile getProfile = new Model.Profile();
                getProfile.ProfileID = int.Parse(cookie["ID"].ToString());

                List<Model.SP_GetGeneralExpense_Result> ProfileGeneralExpenses = handler.getGeneralExpenses(getProfile);

                return View(ProfileGeneralExpenses);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding General Expese Page");
                return Redirect("../Shared/error?Err=An error occurred loading all general expenses");
            }
        }
        #endregion
        
        #region Reapet General Expense
        public ActionResult reapetexpense(string ID)
        {
            try
            {
                getCookie();
                Model.Expense getExpense = new Model.Expense();
                getExpense.ExpenseID = int.Parse(ID);
                Model.SP_GetGeneralExpense_Result GeneralExpense = handler.getGeneralExpense(getExpense);

                if(GeneralExpense != null)
                {
                    Model.SP_GetGeneralExpense_Result newExpense = GeneralExpense;

                    newExpense.Date = DateTime.Now;
                    newExpense.Repeat = false;

                    bool result = handler.newGeneralExpense(newExpense);

                    if (result == true)
                    {
                        Response.Redirect("/Expense/GeneralExpenses");
                    }
                    else
                    {
                        function.logAnError("Error reapeteing general expense");
                        return Redirect("../Shared/error?Err=Error Reapeting Expense");
                    }
                }
                else
                {
                    function.logAnError("Error reapeteing general expense");
                    return Redirect("../Shared/error?Err=Error Reapeting Expense");
                }

                return View();
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error reapeteing general expense");
                return Redirect("../Shared/error?Err=Error Reapeting Expense");
            }
        }
        #endregion

        #region New Travel Expense
        // GET: 
        public ActionResult NewTravelExpense(string ID)
        {
            try
            {
                getCookie();
                Model.Profile getProfileVehicles = new Model.Profile();
                getProfileVehicles.ProfileID = int.Parse(cookie["ID"]);
                List<Model.Vehicle> Vehicles = handler.getVehicles(getProfileVehicles);
                ViewBag.Vehicles = new SelectList(Vehicles, "VehicleID", "Name");

                Model.Job getJob = new Model.Job();
                getJob.JobID = int.Parse(ID);
                Model.SP_GetJob_Result Job = handler.getJob(getJob);
                ViewBag.JobTitle = Job.JobTitle;
                ViewBag.JobID = Job.JobID;

                TravelLog defaultData = new TravelLog();
                defaultData.DefultDate = DateTime.Now.ToString("yyyy-MM-dd");
                defaultData.MaxDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                return View(defaultData);
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

                newTravelLogExpense.Date = DateTime.Parse(Request.Form["Date"]);
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
                    return Redirect("/Expense/JobTravelLog?ID=" + ID);
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
        
        #region Delete Travel Expense
        // GET: 
        public ActionResult DeleteTravleLogItem(string ID)
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
                    "Error loading new Travel Log Expense");
                return RedirectToAction("../Shared/Error");
            }
        }

        // POST:
        [HttpPost]
        public ActionResult DeleteTravleLogItem(FormCollection collection, string ID)
        {
            try
            {
                getCookie();

                Model.TravelLog TravelLogExpense = new Model.TravelLog();

                TravelLogExpense.ExpenseID = int.Parse(ID);

                Model.TravelLog travelLogItem = handler.getTravelLogItem(TravelLogExpense);

                bool result = handler.DeleteTravelExpense(TravelLogExpense);

                if (result == true)
                {
                    return Redirect("/Expense/JobTravelLog?ID=" + travelLogItem.JobID);
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

        #region Edit Travel Expense
        // GET: 
        public ActionResult EditTravleLogItem(string ID)
        {
            try
            {
                getCookie();

                Model.TravelLog getTravelLogItem = new Model.TravelLog();
                getTravelLogItem.ExpenseID = int.Parse(ID);
                Model.TravelLog travelLogItem = handler.getTravelLogItem(getTravelLogItem);

                Model.Profile getProfileVehicles = new Model.Profile();
                getProfileVehicles.ProfileID = int.Parse(cookie["ID"]);
                List<Model.Vehicle> Vehicles = handler.getVehicles(getProfileVehicles);
                SelectList VehiclesList = new SelectList(Vehicles, "VehicleID", "Name");
                Model.Vehicle selectedVehicle = new Model.Vehicle();
                selectedVehicle.VehicleID = travelLogItem.VehicleID;
                selectedVehicle.Name = travelLogItem.VehicleName;
                foreach (var item in VehiclesList)
                {
                    if (item.Value == selectedVehicle.VehicleID.ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }
                ViewBag.Vehicles = VehiclesList;

                ViewBag.JobTitle = travelLogItem.JobTitle;
                ViewBag.JobID = travelLogItem.JobID;

                ViewBag.Date = travelLogItem.Date.ToString("yyyy-MM-dd");

                return View(travelLogItem);
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
        public ActionResult EditTravleLogItem(FormCollection collection, string ID)
        {
            try
            {
                getCookie();

                Model.Profile getProfileVehicles = new Model.Profile();
                getProfileVehicles.ProfileID = int.Parse(cookie["ID"]);
                List<Model.Vehicle> Vehicles = handler.getVehicles(getProfileVehicles);
                ViewBag.Vehicles = new SelectList(Vehicles, "VehicleID", "Name");

                Model.TravelLog newTravelLogExpense = new Model.TravelLog();

                newTravelLogExpense.ExpenseID = int.Parse(ID);
                newTravelLogExpense.Date = DateTime.Parse(Request.Form["Date"]);
                newTravelLogExpense.From = Request.Form["From"].ToString();
                newTravelLogExpense.To = Request.Form["To"].ToString();
                newTravelLogExpense.Reason = Request.Form["Reason"].ToString();
                newTravelLogExpense.OpeningKMs = double.Parse(Request.Form["OpeningKMs"].ToString());
                newTravelLogExpense.ClosingKMs = double.Parse(Request.Form["ClosingKMs"].ToString());
                newTravelLogExpense.VehicleID = int.Parse(Request.Form["VehicleList"].ToString());

                bool result = handler.EditTravelExpense(newTravelLogExpense);

                if (result == true)
                {
                    return Redirect("/Expense/TravleLogItem?ID=" + ID);
                }
                else
                {
                    return RedirectToAction("../Shared/Error");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in new esit travel expense of expense controler");
                return RedirectToAction("../Shared/Error");
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
                ViewBag.JobID = Job.JobID;

                if (Job.EndDate != null)
                {
                    ViewBag.Complete = "Done";
                }
                else
                {
                    ViewBag.Complete = "NotDone";
                }
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