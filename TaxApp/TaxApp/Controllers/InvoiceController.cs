using BLL;
using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaxApp.Controllers
{
    public class InvoiceController : Controller
    {
        IDBHandler handler = new DBHandler();
        HttpCookie cookie;
        Functions function = new Functions();
        NotificationsFunctions notiFunctions = new NotificationsFunctions();
        public void getCookie(bool externalPrintCheck)
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
                else if (externalPrintCheck == true)
                {
                    cookie = Request.Cookies["TaxAppGuestUserID"];

                    if (cookie == null)
                        Response.Redirect(Url.Action("Welcome", "Landing"));
                    if (cookie["ID"] == null)
                        Response.Redirect(Url.Action("Welcome", "Landing"));
                    if (cookie["ID"] == "")
                        Response.Redirect(Url.Action("Welcome", "Landing"));
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

        #region Print Invoice
        // GET: Invoice
        public ActionResult Print(string id = "0")
        {
            try
            {
                getCookie(true);
                
                if (id == "0")
                {
                    function.logAnError("Error loding Print invoice details - No ID Supplied");
                    return RedirectToAction("Error", "Shared");
                }
                else
                {
                    Invoice invoiceNum = new Invoice();
                    invoiceNum.InvoiceNum = id;

                    List<SP_GetInvoice_Result> invoiceDetails = handler.getInvoiceDetails(invoiceNum);
                    ViewBag.InvoiceItems = invoiceDetails;

                    decimal total = new decimal();
                    foreach (SP_GetInvoice_Result item in invoiceDetails)
                    {
                        total += item.TotalCost;
                    }

                    var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                    nfi.NumberGroupSeparator = " ";

                    ViewBag.TotalExcludingVAT = total.ToString("#,0.00", nfi);
                    decimal totalVAT = ((total / 100) * invoiceDetails[0].VATRate);
                    ViewBag.VAT = totalVAT.ToString("#,0.00", nfi);
                    total = (totalVAT) + total;
                    ViewBag.TotalDue = total.ToString("#,0.00", nfi);

                    Profile getProfile = new Profile();
                    getProfile.ProfileID = invoiceDetails[0].ProfileID;
                    getProfile.EmailAddress = "";
                    getProfile.Username = "";
                    getProfile = handler.getProfile(getProfile);
                    ViewBag.VatNum = getProfile.VATNumber;
                    ViewBag.ProfileName = getProfile.FirstName + " " + getProfile.LastName;
                    ViewBag.ProfileEmail = getProfile.EmailAddress;
                    ViewBag.ProfileNo = getProfile.ContactNumber;

                    ViewBag.JobID = id;

                    if (invoiceDetails[0].Paid == true)
                    {
                        ViewBag.Paid = "Paid";
                    }
                    else
                    {
                        ViewBag.Paid = "Unpaid";
                    }

                    return View(invoiceDetails[0]);
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding invoice details for Print");
                return RedirectToAction("Error", "Shared");
            }
        }
        #endregion 
        
        #region Download Invoice PDF
        // GET: Invoice
        public FileResult Download(string id = "0")
        {
                getCookie(true);

            string InvoiceName = "";

            try
            {
                if (id == "0")
                {
                    function.logAnError("Error loding download invoice details - No ID Supplied");
                    Response.Redirect("/Shared/Error?ERR=Error downloading invoice - No ID Supplied");
                }
                else
                {
                    Invoice invoiceNum = new Invoice();
                    invoiceNum.InvoiceNum = id;

                    List<SP_GetInvoice_Result> invoiceDetails = handler.getInvoiceDetails(invoiceNum);

                    Profile getProfile = new Profile();
                    getProfile.ProfileID = invoiceDetails[0].ProfileID;
                    getProfile.EmailAddress = "";
                    getProfile.Username = "";
                    getProfile = handler.getProfile(getProfile);
                    
                    InvoiceName = "Invoice From " + getProfile.FirstName + " " + getProfile.LastName + " - " + invoiceDetails[0].DateTime+".pdf";
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding invoice details for Download");
                Response.Redirect(Url.Action("Error","Shared") +"?ERR=Error downloading invoice");
            }

            return File(function.downloadPage("https://www.mandela.ac.za/"), System.Net.Mime.MediaTypeNames.Application.Octet, InvoiceName);
        }
        #endregion

        #region View Invoice (External)
        // GET: Invoice
        public ActionResult viewInvoice(string id = "0")
        {
            try
            {
                getCookie(true);
                
                if (id == "0")
                {
                    function.logAnError("Error loding Print invoice details - No ID Supplied");
                    return RedirectToAction("Error", "Shared", new { err = "No ID Supplied" });
                }
                else
                {
                    Invoice invoiceNum = new Invoice();
                    invoiceNum.InvoiceNum = id;

                    List<SP_GetInvoice_Result> invoiceDetails = handler.getInvoiceDetails(invoiceNum);
                    ViewBag.InvoiceItems = invoiceDetails;

                    decimal total = new decimal();
                    foreach (SP_GetInvoice_Result item in invoiceDetails)
                    {
                        total += item.TotalCost;
                    }

                    var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                    nfi.NumberGroupSeparator = " ";

                    ViewBag.TotalExcludingVAT = total.ToString("#,0.00", nfi);
                    decimal totalVAT = ((total / 100) * invoiceDetails[0].VATRate);
                    ViewBag.VAT = totalVAT.ToString("#,0.00", nfi);
                    total = (totalVAT) + total;
                    ViewBag.TotalDue = total.ToString("#,0.00", nfi);

                    Profile getProfile = new Profile();
                    getProfile.ProfileID = invoiceDetails[0].ProfileID;
                    getProfile.EmailAddress = "";
                    getProfile.Username = "";
                    getProfile = handler.getProfile(getProfile);
                    ViewBag.VatNum = getProfile.VATNumber;
                    ViewBag.ProfileName = getProfile.FirstName + " " + getProfile.LastName;
                    ViewBag.ProfileEmail = getProfile.EmailAddress;
                    ViewBag.ProfileNo = getProfile.ContactNumber;

                    ViewBag.JobID = id;

                    if (invoiceDetails[0].Paid == true)
                    {
                        ViewBag.Paid = "Paid";
                    }
                    else
                    {
                        ViewBag.Paid = "Unpaid";
                    }

                    return View(invoiceDetails[0]);
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding invoice details for Print");
                return RedirectToAction("Error", "Shared");
            }
        }
        #endregion

        #region View Invoice
        public ActionResult Invoice(string id = "0")
        {
            try
            {
                ViewBag.cat = 'I';
                getCookie(false);
                if (id == "0")
                {
                    function.logAnError("Error loding invoice details - No ID Supplied");
                    return RedirectToAction("Error", "Shared", new { err = "No ID Supplied" });
                }
                else
                {
                    Invoice invoiceNum = new Invoice();
                    invoiceNum.InvoiceNum = id;

                    List<SP_GetInvoice_Result> invoiceDetails = handler.getInvoiceDetails(invoiceNum);
                    ViewBag.InvoiceItems = invoiceDetails;

                    decimal total = new decimal();
                    foreach(SP_GetInvoice_Result item in invoiceDetails)
                    {
                        total += item.TotalCost;
                    }

                    var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                    nfi.NumberGroupSeparator = " ";

                    ViewBag.TotalExcludingVAT = total.ToString("#,0.00", nfi);
                    decimal totalVAT = ((total / 100) * invoiceDetails[0].VATRate);
                    ViewBag.VAT = totalVAT.ToString("#,0.00", nfi);
                    total = (totalVAT) + total;
                    ViewBag.TotalDue = total.ToString("#,0.00", nfi);

                    Profile getProfile = new Profile();
                    getProfile.ProfileID = int.Parse(cookie["ID"]);
                    getProfile.EmailAddress = "";
                    getProfile.Username = "";
                    getProfile = handler.getProfile(getProfile);
                    ViewBag.VatNum = getProfile.VATNumber.Replace(" ", "");
                    ViewBag.ProfileName = getProfile.FirstName + " " + getProfile.LastName;
                    ViewBag.ProfileEmail = getProfile.EmailAddress;
                    ViewBag.ProfileNo = getProfile.ContactNumber;

                    ViewBag.JobID = id;
                    
                    if(invoiceDetails[0].Paid == true)
                    {
                        ViewBag.Paid = "Paid";
                    }
                    else
                    {
                        ViewBag.Paid = "Unpaid";
                    }

                    TaxConsultant consultant = new TaxConsultant();
                    consultant.ProfileID = int.Parse(cookie["ID"]);
                    consultant = handler.getConsumtant(consultant);

                    if (consultant != null)
                    {
                        ViewBag.TaxConsultant = true;
                    }

                    return View(invoiceDetails[0]);
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding invoice details");
                return RedirectToAction("Error", "Shared", new { err = "Error loading invoice" });
            }
        }
        
        public ActionResult JobInvoices(int id = 0)
        {
            try
            {
                ViewBag.cat = 'I';
                getCookie(false);
                if(id.ToString() == null || id.ToString() == "")
                {
                    function.logAnError("Error loding all job invoices - No ID Supplied");
                    return RedirectToAction("jobs","job");
                }
                else
                {
                    Job jobID = new Job();
                jobID.JobID = id;

                Model.SP_GetJob_Result jobDetails = handler.getJob(jobID);
                ViewBag.JobName = jobDetails.JobTitle;
                    ViewBag.JobID = jobDetails.JobID;

                    List<SP_GetInvoice_Result> invoiceDetails = handler.getJobInvoices(jobID);

                    List<List<SP_GetJobIntemsToInvoice_Result>> JobItemsForInvoice = handler.getJobItemsForInvoice(jobID);
                    if (JobItemsForInvoice.ElementAt(0).Count == 0
                        && JobItemsForInvoice.ElementAt(1).Count == 0
                        && JobItemsForInvoice.ElementAt(2).Count  == 0)
                    {
                        ViewBag.Complete = "Done";
                    }
                    else
                    {
                        ViewBag.Complete = "NotDone";
                    }

                    if (invoiceDetails.Count < 1
                        && JobItemsForInvoice.ElementAt(0).Count != 0
                    && JobItemsForInvoice.ElementAt(1).Count != 0
                    && JobItemsForInvoice.ElementAt(2).Count != 0)
                        return Redirect(Url.Action("NewInvoice", "Invoice") + "?ID="+id+"&ReturnTo=Job");

                    if (invoiceDetails.Count < 1
                        && JobItemsForInvoice.ElementAt(0).Count == 0
                    && JobItemsForInvoice.ElementAt(1).Count == 0
                    && JobItemsForInvoice.ElementAt(2).Count == 0)
                        return Redirect(Url.Action("job", "job")+"?ID=" + id);

                    return View(invoiceDetails);
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding all job invoices");
                return RedirectToAction("Error", "Shared");
            }
        }
        #endregion

        #region View Income Page
        public ActionResult Income(string view, string PastInvoiceDisplayCount, string StartDateRange, string EndDateRange)
        {
            try
            {
                ViewBag.cat = 'I';

                getCookie(false);

                ViewBag.view = view;
                ViewBag.SeeMore = false;
                int year = DateTime.Now.Year;
                DateTime sDate = DateTime.Now.AddMonths(-6);
                DateTime eDate = DateTime.Now;

                if (StartDateRange != null && EndDateRange != null
                    && DateTime.TryParse(StartDateRange, out sDate) && DateTime.TryParse(EndDateRange, out eDate)) { }

                if (sDate > eDate)
                {
                    DateTime temp = sDate;
                    sDate = eDate;
                    eDate = temp;
                }

                ViewBag.DateRange = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                ViewBag.StartDateRange = sDate.ToString("yyyy-MM-dd");
                ViewBag.EndDateRange = eDate.ToString("yyyy-MM-dd");

                Profile profileID = new Model.Profile();
                profileID.ProfileID = int.Parse(cookie["ID"]);

                List<SP_GetInvoice_Result> OutinvoiceDetails = handler.getInvoicesOutsatanding(profileID);
                List<SP_GetInvoice_Result> PastinvoiceDetails = handler.getInvoicesPast(profileID, sDate, eDate);
                Model.DashboardIncome IncomeDashboard = handler.getIncomeDashboard(profileID);

                if (PastinvoiceDetails.Count > 8)
                {
                    int x;
                    if (PastInvoiceDisplayCount != null && PastInvoiceDisplayCount != "" 
                        && function.IsDigitsOnly(PastInvoiceDisplayCount))
                        x = int.Parse(PastInvoiceDisplayCount);
                    else
                        x = 8;

                    if (x < PastinvoiceDetails.Count)
                    {
                        PastinvoiceDetails = PastinvoiceDetails.GetRange(0, x);
                        ViewBag.SeeMore = true;
                    }
                    else
                    {
                        PastinvoiceDetails = PastinvoiceDetails.GetRange(0, PastinvoiceDetails.Count);
                        ViewBag.SeeMore = false;
                    }

                    ViewBag.X = x + 8;
                }

                var viewModel = new Model.incomeViewModel();
                viewModel.OutInvoices = OutinvoiceDetails;
                viewModel.PastInvoices = PastinvoiceDetails;
                viewModel.DashboardIncome = IncomeDashboard;

                return View(viewModel);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding income Page");
                return RedirectToAction("Error", "Shared");
            }
        }

        [HttpPost]
        public ActionResult Income(FormCollection collection, string view, string PastInvoiceDisplayCount, string StartDateRange, string EndDateRange)
        {
            try
            {
                ViewBag.cat = 'I';
                int year = DateTime.Now.Year;
                DateTime sDate = DateTime.Now.AddMonths(-6);
                DateTime eDate = DateTime.Now;

                DateTime.TryParse(Request.Form["StartDate"], out sDate);
                DateTime.TryParse(Request.Form["EndDate"], out eDate);

                StartDateRange = sDate.ToString("yyyy-MM-dd");
                EndDateRange = eDate.ToString("yyyy-MM-dd");

                return RedirectToAction("Income", "Invoice", new
                {
                    view,
                    PastInvoiceDisplayCount,
                    StartDateRange,
                    EndDateRange
                });
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error updating date range for income page");
                return RedirectToAction("Error", "Shared");
            }
        }
        #endregion

        #region Email Invoice
        // GET: Invoice
        public ActionResult EmailInvoice(string To, string id = "0")
        {
            try
            {
                getCookie(false);

                if (id == "0")
                {
                    function.logAnError("Error loding Print invoice details - No ID Supplied");
                    return RedirectToAction("Error", "Shared", new { err = "No ID Supplied" });
                }
                else
                {
                    Invoice invoiceNum = new Invoice();
                    invoiceNum.InvoiceNum = id;

                    List<SP_GetInvoice_Result> invoiceDetails = handler.getInvoiceDetails(invoiceNum);
                    ViewBag.InvoiceItems = invoiceDetails;

                    decimal total = new decimal();
                    foreach (SP_GetInvoice_Result item in invoiceDetails)
                    {
                        total += item.TotalCost;
                    }

                    var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                    nfi.NumberGroupSeparator = " ";

                    ViewBag.TotalExcludingVAT = total.ToString("#,0.00", nfi);
                    decimal totalVAT = ((total / 100) * invoiceDetails[0].VATRate);
                    ViewBag.VAT = totalVAT.ToString("#,0.00", nfi);
                    total = (totalVAT) + total;
                    ViewBag.TotalDue = total.ToString("#,0.00", nfi);

                    Profile getProfile = new Profile();
                    getProfile.ProfileID = int.Parse(cookie["ID"]);
                    getProfile.EmailAddress = "";
                    getProfile.Username = "";
                    getProfile = handler.getProfile(getProfile);
                    ViewBag.VatNum = getProfile.VATNumber;
                    ViewBag.ProfileName = getProfile.FirstName + " " + getProfile.LastName;
                    ViewBag.ProfileEmail = getProfile.EmailAddress;
                    ViewBag.ProfileNo = getProfile.ContactNumber;

                    ViewBag.JobID = id;

                    if (invoiceDetails[0].Paid == true)
                    {
                        ViewBag.Paid = "Paid";
                    }
                    else
                    {
                        ViewBag.Paid = "Unpaid";
                    }

                    if(To == "Consultant")
                    {
                        TaxConsultant consultant = new TaxConsultant();
                        consultant.ProfileID = int.Parse(cookie["ID"]);
                        consultant = handler.getConsumtant(consultant);
                        ViewBag.To = consultant.Name + " - " + consultant.EmailAddress;
                    }
                    else
                    {
                        ViewBag.To = invoiceDetails[0].ClientName + " - " + invoiceDetails[0].EmailAddress;
                    }

                    ViewBag.Body = "\n\nInvoice Summary: \n\n" +
                        "From: " + ViewBag.ProfileName + "\n" +
                        "Billed To: " + invoiceDetails[0].ClientName + " " + invoiceDetails[0].CompanyName + "\n" +
                        "Job: " + invoiceDetails[0].JobTitle + "\n\n" +
                        "Total Due: R" + ViewBag.TotalDue;

                    return View(invoiceDetails[0]);
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding invoice details for Print");
                return RedirectToAction("Error", "Shared");
            }
        }

        public ActionResult Email(string id)
        {
            try
            {
                    Invoice invoiceNum = new Invoice();
                    invoiceNum.InvoiceNum = id;

                    List<SP_GetInvoice_Result> invoiceDetails = handler.getInvoiceDetails(invoiceNum);
                    ViewBag.InvoiceItems = invoiceDetails;

                    decimal total = new decimal();
                    foreach (SP_GetInvoice_Result item in invoiceDetails)
                    {
                        total += item.TotalCost;
                    }

                var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                nfi.NumberGroupSeparator = " ";

                ViewBag.TotalExcludingVAT = total.ToString("#,0.00", nfi);
                decimal totalVAT = ((total / 100) * invoiceDetails[0].VATRate);
                ViewBag.VAT = totalVAT.ToString("#,0.00", nfi);
                total = (totalVAT) + total;
                ViewBag.TotalDue = total.ToString("#,0.00", nfi);

                Profile getProfile = new Profile();
                    getProfile.ProfileID = int.Parse(cookie["ID"]);
                    getProfile.EmailAddress = "";
                    getProfile.Username = "";
                    getProfile = handler.getProfile(getProfile);
                    ViewBag.VatNum = getProfile.VATNumber;
                    ViewBag.ProfileName = getProfile.FirstName + " " + getProfile.LastName;
                    ViewBag.ProfileEmail = getProfile.EmailAddress;
                    ViewBag.ProfileNo = getProfile.ContactNumber;

                    ViewBag.JobID = id;

                    if (invoiceDetails[0].Paid == true)
                    {
                        ViewBag.Paid = "Paid";
                    }
                    else
                    {
                        ViewBag.Paid = "Unpaid";
                    }

                    return View(invoiceDetails[0]);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding invoice for email");
                return RedirectToAction("Error", "Shared");
            }
        }

        [HttpPost]
        public ActionResult EmailInvoice(FormCollection collection, string ID, string To)
        {
            try
            {
                getCookie(false);

                Invoice invoiceNum = new Invoice();
                invoiceNum.InvoiceNum = ID;

                List<SP_GetInvoice_Result> invoiceDetails = handler.getInvoiceDetails(invoiceNum);
                ViewBag.InvoiceItems = invoiceDetails;

                decimal total = new decimal();
                foreach (SP_GetInvoice_Result item in invoiceDetails)
                {
                    total += item.TotalCost;
                }

                var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                nfi.NumberGroupSeparator = " ";

                ViewBag.TotalExcludingVAT = total.ToString("#,0.00", nfi);
                decimal totalVAT = ((total / 100) * invoiceDetails[0].VATRate);
                ViewBag.VAT = totalVAT.ToString("#,0.00", nfi);
                total = (totalVAT) + total;
                ViewBag.TotalDue = total.ToString("#,0.00", nfi);

                Profile getProfile = new Profile();
                getProfile.ProfileID = int.Parse(cookie["ID"]);
                getProfile.EmailAddress = "";
                getProfile.Username = "";
                getProfile = handler.getProfile(getProfile);

                string toAddress;
                string toName;
                if (To == "Consultant")
                {
                    TaxConsultant consultant = new TaxConsultant();
                    consultant.ProfileID = int.Parse(cookie["ID"]);
                    consultant = handler.getConsumtant(consultant);
                    toAddress = consultant.EmailAddress;
                    toName = consultant.Name;
                }
                else
                {
                    toAddress = invoiceDetails[0].EmailAddress;
                    toName = invoiceDetails[0].ClientName;
                }

                bool result = function.sendEmail(toAddress,
                    toName,
                    Request.Form["subject"],
                    Request.Form["Message"] 
                    + "\n\nView invoice here: http://localhost:54533/invoice/viewInvoice?ID="+invoiceDetails[0].InvoiceNum,
                    getProfile.FirstName + " " + getProfile.LastName,
                    getProfile.ProfileID);

                if (result == true)
                {
                    Notifications newNoti = new Notifications();
                    newNoti.date = DateTime.Now;
                    newNoti.ProfileID = int.Parse(cookie["ID"]);
                    newNoti.Link = "/Invoice/Invoice?ID=" + ID;
                    newNoti.Details = "Invoice Succesffuly sent to "+ toName;
                    notiFunctions.newNotification(newNoti);

                    ViewBag.Processed = true;
                    return View();
                }
                else
                {
                    function.logAnError("Error sending invoice");
                    return RedirectToAction("Error", "Shared", new { err = "An Error Occured Sending Your Email, Please Try Again Later."});
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in post new invoice of invoice controler");
                return RedirectToAction("Error", "Shared");
            }
        }
        #endregion

        #region New Invoice
        // GET: Invoice/Create
        public ActionResult NewInvoice(string ID, string ReturnTo)
        {
            try
            {
            getCookie(false);

                ViewBag.ReturnTo = ReturnTo;

            Model.Job job = new Model.Job();
            job.JobID = int.Parse(ID);

            Model.SP_GetJob_Result jobDetails = handler.getJob(job);
            ViewBag.JobName = jobDetails.JobTitle;
                ViewBag.JobID = jobDetails.JobID;

                List<List<SP_GetJobIntemsToInvoice_Result>> JobItemsForInvoice = handler.getJobItemsForInvoice(job);
            ViewBag.Hours = JobItemsForInvoice.ElementAt(0);
            ViewBag.Travelss = JobItemsForInvoice.ElementAt(1);
            ViewBag.Expenses = JobItemsForInvoice.ElementAt(2);
                ViewBag.Custom = new List<SP_GetJobIntemsToInvoice_Result>();

                return View();
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding new invoice page");
                return RedirectToAction("Error", "Shared");
            }
        }

        // POST: Invoice/Create
        [HttpPost]
        public ActionResult NewInvoice(FormCollection collection, string ID, string ReturnTo, string submit)
        {
            try
            {
                getCookie(false);

                Model.Job job = new Model.Job();
                job.JobID = int.Parse(ID);

                Model.SP_GetJob_Result jobDetails = handler.getJob(job);
                ViewBag.JobName = jobDetails.JobTitle;
                ViewBag.JobID = jobDetails.JobID;
                ViewBag.ReturnTo = ReturnTo;

                List<List<SP_GetJobIntemsToInvoice_Result>> JobItemsForInvoice = handler.getJobItemsForInvoice(job);
                ViewBag.Hours = JobItemsForInvoice.ElementAt(0);
                ViewBag.Travelss = JobItemsForInvoice.ElementAt(1);
                ViewBag.Expenses = JobItemsForInvoice.ElementAt(2);

                List<SP_GetJobIntemsToInvoice_Result> CustomItems = new List<SP_GetJobIntemsToInvoice_Result>();

                if (Request.Form["customItemsList"] != null)
                {
                    string[] items = Request.Form["customItemsList"].Split('|');
                    int c = 0;
                    foreach (string item in items)
                    {
                        if (item != "")
                        {
                            string[] values = item.Split('*');
                            CustomItems.Add(new SP_GetJobIntemsToInvoice_Result
                            {
                                ID = c,
                                UnitCost = decimal.Parse(values[2]),
                                UnitCount = decimal.Parse(values[0]),
                                Description = values[1],
                                DisplayString = values[0] + "* " + values[1] + " at R" + values[2]
                            });
                            ViewBag.customItemsList += values[0] + "*" + values[1] + "*" + values[2] + "|";
                            c++;
                        }
                    }
                }

                switch (submit)
                {
                    case "Create Invoice":
                        #region
                        List<SP_GetJobIntemsToInvoice_Result> HoursResults = new List<SP_GetJobIntemsToInvoice_Result>();
                        List<SP_GetJobIntemsToInvoice_Result> ExpensesResults = new List<SP_GetJobIntemsToInvoice_Result>();
                        List<SP_GetJobIntemsToInvoice_Result> TravelsResults = new List<SP_GetJobIntemsToInvoice_Result>();
                        List<SP_GetJobIntemsToInvoice_Result> CustomResults = new List<SP_GetJobIntemsToInvoice_Result>();

                        foreach (SP_GetJobIntemsToInvoice_Result item in JobItemsForInvoice.ElementAt(0))
                        {
                            if (Request.Form["Hour+" + item.ID.ToString()] != null)
                            {
                                if (Request.Form["Hour+" + item.ID.ToString()] == item.ID.ToString())
                                {
                                    HoursResults.Add(item);
                                }
                            }
                        }

                        foreach (SP_GetJobIntemsToInvoice_Result item in JobItemsForInvoice.ElementAt(2))
                        {
                            if (Request.Form["Expense+" + item.ID.ToString()] != null)
                            {
                                if (Request.Form["Expense+" + item.ID.ToString()] == item.ID.ToString())
                                {
                                    ExpensesResults.Add(item);
                                }
                            }
                        }

                        foreach (SP_GetJobIntemsToInvoice_Result item in JobItemsForInvoice.ElementAt(1))
                        {
                            if (Request.Form["Travel+" + item.ID.ToString()] != null)
                            {
                                if (Request.Form["Travel+" + item.ID.ToString()] == item.ID.ToString())
                                {
                                    TravelsResults.Add(item);
                                }
                            }
                        }

                        for (int c = 0; c <= CustomItems.Count; c++)
                        {
                            if (Request.Form["Custom+" + c] != null)
                            {
                                if (Request.Form["Custom+" + c] == CustomItems[c].ID.ToString())
                                {
                                    CustomResults.Add(CustomItems[c]);
                                }
                            }
                        }

                        bool result = false;

                        if (Request.Form["AllCheck"] != "True"
                            && HoursResults.Count == 0
                            && ExpensesResults.Count == 0
                            && TravelsResults.Count == 0
                            && CustomResults.Count == 0)
                            result = true;

                        if (result == false)
                        {
                            Job newInvoiceJobID = new Job();
                            newInvoiceJobID.JobID = int.Parse(ID);
                            Invoice newInvoice = new Invoice();
                            newInvoice.InvoiceNum = function.generateNewInvoiceNum();
                            result = handler.newInvoice(newInvoice, newInvoiceJobID);

                            if (result == true)
                            {
                                InvoiceLineItem newDeatilLine = new InvoiceLineItem();
                                newDeatilLine.InvoiceNum = newInvoice.InvoiceNum;

                                if (Request.Form["AllCheck"] == "True")
                                {
                                    foreach (SP_GetJobIntemsToInvoice_Result item in JobItemsForInvoice.ElementAt(0))
                                    {
                                        newDeatilLine.LineItemID = item.ID;
                                        newDeatilLine.Name = item.Description;
                                        newDeatilLine.UnitCost = item.UnitCost;
                                        newDeatilLine.UnitCount = item.UnitCount / 60;
                                        newDeatilLine.Type = 'H';

                                        result = handler.newInvoiceDetailLine(newDeatilLine);

                                        if (result == false)
                                        {
                                            function.logAnError("Error creating new invoice detale line Hours");
                                            return RedirectToAction("Error", "Shared");
                                        }
                                    }

                                    foreach (SP_GetJobIntemsToInvoice_Result item in JobItemsForInvoice.ElementAt(1))
                                    {
                                        newDeatilLine.LineItemID = item.ID;
                                        newDeatilLine.Name = item.Description;
                                        newDeatilLine.UnitCost = item.UnitCost;
                                        newDeatilLine.UnitCount = item.UnitCount;
                                        newDeatilLine.Type = 'T';

                                        result = handler.newInvoiceDetailLine(newDeatilLine);

                                        if (result == false)
                                        {
                                            function.logAnError("Error creating new invoice detale line Expenses");
                                            return RedirectToAction("Error", "Shared");
                                        }
                                    }

                                    foreach (SP_GetJobIntemsToInvoice_Result item in JobItemsForInvoice.ElementAt(2))
                                    {
                                        newDeatilLine.LineItemID = item.ID;
                                        newDeatilLine.Name = item.Description;
                                        newDeatilLine.UnitCost = item.UnitCost;
                                        newDeatilLine.UnitCount = item.UnitCount;
                                        newDeatilLine.Type = 'E';

                                        result = handler.newInvoiceDetailLine(newDeatilLine);

                                        if (result == false)
                                        {
                                            function.logAnError("Error creating new invoice detale line Travel");
                                            return RedirectToAction("Error", "Shared");
                                        }
                                    }

                                    foreach (SP_GetJobIntemsToInvoice_Result item in CustomItems)
                                    {
                                        newDeatilLine.LineItemID = item.ID;
                                        newDeatilLine.Name = item.Description;
                                        newDeatilLine.UnitCost = item.UnitCost;
                                        newDeatilLine.UnitCount = item.UnitCount;
                                        newDeatilLine.Type = 'C';

                                        result = handler.newInvoiceDetailLine(newDeatilLine);

                                        if (result == false)
                                        {
                                            function.logAnError("Error creating new invoice detale line custom");
                                            return RedirectToAction("Error", "Shared");
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (SP_GetJobIntemsToInvoice_Result item in HoursResults)
                                    {
                                        newDeatilLine.LineItemID = item.ID;
                                        newDeatilLine.Name = item.Description;
                                        newDeatilLine.UnitCost = item.UnitCost;
                                        newDeatilLine.UnitCount = item.UnitCount / 60;

                                        result = handler.newInvoiceDetailLine(newDeatilLine);

                                        if (result == false)
                                        {
                                            function.logAnError("Error creating new invoice detale line Hours");
                                            return RedirectToAction("Error", "Shared");
                                        }
                                    }

                                    foreach (SP_GetJobIntemsToInvoice_Result item in ExpensesResults)
                                    {
                                        newDeatilLine.LineItemID = item.ID;
                                        newDeatilLine.Name = item.Description;
                                        newDeatilLine.UnitCost = item.UnitCost;
                                        newDeatilLine.UnitCount = item.UnitCount;

                                        result = handler.newInvoiceDetailLine(newDeatilLine);

                                        if (result == false)
                                        {
                                            function.logAnError("Error creating new invoice detale line Expenses");
                                            return RedirectToAction("Error", "Shared");
                                        }
                                    }

                                    foreach (SP_GetJobIntemsToInvoice_Result item in TravelsResults)
                                    {
                                        newDeatilLine.LineItemID = item.ID;
                                        newDeatilLine.Name = item.Description;
                                        newDeatilLine.UnitCost = item.UnitCost;
                                        newDeatilLine.UnitCount = item.UnitCount;

                                        result = handler.newInvoiceDetailLine(newDeatilLine);

                                        if (result == false)
                                        {
                                            function.logAnError("Error creating new invoice detale line Travel");
                                            return RedirectToAction("Error", "Shared");
                                        }
                                    }

                                    foreach (SP_GetJobIntemsToInvoice_Result item in CustomResults)
                                    {
                                        newDeatilLine.LineItemID = item.ID;
                                        newDeatilLine.Name = item.Description;
                                        newDeatilLine.UnitCost = item.UnitCost;
                                        newDeatilLine.UnitCount = item.UnitCount;

                                        result = handler.newInvoiceDetailLine(newDeatilLine);

                                        if (result == false)
                                        {
                                            function.logAnError("Error creating new invoice detale line Custom");
                                            return RedirectToAction("Error", "Shared");
                                        }
                                    }
                                }

                                return Redirect(Url.Action("Invoice", "Invoice") + "?ID=" + newInvoice.InvoiceNum);
                            }

                            function.logAnError("Error creating new invoice");
                            return RedirectToAction("Error", "Shared");
                        }
                        else
                        {
                            ViewBag.Err = "Please select or add a custom an item to add to the invoice";
                        }
                        #endregion
                        break;
                    case "Add custom item":
                        #region
                        if (Request.Form["name"] == "")
                        {
                            ViewBag.Err = "Please enter a name for custom item";
                            return View();
                        }
                        if (decimal.Parse(Request.Form["Amount"]) <= 0)
                        {
                            ViewBag.Err = "Please enter an amount greater than 0 for custom item Amount";
                            return View();
                        }
                        if (decimal.Parse(Request.Form["Unit"]) <= 0)
                        {
                            ViewBag.Err = "Please enter an amount greater than 0 for custom item Unit";
                            return View();
                        }

                        CustomItems.Add(new SP_GetJobIntemsToInvoice_Result
                        {
                            DisplayString = Request.Form["Unit"] + "* " + Request.Form["name"] + " at R" + Request.Form["Amount"]
                        });
                        ViewBag.customItemsList += Request.Form["Unit"] + "*" + Request.Form["name"] + "*" + Request.Form["Amount"] + "|";
                        #endregion
                        break;
                }
                ViewBag.Custom = CustomItems;
                return View();
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in post new invoice of invoice controler");
                return RedirectToAction("Error", "Shared");
            }
        }

        #region Mark Invoice As Paid
        // GET: Invoice/Create
        public ActionResult MarkAsPaid(string ID = "0")
        {
            try
            {
                getCookie(false);

                if(ID != "0")
                {
                    Invoice invoice = new Invoice();
                    invoice.InvoiceNum = ID;
                    bool result = handler.MarkInvoiceAsPaid(invoice);

                    if(result == true)
                    {
                        Response.Redirect("/Invoice/Invoice?ID="+ID);
                    }
                    else
                    {
                        function.logAnError("Error marking invoice as paid invoice controller - no invoice number supplied");
                        return RedirectToAction("Error", "Shared", new { err = "Error marking invoice as paid"});
                    }
                }
                else
                {
                    function.logAnError("Error marking invoice as paid invoice controller - no invoice number supplied");
                    return RedirectToAction("Error", "Shared", new { err = "Error marking invoice as paid" });
                }

            return View();
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error marking invoice as paid invoice controller");
                return RedirectToAction("Error", "Shared");
            }
        }
        #endregion
    }
}
        #endregion