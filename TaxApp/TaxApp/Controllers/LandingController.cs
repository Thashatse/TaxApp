﻿using System;
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
        Functions function = new Functions();

        #region Login
        // GET: Landing
        public ActionResult Welcome(string Err)
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

                        if (handler.getProfile(checkProfile) != null)
                        {
                            Response.Redirect("/Home/Index");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in welcome method of LandingControles");
                Redirect("/Shared/Error");
            }

            return View();
        }
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
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in welcome method of LandingControles");
                return Redirect("/Shared/Error");
            }
        }
        #endregion

        #region Logout
        // GET: Landing
        public ActionResult Logout(string Err)
        {
            try
            {
                //check if the user is loged in
                cookie = Request.Cookies["TaxAppUserID"];

                if (cookie != null)
                {
                    cookie = new HttpCookie("TaxAppUserID");
                    cookie.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(cookie);
                }

                Response.Redirect("/Home/Index");
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in welcome method of LandingControles");
                Redirect("/Shared/Error");
            }

            return View();
        }
        #endregion

        #region New Profile
        // GET: Landing/NewProfile
        public ActionResult NewProfile(string Err)
        {
            string err = Err;
            if (err == "UserEmailEx")
            {
                ViewBag.Message = "Username or email already registered";
            }
            else if (err == "PassMatch")
            {
                ViewBag.Message = "Passwords do not match, please try again!";
            }
            return View();
        }

        // POST: Landing/NewProfile
        [HttpPost]
        public ActionResult NewProfile(FormCollection collection)
        {
            if (Request.Form["Password"] != Request.Form["PasswordConfirmation"])
            {
                return Redirect("/Landing/NewProfile?ERR=PassMatch");
            }
            else
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

                    Model.Profile profile = handler.getProfile(newProfile);

                    if (profile == null)
                    {
                        bool result = Auth.NewUser(newProfile);

                        if (result == true)
                        {
                            profile = handler.getProfile(newProfile);

                            if (profile != null)
                            {
                                createCookie(profile);
                                return Redirect("/Landing/TaxConsultant?ID=" + profile.ProfileID.ToString());
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
                    else
                    {
                        return Redirect("/Landing/NewProfile?ERR=UserEmailEx");
                    }
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error in new profile method of LandingControles");
                    return View();
                }
            }
        }
        #endregion

        #region Cookie managment
        public void createCookie(Model.Profile profile)
        {
            //log the user in by creating a cookie to manage their state
            cookie = new HttpCookie("TaxAppUserID");
            cookie.Expires = DateTime.Now.AddDays(3);
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
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in tax consultant method of LandingControles");
                return View();
            }
        }
        #endregion

        #region New Email Settings
        // GET: Landing/NewProfile
        public ActionResult EmailSettings()
        {
            return View();
        }

        // POST: Landing/NewProfile
        [HttpPost]
        public ActionResult EmailSettings(FormCollection collection, string ID)
        {
            string profileID = ID;
            try
            {
                Model.EmailSetting newSettings = new Model.EmailSetting();

                newSettings.ProfileID = int.Parse(profileID);
                newSettings.Address = Request.Form["Address"];
                newSettings.Password = Request.Form["Password"];
                newSettings.Host = Request.Form["Host"];
                newSettings.Port = Request.Form["Port"];
                newSettings.DeliveryMethod = Request.Form["DeliveryMethod"];
                _ = Request.Form[5].Split(',')[0];
                newSettings.EnableSsl = Boolean.Parse(Request.Form[5].Split(',')[0]);
                newSettings.UseDefailtCredentials = Boolean.Parse(Request.Form[7].Split(',')[0]);

                bool result = handler.newEmailSettings(newSettings);

                if (result == true)
                {
                    return Redirect("/Home/Index");
                }
                else
                {
                    return RedirectToAction("../Shared/Error");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in email settings method of LandingControles");
                return View();
            }
        }
        #endregion
    }
}
