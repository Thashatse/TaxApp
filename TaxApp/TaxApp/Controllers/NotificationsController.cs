using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;

namespace TaxApp.Controllers
{
    public class NotificationsController : Controller
    {
        NotificationsFunctions notiFunctions = new NotificationsFunctions();
        HttpCookie cookie;
        IDBHandler handler = new DBHandler();
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
                            Response.Redirect(Url.Action("Welcome", "Landing"));
                        }

                        ViewBag.ProfileName = checkProfile.FirstName + " " + checkProfile.LastName;
                        ViewBag.NotificationList = notiFunctions.getNotifications(int.Parse(cookie["ID"]));
                    }
                    else
                    {
                        Response.Redirect(Url.Action("Welcome", "Landing"));
                    }
                }
                else
                {
                    Response.Redirect(Url.Action("Welcome", "Landing"));
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in welcome method of LandingControles");
                Response.Redirect(Url.Action("Error", "Shared") + "?Err=Identity couldn't be verified");
            }
        }
        public ActionResult dismissNotifications(string id = "0", string redirect = "")
        {
            getCookie();

            if (id == "0")
                return RedirectToAction("Error", "Shared");

                Response.Redirect(notiFunctions.dismissNotification(int.Parse(id), redirect));

            return RedirectToAction("Error", "Shared");
        }
    }
}