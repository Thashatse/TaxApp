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

        // GET: Client
        public ActionResult Index()
        {
            return View();
        }

        // GET: Client/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

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
                    return Redirect("/Client/newClient");
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
    }
}
