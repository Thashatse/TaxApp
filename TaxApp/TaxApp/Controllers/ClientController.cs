using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaxApp.Controllers
{
    public class ClientController : Controller
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

        #region View
        // GET: Client
        public ActionResult Client()
        {
            try { 
            getCookie();
            Model.Client getClients = new Model.Client();
            getClients.ProfileID = int.Parse(cookie["ID"].ToString());
            List<Model.Client> Clients = handler.getProfileClients(getClients);
            return View(Clients);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loading clients");
                return RedirectToAction("../Shared/Error");
            }
        }

        // GET: Client/Details/5
        public ActionResult ClientDetails(int ID)
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
                    "Error loding client details");
                return Redirect("/job/jobs");
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Client/Edit/5
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
