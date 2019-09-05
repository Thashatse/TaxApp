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

        public ActionResult Reports()
        {
            getCookie();

            Profile profile = new Model.Profile();
            profile.ProfileID = int.Parse(cookie["ID"].ToString());

            DashboardIncomeExpense IncomeExpense = handler.getDashboardIncomeExpense(profile);

            List<TaxAndVatPeriods> taxAndvatPeriod = handler.getTaxOrVatPeriodForProfile(profile, 'T');
            if (taxAndvatPeriod == null || taxAndvatPeriod.Count == 0)
                return RedirectToAction("TaxVatPeriod", "Tax", new
                {
                    Type = "T"
                });
            TaxDashboard Tax = handler.getTaxCenterDashboard(profile, taxAndvatPeriod[0]);

            taxAndvatPeriod = handler.getTaxOrVatPeriodForProfile(profile, 'V');
            if (taxAndvatPeriod == null || taxAndvatPeriod.Count == 0)
                return RedirectToAction("TaxVatPeriod", "Tax", new
                {
                    Type = "V"
                });
            VATDashboard Vat = handler.getVatCenterDashboard(profile, taxAndvatPeriod[0]);

            ReportsViewModel viewModel = new ReportsViewModel();
            viewModel.DashboardIncomeExpense = IncomeExpense;
            viewModel.TAXDashboard = Tax;
            viewModel.VATDashboard = Vat;

            return View(viewModel);
        }

        public ReportViewModel getReportData(string ID, string StartDateRange, string EndDateRange, string DropDownID)
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
                    report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                    report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                    report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                    report.column1Name = "Start Date";
                    report.column2Name = "Client";
                    report.column3Name = "Job";
                    report.column4Name = "Income (R)";
                    report.column5Name = "Expenses (R)";
                    report.column6Name = "Earnings (before Tax & VAT) (R)";
                    report.column4DataAlignRight = true;
                    report.column5DataAlignRight = true;
                    report.column6DataAlignRight = true;

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
                        Data.column4Data = (job.TotalPaidString);
                        Data.column5Data = (job.AllExpenseTotalString);
                        Data.column6Data = ((job.TotalPaid - job.AllExpenseTotal).ToString("#,0.00", nfi));

                        report.ReportDataList.Add(Data);

                        c4Total += job.TotalPaid;
                        c5Total += job.AllExpenseTotal;
                        c6Total += (job.TotalPaid - job.AllExpenseTotal);
                    }

                    report.column4Total = (c4Total.ToString("#,0.00", nfi));
                    report.column5Total = (c5Total.ToString("#,0.00", nfi));
                    report.column6Total = (c6Total.ToString("#,0.00", nfi));
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
                    report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                    report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                    report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                    report.column1Name = "Invoice Date";
                    report.column2Name = "Client";
                    report.column3Name = "Job";
                    report.column4Name = "Income (before Tax & VAT) [R]";
                    report.column4DataAlignRight = true;

                    report.ReportDataList = new List<ReportDataList>();

                    decimal c4Total = 0;

                    foreach (TAXorVATRecivedList item in IncomeRecivedReport)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.InvoiceDateString);
                        Data.column2Data = (item.clientName);
                        Data.column3Data = (item.JobTitle);
                        Data.column4Data = (item.TotalString);

                        report.ReportDataList.Add(Data);

                        c4Total += item.Total;
                    }

                    report.column4Total = (c4Total.ToString("#,0.00", nfi));
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
                        expense.TotalString = expense.amount.ToString("#,0.00", nfi);

                        ExpenseReport.Add(expense);
                    }
                    foreach (Model.SP_GetJobExpense_Result expenseItem in ProfileJobExpenses)
                    {
                        Model.DashboardExpense expense = new Model.DashboardExpense();

                        expense.name = expenseItem.Name;
                        expense.date = expenseItem.DateString;
                        expense.amount = expenseItem.Amount;
                        expense.TotalString = expense.amount.ToString("#,0.00", nfi);

                        ExpenseReport.Add(expense);
                    }
                    foreach (Model.SP_GetGeneralExpense_Result expenseItem in ProfileGeneralExpenses)
                    {
                        Model.DashboardExpense expense = new Model.DashboardExpense();

                        expense.name = expenseItem.Name;
                        expense.date = expenseItem.DateString;
                        expense.amount = expenseItem.Amount;
                        expense.TotalString = expense.amount.ToString("#,0.00", nfi);

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
                    report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                    report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                    report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                    report.column1Name = "Date";
                    report.column2Name = "Title";
                    report.column3Name = "Amount (R)";
                    report.column3DataAlignRight = true;

                    report.ReportDataList = new List<ReportDataList>();

                    decimal c3Total = 0;

                    foreach (DashboardExpense item in ExpenseReport)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.date);
                        Data.column2Data = (item.name);
                        Data.column3Data = (item.TotalString);

                        report.ReportDataList.Add(Data);

                        c3Total += item.amount;
                    }

                    report.column3Total = (c3Total.ToString("#,0.00", nfi));
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

                        if (DropDownID == null || DropDownID == "")
                            DropDownID = taxPeriod[0].PeriodID.ToString();

                        ViewBag.DropDownID = DropDownID;

                        foreach (TaxAndVatPeriods item in taxPeriod)
                        {
                            if (item.PeriodID.ToString() == DropDownID)
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
                            expense.TotalString = expense.amount.ToString("#,0.00", nfi);

                            ExpenseReport.Add(expense);
                        }
                        foreach (Model.SP_GetJobExpense_Result expenseItem in ProfileJobExpenses)
                        {
                            Model.DashboardExpense expense = new Model.DashboardExpense();

                            expense.name = expenseItem.Name;
                            expense.date = expenseItem.DateString;
                            expense.amount = expenseItem.Amount;
                            expense.TotalString = expense.amount.ToString("#,0.00", nfi);

                            ExpenseReport.Add(expense);
                        }
                        foreach (Model.SP_GetGeneralExpense_Result expenseItem in ProfileGeneralExpenses)
                        {
                            Model.DashboardExpense expense = new Model.DashboardExpense();

                            expense.name = expenseItem.Name;
                            expense.date = expenseItem.DateString;
                            expense.amount = expenseItem.Amount;
                            expense.TotalString = expense.amount.ToString("#,0.00", nfi);

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
                    report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                    report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                    report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                    report.column1Name = "Date";
                    report.column2Name = "Title";
                    report.column3Name = "Expense Amount (R)";
                    report.column4Name = "Income Amount (R)";
                    report.column3DataAlignRight = true;
                    report.column4DataAlignRight = true;

                    report.ReportDataList = new List<ReportDataList>();

                    decimal c3Total = 0;
                    decimal c4Total = 0;

                    foreach (DashboardExpense item in ExpenseReport)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.date);
                        Data.column2Data = (item.name);
                        Data.column3Data = ("-" + item.TotalString);
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
                        Data.column4Data = (item.TotalString);

                        report.ReportDataList.Add(Data);

                        c4Total += item.Total;
                    }

                    report.column3Total = (c3Total.ToString("#,0.00", nfi));
                    report.column4Total = (c4Total.ToString("#,0.00", nfi));

                    report.FooterRowList = new List<ReportFixedFooterRowList>();
                    ReportFixedFooterRowList fotter = new ReportFixedFooterRowList();
                    fotter.column1Data = "Subtotal:";
                    fotter.column4Data = ((footers.Income).ToString("#,0.00", nfi));
                    report.FooterRowList.Add(fotter);
                    fotter = new ReportFixedFooterRowList();
                    fotter.column1Data = "Income Tax " + footers.TaxBraketString + " (Est):";
                    fotter.column4Data = (footers.TAXOwedSTRING);
                    report.FooterRowList.Add(fotter);
                    fotter = new ReportFixedFooterRowList();
                    fotter.column1Data = "Net income:";
                    fotter.column4Data = ((footers.Income - footers.TAXOwed).ToString("#,0.00", nfi));
                    report.FooterRowList.Add(fotter);
                    report.column4FotterAlignRight = true;
                }
                else
                    report = null;
            }
            //Client Income and Expenses
            else if (ID == "0005")
            {
                report = new ReportViewModel();

                try
                {
                    report = handler.getClientReport(ProfileID, sDate, eDate);
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0005 in reports controler");
                }

                if (report.ReportDataList == null)
                    report = null;
            }
            //Client Income
            else if (ID == "0006")
            {
                report = new ReportViewModel();

                try
                {
                    Client ProfileIDClient = new Client();
                    ProfileIDClient.ProfileID = ProfileID.ProfileID;
                    List<Client> clients = handler.getProfileClients(ProfileIDClient);
                    clients = clients.OrderBy(o => o.FirstName).ToList();
                    clients.Insert(0, new Client { FirstName = "All", ClientID = 0 });

                    ViewBag.DropDownFilter = new SelectList(clients, "ClientID", "FirstName");
                    ViewBag.AlsoShowDate = true;

                        if (DropDownID == null || DropDownID == "")
                            DropDownID = clients[0].ClientID.ToString();

                        ViewBag.DropDownID = DropDownID;

                    if (DropDownID != "0")
                        report = handler.getIncomeByClientReport(ProfileID, sDate, eDate, DropDownID);
                    else
                        report = handler.getIncomeByClientReport(ProfileID, sDate, eDate);
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0005 in reports controler");
                }

                if (report.ReportDataList == null)
                    report = null;
            }
            //Client Expenses
            else if (ID == "0007")
            {
                report = new ReportViewModel();

                try
                {
                    Client ProfileIDClient = new Client();
                    ProfileIDClient.ProfileID = ProfileID.ProfileID;
                    List<Client> clients = handler.getProfileClients(ProfileIDClient);
                    clients = clients.OrderBy(o => o.FirstName).ToList();
                    clients.Insert(0, new Client { FirstName = "All", ClientID = 0 });

                    ViewBag.DropDownFilter = new SelectList(clients, "ClientID", "FirstName");
                    ViewBag.AlsoShowDate = true;

                    if (DropDownID == null || DropDownID == "")
                        DropDownID = clients[0].ClientID.ToString();

                    ViewBag.DropDownID = DropDownID;

                    if (DropDownID != "0")
                        report = handler.getExpensesByClientReport(ProfileID, sDate, eDate, DropDownID);
                    else
                        report = handler.getExpensesByClientReport(ProfileID, sDate, eDate);
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0005 in reports controler");
                }

                if (report.ReportDataList == null)
                    report = null;
            }
            //Expenses Report
            else if (ID == "0008")
            {
                report = new ReportViewModel();

                List<Model.TravelLog> ProfileTravelLog = null;
                List<Model.SP_GetJobExpense_Result> ProfileJobExpenses = null;
                List<Model.SP_GetGeneralExpense_Result> ProfileGeneralExpenses = null;

                TaxAndVatPeriods dates = new TaxAndVatPeriods();
                dates.StartDate = sDate;
                dates.EndDate = eDate;

                try
                {
                    ProfileTravelLog = handler.getProfileTravelLog(ProfileID, sDate, eDate);
                    ProfileJobExpenses = handler.getAllJobExpense(ProfileID, sDate, eDate);
                    ProfileGeneralExpenses = handler.getGeneralExpensesReport(ProfileID, sDate, eDate);
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0003 in reports controler");
                }

                if (ProfileTravelLog != null && ProfileJobExpenses != null && ProfileGeneralExpenses != null)
                {
                    report.reportTitle = "Expense";
                    report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                    report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                    report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                    report.column1Name = "Date";
                    report.column2Name = "Title";
                    report.column3Name = "Type";
                    report.column4Name = "General Expense Amount (R)";
                    report.column5Name = "Job Expense Amount (R)";
                    report.column6Name = "Travel Expense Amount (R)";
                    report.column4DataAlignRight = true;
                    report.column5DataAlignRight = true;
                    report.column6DataAlignRight = true;

                    report.ReportDataList = new List<ReportDataList>();

                    decimal c4Total = 0;
                    decimal c5Total = 0;
                    decimal c6Total = 0;
                    decimal Total = 0;

                    foreach (TravelLog item in ProfileTravelLog)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.DateString);
                        Data.column2Data = (item.Reason);
                        Data.column3Data = "Travel Expense";
                        Data.column4Data = ("");
                        Data.column5Data = ("");
                        Data.column6Data = (item.ClientCharge.ToString("#,0.00", nfi));
                        Data.column7Data = item.Date.ToString();

                        report.ReportDataList.Add(Data);

                        c4Total += item.ClientCharge;
                        Total += item.ClientCharge;
                    }
                    foreach (SP_GetJobExpense_Result item in ProfileJobExpenses)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.DateString);
                        Data.column2Data = (item.Name);
                        Data.column3Data = "Job Expense";
                        Data.column4Data = ("");
                        Data.column5Data = (item.Amount.ToString("#,0.00", nfi));
                        Data.column6Data = ("");
                        Data.column7Data = item.Date.ToString();

                        report.ReportDataList.Add(Data);

                        c5Total += item.Amount;
                        Total += item.Amount;
                    }
                    foreach (SP_GetGeneralExpense_Result item in ProfileGeneralExpenses)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.DateString);
                        Data.column2Data = (item.Name);
                        Data.column3Data = "General Expense";
                        Data.column4Data = (item.Amount.ToString("#,0.00", nfi));
                        Data.column5Data = ("");
                        Data.column6Data = ("");
                        Data.column7Data = item.Date.ToString();

                        report.ReportDataList.Add(Data);

                        c6Total += item.Amount;
                        Total += item.Amount;
                    }

                    report.ReportDataList = report.ReportDataList.OrderByDescending(o => o.column7Data).ToList();

                    report.column1Total = ("Subtotal:");
                    report.column4Total = (c4Total.ToString("#,0.00", nfi));
                    report.column5Total = (c5Total.ToString("#,0.00", nfi));
                    report.column6Total = (c6Total.ToString("#,0.00", nfi));

                    report.FooterRowList = new List<ReportFixedFooterRowList>();
                    ReportFixedFooterRowList fotter = new ReportFixedFooterRowList();
                    fotter.column5Data = "All Expense Total (R):";
                    fotter.column6Data = ((Total).ToString("#,0.00", nfi));
                    report.FooterRowList.Add(fotter);
                    report.column6FotterAlignRight = true;
                }
                else
                    report = null;
            }
            //General Expenses Report
            else if (ID == "0009")
            {
                report = new ReportViewModel();

                List<Model.SP_GetGeneralExpense_Result> ProfileGeneralExpenses = null;

                TaxAndVatPeriods dates = new TaxAndVatPeriods();
                dates.StartDate = sDate;
                dates.EndDate = eDate;

                try
                {
                    ProfileGeneralExpenses = handler.getGeneralExpensesReport(ProfileID, sDate, eDate);
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0003 in reports controler");
                }

                if (ProfileGeneralExpenses != null)
                {
                    report.reportTitle = "General Expense";
                    report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                    report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                    report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                    report.column1Name = "Date";
                    report.column2Name = "Title";
                    report.column4Name = "Amount (R)";
                    report.column4DataAlignRight = true;

                    report.ReportDataList = new List<ReportDataList>();

                    decimal c4Total = 0;

                    foreach (SP_GetGeneralExpense_Result item in ProfileGeneralExpenses)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.DateString);
                        Data.column2Data = (item.Name);
                        Data.column4Data = (item.Amount.ToString("#,0.00", nfi));
                        Data.column7Data = item.Date.ToString();

                        report.ReportDataList.Add(Data);

                        c4Total += item.Amount;
                    }

                    report.ReportDataList = report.ReportDataList.OrderByDescending(o => o.column7Data).ToList();

                    report.column4Total = (c4Total.ToString("#,0.00", nfi));
                }
                else
                    report = null;
            }
            //Job Expenses Report
            else if (ID == "0010")
            {
                report = new ReportViewModel();

                List<Model.SP_GetJobExpense_Result> ProfileJobExpenses = null;

                TaxAndVatPeriods dates = new TaxAndVatPeriods();
                dates.StartDate = sDate;
                dates.EndDate = eDate;

                try
                {
                    ProfileJobExpenses = handler.getAllJobExpense(ProfileID, sDate, eDate);
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0010 in reports controler");
                }

                if (ProfileJobExpenses != null)
                {
                    report.reportTitle = "Job Expenses";
                    report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                    report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                    report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                    report.column1Name = "Date";
                    report.column2Name = "Title";
                    report.column4Name = "Amount (R)";
                    report.column4DataAlignRight = true;

                    report.ReportDataList = new List<ReportDataList>();

                    decimal c4Total = 0;

                    foreach (SP_GetJobExpense_Result item in ProfileJobExpenses)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.DateString);
                        Data.column2Data = (item.Name);
                        Data.column4Data = (item.Amount.ToString("#,0.00", nfi));
                        Data.column7Data = item.Date.ToString();

                        report.ReportDataList.Add(Data);

                        c4Total += item.Amount;
                    }

                    report.ReportDataList = report.ReportDataList.OrderByDescending(o => o.column7Data).ToList();

                    report.column4Total = (c4Total.ToString("#,0.00", nfi));
                }
                else
                    report = null;
            }
            //Travel Expenses Report
            else if (ID == "0011")
            {
                report = new ReportViewModel();

                List<Model.TravelLog> ProfileTravelLog = null;

                TaxAndVatPeriods dates = new TaxAndVatPeriods();
                dates.StartDate = sDate;
                dates.EndDate = eDate;

                try
                {
                    ProfileTravelLog = handler.getProfileTravelLog(ProfileID, sDate, eDate);
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0011 in reports controler");
                }

                if (ProfileTravelLog != null)
                {
                    report.reportTitle = "Travel Expenses";
                    report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                    report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                    report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                    report.column1Name = "Date";
                    report.column2Name = "Title";
                    report.column4Name = "Amount (R)";
                    report.column4DataAlignRight = true;

                    report.ReportDataList = new List<ReportDataList>();

                    decimal c4Total = 0;

                    foreach (TravelLog item in ProfileTravelLog)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.DateString);
                        Data.column2Data = (item.Reason);
                        Data.column4Data = (item.ClientCharge.ToString("#,0.00", nfi));
                        Data.column7Data = item.Date.ToString();

                        report.ReportDataList.Add(Data);

                        c4Total += item.ClientCharge;
                    }

                    report.ReportDataList = report.ReportDataList.OrderByDescending(o => o.column7Data).ToList();

                    report.column4Total = (c4Total.ToString("#,0.00", nfi));
                }
                else
                    report = null;
            }
            //Tax report
            else if (ID == "0012")
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

                        if (DropDownID == null || DropDownID == "")
                            DropDownID = taxPeriod[0].PeriodID.ToString();

                        ViewBag.DropDownID = DropDownID;

                        foreach (TaxAndVatPeriods item in taxPeriod)
                        {
                            if (item.PeriodID.ToString() == DropDownID)
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
                            expense.TotalString = expense.amount.ToString("#,0.00", nfi);

                            ExpenseReport.Add(expense);
                        }
                        foreach (Model.SP_GetJobExpense_Result expenseItem in ProfileJobExpenses)
                        {
                            Model.DashboardExpense expense = new Model.DashboardExpense();

                            expense.name = expenseItem.Name;
                            expense.date = expenseItem.DateString;
                            expense.amount = expenseItem.Amount;
                            expense.TotalString = expense.amount.ToString("#,0.00", nfi);

                            ExpenseReport.Add(expense);
                        }
                        foreach (Model.SP_GetGeneralExpense_Result expenseItem in ProfileGeneralExpenses)
                        {
                            Model.DashboardExpense expense = new Model.DashboardExpense();

                            expense.name = expenseItem.Name;
                            expense.date = expenseItem.DateString;
                            expense.amount = expenseItem.Amount;
                            expense.TotalString = expense.amount.ToString("#,0.00", nfi);

                            ExpenseReport.Add(expense);
                        }
                        footers = handler.getTaxCenterDashboard(ProfileID, dates);
                    }
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 00012 in reports controler");
                }

                if (ExpenseReport != null && IncomeRecivedReport != null && footers != null)
                {
                    report.reportTitle = "Income Tax";
                    report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                    report.reportSubHeading = "All values in ZAR";
                    report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                    report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                    report.column1Name = "Date";
                    report.column2Name = "Title";
                    report.column3Name = "Income Amount (R)";
                    report.column3DataAlignRight = true;

                    report.ReportDataList = new List<ReportDataList>();

                    decimal c3Total = 0;
                    decimal c7Total = 0;

                    foreach (DashboardExpense item in ExpenseReport)
                    {
                        c7Total += item.amount * -1;
                    }

                    foreach (TAXorVATRecivedList item in IncomeRecivedReport)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.InvoiceDateString);
                        Data.column2Data = (item.JobTitle + " for " + item.clientName);
                        Data.column3Data = (item.TotalString);

                        report.ReportDataList.Add(Data);

                        c3Total += item.Total;
                    }

                    report.FooterRowList = new List<ReportFixedFooterRowList>();
                    ReportFixedFooterRowList fotter = new ReportFixedFooterRowList();
                    fotter.column2Data = "Income:";
                    fotter.column3Data = (c3Total.ToString("#,0.00", nfi));
                    report.FooterRowList.Add(fotter);
                    fotter = new ReportFixedFooterRowList();
                    fotter.column2Data = "Expenses:";
                    fotter.column3Data = (c7Total.ToString("#,0.00", nfi));
                    report.FooterRowList.Add(fotter);
                    fotter = new ReportFixedFooterRowList();
                    fotter.column2Data = "Subtotal:";
                    fotter.column3Data = ((footers.Income).ToString("#,0.00", nfi));
                    report.FooterRowList.Add(fotter);
                    fotter = new ReportFixedFooterRowList();
                    fotter.column2Data = "Income Tax " + footers.TaxBraketString + " (Est):";
                    fotter.column3Data = (footers.TAXOwedSTRING);
                    report.FooterRowList.Add(fotter);
                    report.column3FotterAlignRight = true;
                }
                else
                    report = null;
            }
            //VAT report
            else if (ID == "0013")
            {
                report = new ReportViewModel();

                List<Model.DashboardExpense> ExpenseReport = null;
                VATDashboard footers = null;
                List<TAXorVATRecivedList> VATRecived = null;

                TaxAndVatPeriods VATRate = null;

                TaxAndVatPeriods dates = new TaxAndVatPeriods();
                dates.StartDate = sDate;
                dates.EndDate = eDate;

                try
                {
                    List<TaxAndVatPeriods> vatPeriod = handler.getTaxOrVatPeriodForProfile(ProfileID, 'V');

                    if (vatPeriod == null || vatPeriod.Count == 0)
                    {
                        Response.Redirect("../Tax/TaxVatPeriod?Type=V");
                    }
                    else
                    {
                        ViewBag.DropDownFilter = new SelectList(vatPeriod, "PeriodID", "PeriodString");

                        if (DropDownID == null || DropDownID == "")
                            DropDownID = vatPeriod[0].PeriodID.ToString();

                        ViewBag.DropDownID = DropDownID;

                        foreach (TaxAndVatPeriods item in vatPeriod)
                        {
                            if (item.PeriodID.ToString() == DropDownID)
                            {
                                dates = item;
                                sDate = item.StartDate;
                                eDate = item.EndDate;
                                VATRate = item;
                            }
                        }

                        ExpenseReport = new List<Model.DashboardExpense>();
                        List<Model.TravelLog> ProfileTravelLog = handler.getProfileTravelLog(ProfileID, sDate, eDate);
                        List<Model.SP_GetJobExpense_Result> ProfileJobExpenses = handler.getAllJobExpense(ProfileID, sDate, eDate);
                        List<Model.SP_GetGeneralExpense_Result> ProfileGeneralExpenses = handler.getGeneralExpensesReport(ProfileID, sDate, eDate);
                        foreach (Model.TravelLog expenseItem in ProfileTravelLog)
                        {
                            Model.DashboardExpense expense = new Model.DashboardExpense();

                            expense.name = expenseItem.Reason;
                            expense.date = expenseItem.DateString;
                            expense.amount = expenseItem.ClientCharge;
                            expense.TotalString = expense.amount.ToString("#,0.00", nfi);
                            expense.VAT = ((expense.amount / 100) * VATRate.VATRate);
                            expense.VATString = expense.VAT.ToString("#,0.00", nfi);

                            ExpenseReport.Add(expense);
                        }
                        foreach (Model.SP_GetJobExpense_Result expenseItem in ProfileJobExpenses)
                        {
                            Model.DashboardExpense expense = new Model.DashboardExpense();

                            expense.name = expenseItem.Name;
                            expense.date = expenseItem.DateString;
                            expense.amount = expenseItem.Amount;
                            expense.TotalString = expense.amount.ToString("#,0.00", nfi);
                            expense.VAT = ((expense.amount / 100) * VATRate.VATRate);
                            expense.VATString = expense.VAT.ToString("#,0.00", nfi);

                            ExpenseReport.Add(expense);
                        }
                        foreach (Model.SP_GetGeneralExpense_Result expenseItem in ProfileGeneralExpenses)
                        {
                            Model.DashboardExpense expense = new Model.DashboardExpense();

                            expense.name = expenseItem.Name;
                            expense.date = expenseItem.DateString;
                            expense.amount = expenseItem.Amount;
                            expense.TotalString = expense.amount.ToString("#,0.00", nfi);
                            expense.VAT = ((expense.amount / 100) * VATRate.VATRate);
                            expense.VATString = expense.VAT.ToString("#,0.00", nfi);

                            ExpenseReport.Add(expense);
                        }

                        footers = handler.getVatCenterDashboard(ProfileID, VATRate);

                        VATRecived = handler.getVATRecivedList(ProfileID, VATRate);
                    }
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0003 in reports controler");
                }

                if (ExpenseReport != null && VATRecived != null && footers != null)
                {
                    report.reportTitle = "VAT";
                    report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                    report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                    report.reportSubHeading = "Vat Rate @ "+VATRate.VATRate.ToString("#,0.00", nfi) + "% & all values in ZAR";
                    report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                    report.column1Name = "Date";
                    report.column2Name = "Title";
                    report.column3Name = "Income Amount (R)";
                    report.column4Name = "VAT Received (R)";
                    report.column5Name = "Expense Amount (R)";
                    report.column6Name = "VAT Paid (R)";
                    report.column3DataAlignRight = true;
                    report.column4DataAlignRight = true;
                    report.column5DataAlignRight = true;
                    report.column6DataAlignRight = true;

                    report.ReportDataList = new List<ReportDataList>();

                    decimal c3Total = 0;
                    decimal c4Total = 0;
                    decimal c5Total = 0;
                    decimal c6Total = 0;

                    foreach (DashboardExpense item in ExpenseReport)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.date);
                        Data.column2Data = (item.name);
                        Data.column5Data = ("-" + item.amount.ToString("#,0.00", nfi));
                        Data.column6Data = ("-" + item.VATString);
                        Data.column4Data = ("");
                        Data.column3Data = ("");

                        report.ReportDataList.Add(Data);

                        c5Total += item.amount * -1;
                        c6Total += item.VAT * -1;
                    }

                    foreach (TAXorVATRecivedList item in VATRecived)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.InvoiceDateString);
                        Data.column2Data = (item.JobTitle + " for " + item.clientName);
                        Data.column3Data = (item.TotalString);
                        Data.column4Data = (item.VATorTAXString);
                        Data.column5Data = ("");
                        Data.column6Data = ("");

                        report.ReportDataList.Add(Data);

                        c3Total += item.Total;
                        c4Total += item.VATorTAX;
                    }

                    report.column3Total = (c3Total.ToString("#,0.00", nfi));
                    report.column4Total = (c4Total.ToString("#,0.00", nfi));
                    report.column5Total = (c5Total.ToString("#,0.00", nfi));
                    report.column6Total = (c6Total.ToString("#,0.00", nfi));

                    report.FooterRowList = new List<ReportFixedFooterRowList>();
                    ReportFixedFooterRowList fotter = new ReportFixedFooterRowList();
                    fotter.column5Data = "VAT Owed est.:";
                    fotter.column6Data = ((footers.VATPAIDOutstandingEst).ToString("#,0.00", nfi));
                    report.FooterRowList.Add(fotter);
                    report.column6FotterAlignRight = true;
                }
                else
                    report = null;
            }
            //Job Earning Per Hour Report
            else if (ID == "0014")
            {
                report = new ReportViewModel();

                try
                {
                    Client ProfileIDClient = new Client();
                    ProfileIDClient.ProfileID = ProfileID.ProfileID;

                    report = handler.getJobEarningPerHourReport(ProfileID, sDate, eDate);
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0014 in reports controler");
                }

                if (report.ReportDataList == null)
                    report = null;
            }

            return report;
        }

        public ActionResult DisplayReport(string StartDateRange, string EndDateRange, string SortBy, string SortDirection, string DropDownID, string reportID = "0")
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

            ReportViewModel report = getReportData(ID, StartDateRange, EndDateRange, DropDownID);

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
            else if (SortBy == "Col7")
                report.ReportDataList = report.ReportDataList.OrderByDescending(o => o.column7Data).ToList();

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
            else if (SortBy == "Col7")
                report.ReportDataList = report.ReportDataList.OrderBy(o => o.column7Data).ToList();

                SortDirection = "D";
            }

            ViewBag.SortDirection = SortDirection;
            #endregion

            return View(report);
        }
        [HttpPost]
        public ActionResult DisplayReport(FormCollection collection, string StartDateRange, string EndDateRange, string SortBy, string DropDownID, string reportID = "0")
        {
            try
            {
                DateTime sDate = DateTime.Now.AddMonths(-12);
                DateTime eDate = DateTime.Now;

                DateTime.TryParse(Request.Form["StartDate"], out sDate);
                DateTime.TryParse(Request.Form["EndDate"], out eDate);

                StartDateRange = sDate.ToString("yyyy-MM-dd");
                EndDateRange = eDate.ToString("yyyy-MM-dd");

                if(Request.Form["DropDownFilter"] != null)
                {
                DropDownID = Request.Form["DropDownFilter"].ToString();
                }

                return RedirectToAction("DisplayReport", "Report", new
                {
                    StartDateRange,
                    EndDateRange,
                    reportID,
                    SortBy,
                    DropDownID
                });
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error updating date range for display reports page");
                return RedirectToAction("../Shared/Error");
            }
        }

        public ActionResult PrintReport(string StartDateRange, string EndDateRange, string SortBy, string SortDirection, string DropDownID, string reportID = "0")
        {
            getCookie();

            string ID = reportID;

            if (ID == "0")
            {
                function.logAnError("No report ID Supplied display report");
                return RedirectToAction("Reports", "Report");
            }

            ReportViewModel report = getReportData(ID, StartDateRange, EndDateRange, DropDownID);

            if (report == null)
                return RedirectToAction("Error", "Shared");

            ViewBag.StartDateRange = report.reportStartDate;
            ViewBag.EndDateRange = report.reportEndDate;
            ViewBag.reportID = reportID;

            #region Sort
            if (SortDirection == null || SortDirection == "")
                SortDirection = "A";

            if (SortDirection == "D")
            {
                if (SortBy == "Col1")
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
                if (SortBy == "Col1")
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
    }
}