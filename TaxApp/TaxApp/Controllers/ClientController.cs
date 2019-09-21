using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;

namespace TaxApp.Controllers
{
    public class ClientController : Controller
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

        #region View
        // GET: Client
        public ActionResult Client()
        {
            try
            {
                ViewBag.cat = "C";
                getCookie();
            Model.Client getClients = new Model.Client();
            getClients.ProfileID = int.Parse(cookie["ID"].ToString());
            List<Model.Client> Clients = handler.getProfileClients(getClients);

                if (Clients.Count == 0)
                    Response.Redirect("../Client/NewClient");

                return View(Clients);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loading clients");
                return RedirectToAction("../Shared/Error?Err=Error loading clients");
            }
        }

        // GET: Client/Details/5
        public ActionResult ClientDetails(int ID = 0)
        {
            if(ID == 0)
                return RedirectToAction("Client", "Client");

            try
            {
            getCookie();
            Model.Client getClient = new Model.Client();
            getClient.ClientID = ID;
            Model.Client client = handler.getClient(getClient);

                if (client == null || client.FirstName == "")
                    return RedirectToAction("Client", "Client");

                DateTime sDate = DateTime.Now.AddYears(-100);
                DateTime eDate = DateTime.Now;

                Model.Profile getJobs = new Model.Profile();
                getJobs.ProfileID = 0;
                List<Model.SP_GetJob_Result> Jobs = handler.getProfileJobsPast(getJobs, client, sDate, eDate);
                Jobs.AddRange(handler.getProfileJobs(getJobs, client));
                Jobs.OrderBy(x => x.StartDate).ToList();

                Profile profileID = new Model.Profile();
                profileID.ProfileID = 0;

                List<SP_GetInvoice_Result> invoices = handler.getInvoices(profileID, client);
                invoices.OrderBy(x => x.DateTime).ToList();

                ClientDetailsViewModel view = new ClientDetailsViewModel();
                view.Client = client;
                view.Jobs = Jobs;
                view.Invoices = invoices;

            return View(view);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding client details");
                return Redirect("../Client/Client");
            }
        }
        #endregion

        #region new
        // GET: Client/Create
        public ActionResult NewClient()
        {
            getCookie();
            return View();
        }

        // POST: Client/Create
        [HttpPost]
        public ActionResult NewClient(FormCollection collection, string ID)
        {
            try
            {
                getCookie();

                Model.Client newClient = new Model.Client();

                newClient.FirstName = Request.Form["FirstName"].ToString();
                newClient.LastName = Request.Form["LastName"].ToString();
                newClient.ContactNumber = Request.Form["ContactNumber"].ToString();
                newClient.EmailAddress = Request.Form["EmailAddress"].ToString();
                newClient.CompanyName = Request.Form["CompanyName"].ToString();
                newClient.PhysiclaAddress = Request.Form["PhysiclaAddress"].ToString();
                newClient.ProfileID = int.Parse(cookie["ID"].ToString());
                newClient.PreferedCommunicationChannel = "EMA";

                bool result = handler.newClient(newClient);

                if (result == true)
                {
                    return Redirect("/Client/Client");
                }
                else
                {
                    return RedirectToAction("../Shared/Error");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in new client of client controler");
                return View();
            }
        }
        #endregion

        #region edit
        // GET: Client/Edit/5
        public ActionResult Edit(int ID)
        {
            try
            {
                getCookie();
                Model.Client getClient = new Model.Client();
                getClient.ClientID = ID;
                Model.Client client = handler.getClient(getClient);
                return View(client);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding client details for edit");
                return Redirect("../Client/Client");
            }
        }

        // POST: Client/Edit/5
        [HttpPost]
        public ActionResult Edit(int ID, FormCollection collection)
        {
            try
            {
                getCookie();
                Model.Client getClient = new Model.Client();
                getClient.ClientID = ID;
                Model.Client client = handler.getClient(getClient);

                client.FirstName = Request.Form["FirstName"].ToString();
                client.LastName = Request.Form["LastName"].ToString();
                client.ContactNumber = Request.Form["ContactNumber"].ToString();
                client.EmailAddress = Request.Form["EmailAddress"].ToString();
                client.CompanyName = Request.Form["CompanyName"].ToString();
                client.PhysiclaAddress = Request.Form["PhysiclaAddress"].ToString();
                client.ProfileID = int.Parse(cookie["ID"].ToString());
                client.PreferedCommunicationChannel = "EMA";

                bool result = handler.editClient(client);

                if (result == true)
                {
                    return Redirect("/Client/ClientDetails?ID="+client.ClientID);
                }
                else
                {
                    Response.Redirect("../Shared/Error?Err=Error saving client details");
                }

                return View();
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error commiting client edit");
                return RedirectToAction("../Shared/Error?Err=Error saving client details");
            }
        }
        #endregion

        #region Delete
        // GET: Client/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Client/Delete/5
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
        #endregion
    }
}