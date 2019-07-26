using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Model;

namespace TaxApp.Controllers
{
    public class VatController : Controller
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

        public ActionResult VatCenter(string view, string period)
        {
            getCookie();

            try
            {
                VATDashboard dashboard = null;
                List<TAXorVATRecivedList> VATRecived = null;
                List<Model.DashboardExpense> VATPaid = null;

                Profile profileID = new Profile();
                profileID.ProfileID = int.Parse(cookie["ID"]);

                List<TaxAndVatPeriods> vatPeriod = handler.getTaxOrVatPeriodForProfile(profileID, 'V');

                if(vatPeriod == null || vatPeriod.Count == 0)
                {
                    Response.Redirect("../Tax/TaxVatPeriod?Type=V");
                }
                else
                {
                    ViewBag.VatPeriodList = new SelectList(vatPeriod, "PeriodID", "PeriodString");
                    ViewBag.View = view;

                    ViewBag.VatPeriod = null;

                    if (period == null || period == "")
                    {
                        Response.Redirect("../Vat/VatCenter?period=" + vatPeriod[0].PeriodID + "&view=" + view);
                    }
                        foreach (TaxAndVatPeriods item in vatPeriod)
                        {
                            if (item.PeriodID.ToString() == period)
                            {
                                ViewBag.VatPeriod = item.PeriodString;

                                dashboard = handler.getVatCenterDashboard(profileID, item);

                                VATRecived = handler.getVATRecivedList(profileID, item);

                                VATPaid = new List<Model.DashboardExpense>();
                                List<Model.TravelLog> ProfileTravelLog = handler.getProfileTravelLog(profileID);
                                List<Model.SP_GetJobExpense_Result> ProfileJobExpenses = handler.getAllJobExpense(profileID);
                                List<Model.SP_GetGeneralExpense_Result> ProfileGeneralExpenses = handler.getGeneralExpenses(profileID);
                                var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                                nfi.NumberGroupSeparator = " ";
                                foreach (Model.TravelLog expenseItem in ProfileTravelLog)
                                {
                                    Model.DashboardExpense expense = new Model.DashboardExpense();

                                    expense.name = expenseItem.Reason;
                                    expense.date = expenseItem.DateString;
                                    expense.dateSort = expenseItem.Date;
                                    expense.deatil = expenseItem.TotalKMs.ToString();
                                    expense.deatilTitle = "Total Km's:";
                                    expense.amountTital = "Cost to Customer:";
                                    expense.amount = expenseItem.ClientCharge;
                                    expense.URL = "../Expense/TravleLogItem?ID=" + expenseItem.ExpenseID;
                                    expense.expenseType = "Travel";
                                    expense.VAT = ((expense.amount / 100) * item.VATRate);
                                    expense.VATString = expense.VAT.ToString("#,0.##", nfi);
                                    expense.TotalString = expense.amount.ToString("#,0.##", nfi);

                                    VATPaid.Add(expense);
                                }
                                foreach (Model.SP_GetJobExpense_Result expenseItem in ProfileJobExpenses)
                                {
                                    Model.DashboardExpense expense = new Model.DashboardExpense();

                                    expense.name = expenseItem.Name;
                                    expense.date = expenseItem.DateString;
                                    expense.dateSort = expenseItem.Date;
                                    expense.deatil = expenseItem.JobTitle;
                                    expense.deatilTitle = "Job:";
                                    expense.amountTital = "Price:";
                                    expense.amount = expenseItem.Amount;
                                    expense.URL = "../Expense/JobExpense?ID=" + expenseItem.ExpenseID;
                                    expense.expenseType = "Job";
                                    expense.VAT = ((expense.amount / 100) * item.VATRate);
                                    expense.VATString = expense.VAT.ToString("#,0.##", nfi);
                                    expense.TotalString = expense.amount.ToString("#,0.##", nfi);

                                    VATPaid.Add(expense);
                                }
                                foreach (Model.SP_GetGeneralExpense_Result expenseItem in ProfileGeneralExpenses)
                                {
                                    Model.DashboardExpense expense = new Model.DashboardExpense();

                                    expense.name = expenseItem.Name;
                                    expense.date = expenseItem.DateString;
                                    expense.dateSort = expenseItem.Date;
                                    expense.deatil = expenseItem.Repeat.ToString();
                                    expense.deatilTitle = "Recuring:";
                                    expense.amountTital = "Price:";
                                    expense.amount = expenseItem.Amount;
                                    expense.URL = "../Expense/GeneralExpense?ID=" + expenseItem.ExpenseID;
                                    expense.expenseType = "General";
                                    expense.VAT = ((expense.amount / 100) * item.VATRate);
                                    expense.VATString = expense.VAT.ToString("#,0.##", nfi);
                                    expense.TotalString = expense.amount.ToString("#,0.##", nfi);

                                    VATPaid.Add(expense);
                                }
                                VATPaid = VATPaid.OrderBy(x => x.dateSort).ToList();
                            }
                        }

                    if(ViewBag.VatPeriod == null)
                    {
                        Response.Redirect("../Shared/Error?Err=An error occurred loading data for vat period");
                    }

                    VatCenter viewModel = new VatCenter();
                    viewModel.VATDashboard = dashboard;
                    viewModel.VATRecivedList = VATRecived;
                    viewModel.VATPaid = VATPaid;

                    return View(viewModel);
                }

                return Redirect("../Shared/Error");
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Vat Center");
                return Redirect("../Shared/Error?Err=An error occurred loading the vat center");
            }
        }
        [HttpPost]
        public ActionResult VatCenter(FormCollection collection, string view, string period)
        {
            getCookie();

            try
            {
                Profile profileID = new Profile();
                profileID.ProfileID = int.Parse(cookie["ID"]);

                List<TaxAndVatPeriods> vatPeriod = handler.getTaxOrVatPeriodForProfile(profileID, 'V');

                Response.Redirect("../Vat/VatCenter?period=" + Request.Form["VatPeriodList"].ToString());

                return Redirect("../Shared/Error?Err=An error occurred updating the vat period");
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Vat Center");
                return Redirect("../Shared/Error?Err=An error occurred loading the vat center");
            }
        }
    }
}