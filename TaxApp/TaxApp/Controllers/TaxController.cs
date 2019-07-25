using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Model;

namespace TaxApp.Controllers
{
    public class TaxController : Controller
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

        #region Tax Center
        public ActionResult TaxCenter(string view, string period)
        {
            getCookie();

            try
            {
                TaxDashboard dashboard = null;
                List<TAXRecivedList> VATRecived = null;

                Profile profileID = new Profile();
                profileID.ProfileID = int.Parse(cookie["ID"]);

                List<TaxAndVatPeriods> taxPeriod = handler.getTaxOrVatPeriodForProfile(profileID, 'T');

                if (taxPeriod == null || taxPeriod.Count == 0)
                {
                    Response.Redirect("../Tax/TaxVatPeriod?Type=T");
                }
                else
                {
                    /**
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
    **/
                    }

                    if (ViewBag.VatPeriod == null)
                    {
                        Response.Redirect("../Shared/Error?Err=An error occurred loading data for tax period");
                    }

                    //VatCenter viewModel = new VatCenter();
                    //viewModel.VATDashboard = dashboard;
                    //viewModel.VATRecivedList = VATRecived;
                    //viewModel.VATPaid = VATPaid;

                    return View();
                //}

                return Redirect("../Shared/Error");
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Tax Center");
                return Redirect("../Shared/Error?Err=An error occurred loading the Tax center");
            }
        }
        [HttpPost]
        public ActionResult TaxCenter(FormCollection collection, string view, string period)
        {
            getCookie();

            try
            {
                Profile profileID = new Profile();
                profileID.ProfileID = int.Parse(cookie["ID"]);

                List<TaxAndVatPeriods> taxPeriod = handler.getTaxOrVatPeriodForProfile(profileID, 'V');

                Response.Redirect("../Tax/TaxCenter?period=" + Request.Form["TaxPeriodList"].ToString());

                return Redirect("../Shared/Error?Err=An error occurred updating the Tax period");
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Tax Center");
                return Redirect("../Shared/Error?Err=An error occurred loading the Tax center");
            }
        }
        #endregion

        #region TaxVatPeriod
        public ActionResult TaxVatPeriod(string type)
        {
            if(type == null || type == "")
            {
                Response.Redirect("../Shared/Error");
            }
            else
            {
                if (type == "V")
                {
                    ViewBag.Type = "VAT";
                }
                else if (type == "T")
                {
                    ViewBag.Type = "Tax";
                }
                else
                {
                    Response.Redirect("../Shared/Error");
                }

                return View();
            }

            return Redirect("../Shared/Error");
        }

        [HttpPost]
        public ActionResult TaxVatPeriod(FormCollection collection, string type)
        {
            getCookie();
            try
            {
                if (type == "V")
                {
                    ViewBag.Type = "VAT";
                }
                else if (type == "T")
                {
                    ViewBag.Type = "Tax";
                }

                if (type == null || type == "")
                {
                    Response.Redirect("../Shared/Error?Err=Error creating TAX or VAT Period");
                }
                else
                {
                    TaxAndVatPeriods period = new TaxAndVatPeriods();

                    period.StartDate = DateTime.Parse(Request.Form["StartDate"]);
                    period.EndDate = DateTime.Parse(Request.Form["EndDate"]);
                    period.Type = type[0];
                    period.ProfileID = int.Parse(cookie["ID"]);

                    bool result = handler.newTaxOrVatPeriod(period);

                    if(result == true)
                    {
                        if (type == "V")
                        {
                            Response.Redirect("../Vat/VatCenter");
                        }
                        else if (type == "T")
                        {
                            Response.Redirect("../Tax/TaxBrakets?ID="+handler.SP_GetLatestTaxAndVatPeriodID().PeriodID+ "&period="+
                                period.StartDate.ToString("dd MMM yyyy")+" - "+ period.EndDate.ToString("dd MMM yyyy"));
                        }
                        else
                        {
                            Response.Redirect("../Shared/Error?Err=Error creating TAX or VAT Period");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Shared/Error?Err=Error creating TAX or VAT Period");
                    }
                }

                return Redirect("../Shared/Error");
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error creating TAX or VAT Period");
                return Redirect("../Shared/Error?Err=Error creating TAX or VAT Period");
            }
        }
        #endregion

        #region Tax Brakets
        public ActionResult TaxBrakets(string ID, string period)
        {
            TaxBraketsView view = new TaxBraketsView();
            view.getRate = null;
            view.setRate = null;

            try
            {
                ViewBag.Period = period;
                ViewBag.ID = ID;

                TaxAndVatPeriods PeriodID = new TaxAndVatPeriods();
                PeriodID.PeriodID = int.Parse(ID);
                List<TaxPeriodRates> brakets = handler.getTaxPeriodBrakets(PeriodID);

                view.getRate = brakets;
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding new Tax Brakets");
                return Redirect("../Shared/Error?Err=An error occurred loading the new tax braket page");
            }

            return View(view);
        }

        [HttpPost]
        public ActionResult TaxBrakets(FormCollection collection, string ID, string period)
        {
            TaxBraketsView view = new TaxBraketsView();
            view.getRate = null;
            view.setRate = null;

            try
            {
                TaxAndVatPeriods PeriodID = new TaxAndVatPeriods();
                PeriodID.PeriodID = int.Parse(ID);
                List<TaxPeriodRates> brakets = handler.getTaxPeriodBrakets(PeriodID);

                TaxPeriodRates newRate = new TaxPeriodRates();

                newRate.Rate = decimal.Parse(Request.Form["setRate.Rate"].ToString());
                newRate.Threashold = decimal.Parse(Request.Form["setRate.Threashold"].ToString());
                if (brakets.Count<1)
                {
                newRate.Type =  Request.Form["Type"].ToString()[0];
                }
                else
                {
                newRate.Type =  'I';
                }
                newRate.PeriodID = int.Parse(ID);

                bool result = handler.newPeriodTaxBraket(newRate);

                if(result == true && newRate.Type == 'I')
                {
                    ViewBag.Period = period;
                    ViewBag.ID = ID;

                    brakets = handler.getTaxPeriodBrakets(PeriodID);

                    view.getRate = brakets;
                }
                else if (result == true && newRate.Type == 'C')
                {
                    Response.Redirect("../Tax/TaxCenter?period=" + ID);
                }
                else
                {
                    function.logAnError("Error creating new Tax Braket");
                    return Redirect("../Shared/Error?Err=An error occurred creating new tax braket");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding new Tax Brakets");
                return Redirect("../Shared/Error?Err=An error occurred loading the new tax braket page");
            }

            return View(view);
        }
        #endregion
    }
}