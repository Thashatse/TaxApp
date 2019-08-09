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
        public ReportViewModel getReportData(string ID, string StartDateRange, string EndDateRange)
        {
            ReportViewModel report = null;

            Profile getJobs = new Profile();
            getJobs.ProfileID = int.Parse(cookie["ID"].ToString());

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

            if (ID == "0001")
            {
                report = new ReportViewModel();

                List<SP_GetJob_Result> jobsReport = handler.getJobsReport(getJobs, sDate, eDate);

                report.reportTitle = "Jobs Report";
                report.reportCondition = "For date range: "+ sDate.ToString("dd MMM yyyy") + " - " + eDate.ToString("dd MMM yyyy");
                report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                report.column1Name = "Start Date";
                report.column2Name = "Client";
                report.column3Name = "Job";
                report.column4Name = "Income";
                report.column5Name = "Expenses";
                report.column6Name = "Earnings (before Tax & VAT)";

                List<string> column1Data = new List<string>();
                List<string> column2Data = new List<string>();
                List<string> column3Data = new List<string>();
                List<string> column4Data = new List<string>();
                List<string> column5Data = new List<string>();
                List<string> column6Data = new List<string>();

                decimal c4Total = 0;
                decimal c5Total = 0;
                decimal c6Total = 0;

                foreach (SP_GetJob_Result job in jobsReport)
                {
                    column1Data.Add(job.StartDateString);
                    column2Data.Add(job.ClientFirstName);
                    column3Data.Add(job.JobTitle);
                    column4Data.Add("R " + job.TotalPaidString);
                    column5Data.Add("R " + job.AllExpenseTotalString);
                    column6Data.Add("R " + (job.TotalPaid - job.AllExpenseTotal).ToString("#,0.##", nfi));

                    c4Total += job.TotalPaid;
                    c5Total += job.AllExpenseTotal;
                    c6Total += (job.TotalPaid - job.AllExpenseTotal);
                }

                report.column4Total = ("R " + c4Total.ToString("#,0.##", nfi));
                report.column5Total = ("R " + c5Total.ToString("#,0.##", nfi));
                report.column6Total = ("R " + c6Total.ToString("#,0.##", nfi));

                report.column1Data = column1Data;
                report.column2Data = column2Data;
                report.column3Data = column3Data;
                report.column4Data = column4Data;
                report.column5Data = column5Data;
                report.column6Data = column6Data;
            }

            return report;
        }
        public ActionResult DisplayReport(string StartDateRange, string EndDateRange, string reportID = "0")
        {
            getCookie();

            string ID = reportID;

            if (ID == "0")
            {
                function.logAnError("No report ID Supplied display report");
                return RedirectToAction("Reports", "Report");
            }

            ReportViewModel report = getReportData(ID, StartDateRange, EndDateRange);

            ViewBag.StartDateRange = report.reportStartDate;
            ViewBag.EndDateRange = report.reportEndDate;
            ViewBag.reportID = reportID;

            return View(report);
        }
        [HttpPost]
        public ActionResult DisplayReport(FormCollection collection, string StartDateRange, string EndDateRange, string reportID = "0")
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
                    reportID
                });
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error updating date range for display reports page");
                return RedirectToAction("../Shared/Error");
            }
        }

        public ActionResult PrintReport(string StartDateRange, string EndDateRange, string reportID = "0")
        {
            getCookie();

            string ID = reportID;

            if (ID == "0")
            {
                function.logAnError("No report ID Supplied Print report");
                Response.Redirect("/Report/Reports");
            }

            return View(getReportData(ID, StartDateRange, EndDateRange));
        }
    }
}