using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Model;

namespace TaxApp.Controllers
{
    public class TrackController : Controller
    {
        IDBHandler handler = new DBHandler();
        HttpCookie cookie;
        Functions function = new Functions();

        public void getCookie()
        {
            try
            {
                //check if the user is loged in
                cookie = Request.Cookies["TaxAppGeustUserID"];

                if (cookie != null)
                {
                    //show the nav tabs menue only for customers
                    if (cookie["ID"] != null || cookie["ID"] != "")
                    {
                        Client checkClient = new Client();

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
                    "Error in get cookie Geust of Track (Share) contorler");
                Redirect("/Shared/Error");
            }
        }

        #region verify Identity
        public ActionResult verifyIdentity(string ID, string Type)
        {
            ShareVerifyIdentityModel data = new ShareVerifyIdentityModel();
            data.userName = "Test First Test Last";

            if (Type == "Job")
            {

            }
            else if (Type == "TAX")
            {

            }
            else if (Type == "VAT")
            {

            }

            return View(data);
        }
        [HttpPost]
        public ActionResult verifyIdentity(FormCollection collection, string ID, string Type)
        {
            ShareVerifyIdentityModel data = new ShareVerifyIdentityModel();
            data.userName = "Test First Test Last";

            if (Type == "Job")
            {

            }
            else if (Type == "TAX")
            {

            }
            else if (Type == "VAT")
            {

            }

            return View(data);
        }
        #endregion
        public ActionResult Job(string ID)
        {
            getCookie();
            return View();
        }

        public ActionResult TAX(string ID)
        {
            getCookie();
            return View();
        }

        public ActionResult VAT(string ID)
        {
            getCookie();
            return View();
        }
    }
}