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
                            Response.Redirect(Url.Action("Welcome", "Landing"));
                        }

                        ViewBag.ProfileName = checkProfile.FirstName + " " + checkProfile.LastName;
                        ViewBag.NotificationList = notiFunctions.getNotifications(int.Parse(cookie["ID"]));
                    }
                    else
                    {
                        Response.Redirect(Url.Action("Welcome", "Landing"));
                    }
                }
                else
                {
                    Response.Redirect(Url.Action("Welcome", "Landing"));
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in welcome method of LandingControles");
                Response.Redirect(Url.Action("Error", "Shared") + "?Err=Identity couldn't be verified");
            }
        }

        public ActionResult VatCenter(string view, string period)
        {
            getCookie();

            try
            {
                VatCenter viewModel = new VatCenter();

                VATDashboard dashboard = null;
                List<TAXorVATRecivedList> VATRecived = null;
                List<Model.DashboardExpense> VATPaid = null;

                Profile profileID = new Profile();
                profileID.ProfileID = int.Parse(cookie["ID"]);

                List<TaxAndVatPeriods> vatPeriod = handler.getTaxOrVatPeriodForProfile(profileID, 'V');

                if(vatPeriod == null || vatPeriod.Count == 0)
                {
                    return RedirectToAction("TaxVatPeriod", "Tax", new
                    {
                        Type = "V"
                    });
                }
                else
                {
                    ViewBag.VatPeriodList = new SelectList(vatPeriod, "PeriodID", "PeriodString");
                    ViewBag.View = view;

                    ViewBag.VatPeriod = null;

                    var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                    nfi.NumberGroupSeparator = " ";

                    if (period == null || period == "")
                    {
                        return RedirectToAction("VatCenter", "Vat", new{period=vatPeriod[0].PeriodID, view});
                    }
                        foreach (TaxAndVatPeriods item in vatPeriod)
                        {
                            if (item.PeriodID.ToString() == period)
                        {
                            DateTime sDate = item.StartDate;
                            DateTime eDate = item.EndDate;

                            ViewBag.VatPeriod = item.PeriodString;

                                dashboard = handler.getVatCenterDashboard(profileID, item);

                            decimal totalAmount = 0;
                            decimal vatTotalAmount = 0;
                            VATRecived = handler.getVATRecivedList(profileID, item);
                            foreach (TAXorVATRecivedList vatRecived in VATRecived)
                            {
                                totalAmount += vatRecived.Total;
                                vatTotalAmount += vatRecived.VATorTAX;
                            }
                            ViewBag.totalAmountRecived = totalAmount.ToString("#,0.00", nfi);
                            ViewBag.vatTotalAmountRecived = vatTotalAmount.ToString("#,0.00", nfi);

                            totalAmount = 0;
                            vatTotalAmount = 0;

                            VATPaid = new List<Model.DashboardExpense>();
                                List<Model.TravelLog> ProfileTravelLog = handler.getProfileTravelLog(profileID, sDate, eDate);
                                List<Model.SP_GetJobExpense_Result> ProfileJobExpenses = handler.getAllJobExpense(profileID, sDate, eDate);
                                List<Model.SP_GetGeneralExpense_Result> ProfileGeneralExpenses = handler.getGeneralExpensesReport(profileID, sDate, eDate);
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
                                //Change befor Publishing
                                expense.URL = "/Expense/TravleLogItem?ID=" + expenseItem.ExpenseID;
                                    //expense.URL = "http://sict-iis.nmmu.ac.za/taxapp/Expense/TravleLogItem?ID=" + expenseItem.ExpenseID;
                                    expense.expenseType = "Travel";
                                    expense.VAT = expense.amount - (expense.amount / ((item.VATRate/100)+1));
                                    expense.VATString = expense.VAT.ToString("#,0.00", nfi);
                                    expense.TotalString = expense.amount.ToString("#,0.00", nfi);

                                totalAmount += expense.amount;
                                vatTotalAmount += expense.VAT;

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
                                //Change befor Publishing
                                expense.URL = "/Expense/JobExpense?ID=" + expenseItem.ExpenseID;
                                    //expense.URL = "http://sict-iis.nmmu.ac.za/taxapp/Expense/JobExpense?ID=" + expenseItem.ExpenseID;
                                    expense.expenseType = "Job";
                                    expense.VAT = expense.amount - (expense.amount / ((item.VATRate / 100) + 1));
                                    expense.VATString = expense.VAT.ToString("#,0.00", nfi);
                                    expense.TotalString = expense.amount.ToString("#,0.00", nfi);

                                totalAmount += expense.amount;
                                vatTotalAmount += expense.VAT;

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
                                //Change befor Publishing
                                expense.URL = "/Expense/GeneralExpense?ID=" + expenseItem.ExpenseID;
                                    //expense.URL = "http://sict-iis.nmmu.ac.za/taxapp/Expense/GeneralExpense?ID=" + expenseItem.ExpenseID;
                                    expense.expenseType = "General";
                                    expense.VAT = expense.amount - (expense.amount / ((item.VATRate / 100) + 1));
                                    expense.VATString = expense.VAT.ToString("#,0.00", nfi);
                                    expense.TotalString = expense.amount.ToString("#,0.00", nfi);

                                totalAmount += expense.amount;
                                vatTotalAmount += expense.VAT;

                                VATPaid.Add(expense);
                                }
                                VATPaid = VATPaid.OrderBy(x => x.dateSort).ToList();

                            ViewBag.totalAmountPaid = totalAmount.ToString("#,0.00", nfi);
                            ViewBag.vatTotalAmountPaid = vatTotalAmount.ToString("#,0.00", nfi);


                            viewModel.period = item;
                    ViewBag.PeriodID = item.PeriodID;
                        }
                        }

                    if(ViewBag.VatPeriod == null)
                        return RedirectToAction("Error", "Shared", new { Err="An error occurred loading data for vat period" });

                    viewModel.VATDashboard = dashboard;
                    viewModel.VATRecivedList = VATRecived;
                    viewModel.VATPaid = VATPaid;

                    return View(viewModel);
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Vat Center");
                        return RedirectToAction("Error", "Shared", new { Err="An error occurred loading the vat center" });
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

                return RedirectToAction("VatCenter", "Vat", new{period=Request.Form["VatPeriodList"].ToString()});
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Vat Center");
                return RedirectToAction("Error", "Shared", new { Err="An error occurred loading the vat center"});
            }
        }
    }
}