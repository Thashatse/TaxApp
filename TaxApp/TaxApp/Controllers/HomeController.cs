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
        
        public ActionResult Index()
        {
            Thread zero = new Thread(function.repeatExpense);
            zero.Start();

            ViewBag.Title = "Dashboard";

            try
            {
                getCookie();

                Model.Profile profile = new Model.Profile();
                profile.ProfileID = int.Parse(cookie["ID"].ToString());

                List<Model.SP_GetJob_Result> jobs = handler.getProfileJobsDashboard(profile);

                Model.DashboardIncomeExpense dashboardIncomeExpense = handler.getDashboardIncomeExpense(profile);

                List<Model.DashboardExpense> dashboardExpenses = new List<Model.DashboardExpense>();
                List<Model.TravelLog> ProfileTravelLog = handler.getProfileTravelLog(profile);
                List<Model.SP_GetJobExpense_Result> ProfileJobExpenses = handler.getAllJobExpense(profile);
                List<Model.SP_GetGeneralExpense_Result> ProfileGeneralExpenses = handler.getGeneralExpenses(profile);
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
                dashboardExpenses = dashboardExpenses.OrderBy(x => x.dateSort).ToList();
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