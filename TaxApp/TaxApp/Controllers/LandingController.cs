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
        HttpCookie cookie;

        // GET: Landing
        public ActionResult Welcome(string Err)
        {
            string err = Err;
            if (err == "UserPassNA")
            {
                ViewBag.Message = "Incorrect username or password";
            }
            else if (err == "PassNull")
            {
                ViewBag.Message = "please enter a password";
            }
            else if (err == "UserNull")
            {
                ViewBag.Message = "Please enter a valid email address or username";
            }
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

                if(newProfile.EmailAddress == ""
                    || newProfile.Username == "")
                {
                    return Redirect("/Landing/Welcome?Err=UserNull");
                }
                else if (newProfile.Password == "")
                {
                    return Redirect("/landing/Welcome?Err=PassNull");
                }
                else if (newProfile.EmailAddress != ""
                    && newProfile.Username != ""
                    && newProfile.Password != "")
                {
                string[] result = Auth.AuthenticateEmail(newProfile, Request.Form["Password"].ToString());

                if (result[0] == "PassN/A")
                {
                    return Redirect("/landing/Welcome?Err=UserPassNA");
                }
                else if (result[0] == "AccountN/A")
                {
                    return Redirect("/landing/Welcome?Err=UserPassNA");
                }
                else if (result[0] != null 
                    | result[1] != null)
                    {
                        createCookie(handler.getProfile(newProfile));
                        return Redirect("/Home/index");
                }
                }
                return Redirect("/Shared/Error");
            }
            catch
            {
                return Redirect("/Shared/Error");
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
                    Model.Profile profile = handler.getProfile(newProfile);
                    
                    if(profile != null)
                    {
                        createCookie(profile);
                        return Redirect("/Landing/TaxConsultant?ID="+profile.ProfileID.ToString());
                    }
                    else
                    {
                        return RedirectToAction("../Shared/Error");
                    }
                    
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

        #region Cookie managment
        public void createCookie(Model.Profile profile)
        {
            //log the user in by creating a cookie to manage their state
            cookie = new HttpCookie("CheveuxUserID");
            // Set the user id in it.
            cookie["ID"] = profile.ProfileID.ToString();
            // Add it to the current web response.
            Response.Cookies.Add(cookie);
        }
        #endregion

        #region New Consultant
        // GET: Landing/NewProfile
        public ActionResult TaxConsultant()
        {
            return View();
        }

        // POST: Landing/NewProfile
        [HttpPost]
        public ActionResult TaxConsultant(FormCollection collection, string ID)
        {
            string profileID = ID;
            try
            {
                Model.TaxConsultant newConsultant = new Model.TaxConsultant();

                newConsultant.Name = Request.Form["Name"];
                newConsultant.EmailAddress = Request.Form["EmailAddress"];
                newConsultant.ProfileID = int.Parse(profileID);

                bool result = handler.newConsultant(newConsultant);

                if (result == true)
                {
                        return Redirect("/Landing/EmailSettings?ID=" + profileID.ToString());
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
