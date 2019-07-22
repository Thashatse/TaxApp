using BLL;
using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        #region Print Invoice
        // GET: Invoice
        public ActionResult Print(string id = "0")
        {
            try
            {
                getCookie();
                if (id == "0")
                {
                    function.logAnError("Error loding Print invoice details - No ID Supplied");
                    return Redirect("../Shared/error");
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
                    ViewBag.TotalExcludingVAT = total.ToString("0.##");
                    decimal totalVAT = ((total / 100) * invoiceDetails[0].VATRate);
                    ViewBag.VAT = totalVAT.ToString("0.##");
                    total = (totalVAT) + total;
                    ViewBag.TotalDue = total.ToString("0.##");

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
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding invoice details for Print");
                return Redirect("../Shared/Error");
            }
        }
        #endregion

        #region View Invoice
        // GET: Invoice
        public ActionResult Invoice(string id = "0")
        {
            try
            {
                getCookie();
                if (id == "0")
                {
                    function.logAnError("Error loding invoice details - No ID Supplied");
                    return Redirect("../Shared/error");
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

                    ViewBag.TotalExcludingVAT = total.ToString("#,0.##", nfi);
                    decimal totalVAT = ((total / 100) * invoiceDetails[0].VATRate);
                    ViewBag.VAT = totalVAT.ToString("#,0.##", nfi);
                    total = (totalVAT) + total;
                    ViewBag.TotalDue = total.ToString("#,0.##", nfi);

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
                    
                    if(invoiceDetails[0].Paid == true)
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
                    "Error loding invoice details");
                return Redirect("../Shared/Error");
            }
        }
        
        // GET: Invoice/Details/5
        public ActionResult JobInvoices(int id = 0)
        {
            try
            {
                getCookie();
                if(id.ToString() == null || id.ToString() == "")
                {
                    function.logAnError("Error loding all job invoices - No ID Supplied");
                    return Redirect("/job/jobs");
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

                    return View(invoiceDetails);
                }
                
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding all job invoices");
                return Redirect("../Shared/Error");
            }
        }
        #endregion

        #region View Income Page
        public ActionResult Income()
        {
            try
            {
                getCookie();

                Profile profileID = new Model.Profile();
                profileID.ProfileID = int.Parse(cookie["ID"]);

                List<SP_GetInvoice_Result> OutinvoiceDetails = handler.getInvoicesOutsatanding(profileID);
                List<SP_GetInvoice_Result> PastinvoiceDetails = handler.getInvoicesPast(profileID);
                Model.DashboardIncome IncomeDashboard = handler.getIncomeDashboard(profileID);

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
                return Redirect("../Shared/Error");
            }
        }
        #endregion

        #region Email Invoice
        // GET: Invoice
        public ActionResult EmailToCustomer(string id = "0")
        {
            try
            {
                getCookie();
                if (id == "0")
                {
                    function.logAnError("Error loding Print invoice details - No ID Supplied");
                    return Redirect("../Shared/error");
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
                    ViewBag.TotalExcludingVAT = total.ToString("0.##");
                    decimal totalVAT = ((total / 100) * invoiceDetails[0].VATRate);
                    ViewBag.VAT = totalVAT.ToString("0.##");
                    total = (totalVAT) + total;
                    ViewBag.TotalDue = total.ToString("0.##");

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
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding invoice details for Print");
                return Redirect("../Shared/Error");
            }
        }

        [HttpPost]
        public ActionResult EmailToCustomer(FormCollection collection, string ID)
        {
            try
            {
                getCookie();

                Invoice invoiceNum = new Invoice();
                invoiceNum.InvoiceNum = ID;

                List<SP_GetInvoice_Result> invoiceDetails = handler.getInvoiceDetails(invoiceNum);
                ViewBag.InvoiceItems = invoiceDetails;

                decimal total = new decimal();
                foreach (SP_GetInvoice_Result item in invoiceDetails)
                {
                    total += item.TotalCost;
                }
                ViewBag.TotalExcludingVAT = total.ToString("0.##");
                decimal totalVAT = ((total / 100) * invoiceDetails[0].VATRate);
                ViewBag.VAT = totalVAT.ToString("0.##");
                total = (totalVAT) + total;
                ViewBag.TotalDue = total.ToString("0.##");

                Profile getProfile = new Profile();
                getProfile.ProfileID = int.Parse(cookie["ID"]);
                getProfile.EmailAddress = "";
                getProfile.Username = "";
                getProfile = handler.getProfile(getProfile);

                bool result = function.sendEmail(invoiceDetails[0].EmailAddress,
                    invoiceDetails[0].ClientName,
                    Request.Form["subject"],
                    Request.Form["Message"],
                    getProfile.FirstName + " " + getProfile.LastName,
                    getProfile.ProfileID);

                if (result == true)
                {
                    Response.Write("<script>parent.close_window();</script>");
                }
                else
                {
                    function.logAnError("Error creating new invoice");
                    Redirect("../Shared/Error?Err=An Error Occured Sending Your Email, Please Try Again Later.");
                }

                return RedirectToAction("../Shared/Error?Err=An Error Occured Sending Your Email, Please Try Again Later.");
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in post new invoice of invoice controler");
                return View();
            }
        }
        #endregion

        #region New Invoice
        // GET: Invoice/Create
        public ActionResult NewInvoice(string ID)
        {
            try
            {
            getCookie();

            Model.Job job = new Model.Job();
            job.JobID = int.Parse(ID);

            Model.SP_GetJob_Result jobDetails = handler.getJob(job);
            ViewBag.JobName = jobDetails.JobTitle;
                ViewBag.JobID = jobDetails.JobID;

                List<List<SP_GetJobIntemsToInvoice_Result>> JobItemsForInvoice = handler.getJobItemsForInvoice(job);
            ViewBag.Hours = JobItemsForInvoice.ElementAt(0);
            ViewBag.Travelss = JobItemsForInvoice.ElementAt(1);
            ViewBag.Expenses = JobItemsForInvoice.ElementAt(2);

            return View();
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding new invoice page");
                return Redirect("/job/job?ID=" + ID);
            }
        }

        // POST: Invoice/Create
        [HttpPost]
        public ActionResult NewInvoice(FormCollection collection, string ID)
        {
            try
            {
                    getCookie();

                Model.Job job = new Model.Job();
                job.JobID = int.Parse(ID);

                Model.SP_GetJob_Result jobDetails = handler.getJob(job);
                ViewBag.JobName = jobDetails.JobTitle;

                List<List<SP_GetJobIntemsToInvoice_Result>> JobItemsForInvoice = handler.getJobItemsForInvoice(job);
                ViewBag.Hours = JobItemsForInvoice.ElementAt(0);
                ViewBag.Travelss = JobItemsForInvoice.ElementAt(1);
                ViewBag.Expenses = JobItemsForInvoice.ElementAt(2);

                List<SP_GetJobIntemsToInvoice_Result> HoursResults = new List<SP_GetJobIntemsToInvoice_Result>();
                List<SP_GetJobIntemsToInvoice_Result> ExpensesResults = new List<SP_GetJobIntemsToInvoice_Result>();
                List<SP_GetJobIntemsToInvoice_Result> TravelsResults = new List<SP_GetJobIntemsToInvoice_Result>();

                foreach(SP_GetJobIntemsToInvoice_Result item in JobItemsForInvoice.ElementAt(0))
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

                bool result = false;

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
                            newDeatilLine.UnitCount = item.UnitCount/60;
                            newDeatilLine.Type = 'H';

                            result = handler.newInvoiceDetailLine(newDeatilLine);

                            if (result == false)
                            {
                                function.logAnError("Error creating new invoice detale line Hours");
                                Redirect("/Shared/Error");
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
                                Redirect("/Shared/Error");
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
                                Redirect("/Shared/Error");
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
                                Redirect("/Shared/Error");
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
                                Redirect("/Shared/Error");
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
                                Redirect("/Shared/Error");
                            }
                        }
                    }
                    Response.Redirect("../Invoice/Invoice?ID=" + newInvoice.InvoiceNum);
                }
                else
                {
                    function.logAnError("Error creating new invoice");
                    Redirect("../Shared/Error");
                }

                return RedirectToAction("/Shared/Error");
            }
            catch(Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in post new invoice of invoice controler");
                return View();
            }
        }
        #endregion
        
        #region Mark Invoice As Paid
        // GET: Invoice/Create
        public ActionResult MarkAsPaid(string ID = "0")
        {
            try
            {
                getCookie();

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
                        Response.Redirect("../Shared/Error?Err=Error marking invoice as paid");
                    }
                }
                else
                {
                    function.logAnError("Error marking invoice as paid invoice controller - no invoice number supplied");
                    Response.Redirect("/Shared/Error?Err=Error marking invoice as paid");
                }

            return View();
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error marking invoice as paid invoice controller");
                return Redirect("/job/job?ID=" + ID);
            }
        }
        #endregion

        // GET: Invoice/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Invoice/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Invoice/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Invoice/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
