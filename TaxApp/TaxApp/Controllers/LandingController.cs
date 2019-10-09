using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using System.Threading;
using Model;

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
            Thread zero = new Thread(function.repeatExpense);
            zero.Start();
            Profile username = new Profile();

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
                            //Change befor Publishing
                            Response.Redirect("/Home/Index");
                            //Response.Redirect("http://sict-iis.nmmu.ac.za/taxapp/Home/Index");
                        }
                    }
                    else if(cookie["User"] != null || cookie["User"] != "")
                    {
                        username.Username = cookie["User"];
                    }
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in welcome method of LandingControles");
                return RedirectToAction("Error", "Shared");
            }

            if (Err == "UserPassNA")
                ViewBag.Message = "Username and password combination is incorrect error";

            return View(username);
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
                    return RedirectToAction("Welcome","Landing" , new { err = "UserNull" });
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
                        return RedirectToAction("Welcome", "Landing", new { err = "UserPassNA" });
                }
                else if (result[0] == "AccountN/A")
                    {
                        return RedirectToAction("Welcome", "Landing", new { err = "UserPassNA" });
                }
                else if (result[0] != null 
                    | result[1] != null)
                    {
                        if(Request.Form["customCheckLogin"] != null)
                            createCookie(handler.getProfile(newProfile), true);
                        else
                            createCookie(handler.getProfile(newProfile), false);
                        return RedirectToAction("index", "Home");
                }
                }
                return RedirectToAction("Error", "Shared", new { Err = "An error occurred while processing your request, please try again later."});
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in welcome method of LandingControles");
                return RedirectToAction("Error", "Shared", new { Err = "An error occurred while processing your request, please try again later." });
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

                return RedirectToAction("Welcome", "Landing");
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in welcome method of LandingControles");
                return RedirectToAction("Error", "Shared");
            }
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
                return Redirect(Url.Action("NewProfile", "Landing") +"?ERR=PassMatch");
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
                    newProfile.VATRate = Convert.ToDecimal(Request.Form["VATRate"]);
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
                                createCookie(profile, false);
                                return Redirect(Url.Action("TaxConsultant", "Landing") + "?ID=" + profile.ProfileID.ToString());
                            }
                            else
                            {
                                return RedirectToAction("Error", "Shared");
                            }

                        }
                        else
                        {
                            return RedirectToAction("Error", "Shared");
                        }
                    }
                    else
                    {
                        return Redirect(Url.Action("NewProfile", "Landing") + "?ERR=UserEmailEx");
                    }
                }
                catch (Exception e)
                {
                    function.logAnError(e.ToString() +
                        "Error in new profile method of LandingControles");
                    return RedirectToAction("Error", "Shared");
                }
            }
        }
        #endregion

        #region Cookie managment
        public void createCookie(Model.Profile profile, bool remeber)
        {
            //log the user in by creating a cookie to manage their state
            cookie = new HttpCookie("TaxAppUserID");
            if(remeber == true)
            {
                cookie.Expires = DateTime.Now.AddDays(14);
                // Set the user id in it.
                cookie["ID"] = profile.ProfileID.ToString();
                cookie["User"] = profile.Username.ToString();
            }
            else
            {
                cookie.Expires = DateTime.Now.AddDays(1);
                // Set the user id in it.
                cookie["ID"] = profile.ProfileID.ToString();
            }
            // Add it to the current web response.
            Response.Cookies.Add(cookie);
        }
        #endregion

        #region New Consultant
        public ActionResult TaxConsultant(string ID, string Return)
        {
            ViewBag.Return = Return;
            ViewBag.ProfileID = ID;
            return View();
        }

        [HttpPost]
        public ActionResult TaxConsultant(FormCollection collection, string ID, string Return)
        {
            string profileID = ID;
            ViewBag.Return = Return;
            ViewBag.ProfileID = ID;

            try
            {
                Model.TaxConsultant newConsultant = new Model.TaxConsultant();

                newConsultant.Name = Request.Form["Name"];
                newConsultant.EmailAddress = Request.Form["EmailAddress"];
                newConsultant.ProfileID = int.Parse(profileID);

                bool result = handler.newConsultant(newConsultant);

                if (result == true)
                {
                    if(Return == "Consultant")
                    {
                        return RedirectToAction("Consultant", "Tax");
                    }
                    else
                    {
                        return Redirect(Url.Action("EmailSettings", "Landing") +"?ID=" + profileID.ToString());
                    }
                }
                else
                {
                    return RedirectToAction("Error", "Shared");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in tax consultant method of LandingControles");
                return RedirectToAction("Error", "Shared");
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
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Error", "Shared");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in email settings method of LandingControles");
                return RedirectToAction("Error", "Shared");
            }
        }
        #endregion
    }
}
