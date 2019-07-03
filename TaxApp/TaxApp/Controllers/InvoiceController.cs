using BLL;
using Model;
using System;
using System.Collections.Generic;
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

                        if (handler.getProfile(checkProfile) == null)
                        {
                            Response.Redirect("/Landing/Welcome");
                        }
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

        #region View Invoice
        // GET: Invoice
        public ActionResult Invoices()
        {
            return View();
        }

        // GET: Invoice/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        #endregion

        #region Invoice
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
                newInvoiceJobID.JobID = int.Parse(cookie["ID"].ToString());
                Invoice newInvoice = new Invoice();
                newInvoice.InvoiceNum = function.generateNewInvoiceNum();
                result = handler.newInvoice(newInvoice, newInvoiceJobID);

                if(result == true)
                {
                    InvoiceLineItem newDeatilLine = new InvoiceLineItem();
                    newDeatilLine.InvoiceNum = newInvoice.InvoiceNum;

                    foreach(SP_GetJobIntemsToInvoice_Result item in HoursResults)
                {
                        newDeatilLine.LineItemID = item.ID;
                        newDeatilLine.Name = item.Description;
                        newDeatilLine.UnitCost = item.UnitCost;
                        newDeatilLine.UnitCount = item.UnitCount;

                        result = handler.newInvoiceDetailLine(newDeatilLine);

                        if(result == false)
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
                else
                {
                    function.logAnError("Error creating new invoice");
                    Redirect("/Shared/Error");
                }

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in post new invoice of invoice controler");
                return View();
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
