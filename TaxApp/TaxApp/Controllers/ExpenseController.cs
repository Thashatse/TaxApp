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
        NotificationsFunctions notiFunctions = new NotificationsFunctions();

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
                        ViewBag.NotificationList = notiFunctions.getNotifications(int.Parse(cookie["ID"]));
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

                DateTime sDate = DateTime.Now.AddYears(-1);
                DateTime eDate = DateTime.Now;

                List<Model.TravelLog> ProfileTravelLog = handler.getProfileTravelLog(getProfile, sDate, eDate);
                List<Model.SP_GetJobExpense_Result> ProfileJobExpenses = handler.getAllJobExpense(getProfile, sDate, eDate);
                List<Model.SP_GetGeneralExpense_Result> ProfileGeneralExpenses = handler.getGeneralExpenses(getProfile, sDate, eDate);

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
                newExpense.PrimaryExpenseID = -1;

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

                bool check = true;

                if (Request.Form["Amount"] == null || Request.Form["Amount"] == "")
                {
                    ViewBag.Err = "Please enter an Amount";
                    check = false;
                }

                if(check == true)
                {
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

        #region Expense Catagories
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
        public ActionResult JobExpenses(string ID, string downloadAll)
        {
            try
            {
                ViewBag.cat = "JE";
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

                    foreach (SP_GetJobExpense_Result file in JobExpenses)
                    {
                        if(file.Invoice_ReceiptCopy != null)
                    {
                        ViewBag.files = "True";
                        if (downloadAll == "True")
                        {
                            string redirect = "<script>window.open('../Functions/DownloadFile?ID=" + file.ExpenseID + "&type=JE');</script>";
                            Response.Write(redirect);
                        }
                    }
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
        public ActionResult AllJobExpenses(string view, string StartDateRange, string EndDateRange)
        {
            try
            {
                getCookie();

                ViewBag.view = view;
                ViewBag.cat = "JE";

                int year = DateTime.Now.Year;
                DateTime sDate = DateTime.Now.AddMonths(-6);
                DateTime eDate = DateTime.Now;

                if (StartDateRange != null && EndDateRange != null
                    && DateTime.TryParse(StartDateRange, out sDate) && DateTime.TryParse(EndDateRange, out eDate)) { }

                if (sDate > eDate)
                {
                    DateTime temp = sDate;
                    sDate = eDate;
                    eDate = temp;
                }

                ViewBag.DateRange = "From "+sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                ViewBag.StartDateRange = sDate.ToString("yyyy-MM-dd");
                ViewBag.EndDateRange = eDate.ToString("yyyy-MM-dd");

                Profile profileID = new Profile();
                profileID.ProfileID = int.Parse(cookie["ID"]);

                List<Model.SP_GetJobExpense_Result> JobExpenses = handler.getAllJobExpense(profileID, sDate, eDate);

                return View(JobExpenses);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding job expense List");
                return Redirect("../Expense/Expenses");
            }
        }
        [HttpPost]
        public ActionResult AllJobExpenses(FormCollection collection, string view, string StartDateRange, string EndDateRange)
        {
            try
            {
                int year = DateTime.Now.Year;
                DateTime sDate = DateTime.Now.AddMonths(-6);
                DateTime eDate = DateTime.Now;

                DateTime.TryParse(Request.Form["StartDate"], out sDate);
                DateTime.TryParse(Request.Form["EndDate"], out eDate);

                StartDateRange = sDate.ToString("yyyy-MM-dd");
                EndDateRange = eDate.ToString("yyyy-MM-dd");

                return RedirectToAction("AllJobExpenses", "Expense", new
                {
                    view,
                    StartDateRange,
                    EndDateRange
                });
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error updating date range for all job expenses");
                return RedirectToAction("../Shared/Error");
            }
        }
        public ActionResult JobExpense(string ID)
        {
            try
            {
                ViewBag.cat = "JE";
                getCookie();
                Model.Expense getExpense = new Model.Expense();
                getExpense.ExpenseID = int.Parse(ID);
                Model.SP_GetJobExpense_Result JobExpense = handler.getJobExpense(getExpense);

                Model.Job getJob = new Model.Job();
                getJob.JobID = JobExpense.JobID;
                Model.SP_GetJob_Result Job = handler.getJob(getJob);
                ViewBag.JobTitle = Job.JobTitle;
                ViewBag.JobID = Job.JobID;
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

        #region Invoice And Recipts File View
        public ActionResult InvoiceAndReceipts(string view, string StartDateRange, string EndDateRange, string downloadAll)
        {
            try
            {
                getCookie();

                ViewBag.view = view;

                int year = DateTime.Now.Year;
                DateTime sDate = DateTime.Now.AddMonths(-6);
                DateTime eDate = DateTime.Now;

                if (StartDateRange != null && EndDateRange != null
                    && DateTime.TryParse(StartDateRange, out sDate) && DateTime.TryParse(EndDateRange, out eDate)) { }

                if (sDate > eDate)
                {
                    DateTime temp = sDate;
                    sDate = eDate;
                    eDate = temp;
                }

                ViewBag.DateRange = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                ViewBag.StartDateRange = sDate.ToString("yyyy-MM-dd");
                ViewBag.EndDateRange = eDate.ToString("yyyy-MM-dd");

                Profile profileID = new Profile();
                profileID.ProfileID = int.Parse(cookie["ID"]);

                List<InvoiceAndReciptesFile> files = handler.getInvoiceAndReciptesFiles(profileID, sDate, eDate);

                if (downloadAll == "True")
                {
                    foreach(InvoiceAndReciptesFile file in files)
                    {
                        string redirect = "<script>window.open('../Functions/DownloadFile?ID="+file.ID+"&type="+file.Type+"');</script>";
                        Response.Write(redirect);
                    }
                }

                return View(files);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Invoice And Reciptes List");
                return Redirect("../Shared/Error");
            }
        }
        [HttpPost]
        public ActionResult InvoiceAndReceipts(FormCollection collection, string view, string StartDateRange, string EndDateRange)
        {
            try
            {
                int year = DateTime.Now.Year;
                DateTime sDate = DateTime.Now.AddMonths(-6);
                DateTime eDate = DateTime.Now;

                DateTime.TryParse(Request.Form["StartDate"], out sDate);
                DateTime.TryParse(Request.Form["EndDate"], out eDate);

                StartDateRange = sDate.ToString("yyyy-MM-dd");
                EndDateRange = eDate.ToString("yyyy-MM-dd");

                return RedirectToAction("InvoiceAndReceipts", "Expense", new
                {
                    view,
                    StartDateRange,
                    EndDateRange
                });
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error updating date range for Invoice And Reciptes in expense report");
                return RedirectToAction("../Shared/Error");
            }
        }
        #endregion

        #region General Expense View
        public ActionResult GeneralExpense(string ID)
        {
            try
            {
                ViewBag.cat = "GE";
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

        public ActionResult GeneralExpenses(string view, string ExpenseDisplayCount, string StartDateRange, string EndDateRange, string downloadAll)
        {
            try
            {
                ViewBag.cat = "GE";
                getCookie();

                ViewBag.view = view;
                ViewBag.SeeMore = false;
                int year = DateTime.Now.Year;
                DateTime sDate = DateTime.Now.AddMonths(-6);
                DateTime eDate = DateTime.Now;

                if (StartDateRange != null && EndDateRange != null
                    && DateTime.TryParse(StartDateRange, out sDate) && DateTime.TryParse(EndDateRange, out eDate)) { }

                if (sDate > eDate)
                {
                    DateTime temp = sDate;
                    sDate = eDate;
                    eDate = temp;
                }

                ViewBag.DateRange = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                ViewBag.StartDateRange = sDate.ToString("yyyy-MM-dd");
                ViewBag.EndDateRange = eDate.ToString("yyyy-MM-dd");

                Model.Profile getProfile = new Model.Profile();
                getProfile.ProfileID = int.Parse(cookie["ID"].ToString());

                List<Model.SP_GetGeneralExpense_Result> ProfileGeneralExpenses = handler.getGeneralExpenses(getProfile, sDate, eDate);

                foreach (SP_GetGeneralExpense_Result file in ProfileGeneralExpenses)
                {
                    if (file.Invoice_ReceiptCopy != null)
                    {
                        ViewBag.files = "True";
                        if (downloadAll == "True")
                        {
                            string redirect = "<script>window.open('../Functions/DownloadFile?ID=" + file.ExpenseID + "&type=GE');</script>";
                            Response.Write(redirect);
                        }
                    }
                }

                if (ProfileGeneralExpenses.Count > 11)
                {
                    int x;
                    if (ExpenseDisplayCount != null && ExpenseDisplayCount != "" 
                        && function.IsDigitsOnly(ExpenseDisplayCount))
                        x = int.Parse(ExpenseDisplayCount);
                    else
                        x = 11;

                    if (x < ProfileGeneralExpenses.Count)
                    {
                        ProfileGeneralExpenses = ProfileGeneralExpenses.GetRange(0, x);
                        ViewBag.SeeMore = true;
                    }
                    else
                    {
                        ProfileGeneralExpenses = ProfileGeneralExpenses.GetRange(0, ProfileGeneralExpenses.Count);
                        ViewBag.SeeMore = false;
                    }

                    ViewBag.X = x + 11;
                }

                return View(ProfileGeneralExpenses);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding General Expese Page");
                return Redirect("../Shared/error?Err=An error occurred loading all general expenses");
            }
        }

        [HttpPost]
        public ActionResult GeneralExpenses(FormCollection collection, string view, string ExpenseDisplayCount, string StartDateRange, string EndDateRange)
        {
            try
            {
                int year = DateTime.Now.Year;
                DateTime sDate = DateTime.Now.AddMonths(-6);
                DateTime eDate = DateTime.Now;

                DateTime.TryParse(Request.Form["StartDate"], out sDate);
                DateTime.TryParse(Request.Form["EndDate"], out eDate);

                StartDateRange = sDate.ToString("yyyy-MM-dd");
                EndDateRange = eDate.ToString("yyyy-MM-dd");

                return RedirectToAction("GeneralExpenses", "Expense", new
                {
                    view,
                    ExpenseDisplayCount,
                    StartDateRange,
                    EndDateRange
                });
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error updating date range for jobs page");
                return RedirectToAction("../Shared/Error");
            }
        }
        #endregion

        #region Reapet General Expense
        public ActionResult reapetexpense(string ID)
        {
            try
            {
                ViewBag.cat = "GE";
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
                    "Error loding general expense Details for ");
                return Redirect("/Expense/Expenses?ID=" + ID);
            }
        }
        [HttpPost]
        public ActionResult reapetexpense(FormCollection collection, string ID)
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

                    newExpense.Amount = Decimal.Parse(Request.Form["Amount"].ToString());
                    newExpense.Repeat = bool.Parse(Request.Form["Repeat"].ToString().Split(',')[0]);

                    if (newExpense.PrimaryExpenseID == 0)
                        newExpense.PrimaryExpenseID = newExpense.ExpenseID;

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
                bool check = true;
                
                if (Request.Form["ClosingKMs"] == null || Request.Form["ClosingKMs"] == ""
                    || Request.Form["OpeningKMs"] == null || Request.Form["OpeningKMs"] == "")
                {
                    check = false;
                    ViewBag.err = "Please enter Opening KMs & Closing KMs";
                }
                else
                {

                    if (double.Parse(Request.Form["OpeningKMs"].ToString()) > double.Parse(Request.Form["ClosingKMs"].ToString()))
                    {
                        check = false;
                        ViewBag.err = "Opening KMs canot be greater that Closing KMs";
                    }
                    else
                    {
                        newTravelLogExpense.OpeningKMs = double.Parse(Request.Form["OpeningKMs"].ToString());
                        newTravelLogExpense.ClosingKMs = double.Parse(Request.Form["ClosingKMs"].ToString());
                    }
                }

                if (Request.Form["Reason"] == null || Request.Form["Reason"] == "")
                {
                    check = false;
                    ViewBag.err = "Please enter a Reason";
                }
                else
                {
                    newTravelLogExpense.Reason = Request.Form["Reason"].ToString();
                }

                if (Request.Form["To"] == null || Request.Form["To"] == "")
                {
                    check = false;
                    ViewBag.err = "Please enter a destination (To)";
                }
                else
                {
                    newTravelLogExpense.To = Request.Form["To"].ToString();
                }

                if (Request.Form["From"] == null || Request.Form["From"] == "")
                {
                    check = false;
                    ViewBag.err = "Please enter a starting point (From)";
                }
                else
                {
                    newTravelLogExpense.From = Request.Form["From"].ToString();
                }

                newTravelLogExpense.Date = DateTime.Parse(Request.Form["Date"]);
                newTravelLogExpense.VehicleID = int.Parse(Request.Form["VehicleList"].ToString());
                
                if (check == true) {

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

                Model.Job getJob = new Model.Job();
                getJob.JobID = int.Parse(ID);
                Model.SP_GetJob_Result Job = handler.getJob(getJob);
                ViewBag.JobTitle = Job.JobTitle;
                ViewBag.JobID = Job.JobID;

                newTravelLogExpense.DefultDate = newTravelLogExpense.Date.ToString("yyyy-MM-dd");
                newTravelLogExpense.MaxDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                return View(newTravelLogExpense);
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

        #region New Vehicale
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

        #region Vehicles
        // GET
        public ActionResult Vehicles()
        {
            try
            {
                ViewBag.cat = "TL";
                getCookie();
                Profile getVehicles = new Profile();
                getVehicles.ProfileID = int.Parse(cookie["ID"].ToString());
                List<Vehicle> vehicles = handler.getVehicles(getVehicles);
                return View(vehicles);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loading Vehicles view");
                return RedirectToAction("../Shared/Error");
            }
        }
        #endregion

        #region Edit Vehicles
        // GET
        public ActionResult EditVehicles(string ID)
        {
            try
            {
                getCookie();

                Vehicle getVehicle = new Vehicle();
                getVehicle.VehicleID = int.Parse(ID);
                Vehicle vehicle = handler.getVehicle(getVehicle);

                return View(vehicle);

            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loading edit Vehicles view");
                return RedirectToAction("../Shared/Error?Err=An error occurred loading data for vehicles");
            }
        }

        [HttpPost]
        public ActionResult EditVehicles(FormCollection collection, string ID)
        {
            try
            {
                getCookie();

                Vehicle vehicle = new Vehicle();
                vehicle.Name = Request.Form["Name"].ToString();
                vehicle.SARSFuelCost = decimal.Parse(Request.Form["SARSFuelCost"].ToString());
                vehicle.SARSMaintenceCost = decimal.Parse(Request.Form["SARSMaintenceCost"].ToString());
                vehicle.SARSFixedCost = decimal.Parse(Request.Form["SARSFixedCost"].ToString());
                vehicle.ClientCharge = decimal.Parse(Request.Form["ClientCharge"].ToString());
                vehicle.ProfielID = int.Parse(cookie["ID"]);
                vehicle.VehicleID = int.Parse(ID);

                bool result = handler.editVehicle(vehicle);

                if (result == true)
                {
                    return Redirect("/Expense/vehicles");
                }
                else
                {
                    return RedirectToAction("../Shared/Error?Err=An error occurred editing vehicles");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error executing edit Vehicle");
                return RedirectToAction("../Shared/Error?Err=An error occurred editing vehicles");
            }
        }
        #endregion

        #region Travel Loge View
        public ActionResult JobTravelLog(string ID)
        {
            try
            {
                ViewBag.cat = "TL";
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
        public ActionResult TravleLog(string view, string ExpenseDisplayCount, string StartDateRange, string EndDateRange)
        {
            try
            {
                ViewBag.cat = "TL";
                getCookie();

                ViewBag.view = view;
                ViewBag.SeeMore = false;
                int year = DateTime.Now.Year;
                DateTime sDate = DateTime.Now.AddMonths(-6);
                DateTime eDate = DateTime.Now;

                if (StartDateRange != null && EndDateRange != null
                    && DateTime.TryParse(StartDateRange, out sDate) && DateTime.TryParse(EndDateRange, out eDate)) { }

                if (sDate > eDate)
                {
                    DateTime temp = sDate;
                    sDate = eDate;
                    eDate = temp;
                }

                ViewBag.DateRange = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                ViewBag.StartDateRange = sDate.ToString("yyyy-MM-dd");
                ViewBag.EndDateRange = eDate.ToString("yyyy-MM-dd");

                Model.Profile getProfile = new Model.Profile();
                getProfile.ProfileID = int.Parse(cookie["ID"].ToString());
                List <Model.TravelLog> ProfileTravelLog = handler.getProfileTravelLog(getProfile, sDate, eDate);


                if (ProfileTravelLog.Count > 11)
                {
                    int x;
                    if (ExpenseDisplayCount != null && ExpenseDisplayCount != ""
                        && function.IsDigitsOnly(ExpenseDisplayCount))
                        x = int.Parse(ExpenseDisplayCount);
                    else
                        x = 11;

                    if (x < ProfileTravelLog.Count)
                    {
                        ProfileTravelLog = ProfileTravelLog.GetRange(0, x);
                        ViewBag.SeeMore = true;
                    }
                    else
                    {
                        ProfileTravelLog = ProfileTravelLog.GetRange(0, ProfileTravelLog.Count);
                        ViewBag.SeeMore = false;
                    }

                    ViewBag.X = x + 11;
                }

                return View(ProfileTravelLog);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Profile travel log List");
                return Redirect("/Expense/Expenses");
            }
        }

        [HttpPost]
        public ActionResult TravleLog(FormCollection collection, string view, string ExpenseDisplayCount, string StartDateRange, string EndDateRange)
        {
            try
            {
                ViewBag.cat = "TL";
                int year = DateTime.Now.Year;
                DateTime sDate = DateTime.Now.AddMonths(-6);
                DateTime eDate = DateTime.Now;

                DateTime.TryParse(Request.Form["StartDate"], out sDate);
                DateTime.TryParse(Request.Form["EndDate"], out eDate);

                StartDateRange = sDate.ToString("yyyy-MM-dd");
                EndDateRange = eDate.ToString("yyyy-MM-dd");

                return RedirectToAction("TravleLog", "Expense", new
                {
                    view,
                    ExpenseDisplayCount,
                    StartDateRange,
                    EndDateRange
                });
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error updating date range for jobs page");
                return RedirectToAction("../Shared/Error");
            }
        }

        public ActionResult TravleLogItem(string ID)
        {
            try
            {
                ViewBag.cat = "TL";
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