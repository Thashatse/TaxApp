using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using Model;

namespace TaxApp.Controllers
{
    public class HomeController : Controller
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
        
        public ActionResult Search(string view, string StartDateRange, string EndDateRange, string term, string cat)
        {
            try
            {
                getCookie();

                List<SearchViewModel> results = null;

                if (cat == null || cat == "")
                    cat = "A";

                if (cat == "J")
                    ViewBag.SearchType = "Job";

                if (cat == "WL")
                    ViewBag.SearchType = "Work Log";

                if (cat == "TC")
                    ViewBag.SearchType = "Tax Consultnat";

                if (cat == "I")
                    ViewBag.SearchType = "Invoice";

                if (cat == "TL")
                    ViewBag.SearchType = "Travel Log";

                if (cat == "JE")
                    ViewBag.SearchType = "Job Expense";

                if (cat == "C")
                    ViewBag.SearchType = "Client";

                if (cat == "GE")
                    ViewBag.SearchType = "Genetal Expense";

                ViewBag.view = view;
                    ViewBag.term = term;
                    ViewBag.cat = cat;

                    int year = DateTime.Now.Year;
                    DateTime sDate = DateTime.Now.AddYears(-100);
                    DateTime eDate = DateTime.Now;

                    if (StartDateRange != null && EndDateRange != null
                        && DateTime.TryParse(StartDateRange, out sDate) && DateTime.TryParse(EndDateRange, out eDate)) { }

                    if (sDate > eDate)
                    {
                        DateTime temp = sDate;
                        sDate = eDate;
                        eDate = temp;
                    }

                    ViewBag.DateRange = sDate.ToString("dd MMM yyyy") + " - " + eDate.ToString("dd MMM yyyy");
                    ViewBag.StartDateRange = sDate.ToString("yyyy-MM-dd");
                    ViewBag.EndDateRange = eDate.ToString("yyyy-MM-dd");

                if (term != null && term != "")
                {
                    results = handler.getSearchResults(term, int.Parse(cookie["ID"]), sDate, eDate, cat);
                }

                return View(results);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding search term: "+term);
                return Redirect("../Shared/Error?An error occurred while processing your search request.");
            }
        }
        [HttpPost]
        public ActionResult Search(FormCollection collection, string view, string StartDateRange, string EndDateRange, string term, string cat)
        {
            try
            {
                int year = DateTime.Now.Year;
                DateTime sDate = DateTime.Now.AddYears(-100);
                DateTime eDate = DateTime.Now;

                DateTime.TryParse(Request.Form["StartDate"], out sDate);
                DateTime.TryParse(Request.Form["EndDate"], out eDate);

                StartDateRange = sDate.ToString("yyyy-MM-dd");
                EndDateRange = eDate.ToString("yyyy-MM-dd");

                return RedirectToAction("Home", "Search", new
                {
                    view,
                    StartDateRange,
                    EndDateRange,
                    term,
                    cat
                });
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error updating date range for jobs page");
                return RedirectToAction("../Shared/Error");
            }
        }

        public ActionResult Index()
        {
            getCookie();

            Thread zero = new Thread(function.runAutoFunctions);
            zero.Start(cookie["ID"]);

            ViewBag.Title = "Dashboard";

            ViewBag.NotificationList = notiFunctions.getNotifications(int.Parse(cookie["ID"]));

            try
            {

                Model.Profile profile = new Model.Profile();
                profile.ProfileID = int.Parse(cookie["ID"].ToString());

                DateTime sDate = DateTime.Now.AddYears(-1);
                DateTime eDate = DateTime.Now;

                List<Model.SP_GetJob_Result> jobs = handler.getProfileJobsDashboard(profile);

                Model.DashboardIncomeExpense dashboardIncomeExpense = handler.getDashboardIncomeExpense(profile);

                List<Model.DashboardExpense> dashboardExpenses = new List<Model.DashboardExpense>();
                List<Model.TravelLog> ProfileTravelLog = handler.getProfileTravelLog(profile, sDate, eDate);
                List<Model.SP_GetJobExpense_Result> ProfileJobExpenses = handler.getAllJobExpense(profile);
                List<Model.SP_GetGeneralExpense_Result> ProfileGeneralExpenses = handler.getGeneralExpenses(profile, sDate, eDate);
                foreach(Model.TravelLog item in ProfileTravelLog)
                {
                    Model.DashboardExpense expense = new Model.DashboardExpense();

                    expense.name = item.Reason;
                    expense.date = item.DateString;
                    expense.dateSort = item.Date;
                    expense.deatil = item.TotalKMs.ToString();
                    expense.deatilTitle = "Total Km's:";
                    expense.amountTital = "Cost to Customer:";
                    expense.amount = item.ClientCharge;
                    expense.URL = "../Expense/TravleLogItem?ID=" + item.ExpenseID;
                    expense.expenseType = "Travel";

                    dashboardExpenses.Add(expense);
                }
                foreach (Model.SP_GetJobExpense_Result item in ProfileJobExpenses)
                {
                    Model.DashboardExpense expense = new Model.DashboardExpense();

                    expense.name = item.Name;
                    expense.date = item.DateString;
                    expense.dateSort = item.Date;
                    expense.deatil = item.JobTitle;
                    expense.deatilTitle = "Job:";
                    expense.amountTital = "Price:";
                    expense.amount = item.Amount;
                    expense.URL = "../Expense/JobExpense?ID="+item.ExpenseID;
                    expense.expenseType = "Job";

                    dashboardExpenses.Add(expense);
                }
                foreach (Model.SP_GetGeneralExpense_Result item in ProfileGeneralExpenses)
                {
                    Model.DashboardExpense expense = new Model.DashboardExpense();

                    expense.name = item.Name;
                    expense.date = item.DateString;
                    expense.dateSort = item.Date;
                    expense.deatil = item.Repeat.ToString();
                    expense.deatilTitle = "Recuring:";
                    expense.amountTital = "Price:";
                    expense.amount = item.Amount;
                    expense.URL = "../Expense/GeneralExpense?ID=" + item.ExpenseID;
                    expense.expenseType = "General";

                    dashboardExpenses.Add(expense);
                }
                dashboardExpenses = dashboardExpenses.OrderByDescending(x => x.dateSort).ToList();
                List<Model.SP_GetInvoice_Result> OutinvoiceDetails = handler.getInvoicesOutsatanding(profile);

                List<VATDashboard> VAT = new List<VATDashboard>();
                List<TaxAndVatPeriods> vatPeriod = handler.getTaxOrVatPeriodForProfile(profile, 'V');
                int v = 0;
                if (vatPeriod != null && vatPeriod.Count != 0)
                {
                    foreach (TaxAndVatPeriods item in vatPeriod)
                    {
                       if(v < 3)
                       {
                            VATDashboard periodVAT = handler.getVatCenterDashboard(profile, item);
                            periodVAT.PeriodString = item.PeriodString;
                            VAT.Add(periodVAT);
                       }
                       v++;
                    }
                }

                List<TaxDashboard> TAX = new List<TaxDashboard>();
                List<TaxAndVatPeriods> taxPeriod = handler.getTaxOrVatPeriodForProfile(profile, 'T');
                int t = 0;
                if (taxPeriod != null && taxPeriod.Count != 0)
                {
                    foreach (TaxAndVatPeriods item in taxPeriod)
                    {
                       if(t < 3)
                       {
                            TaxDashboard periodTAX = handler.getTaxCenterDashboard(profile, item);
                            periodTAX.PeriodString = item.PeriodString;
                            TAX.Add(periodTAX);
                       }
                       t++;
                    }
                }

                    var viewModel = new Model.homeViewModel();
                viewModel.Jobs = jobs;
                viewModel.DashboardIncomeExpense = dashboardIncomeExpense;
                viewModel.Expenses = dashboardExpenses;
                viewModel.OutInvoices = OutinvoiceDetails;
                viewModel.VAT = VAT;
                viewModel.TAX = TAX;

                return View(viewModel);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding job details");
                return RedirectToAction("../Shared/Error");
            }
        }
    }
}