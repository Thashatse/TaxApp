using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;
using System.Globalization;

namespace TaxApp.Controllers
{
    public class ReportController : Controller
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

        // GET: Report
        public ActionResult Reports()
        {
            getCookie();

            Profile profile = new Model.Profile();
            profile.ProfileID = int.Parse(cookie["ID"].ToString());

            DashboardIncomeExpense IncomeExpense = handler.getDashboardIncomeExpense(profile);

            List<TaxAndVatPeriods> taxAndvatPeriod = handler.getTaxOrVatPeriodForProfile(profile, 'T');
            TaxDashboard Tax = handler.getTaxCenterDashboard(profile, taxAndvatPeriod[0]);

            taxAndvatPeriod = handler.getTaxOrVatPeriodForProfile(profile, 'V');
            VATDashboard Vat = handler.getVatCenterDashboard(profile, taxAndvatPeriod[0]);

            ReportsViewModel viewModel = new ReportsViewModel();
            viewModel.DashboardIncomeExpense = IncomeExpense;
            viewModel.TAXDashboard = Tax;
            viewModel.VATDashboard = Vat;

            return View(viewModel);
        }

        public ReportViewModel getReportData(string ID, string StartDateRange, string EndDateRange, string period)
        {
            ReportViewModel report = null;

            Profile ProfileID = new Profile();
            ProfileID.ProfileID = int.Parse(cookie["ID"].ToString());

            DateTime sDate = DateTime.Now.AddMonths(-12);
            DateTime eDate = DateTime.Now;

            if (StartDateRange != null && EndDateRange != null
                && DateTime.TryParse(StartDateRange, out sDate) && DateTime.TryParse(EndDateRange, out eDate)) { }

            if (sDate > eDate)
            {
                DateTime temp = sDate;
                sDate = eDate;
                eDate = temp;
            }

            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";

            //Jobs report
            if (ID == "0001")
            {
                report = new ReportViewModel();
                List<SP_GetJob_Result> jobsReport = null;

                try
                {
                    jobsReport = handler.getJobsReport(ProfileID, sDate, eDate);
                }
                catch(Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0001 in reports controler");
                }

                if (jobsReport != null)
                {
                    report.reportTitle = "Jobs Report";
                    report.reportCondition = "For date range: " + sDate.ToString("dd MMM yyyy") + " - " + eDate.ToString("dd MMM yyyy");
                    report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                    report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                    report.column1Name = "Start Date";
                    report.column2Name = "Client";
                    report.column3Name = "Job";
                    report.column4Name = "Income";
                    report.column5Name = "Expenses";
                    report.column6Name = "Earnings (before Tax & VAT)";

                    report.ReportDataList = new List<ReportDataList>();

                    decimal c4Total = 0;
                    decimal c5Total = 0;
                    decimal c6Total = 0;

                    foreach (SP_GetJob_Result job in jobsReport)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (job.StartDateString);
                        Data.column2Data = (job.ClientFirstName);
                        Data.column3Data = (job.JobTitle);
                        Data.column4Data = ("R " + job.TotalPaidString);
                        Data.column5Data = ("R " + job.AllExpenseTotalString);
                        Data.column6Data = ("R " + (job.TotalPaid - job.AllExpenseTotal).ToString("#,0.##", nfi));

                        report.ReportDataList.Add(Data);

                        c4Total += job.TotalPaid;
                        c5Total += job.AllExpenseTotal;
                        c6Total += (job.TotalPaid - job.AllExpenseTotal);
                    }

                    report.column4Total = ("R " + c4Total.ToString("#,0.##", nfi));
                    report.column5Total = ("R " + c5Total.ToString("#,0.##", nfi));
                    report.column6Total = ("R " + c6Total.ToString("#,0.##", nfi));
                }
                else
                    report = null;
            }
            //Income report
            else if (ID == "0002")
            {
                report = new ReportViewModel();
                List<TAXorVATRecivedList> IncomeRecivedReport = null;

                TaxPeriodRates rate = new TaxPeriodRates();
                rate.Rate = 0;
                TaxAndVatPeriods dates = new TaxAndVatPeriods();
                dates.StartDate = sDate;
                dates.EndDate = eDate;

                try
                {
                    IncomeRecivedReport = handler.getTAXRecivedList(ProfileID, dates, rate);
                }
                catch(Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0002 in reports controler");
                }

                if (IncomeRecivedReport != null)
                {
                    report.reportTitle = "Income Report";
                    report.reportCondition = "For date range: " + sDate.ToString("dd MMM yyyy") + " - " + eDate.ToString("dd MMM yyyy");
                    report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                    report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                    report.column1Name = "Invoice Date";
                    report.column2Name = "Client";
                    report.column3Name = "Job";
                    report.column4Name = "Income (before Tax & VAT)";

                    report.ReportDataList = new List<ReportDataList>();

                    decimal c4Total = 0;

                    foreach (TAXorVATRecivedList item in IncomeRecivedReport)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.InvoiceDateString);
                        Data.column2Data = (item.clientName);
                        Data.column3Data = (item.JobTitle);
                        Data.column4Data = ("R " + item.TotalString);

                        report.ReportDataList.Add(Data);

                        c4Total += item.Total;
                    }

                    report.column4Total = ("R " + c4Total.ToString("#,0.##", nfi));
                }
                else
                    report = null;
            }
            //Earning report
            else if (ID == "0003")
            {
                report = new ReportViewModel();
                List<Model.DashboardExpense> ExpenseReport = null;

                TaxAndVatPeriods dates = new TaxAndVatPeriods();
                dates.StartDate = sDate;
                dates.EndDate = eDate;

                try
                {
                    ExpenseReport = new List<Model.DashboardExpense>();
                    List<Model.TravelLog> ProfileTravelLog = handler.getProfileTravelLog(ProfileID, sDate, eDate);
                    List<Model.SP_GetJobExpense_Result> ProfileJobExpenses = handler.getAllJobExpense(ProfileID, sDate, eDate);
                    List<Model.SP_GetGeneralExpense_Result> ProfileGeneralExpenses = handler.getGeneralExpenses(ProfileID, sDate, eDate);
                    foreach (Model.TravelLog expenseItem in ProfileTravelLog)
                    {
                        Model.DashboardExpense expense = new Model.DashboardExpense();

                        expense.name = expenseItem.Reason;
                        expense.date = expenseItem.DateString;
                        expense.amount = expenseItem.ClientCharge;
                        expense.TotalString = expense.amount.ToString("#,0.##", nfi);

                        ExpenseReport.Add(expense);
                    }
                    foreach (Model.SP_GetJobExpense_Result expenseItem in ProfileJobExpenses)
                    {
                        Model.DashboardExpense expense = new Model.DashboardExpense();

                        expense.name = expenseItem.Name;
                        expense.date = expenseItem.DateString;
                        expense.amount = expenseItem.Amount;
                        expense.TotalString = expense.amount.ToString("#,0.##", nfi);

                        ExpenseReport.Add(expense);
                    }
                    foreach (Model.SP_GetGeneralExpense_Result expenseItem in ProfileGeneralExpenses)
                    {
                        Model.DashboardExpense expense = new Model.DashboardExpense();

                        expense.name = expenseItem.Name;
                        expense.date = expenseItem.DateString;
                        expense.amount = expenseItem.Amount;
                        expense.TotalString = expense.amount.ToString("#,0.##", nfi);

                        ExpenseReport.Add(expense);
                    }
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0003 in reports controler");
                }

                if (ExpenseReport != null)
                {
                    report.reportTitle = "Expense Report";
                    report.reportCondition = "For date range: " + sDate.ToString("dd MMM yyyy") + " - " + eDate.ToString("dd MMM yyyy");
                    report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                    report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                    report.column1Name = "Date";
                    report.column2Name = "Title";
                    report.column3Name = "Amount";

                    report.ReportDataList = new List<ReportDataList>();

                    decimal c3Total = 0;

                    foreach (DashboardExpense item in ExpenseReport)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.date);
                        Data.column2Data = (item.name);
                        Data.column3Data = ("R " + item.TotalString);

                        report.ReportDataList.Add(Data);

                        c3Total += item.amount;
                    }

                    report.column3Total = ("R " + c3Total.ToString("#,0.##", nfi));
                }
                else
                    report = null;
            }
            //Net income report
            else if (ID == "0004")
            {
                report = new ReportViewModel();
                List<Model.DashboardExpense> ExpenseReport = null;
                List<TAXorVATRecivedList> IncomeRecivedReport = null;

                TaxPeriodRates rate = new TaxPeriodRates();
                rate.Rate = 0;
                TaxAndVatPeriods dates = new TaxAndVatPeriods();
                dates.StartDate = sDate;
                dates.EndDate = eDate;
                TaxDashboard footers = null;

                try
                {
                    List<TaxAndVatPeriods> taxPeriod = handler.getTaxOrVatPeriodForProfile(ProfileID, 'T');

                    if (taxPeriod == null || taxPeriod.Count == 0)
                    {
                        Response.Redirect("../Tax/TaxVatPeriod?Type=T");
                    }
                    else
                    {
                        ViewBag.DropDownFilter = new SelectList(taxPeriod, "PeriodID", "PeriodString");

                        if (period == null || period == "")
                            period = taxPeriod[0].PeriodID.ToString();

                        ViewBag.period = period;

                        foreach (TaxAndVatPeriods item in taxPeriod)
                        {
                            if (item.PeriodID.ToString() == period)
                            {
                                dates = item;
                                sDate = item.StartDate;
                                eDate = item.EndDate;
                            }
                        }

                        IncomeRecivedReport = handler.getTAXRecivedList(ProfileID, dates, rate);
                        ExpenseReport = new List<Model.DashboardExpense>();
                        List<Model.TravelLog> ProfileTravelLog = handler.getProfileTravelLog(ProfileID, sDate, eDate);
                        List<Model.SP_GetJobExpense_Result> ProfileJobExpenses = handler.getAllJobExpense(ProfileID, sDate, eDate);
                        List<Model.SP_GetGeneralExpense_Result> ProfileGeneralExpenses = handler.getGeneralExpenses(ProfileID, sDate, eDate);
                        foreach (Model.TravelLog expenseItem in ProfileTravelLog)
                        {
                            Model.DashboardExpense expense = new Model.DashboardExpense();

                            expense.name = expenseItem.Reason;
                            expense.date = expenseItem.DateString;
                            expense.amount = expenseItem.ClientCharge;
                            expense.TotalString = expense.amount.ToString("#,0.##", nfi);

                            ExpenseReport.Add(expense);
                        }
                        foreach (Model.SP_GetJobExpense_Result expenseItem in ProfileJobExpenses)
                        {
                            Model.DashboardExpense expense = new Model.DashboardExpense();

                            expense.name = expenseItem.Name;
                            expense.date = expenseItem.DateString;
                            expense.amount = expenseItem.Amount;
                            expense.TotalString = expense.amount.ToString("#,0.##", nfi);

                            ExpenseReport.Add(expense);
                        }
                        foreach (Model.SP_GetGeneralExpense_Result expenseItem in ProfileGeneralExpenses)
                        {
                            Model.DashboardExpense expense = new Model.DashboardExpense();

                            expense.name = expenseItem.Name;
                            expense.date = expenseItem.DateString;
                            expense.amount = expenseItem.Amount;
                            expense.TotalString = expense.amount.ToString("#,0.##", nfi);

                            ExpenseReport.Add(expense);
                        }
                        footers = handler.getTaxCenterDashboard(ProfileID, dates);
                    }
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0003 in reports controler");
                }

                if (ExpenseReport != null && IncomeRecivedReport != null && footers != null)
                {
                    report.reportTitle = "Net income report";
                    report.reportCondition = "For date range: " + sDate.ToString("dd MMM yyyy") + " - " + eDate.ToString("dd MMM yyyy");
                    report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                    report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                    report.column1Name = "Date";
                    report.column2Name = "Title";
                    report.column3Name = "Expense Amount";
                    report.column4Name = "Income Amount";

                    report.ReportDataList = new List<ReportDataList>();

                    decimal c3Total = 0;
                    decimal c4Total = 0;

                    foreach (DashboardExpense item in ExpenseReport)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.date);
                        Data.column2Data = (item.name);
                        Data.column3Data = ("-R " + item.TotalString);
                        Data.column4Data = ("");

                        report.ReportDataList.Add(Data);

                        c3Total += item.amount*-1;
                    }

                    foreach (TAXorVATRecivedList item in IncomeRecivedReport)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.InvoiceDateString);
                        Data.column2Data = (item.JobTitle + " for " + item.clientName);
                        Data.column3Data = ("");
                        Data.column4Data = ("R " + item.TotalString);

                        report.ReportDataList.Add(Data);

                        c4Total += item.Total;
                    }

                    report.column3Total = ("R " + c3Total.ToString("#,0.##", nfi));
                    report.column4Total = ("R " + c4Total.ToString("#,0.##", nfi));

                    report.FooterRowList = new List<ReportFixedFooterRowList>();
                    ReportFixedFooterRowList fotter = new ReportFixedFooterRowList();
                    fotter.column1Data = "Subtotal:";
                    fotter.column4Data = ("R " + (footers.Income).ToString("#,0.##", nfi));
                    report.FooterRowList.Add(fotter);
                    fotter = new ReportFixedFooterRowList();
                    fotter.column1Data = "Income Tax " + footers.TaxBraketString + " (Est):";
                    fotter.column4Data = ("R " + footers.TAXOwedSTRING);
                    report.FooterRowList.Add(fotter);
                    fotter = new ReportFixedFooterRowList();
                    fotter.column1Data = "Net income:";
                    fotter.column4Data = ("R " + (footers.Income - footers.TAXOwed).ToString("#,0.##", nfi));
                    report.FooterRowList.Add(fotter);
                }
                else
                    report = null;
            }

            return report;
        }
        public ActionResult DisplayReport(string StartDateRange, string EndDateRange, string SortBy, string SortDirection, string period, string reportID = "0")
        {
            getCookie();

            string ID = reportID;

            Profile ProfileID = new Profile();
            ProfileID.ProfileID = int.Parse(cookie["ID"].ToString());

            if (ID == "0")
            {
                function.logAnError("No report ID Supplied display report");
                return RedirectToAction("Reports", "Report");
            }

            ReportViewModel report = getReportData(ID, StartDateRange, EndDateRange, period);

            if(report == null)
                return RedirectToAction("Error", "Shared");

            ViewBag.StartDateRange = report.reportStartDate;
            ViewBag.EndDateRange = report.reportEndDate;
            ViewBag.reportID = reportID;

            #region Sort
            if (SortDirection == null || SortDirection == "")
                SortDirection = "A";

            if (SortDirection == "D")
            {
            if(SortBy == "Col1")
                report.ReportDataList = report.ReportDataList.OrderByDescending(o => o.column1Data).ToList();
            else if (SortBy == "Col2")
                report.ReportDataList = report.ReportDataList.OrderByDescending(o => o.column2Data).ToList();
            else if (SortBy == "Col3")
                report.ReportDataList = report.ReportDataList.OrderByDescending(o => o.column3Data).ToList();
            else if (SortBy == "Col4")
                report.ReportDataList = report.ReportDataList.OrderByDescending(o => o.column4Data).ToList();
            else if (SortBy == "Col5")
                report.ReportDataList = report.ReportDataList.OrderByDescending(o => o.column5Data).ToList();
            else if (SortBy == "Col6")
                report.ReportDataList = report.ReportDataList.OrderByDescending(o => o.column6Data).ToList();

                SortDirection = "A";
            }
            else
            {
            if(SortBy == "Col1")
                report.ReportDataList = report.ReportDataList.OrderBy(o => o.column1Data).ToList();
            else if (SortBy == "Col2")
                report.ReportDataList = report.ReportDataList.OrderBy(o => o.column2Data).ToList();
            else if (SortBy == "Col3")
                report.ReportDataList = report.ReportDataList.OrderBy(o => o.column3Data).ToList();
            else if (SortBy == "Col4")
                report.ReportDataList = report.ReportDataList.OrderBy(o => o.column4Data).ToList();
            else if (SortBy == "Col5")
                report.ReportDataList = report.ReportDataList.OrderBy(o => o.column5Data).ToList();
            else if (SortBy == "Col6")
                report.ReportDataList = report.ReportDataList.OrderBy(o => o.column6Data).ToList();

                SortDirection = "D";
            }

            ViewBag.SortDirection = SortDirection;
            #endregion

            return View(report);
        }
        [HttpPost]

        public ActionResult DisplayReport(FormCollection collection, string StartDateRange, string EndDateRange, string SortBy, string period, string reportID = "0")
        {
            try
            {
                DateTime sDate = DateTime.Now.AddMonths(-12);
                DateTime eDate = DateTime.Now;

                DateTime.TryParse(Request.Form["StartDate"], out sDate);
                DateTime.TryParse(Request.Form["EndDate"], out eDate);

                StartDateRange = sDate.ToString("yyyy-MM-dd");
                EndDateRange = eDate.ToString("yyyy-MM-dd");

                return RedirectToAction("DisplayReport", "Report", new
                {
                    StartDateRange,
                    EndDateRange,
                    reportID,
                    SortBy,
                    period
                });
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error updating date range for display reports page");
                return RedirectToAction("../Shared/Error");
            }
        }

        public ActionResult PrintReport(string StartDateRange, string EndDateRange, string period, string reportID = "0")
        {
            getCookie();

            string ID = reportID;

            if (ID == "0")
            {
                function.logAnError("No report ID Supplied Print report");
                Response.Redirect("/Report/Reports");
            }

            return View(getReportData(ID, StartDateRange, EndDateRange, period));
        }
    }
}