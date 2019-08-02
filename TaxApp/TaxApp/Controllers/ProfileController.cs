using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;

namespace TaxApp.Controllers
{
    public class ProfileController : Controller
    {
        IDBHandler handler = new DBHandler();
        HttpCookie cookie;
        Functions function = new Functions();
        Authentication Auth = new Authentication();
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

        // GET: Profile/Details/5
        public ActionResult profile(string Err)
        {
            try
            {
                getCookie();

                Profile getProfile = new Profile();
                getProfile.ProfileID = int.Parse(cookie["ID"].ToString());
                getProfile.EmailAddress = "";
                getProfile.Username = "";
                getProfile = handler.getProfile(getProfile);

                string err = Err;
                if (err == "UserEmailEx")
                    ViewBag.Message = "Username or email already registered";

                return View(getProfile);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Profile in Profile Controler");
                return Redirect("../Shared/Error?Err=An error occurred while loading profile.");
            }
        }

        [HttpPost]
        public ActionResult profile(FormCollection collection)
        {
            try
            {
                getCookie();

                Profile getProfile = new Profile();

                getProfile.ProfileID = int.Parse(cookie["ID"].ToString());
                getProfile.EmailAddress = "";
                getProfile.Username = "";
                getProfile = handler.getProfile(getProfile);

                Profile authProfile = new Model.Profile();

                authProfile.EmailAddress = getProfile.EmailAddress;
                authProfile.Username = getProfile.Username;
                authProfile.Password = Request.Form["Password"];

                if (authProfile.Password == "")
                {
                    ViewBag.Error = "Enter your Password";
                    return View(getProfile);
                }
                else if (authProfile.EmailAddress != ""
                    && authProfile.Username != ""
                    && authProfile.Password != "")
                {
                    string[] authResult = Auth.AuthenticateEmail(authProfile, authProfile.Password);

                    if (authResult[0] == "PassN/A")
                    {
                        ViewBag.Error = "Incorect Password";
                        return View(getProfile);
                    }
                    else if (authResult[0] != null
                        | authResult[1] != null)
                    {
                        Profile updateProfile = new Profile();

                        updateProfile.ProfileID = 0;
                        updateProfile.FirstName = Request.Form["FirstName"];
                        updateProfile.LastName = Request.Form["LastName"];
                        updateProfile.CompanyName = Request.Form["CompanyName"];
                        updateProfile.EmailAddress = Request.Form["EmailAddress"];
                        updateProfile.ContactNumber = Request.Form["ContactNumber"];
                        updateProfile.PhysicalAddress = Request.Form["PhysicalAddress"];
                        updateProfile.VATNumber = Request.Form["VATNumber"];
                        updateProfile.DefaultHourlyRate = Convert.ToDecimal(Request.Form["DefaultHourlyRate"]);
                        updateProfile.Username = Request.Form["Username"];

                        if (Request.Form["NewPassword"] != "" && Request.Form["NewPasswordConfirmation"] != "")
                        {
                            if (Request.Form["NewPassword"] != Request.Form["NewPasswordConfirmation"])
                            {
                                ViewBag.Error = "New Passwords do not match";
                                return View(getProfile);
                            }
                            else
                            {
                               updateProfile.Password = Auth.generatePassHash(Request.Form["NewPassword"]);
                            }
                        }
                        else
                        {
                            updateProfile.Password = getProfile.Password;
                        }

                        Profile profile = handler.getProfile(updateProfile);

                        updateProfile.ProfileID = int.Parse(cookie["ID"].ToString());

                        bool result = false;

                        if (profile == null)
                            result = true;
                        else if (profile.ProfileID == updateProfile.ProfileID)
                            result = true;

                        if (result == true)
                            result = handler.editprofile(updateProfile);
                        else
                            return Redirect("/Profile/Profile?ERR=UserEmailEx");


                        if (result == true)
                            return Redirect("/Profile/Profile");
                        else
                            return RedirectToAction("../Shared/Error?Err=An error occurred while updating profile.");
                    }
                }

                ViewBag.Error = "An error occurred while Updating profile.";
                return View(getProfile);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Updating Profile in Profile Controler");
                return Redirect("../Shared/Error?Err=An error occurred while Updating profile.");
            }
        }
    }
}
