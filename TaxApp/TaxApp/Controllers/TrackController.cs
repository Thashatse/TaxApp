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
    public class TrackController : Controller
    {
        IDBHandler handler = new DBHandler();
        HttpCookie cookie;
        Functions function = new Functions();
        NotificationsFunctions notiFunctions = new NotificationsFunctions();

        public string getCookie(string ID, string Type)
        {
            try
            {
                //check if the user is loged in
                cookie = Request.Cookies["TaxAppGuestUserID"];

                if (cookie == null)
                    return (Type);
                if (cookie["ID"] == null)
                    return (Type);
                if (cookie["ID"] == "")
                    return (Type);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in welcome method of LandingControles");
                Response.Redirect(Url.Action("Error", "Shared") + "?Err=Identity couldn't be verified");
            }

                return "";
        }

        #region verify Identity
        public ActionResult verifyIdentity(string ID, string Type, string Err)
        {
            ShareVerifyIdentityModel data = new ShareVerifyIdentityModel();

            ViewBag.Message = Err;
            
            if ((Type == "Job" || Type == "TAX" || Type == "VAT") && ID != "")
            {
                try
                {
                int OTP = function.generateOTP();
                Tuple<bool, string, string, int> check = handler.NewExternalUserOTP(int.Parse(ID), OTP, Type);

                if (!check.Item1)
                        return RedirectToAction("Error", "Shared");

                    data.userName = check.Item2;

                bool result = function.sendEmail(check.Item3,
                        check.Item2,
                        "Verify your identity - TaxApp",
                        "Hello, \n\n Your OTP is: "+ OTP + "\n\n Regards, \n The TaxApp Team.",
                        "TaxApp",
                        0);
                    ViewBag.UserID = check.Item4;
                }
                catch (Exception err)
                {
                    function.logAnError("Error creating OTP in Track controller Error: " + err);
                    return RedirectToAction("Error", "Shared", new { Err = "Error generating OTP. Either sharing has been turned of or the link provided is broken." });
                }
            }
            else
                return RedirectToAction("Error", "Shared", new { Err = "Broken link. The link provided cannot be found please try again." });

            return View(data);
        }

        [HttpPost]
        public ActionResult verifyIdentity(FormCollection collection, string ID, string Type)
        {
            ShareVerifyIdentityModel data = new ShareVerifyIdentityModel();
            data.userName = "Test First Test Last";

            int OTP = 0;
            int.TryParse(Request.Form["OTP"], out OTP);

            if (OTP == handler.GetExternalUserOTP(int.Parse(ID), Type))
            {
                cookie = new HttpCookie("TaxAppGuestUserID");
                cookie.Expires = DateTime.Now.AddHours(3);
                // Set the user id in it.
                cookie["ID"] = Request.Form["userId"];
                // Add it to the current web response.
                Response.Cookies.Add(cookie);

                if (Type == "Job")
                {
                    Job getJob = new Model.Job();
                    getJob.JobID = int.Parse(ID);
                    SP_GetJob_Result Job = handler.getJob(getJob);

                    Notifications newNoti = new Notifications();
                    newNoti.date = DateTime.Now;
                    newNoti.ProfileID = Job.ProfileID;
                    newNoti.Link = "/Job/Job?ID=" + ID;
                    newNoti.Details = Job.ClientFirstName + " has accessed a Job. Manage sharing settings here.";

                    bool create = true;
                    List<Notifications> ProfileNotis = notiFunctions.getNotifications(int.Parse(cookie["ID"]));
                    foreach (Notifications noti in ProfileNotis)
                    {
                        if (noti.Details == newNoti.Details
                            && noti.Link == newNoti.Link)
                            create = false;
                    }

                    if(create)
                        notiFunctions.newNotification(newNoti);

                    return RedirectToAction("Job", "Track", new
                    {
                        JobID = ID
                    });
                }
                if (Type == "TAX")
                {
                    TaxConsultant taxConsultant = new TaxConsultant();
                    taxConsultant.ProfileID = int.Parse(cookie["ID"]);
                    taxConsultant = handler.getConsumtant(taxConsultant);

                    Notifications newNoti = new Notifications();
                    newNoti.date = DateTime.Now;
                    newNoti.ProfileID = int.Parse(cookie["ID"]);
                    newNoti.Link = "/Tax/TaxCenter?period=" + ID;
                    newNoti.Details = taxConsultant.Name + " has accessed a Tax Period. Manage sharing settings here.";

                    bool create = true;
                    List<Notifications> ProfileNotis = notiFunctions.getNotifications(int.Parse(cookie["ID"]));
                    foreach (Notifications noti in ProfileNotis)
                    {
                        if (noti.Details == newNoti.Details
                            && noti.Link == newNoti.Link)
                            create = false;
                    }

                    if (create)
                        notiFunctions.newNotification(newNoti);

                    return RedirectToAction("TAX", "Track", new
                    {
                        TaxID = ID
                    });
                }
                if (Type == "VAT")
                {
                    TaxConsultant taxConsultant = new TaxConsultant();
                    taxConsultant.ProfileID = int.Parse(cookie["ID"]);
                    taxConsultant = handler.getConsumtant(taxConsultant);

                    Notifications newNoti = new Notifications();
                    newNoti.date = DateTime.Now;
                    newNoti.ProfileID = int.Parse(cookie["ID"]);
                    newNoti.Link = "/Vat/VatCenter?period=" + ID;
                    newNoti.Details = taxConsultant.Name + " has accessed a VAT Period. Manage sharing settings here.";

                    bool create = true;
                    List<Notifications> ProfileNotis = notiFunctions.getNotifications(int.Parse(cookie["ID"]));
                    foreach (Notifications noti in ProfileNotis)
                    {
                        if (noti.Details == newNoti.Details
                            && noti.Link == newNoti.Link)
                            create = false;
                    }

                    if (create)
                        notiFunctions.newNotification(newNoti);

                    return RedirectToAction("VAT", "Track", new
                    {
                        VATID = ID
                    });
                }
            }
            else
            {
                string Err = "Incorrect OTP. We've sent you a new one, please try again";
                return RedirectToAction("verifyIdentity", "Track", new
                {
                    Type,
                    Err,
                    ID
                });
            }

            ViewBag.Message = "An error occurred while processing your request.";
            return View(data);
        }
        #endregion

        public ActionResult Job(string JobID, string Report = "false")
        {
            string type = getCookie(JobID, "Job");
            if (type != "")
                return Redirect(Url.Action("VerifyIdentity", "Track") + "?ID=" + JobID + "&Type=" + type);
            else
            {
                SP_GetJob_Result Job = null;
                List<Model.Worklog> JobHours = null;
                List<Model.SP_GetJobExpense_Result> JobExpenses = null;
                List<Model.TravelLog> JobTravelLog = null;
                List<SP_GetInvoice_Result> invoiceDetails = null;

                try
                {
                    Model.Job getJob = new Model.Job();
                    getJob.JobID = int.Parse(JobID);

                    Job = handler.getJob(getJob);

                    JobHours = handler.getJobHours(getJob);
                     
                    JobExpenses = handler.getJobExpenses(getJob);

                    JobTravelLog = handler.getJobTravelLog(getJob);

                    invoiceDetails = handler.getJobInvoices(getJob);
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loding job details for external view");
                    return RedirectToAction("Error", "Shared");
                }


                TrackJobViewModel view = new TrackJobViewModel();
                view.jobDetails = Job;
                view.Worklog = JobHours;
                view.JobExpenses = JobExpenses;
                view.JobTravelLog = JobTravelLog;
                view.invoices = invoiceDetails;

                ViewBag.Report = bool.Parse(Report);
                ViewBag.Generated = DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt");
            return View(view);
            }
            return RedirectToAction("Error", "Shared");
        }

        public ActionResult TAX(string TaxID, string SortDirection, string SortBy, string Report = "false")
        {
            string type = getCookie(TaxID, "TAX");
            if (type != "")
                return Redirect(Url.Action("VerifyIdentity", "Track") + "?ID=" + TaxID + "&Type=" + type);
            else
            {
            TaxConsultant taxConsultant = new TaxConsultant();
            taxConsultant.ProfileID = int.Parse(cookie["ID"]);
            taxConsultant = handler.getConsumtant(taxConsultant);

                if (taxConsultant != null)
                {
                    ViewBag.UserName = taxConsultant.Name;
                    ViewBag.ID = TaxID;

                    Profile ProfileID = new Profile();
                    ProfileID.ProfileID = int.Parse(cookie["ID"]);
                    ProfileID.EmailAddress = cookie["ID"];
                    ProfileID.Username = cookie["ID"];
                    ProfileID = handler.getProfile(ProfileID);
                    ViewBag.ProfileName = ProfileID.FirstName + " " + ProfileID.LastName;

                    var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                    nfi.NumberGroupSeparator = " ";

                    ReportViewModel report = new ReportViewModel();
                    List<Model.DashboardExpense> ExpenseReport = null;
                    List<TAXorVATRecivedList> IncomeRecivedReport = null;
                    TaxDashboard footers = null;

                    try
                    {
                        List<TaxAndVatPeriods> taxPeriod = handler.getTaxOrVatPeriodForProfile(ProfileID, 'T');
                        TaxAndVatPeriods TaxRate = null;

                        foreach (TaxAndVatPeriods item in taxPeriod)
                        {
                            if (item.PeriodID.ToString() == TaxID)
                            {
                                TaxRate = item;
                            }
                        }

                        DateTime sDate = TaxRate.StartDate;
                        DateTime eDate = TaxRate.EndDate;

                        TaxPeriodRates rate = new TaxPeriodRates();
                        rate.Rate = 0;

                        taxPeriod = handler.getTaxOrVatPeriodForProfile(ProfileID, 'T');

                        IncomeRecivedReport = handler.getTAXRecivedList(ProfileID, TaxRate, rate);
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
                        footers = handler.getTaxCenterDashboard(ProfileID, TaxRate);

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

                                c3Total += item.amount * -1;
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

                            ViewBag.Report = bool.Parse(Report);
                            ViewBag.Generated = DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt");
                        }
                        else
                            report = null;
                    }
                    catch (Exception e)
                    {
                        function.logAnError(e.ToString() +
                            "Error loading Track VAT in VAT of Track Conroler");
                        return RedirectToAction("Error", "Shared");
                    }

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

                        SortDirection = "D";
                    }

                    ViewBag.SortDirection = SortDirection;
                    #endregion

                    TrackTAXandVATViewModel viewModel = new TrackTAXandVATViewModel();

                    viewModel.Report = report;
                    viewModel.TAXDashboard = footers;

                    return View(viewModel);
                }

            }
            return RedirectToAction("Error", "Shared");
        }

        public ActionResult VAT(string VATID, string SortDirection, string SortBy, string Report = "false")
        {
            string type = getCookie(VATID, "VAT");
            if (type != "")
                return Redirect(Url.Action("VerifyIdentity", "Track")+ "?ID=" + VATID + "&Type="+type);
            else
            {
                TaxConsultant taxConsultant = new TaxConsultant();
                taxConsultant.ProfileID = int.Parse(cookie["ID"]);
                taxConsultant = handler.getConsumtant(taxConsultant);

                if (taxConsultant != null)
                {
                ViewBag.UserName = taxConsultant.Name;
                ViewBag.ID = VATID;

                Profile ProfileID = new Profile();
                ProfileID.ProfileID = int.Parse(cookie["ID"]);
                ProfileID.EmailAddress = cookie["ID"];
                ProfileID.Username = cookie["ID"];
                ProfileID = handler.getProfile(ProfileID);
                ViewBag.ProfileName = ProfileID.FirstName + " " + ProfileID.LastName;

                var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                nfi.NumberGroupSeparator = " ";

                TrackTAXandVATViewModel viewModel = new TrackTAXandVATViewModel();

                ReportViewModel report = new ReportViewModel();
                List<Model.DashboardExpense> ExpenseReport = null;
                VATDashboard footers = null;
                List<TAXorVATRecivedList> VATRecived = null;

                try
                {
                    List<TaxAndVatPeriods> vatPeriod = handler.getTaxOrVatPeriodForProfile(ProfileID, 'V');
                    TaxAndVatPeriods VATRate = null;

                    foreach (TaxAndVatPeriods item in vatPeriod)
                    {
                        if (item.PeriodID.ToString() == VATID)
                        {
                            VATRate = item;
                        }
                    }

                    DateTime sDate = VATRate.StartDate;
                    DateTime eDate = VATRate.EndDate;

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

                    if (ExpenseReport != null && VATRecived != null && footers != null)
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
                        fotter.column6Data = ((footers.VATPAIDOutstandingEst).ToString("#,0.00", nfi));
                        report.FooterRowList.Add(fotter);
                        report.column6FotterAlignRight = true;

                            ViewBag.Report = bool.Parse(Report);
                            ViewBag.Generated = DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt");
                        }
                    else
                        report = null;

                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error loading Track VAT in VAT of Track Conroler");
                        return RedirectToAction("Error", "Shared");
                    }


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

                    SortDirection = "D";
                }

                ViewBag.SortDirection = SortDirection;
                #endregion


                viewModel.Report = report;
                viewModel.VATDashboard = footers;

            return View(viewModel);

                }
            }
            return RedirectToAction("Error", "Shared");
        }

        public ActionResult ReceiptsDownload(string VATID, string TAXID)
        {
            string link = getCookie(VATID, "Job");
            if (link == "")
            {
                Profile profileID = new Profile();
                TaxAndVatPeriods TaxRate = null;
                List<TaxAndVatPeriods> period = new List<TaxAndVatPeriods>();
                DateTime sDate = new DateTime();
                DateTime eDate = new DateTime();

                if (TAXID != "" && TAXID != null)
                {
                profileID.ProfileID = int.Parse(cookie["ID"]);
                    period = handler.getTaxOrVatPeriodForProfile(profileID, 'T');

                    if(period.Count > 0)
                    {
                    foreach (TaxAndVatPeriods item in period)
                    {
                        if (item.PeriodID.ToString() == TAXID)
                        {
                            TaxRate = item;
                        }
                        sDate = TaxRate.StartDate;
                        eDate = TaxRate.EndDate;
                    }
                    }
                }

                if (VATID != "" && VATID != null)
                {
                profileID.ProfileID = int.Parse(cookie["ID"]);
                    period = handler.getTaxOrVatPeriodForProfile(profileID, 'V');

                    if (period.Count > 0)
                    {
                        foreach (TaxAndVatPeriods item in period)
                        {
                            if (item.PeriodID.ToString() == VATID)
                            {
                                TaxRate = item;
                            }
                            sDate = TaxRate.StartDate;
                            eDate = TaxRate.EndDate;
                        }
                    }
                }

                if (period.Count > 0)
                {
                    List<InvoiceAndReciptesFile> files = handler.getInvoiceAndReciptesFiles(profileID, sDate, eDate);
                

                    foreach (InvoiceAndReciptesFile file in files)
                    {
                        string redirect = "<script>window.open('/Functions/DownloadFile?ID=" + file.ID + "&type=" + file.Type + "');</script>";
                        Response.Write(redirect);
                    }
                }
            }

            return Content(@"<body>
                       <script type='text/javascript'>
                         window.close();
                       </script>
                     </body> ");
        }
    }
}