using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;

namespace TaxApp.Controllers
{
    public class LandingController : Controller
    {
        IDBHandler handler = new DBHandler();
        Authentication Auth = new Authentication();

        // GET: Landing
        public ActionResult Welcome()
        {
            return View();
        }

#region Login
        // POST: Landing/Welcome
        [HttpPost]
        public ActionResult Welcome(FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                Model.Profile newProfile = new Model.Profile();

                newProfile.EmailAddress = Request.Form["Username"];
                newProfile.Username = Request.Form["Username"];
                newProfile.Password = Request.Form["Password"];

                string[] result = Auth.AuthenticateEmail(newProfile, Request.Form["Password"].ToString());

                if (result[0] == "PassN/A")
                {
                    return RedirectToAction("WrongPass");
                }
                else if (result[0] == "AccountN/A")
                {
                    return RedirectToAction("No Account");
                }
                else if (result[0] != null 
                    | result[1] != null)
                {
                    return RedirectToAction("../Home/index");
                }
                
                return RedirectToAction("../Shared/Error");
            }
            catch
            {
                return RedirectToAction("../Shared/Error");
            }
        }
        #endregion

        #region New Profile
        // GET: Landing/NewProfile
        public ActionResult NewProfile()
        {
            return View();
        }

        // POST: Landing/NewProfile
        [HttpPost]
        public ActionResult NewProfile(FormCollection collection)
        {
            try
            {
                Model.Profile newProfile = new Model.Profile();

                newProfile.FirstName = Request.Form["FirstName"];
                newProfile.LastName = Request.Form["LastName"];
                newProfile.CompanyName = Request.Form["CompanyName"];
                newProfile.EmailAddress = Request.Form["EmailAddress"];
                newProfile.ContactNumber = Request.Form["ContactNumber"];
                newProfile.PhysicalAddress = Request.Form["PhysicalAddress"];
                newProfile.VATNumber = Request.Form["VATNumber"];
                newProfile.DefaultHourlyRate = Convert.ToDecimal(Request.Form["DefaultHourlyRate"]);
                newProfile.Username = Request.Form["Username"];
                newProfile.Password = Request.Form["Password"];

                bool result = Auth.NewUser(newProfile);

                if(result == true)
                {
                return RedirectToAction("../Home/index");
                }
                else
                {
                    return RedirectToAction("../Shared/Error");
                }
            }
            catch
            {
                return View();
            }
        }
        #endregion
    }
}
