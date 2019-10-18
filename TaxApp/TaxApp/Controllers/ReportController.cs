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
                Response.Redirect(Url.Action("Error", "Shared") + "?Err=Identity couldn't be verified, please try again later.");
            }
        }

        public ActionResult Reports()
        {
            getCookie();

            Profile profile = new Model.Profile();
            if (cookie == null)
                getCookie();
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

        public ReportViewModel getReportData(string ID, string StartDateRange, string EndDateRange, string DropDownID, string view = "", string DownloadID = "0")
        {
            ReportViewModel report = null;

            Profile ProfileID = new Profile();
            if (DownloadID == "0")
            {
                if (cookie == null)
                    getCookie();
                ProfileID.ProfileID = int.Parse(cookie["ID"].ToString());
            }
            else
            {
                ProfileID.ProfileID = int.Parse(DownloadID);
            }

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

            ViewBag.View = view;

            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";

            //Default Chart Settings
            ViewBag.chartLabel = "''";
            ViewBag.chartLabels = "";
            ViewBag.chartData = "";
            ViewBag.chartPrefix = "";
            ViewBag.chartSufix = "";
            ViewBag.BarChart = false;
            ViewBag.LineChart = false;

            //Jobs report
            if (ID == "0001")
            {
                ViewBag.AlsoShowDate = true;

                report = new ReportViewModel();
                List<SP_GetJob_Result> jobsReport = null;

                try
                {
                    jobsReport = handler.getJobsReport(ProfileID, sDate, eDate);
                }
                catch (Exception e)
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

                    int chartDataCount =0;
                    string chartLabels = "";
                    string chartData = "";

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

                        if(chartDataCount == 0)
                        {
                        chartLabels += "'" + job.JobTitle +" for "+ job.ClientFirstName + "'";
                            chartData += "'" + (job.TotalPaid - job.AllExpenseTotal).ToString("0.00", nfi) + "'";
                        }
                        else
                        {
                            chartLabels += ", '" + job.JobTitle + " for " + job.ClientFirstName + "'";
                            chartData += ", '" + (job.TotalPaid - job.AllExpenseTotal).ToString("0.00", nfi) + "'";
                        }
                        chartDataCount++;
                    }

                    report.column4Total = (c4Total.ToString("#,0.00", nfi));
                    report.column5Total = (c5Total.ToString("#,0.00", nfi));
                    report.column6Total = (c6Total.ToString("#,0.00", nfi));

                    ViewBag.chartLabel = "'Jobs Report'";
                    ViewBag.chartLabels = chartLabels;
                    ViewBag.chartData = chartData;
                    ViewBag.chartPrefix = "R";
                    ViewBag.chartSufix = "";
                    ViewBag.BarChart = true;
                    ViewBag.LineChart = false;
                }
                else
                    report = null;
            }
            //Income report
            else if (ID == "0002")
            {
                ViewBag.AlsoShowDate = true;

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
                catch (Exception e)
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

                    int chartDataCount = 0;
                    string chartLabels = "";
                    string chartData = "";

                    foreach (TAXorVATRecivedList item in IncomeRecivedReport)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.InvoiceDateString);
                        Data.column2Data = (item.clientName);
                        Data.column3Data = (item.JobTitle);
                        Data.column4Data = (item.TotalString);

                        report.ReportDataList.Add(Data);

                        c4Total += item.Total;

                        if (chartDataCount == 0)
                        {
                            chartLabels += "'" + item.JobTitle + " for " + item.clientName + "'";
                            chartData += "'" + (item.Total).ToString("0.00", nfi) + "'";
                        }
                        else
                        {
                            chartLabels += ", '" + item.JobTitle + " for " + item.clientName + "'";
                            chartData += ", '" + (item.Total).ToString("0.00", nfi) + "'";
                        }
                        chartDataCount++;
                    }

                    report.column4Total = (c4Total.ToString("#,0.00", nfi));

                    ViewBag.chartLabel = "'Jobs Report'";
                    ViewBag.chartLabels = chartLabels;
                    ViewBag.chartData = chartData;
                    ViewBag.chartPrefix = "R";
                    ViewBag.chartSufix = "";
                    ViewBag.BarChart = true;
                    ViewBag.LineChart = false;
                }
                else
                    report = null;
            }
            //Expense report
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
                        Response.Redirect(Url.Action("TaxVatPeriod", "Tax") + "?Type=T");
                    }
                    else
                    {
                        int i = 0, currentPeriod = 0;
                        foreach (TaxAndVatPeriods check in taxPeriod)
                        {
                            if (DateTime.Now > check.StartDate && DateTime.Now < check.EndDate)
                                currentPeriod = i;
                            i++;
                        }

                        if (DropDownID == null || DropDownID == "")
                            DropDownID = taxPeriod[currentPeriod].PeriodID.ToString();

                        ViewBag.DropDownID = DropDownID;
                        ViewBag.DropdownName = "Tax Period";
                        taxPeriod = taxPeriod.OrderByDescending(o => o.StartDate).ToList();
                        List<SelectListItem> dropDownList = new List<SelectListItem>();
                        foreach (var item in taxPeriod)
                        {
                            if (item.PeriodID == int.Parse(DropDownID))
                            {
                                dropDownList.Add(new SelectListItem() { Text = item.PeriodString, Value = item.PeriodID.ToString(), Selected = true });
                            }
                            else
                            {
                                dropDownList.Add(new SelectListItem() { Text = item.PeriodString, Value = item.PeriodID.ToString() });
                            }
                        }
                        ViewBag.DropDownFilter = dropDownList;

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
                            expense.dateSort = expenseItem.Date;
                            expense.amount = expenseItem.ClientCharge;
                            expense.TotalString = expense.amount.ToString("#,0.00", nfi);

                            ExpenseReport.Add(expense);
                        }
                        foreach (Model.SP_GetJobExpense_Result expenseItem in ProfileJobExpenses)
                        {
                            Model.DashboardExpense expense = new Model.DashboardExpense();

                            expense.name = expenseItem.Name;
                            expense.date = expenseItem.DateString;
                            expense.dateSort = expenseItem.Date;
                            expense.amount = expenseItem.Amount;
                            expense.TotalString = expense.amount.ToString("#,0.00", nfi);

                            ExpenseReport.Add(expense);
                        }
                        foreach (Model.SP_GetGeneralExpense_Result expenseItem in ProfileGeneralExpenses)
                        {
                            Model.DashboardExpense expense = new Model.DashboardExpense();

                            expense.name = expenseItem.Name;
                            expense.date = expenseItem.DateString;
                            expense.dateSort = expenseItem.Date;
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

                    ExpenseReport = ExpenseReport.OrderBy(o => o.dateSort).ToList();
                    foreach (DashboardExpense item in ExpenseReport)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.date);
                        Data.column2Data = (item.name);
                        Data.column3Data = ("-" + item.TotalString);
                        Data.column4Data = ("");

                        report.ReportDataList.Add(Data);

                        c3Total += item.amount * -1;
                    }

                    IncomeRecivedReport = IncomeRecivedReport.OrderBy(o => o.InvoiceDate).ToList();
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
                ViewBag.AlsoShowDate = true;

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
            //Income by Client
            else if (ID == "0006")
            {
                report = new ReportViewModel();

                try
                {
                    Client ProfileIDClient = new Client();
                    ProfileIDClient.ProfileID = ProfileID.ProfileID;
                    List<Client> clients = handler.getProfileClients(ProfileIDClient);

                    if (DropDownID == null || DropDownID == "")
                        DropDownID = clients[0].ClientID.ToString();

                    ViewBag.DropDownID = DropDownID;
                    clients = clients.OrderBy(o => o.FirstName).ToList();
                    clients.Insert(0, new Client { FirstName = "All", ClientID = 0 });
                    ViewBag.DropdownName = "Client";
                    List<SelectListItem> dropDownList = new List<SelectListItem>();
                    foreach (var item in clients)
                    {
                        if (item.ClientID == int.Parse(DropDownID))
                        {
                            dropDownList.Add(new SelectListItem() { Text = item.FirstName, Value = item.ClientID.ToString(), Selected = true });
                        }
                        else
                        {
                            dropDownList.Add(new SelectListItem() { Text = item.FirstName, Value = item.ClientID.ToString() });
                        }
                    }
                    ViewBag.DropDownFilter = dropDownList;
                    ViewBag.AlsoShowDate = true;

                    if (DropDownID != "0")
                        report = handler.getIncomeByClientReport(ProfileID, sDate, eDate, DropDownID);
                    else
                        report = handler.getIncomeByClientReport(ProfileID, sDate, eDate);

                    foreach(Client client in clients)
                    {
                        if (client.ClientID == int.Parse(DropDownID))
                            report.reportSubHeading = "For " + client.FirstName;
                    }

                        ViewBag.chartLabel = "'Client Report'";
                        ViewBag.chartLabels = report.chartLabels;
                        ViewBag.chartData = report.chartData;
                        ViewBag.chartPrefix = "R";
                        ViewBag.chartSufix = "";
                        ViewBag.BarChart = true;
                        ViewBag.LineChart = false;
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

                    if (DropDownID == null || DropDownID == "")
                        DropDownID = clients[0].ClientID.ToString();

                    ViewBag.DropDownID = DropDownID;
                    clients = clients.OrderBy(o => o.FirstName).ToList();
                    clients.Insert(0, new Client { FirstName = "All", ClientID = 0 });
                    ViewBag.DropdownName = "Client";
                    List<SelectListItem> dropDownList = new List<SelectListItem>();
                    foreach (var item in clients)
                    {
                        if (item.ClientID == int.Parse(DropDownID))
                        {
                            dropDownList.Add(new SelectListItem() { Text = item.FirstName, Value = item.ClientID.ToString(), Selected = true });
                        }
                        else
                        {
                            dropDownList.Add(new SelectListItem() { Text = item.FirstName, Value = item.ClientID.ToString()});
                        }
                    }
                    ViewBag.DropDownFilter = dropDownList;
                    ViewBag.AlsoShowDate = true;

                    if (DropDownID != "0")
                        report = handler.getExpensesByClientReport(ProfileID, sDate, eDate, DropDownID);
                    else
                        report = handler.getExpensesByClientReport(ProfileID, sDate, eDate);

                    ViewBag.chartLabel = "'Client Report'";
                    ViewBag.chartLabels = report.chartLabels;
                    ViewBag.chartData = report.chartData;
                    ViewBag.chartPrefix = "R";
                    ViewBag.chartSufix = "";
                    ViewBag.BarChart = true;
                    ViewBag.LineChart = false;
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
                ViewBag.AlsoShowDate = true;

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
                        "Error loading report 0008 in reports controler");
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

                    ViewBag.chartLabel = "'Expense Report'";
                    ViewBag.chartLabels = "'General Expenses Total', 'Job Expenses Total', 'Travel Expenses Total'";
                    ViewBag.chartData = ""+c6Total.ToString("0.00", nfi)
                        +", " + c5Total.ToString("0.00", nfi)  
                        + ", " + c4Total.ToString("0.00", nfi) + "";
                    ViewBag.chartPrefix = "R";
                    ViewBag.chartSufix = "";
                    ViewBag.BarChart = true;
                    ViewBag.LineChart = false;
                }
                else
                    report = null;
            }
            //General Expenses Report
            else if (ID == "0009")
            {
                ViewBag.AlsoShowDate = true;

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
                ViewBag.AlsoShowDate = true;

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
            //Vehicle Travel Expenses Report
            else if (ID == "0011")
            {
                report = new ReportViewModel();

                List<Model.TravelLog> ProfileTravelLog = null;

                TaxAndVatPeriods dates = new TaxAndVatPeriods();
                dates.StartDate = sDate;
                dates.EndDate = eDate;

                try
                {
                    Model.Profile getProfileVehicles = new Model.Profile();
                    if (DownloadID == "0")
                    {
                        if (cookie == null)
                            getCookie();
                        getProfileVehicles.ProfileID = int.Parse(cookie["ID"]);
                    }
                    else
                    {
                        getProfileVehicles.ProfileID = int.Parse(DownloadID);
                    }
                    List<Model.Vehicle> Vehicles = handler.getVehicles(getProfileVehicles);
                    Vehicles = Vehicles.OrderBy(o => o.Name).ToList();
                    Vehicles.Insert(0, new Vehicle { Name = "All", VehicleID = 0 });

                    if (DropDownID == null || DropDownID == "")
                        DropDownID = Vehicles[0].VehicleID.ToString();

                    ViewBag.DropDownID = DropDownID;

                    ViewBag.DropdownName = "Vehicle";
                    ViewBag.DropDownFilter = new SelectList(Vehicles, "VehicleID", "Name");
                    Vehicles = Vehicles.OrderBy(o => o.Name).ToList();
                    List<SelectListItem> dropDownList = new List<SelectListItem>();
                    foreach (var item in Vehicles)
                    {
                        if (item.VehicleID == int.Parse(DropDownID))
                        {
                            dropDownList.Add(new SelectListItem() { Text = item.Name, Value = item.VehicleID.ToString(), Selected = true });
                        }
                        else
                        {
                            dropDownList.Add(new SelectListItem() { Text = item.Name, Value = item.VehicleID.ToString() });
                        }
                    }
                    ViewBag.DropDownFilter = dropDownList;
                    ViewBag.AlsoShowDate = true;

                    if (DropDownID != "0")
                        ProfileTravelLog = handler.getProfileTravelLog(ProfileID, sDate, eDate, DropDownID);
                    else
                        ProfileTravelLog = handler.getProfileTravelLog(ProfileID, sDate, eDate);

                    foreach (Vehicle vehicle in Vehicles)
                    {
                        if (vehicle.VehicleID == int.Parse(DropDownID))
                            report.reportSubHeading = "For " + vehicle.Name;
                    }
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0011 in reports controler");
                }

                if (ProfileTravelLog != null)
                {
                    report.reportTitle = "Vehicle Travel Expenses";
                    report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                    report.reportEndDate = eDate.ToString("yyyy-MM-dd");
                    report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                    report.column1Name = "Date";
                    report.column2Name = "Title";
                    report.column4Name = "Amount (Client charge | R)";
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
                       Response.Redirect(Url.Action("TaxVatPeriod", "Tax") + "?Type=T");
                    }
                    else
                    {

                        int i = 0, currentPeriod = 0;
                        foreach (TaxAndVatPeriods check in taxPeriod)
                        {
                            if (DateTime.Now > check.StartDate && DateTime.Now < check.EndDate)
                                currentPeriod = i;
                            i++;
                        }

                        if (DropDownID == null || DropDownID == "")
                            DropDownID = taxPeriod[currentPeriod].PeriodID.ToString();

                        ViewBag.DropDownID = DropDownID;
                        ViewBag.DropdownName = "Tax Period";
                        taxPeriod = taxPeriod.OrderByDescending(o => o.StartDate).ToList();
                        List<SelectListItem> dropDownList = new List<SelectListItem>();
                        foreach (var item in taxPeriod)
                        {
                            if (item.PeriodID == int.Parse(DropDownID))
                            {
                                dropDownList.Add(new SelectListItem() { Text = item.PeriodString, Value = item.PeriodID.ToString(), Selected = true });
                            }
                            else
                            {
                                dropDownList.Add(new SelectListItem() { Text = item.PeriodString, Value = item.PeriodID.ToString() });
                            }
                        }
                        ViewBag.DropDownFilter = dropDownList;

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
                        Response.Redirect(Url.Action("TaxVatPeriod", "Tax") + "?Type=V");
                    }
                    else
                    {

                        int i = 0, currentPeriod = 0;
                        foreach (TaxAndVatPeriods check in vatPeriod)
                        {
                            if (DateTime.Now > check.StartDate && DateTime.Now < check.EndDate)
                                currentPeriod = i;
                            i++;
                        }

                        if (DropDownID == null || DropDownID == "")
                            DropDownID = vatPeriod[currentPeriod].PeriodID.ToString();

                        ViewBag.DropDownID = DropDownID;
                        ViewBag.DropdownName = "Vat Period";
                        vatPeriod = vatPeriod.OrderByDescending(o => o.StartDate).ToList();
                        List<SelectListItem> dropDownList = new List<SelectListItem>();
                        foreach (var item in vatPeriod)
                        {
                            if (item.PeriodID == int.Parse(DropDownID))
                            {
                                dropDownList.Add(new SelectListItem() { Text = item.PeriodString, Value = item.PeriodID.ToString(), Selected = true });
                            }
                            else
                            {
                                dropDownList.Add(new SelectListItem() { Text = item.PeriodString, Value = item.PeriodID.ToString() });
                            }
                        }
                        ViewBag.DropDownFilter = dropDownList;

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
                            expense.VAT = expense.amount - (expense.amount / ((VATRate.VATRate / 100) + 1));
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
                            expense.VAT = expense.amount - (expense.amount / ((VATRate.VATRate / 100) + 1));
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
                            expense.VAT = expense.amount - (expense.amount / ((VATRate.VATRate / 100) + 1));
                            expense.VATString = expense.VAT.ToString("#,0.00", nfi);

                            ExpenseReport.Add(expense);
                        }

                        VATRecived = handler.getVATRecivedList(ProfileID, VATRate);
                    }
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0003 in reports controler");
                }

                if (ExpenseReport != null && VATRecived != null)
                {
                    report.reportTitle = "VAT";
                    report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                    report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                    report.reportSubHeading = "Vat Rate @ " + VATRate.VATRate.ToString("#,0.00", nfi) + "% & all values in ZAR";
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
                    fotter.column6Data = ((c4Total + c6Total).ToString("#,0.00", nfi));
                    report.FooterRowList.Add(fotter);
                    report.column6FotterAlignRight = true;
                }
                else
                    report = null;
            }
            //Job Earning Per Hour Report
            else if (ID == "0014")
            {
                ViewBag.AlsoShowDate = true;

                report = new ReportViewModel();

                try
                {
                    Client ProfileIDClient = new Client();
                    ProfileIDClient.ProfileID = ProfileID.ProfileID;

                    report = handler.getJobEarningPerHourReport(ProfileID, sDate, eDate);

                    ViewBag.chartLabel = "'Job Earning Per Hour Report'";
                    ViewBag.chartLabels = report.chartLabels;
                    ViewBag.chartData = report.chartData;
                    ViewBag.chartPrefix = "R";
                    ViewBag.chartSufix = " per Hour";
                    ViewBag.BarChart = true;
                    ViewBag.LineChart = false;
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0014 in reports controler");
                }

                if (report.ReportDataList == null)
                    report = null;
            }
            //SARS Travel Logbook report
            else if (ID == "0015")
            {
                ViewBag.AlsoShowDate = true;

                report = new ReportViewModel();

                List<Model.TravelLog> ProfileTravelLog = null;

                TaxAndVatPeriods dates = new TaxAndVatPeriods();
                dates.StartDate = sDate;
                dates.EndDate = eDate;

                try
                {
                    Model.Profile getProfileVehicles = new Model.Profile();
                    if (DownloadID == "0")
                    {
                        if (cookie == null)
                            getCookie();
                        getProfileVehicles.ProfileID = int.Parse(cookie["ID"]);
                    }
                    else
                    {
                        getProfileVehicles.ProfileID = int.Parse(DownloadID);
                    }

                    List<Model.Vehicle> Vehicles = handler.getVehicles(getProfileVehicles);

                    if (DropDownID == null || DropDownID == "")
                        DropDownID = Vehicles[0].VehicleID.ToString();

                    ViewBag.DropDownID = DropDownID;
                    Vehicles = Vehicles.OrderBy(o => o.Name).ToList();
                    List<SelectListItem> dropDownList = new List<SelectListItem>();
                    foreach (var item in Vehicles)
                    {
                        if (item.VehicleID == int.Parse(DropDownID))
                        {
                            dropDownList.Add(new SelectListItem() { Text = item.Name, Value = item.VehicleID.ToString(), Selected = true });
                        }
                        else
                        {
                            dropDownList.Add(new SelectListItem() { Text = item.Name, Value = item.VehicleID.ToString() });
                        }
                    }
                    ViewBag.DropDownFilter = dropDownList;
                    ViewBag.DropdownName = "Vehicle";
                    ViewBag.AlsoShowDate = true;

                    if (DropDownID != "0")
                        ProfileTravelLog = handler.getProfileTravelLog(ProfileID, sDate, eDate, DropDownID);
                    else
                        ProfileTravelLog = handler.getProfileTravelLog(ProfileID, sDate, eDate);

                    foreach (Vehicle vehicle in Vehicles)
                    {
                        if (vehicle.VehicleID == int.Parse(DropDownID))
                            report.reportSubHeading = "DAILY BUSINESS TRAVEL RECORDS For " + vehicle.Name;
                    }
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0011 in reports controler");
                }

                if (ProfileTravelLog != null)
                {
                    report.reportTitle = "SARS Travel Logbook";
                    report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                    report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                    report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                    report.column1Name = "Date";
                    report.column2Name = "Opening KM's";
                    report.column3Name = "Closing KM's";
                    report.column4Name = "Total KM's";
                    report.column5Name = "From";
                    report.column6Name = "To";
                    report.column7Name = "Reason";
                    report.column8Name = "Fuel Cost (R)";
                    report.column9Name = "Maintenance Cost (R)";
                    report.column2DataAlignRight = true;
                    report.column3DataAlignRight = true;
                    report.column4DataAlignRight = true;
                    report.column8DataAlignRight = true;
                    report.column9DataAlignRight = true;

                    report.ReportDataList = new List<ReportDataList>();

                    double c4Total = 0;
                    decimal c8Total = 0;
                    decimal c9Total = 0;

                    foreach (TravelLog item in ProfileTravelLog)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (item.DateString);
                        Data.column2Data = (item.OpeningKMs.ToString("#,0.00", nfi));
                        Data.column3Data = (item.ClosingKMs.ToString("#,0.00", nfi));
                        Data.column4Data = (item.TotalKMs.ToString("#,0.00", nfi));
                        Data.column5Data = (item.From);
                        Data.column6Data = (item.To);
                        Data.column7Data = item.Reason.ToString();
                        Data.column8Data = (item.SARSFuelCost.ToString("#,0.00", nfi));
                        Data.column9Data = (item.SARSMaintenceCost.ToString("#,0.00", nfi));
                        Data.columnSortData = item.Date.ToString();

                        report.ReportDataList.Add(Data);

                        c4Total += item.TotalKMs;
                        c8Total += item.SARSFuelCost;
                        c9Total += item.SARSMaintenceCost;
                    }

                    report.ReportDataList = report.ReportDataList.OrderBy(o => o.columnSortData).ToList();

                    report.column4Total = (c4Total.ToString("#,0.00", nfi));
                    report.column8Total = (c8Total.ToString("#,0.00", nfi));
                    report.column9Total = (c9Total.ToString("#,0.00", nfi));
                }
                else
                    report = null;
            }
            //Jobs Per month
            else if (ID == "0016")
            {
                report = new ReportViewModel();

                try
                {
                    List<string> years = new List<string>();
                    int year = int.Parse(DateTime.Now.Year.ToString());
                    for(int i = 100; i > 0; i--)
                    {
                        years.Add(year.ToString());
                        year--;
                    }


                    if (DropDownID == null || DropDownID == "")
                        DropDownID = DateTime.Now.Year.ToString();

                    ViewBag.DropDownID = DropDownID;
                    ViewBag.DropdownName = "Year";
                    List<SelectListItem> dropDownList = new List<SelectListItem>();
                    foreach (var item in years)
                    {
                        if (item == DropDownID)
                        {
                            dropDownList.Add(new SelectListItem() { Text = item, Value = item.ToString(), Selected = true });
                        }
                        else
                        {
                            dropDownList.Add(new SelectListItem() { Text = item, Value = item.ToString()});
                        }
                    }
                    ViewBag.DropDownFilter = dropDownList;
                    ViewBag.AlsoShowDate = false;

                    report = handler.getJobPerMonthReport(ProfileID, DropDownID);
                    
                        ViewBag.chartLabel = "'Jobs per Month Report'";
                        ViewBag.chartLabels = report.chartLabels;
                        ViewBag.chartData = report.chartData;
                        ViewBag.chartPrefix = "";
                        ViewBag.chartSufix = " Jobs";
                        ViewBag.BarChart = false;
                        ViewBag.LineChart = true;
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0016 in reports controler");
                }

                if (report.ReportDataList == null)
                    report = null;
            }
            //Jobs Per Year
            else if (ID == "0017")
            {
                report = new ReportViewModel();

                try
                {
                    ViewBag.AlsoShowDate = false;

                    report = handler.getJobPerYearReport(ProfileID);
                    
                        ViewBag.chartLabel = "'Jobs per Month Report'";
                        ViewBag.chartLabels = report.chartLabels;
                        ViewBag.chartData = report.chartData;
                        ViewBag.chartPrefix = "";
                        ViewBag.chartSufix = " Jobs";
                        ViewBag.BarChart = false;
                        ViewBag.LineChart = true;
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0017 in reports controler");
                }

                if (report.ReportDataList == null)
                    report = null;
            }
            //Income Per month
            else if (ID == "0018")
            {
                report = new ReportViewModel();

                try
                {
                    List<string> years = new List<string>();
                    int year = int.Parse(DateTime.Now.Year.ToString());
                    for(int i = 100; i > 0; i--)
                    {
                        years.Add(year.ToString());
                        year--;
                    }

                    if (DropDownID == null || DropDownID == "")
                        DropDownID = DateTime.Now.Year.ToString();

                    ViewBag.DropDownID = DropDownID;

                    ViewBag.DropdownName = "Year";
                    List<SelectListItem> dropDownList = new List<SelectListItem>();
                    foreach (var item in years)
                    {
                        if (item == DropDownID)
                        {
                            dropDownList.Add(new SelectListItem() { Text = item, Value = item.ToString(), Selected = true });
                        }
                        else
                        {
                            dropDownList.Add(new SelectListItem() { Text = item, Value = item.ToString() });
                        }
                    }
                    ViewBag.DropDownFilter = dropDownList;
                    ViewBag.AlsoShowDate = false;

                    report = handler.getIncomeRecivedListPerMonth(ProfileID, DropDownID);
                    
                        ViewBag.chartLabel = "'Income per Month Report'";
                        ViewBag.chartLabels = report.chartLabels;
                        ViewBag.chartData = report.chartData;
                        ViewBag.chartPrefix = "R";
                        ViewBag.chartSufix = "";
                        ViewBag.BarChart = false;
                        ViewBag.LineChart = true;
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0018 in reports controler");
                }

                if (report.ReportDataList == null)
                    report = null;
            }
            //Income Per Year
            else if (ID == "0019")
            {
                report = new ReportViewModel();

                try
                {
                    ViewBag.AlsoShowDate = false;

                    report = handler.getIncomeRecivedListPerYear(ProfileID);
                    
                        ViewBag.chartLabel = "'Income pre Year Report'";
                        ViewBag.chartLabels = report.chartLabels;
                        ViewBag.chartData = report.chartData;
                        ViewBag.chartPrefix = "R";
                        ViewBag.chartSufix = "";
                        ViewBag.BarChart = false;
                        ViewBag.LineChart = true;
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading report 0019 in reports controler");
                }

                if (report.ReportDataList == null)
                    report = null;
            }
            //Expenses y catagory Report
            else if (ID == "0020")
            {
                ViewBag.AlsoShowDate = true;

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
                        "Error loading report 0020 in reports controler");
                }

                if (ProfileTravelLog != null && ProfileJobExpenses != null && ProfileGeneralExpenses != null)
                {
                    report.reportTitle = "Expenses by Category Report";
                    report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                    report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                    report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                    report.column1Name = "Category";
                    report.column2Name = "Total Amount (R)";
                    report.column2DataAlignRight = true;

                    report.ReportDataList = new List<ReportDataList>();

                    decimal c2Total = 0;

                    List<Model.ExpenseCategory> cats = handler.getExpenseCatagories();
                    cats.Insert(0, new ExpenseCategory { Name = "Vehicle Travel Expenses", CategoryID = 0 });
                    foreach(ExpenseCategory cat in cats)
                    {
                        ReportDataList Data = new ReportDataList();
                        Data.column1Data = (cat.Name);
                        Data.column2Data = ("0");
                        report.ReportDataList.Add(Data);
                    }

                    foreach (TravelLog item in ProfileTravelLog)
                    {
                        c2Total += item.ClientCharge;
                    }

                    for (int i = 0; i<report.ReportDataList.Count(); i++)
                    {
                        if(report.ReportDataList[i].column1Data == "Vehicle Travel Expenses")
                        {
                            report.ReportDataList[i].column2Data = (c2Total.ToString());
                            i = report.ReportDataList.Count();
                        }
                    }

                    foreach (SP_GetJobExpense_Result item in ProfileJobExpenses)
                    {
                        for (int i = 0; i < report.ReportDataList.Count(); i++)
                        {
                            if (report.ReportDataList[i].column1Data == item.CatName)
                            {
                                report.ReportDataList[i].column2Data = (decimal.Parse(report.ReportDataList[i].column2Data)+item.Amount).ToString();
                                i = report.ReportDataList.Count();
                            }
                        }

                        c2Total += item.Amount;
                    }
                    foreach (SP_GetGeneralExpense_Result item in ProfileGeneralExpenses)
                    {
                        for (int i = 0; i < report.ReportDataList.Count(); i++)
                        {
                            if (report.ReportDataList[i].column1Data == item.CatName)
                            {
                                report.ReportDataList[i].column2Data = (decimal.Parse(report.ReportDataList[i].column2Data) + item.Amount).ToString();
                                i = report.ReportDataList.Count();
                            }
                        }

                        c2Total += item.Amount;
                    }

                    report.ReportDataList = report.ReportDataList.OrderBy(o => o.column1Data).ToList();

                    report.FooterRowList = new List<ReportFixedFooterRowList>();
                    ReportFixedFooterRowList fotter = new ReportFixedFooterRowList();
                    fotter.column1Data = "Total (R):";
                    fotter.column2Data = ((c2Total).ToString());
                    report.FooterRowList.Add(fotter);
                    report.column2FotterAlignRight = true;

                    ViewBag.chartLabel = "'Expenses by Category Report'";
                    ViewBag.chartLabels = "'General Expenses Total', 'Job Expenses Total', 'Travel Expenses Total'";

                    int chartDataCount = 0;
                    string chartLabels = "";
                    string chartData = "";

                    for (int i = 0; i < report.ReportDataList.Count(); i++)
                    {
                        if (chartDataCount == 0)
                        {
                            chartLabels += "'" + report.ReportDataList[i].column1Data + "'";
                            chartData += "'" + (decimal.Parse(report.ReportDataList[i].column2Data)).ToString("0.00", nfi) + "'";
                        }
                        else
                        {
                            chartLabels += ", '" + report.ReportDataList[i].column1Data + "'";
                            chartData += ", '" + (decimal.Parse(report.ReportDataList[i].column2Data)).ToString("0.00", nfi) + "'";
                        }
                        chartDataCount++;
                        report.ReportDataList[i].column2Data = decimal.Parse(report.ReportDataList[i].column2Data).ToString("#,0.00", nfi);
                }

                    ViewBag.chartLabels = chartLabels;
                    ViewBag.chartData = chartData;
                    ViewBag.chartPrefix = "R";
                    ViewBag.chartSufix = "";
                    ViewBag.BarChart = true;
                    ViewBag.LineChart = false;
                }
                else
                    report = null;
            }

            return report;
        }

        public ActionResult DisplayReport(string StartDateRange, string EndDateRange, string SortBy, string SortDirection, string DropDownID, string reportID = "0", string view = "", string DownloadID = "0")
        {
            getCookie();

            if (cookie == null)
                getCookie();
            ViewBag.ProfileID = int.Parse(cookie["ID"].ToString());

            string ID = reportID;
            
            if (ID == "0")
            {
                function.logAnError("No report ID Supplied display report");
                return RedirectToAction("Reports", "Report");
            }

            ReportViewModel report = getReportData(ID, StartDateRange, EndDateRange, DropDownID, view, DownloadID);

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
                else if (SortBy == "Col7")
                    report.ReportDataList = report.ReportDataList.OrderByDescending(o => o.column7Data).ToList();
                else if (SortBy == "Col8")
                    report.ReportDataList = report.ReportDataList.OrderByDescending(o => o.column8Data).ToList();
                else if (SortBy == "Col9")
                    report.ReportDataList = report.ReportDataList.OrderByDescending(o => o.column9Data).ToList();
                else if (SortBy == "Col10")
                    report.ReportDataList = report.ReportDataList.OrderByDescending(o => o.column10Data).ToList();

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
                else if (SortBy == "Col7")
                    report.ReportDataList = report.ReportDataList.OrderBy(o => o.column7Data).ToList();
                else if (SortBy == "Col8")
                    report.ReportDataList = report.ReportDataList.OrderBy(o => o.column8Data).ToList();
                else if (SortBy == "Col9")
                    report.ReportDataList = report.ReportDataList.OrderBy(o => o.column9Data).ToList();
                else if (SortBy == "Col10")
                    report.ReportDataList = report.ReportDataList.OrderBy(o => o.column10Data).ToList();

                SortDirection = "D";
            }

            ViewBag.SortDirection = SortDirection;
            #endregion

            return View(report);
        }

        [HttpPost]
        public ActionResult DisplayReport(FormCollection collection, string StartDateRange, string EndDateRange, string SortBy, string DropDownID, string reportID = "0", string view = "", string DownloadID = "0")
        {
            try
            {
                DateTime sDate = DateTime.Now.AddMonths(-12);
                DateTime eDate = DateTime.Now;

                DateTime.TryParse(Request.Form["StartDate"], out sDate);
                DateTime.TryParse(Request.Form["EndDate"], out eDate);

                StartDateRange = sDate.ToString("yyyy-MM-dd");
                EndDateRange = eDate.ToString("yyyy-MM-dd");

                if (Request.Form["DropDownFilter"] != null)
                {
                    DropDownID = Request.Form["DropDownFilter"].ToString();
                }

                return RedirectToAction("DisplayReport", "Report", new
                {
                    StartDateRange,
                    EndDateRange,
                    reportID,
                    SortBy,
                    DropDownID,
                    DownloadID,
                    view
                }); ;
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error updating date range for display reports page");
                return RedirectToAction("Error", "Shared");
            }
        }

        public ActionResult PrintReport(string StartDateRange, string EndDateRange, string SortBy, string SortDirection, string DropDownID, string reportID = "0", string view = "", string DownloadID = "0")
        {
            if(DownloadID == "0")
                getCookie();

            string ID = reportID;

            if (ID == "0")
            {
                function.logAnError("No report ID Supplied display report");
                return RedirectToAction("Reports", "Report");
            }

            ReportViewModel report = getReportData(ID, StartDateRange, EndDateRange, DropDownID, view, DownloadID);

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
    
        public FileResult DownloadReport(string StartDateRange, string EndDateRange, string SortBy, string SortDirection, string DropDownID, string reportID = "0", string view = "", string DownloadID = "0")
        {
            if (DownloadID == "0")
                getCookie();

            string ID = reportID;

            if (ID == "0")
            {
                function.logAnError("No report ID Supplied display report");
                Response.Redirect("/Shared/Error?ERR=Error downloading report - No ID Supplied");
            }

            ReportViewModel report = getReportData(ID, StartDateRange, EndDateRange, DropDownID, view, DownloadID);

            if (report == null)
                Response.Redirect(Url.Action("Error", "Shared") + "?ERR=Error downloading report");

            return File(function.downloadPage("http://sict-iis.nmmu.ac.za/taxapp/Report/PrintReport?reportID="+reportID+"&StartDateRange="+StartDateRange+"&EndDateRange="+EndDateRange+"&SortBy="+SortBy+"&SortDirection="+SortDirection+"&DropDownID="+DropDownID+ "&DownloadID"+DownloadID), System.Net.Mime.MediaTypeNames.Application.Octet, 
                report.reportTitle+" - Generated: "+ DateTime.Now+".pdf");
        }
    }
}