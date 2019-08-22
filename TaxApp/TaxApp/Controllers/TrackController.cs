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

        public string getCookie(string ID, string Type)
        {
            try
            {
                //check if the user is loged in
                cookie = Request.Cookies["TaxAppGuestUserID"];

                if (cookie == null)
                    return ("../Track/verifyIdentity?ID="+ID+"&Type="+Type);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in welcome method of LandingControles");
                Redirect("/Shared/Error");
            }

                return "";
        }

        #region verify Identity
        public ActionResult verifyIdentity(string ID, string Type, string Err)
        {
            ShareVerifyIdentityModel data = new ShareVerifyIdentityModel();

            ViewBag.Message = Err;
            
            if (Type == "Job" || Type == "TAX" || Type == "VAT")
            {
                try
                {
                int OTP = function.generateOTP();
                Tuple<bool, string, string, int> check = handler.NewExternalUserOTP(int.Parse(ID), OTP, Type);

                if (!check.Item1)
                    Response.Redirect("../Shared/Error?Err=");

                data.userName = check.Item2;

                bool result = function.sendEmail(check.Item3,
                        check.Item2,
                        "Verify your identity - TaxApp",
                        "Hello, \n\n Your OTP is: "+ OTP + "\n\n Regards, \n The TaxApp Team.",
                        "TaxApp",
                        0);
                    ViewBag.UserID = check.Item4;
                }
                catch (Exception err)
                {
                    function.logAnError("Error creating OTP in Track controller Error: " + err);
                    Response.Redirect("../Shared/Error?Err=Error generating OTP. Either sharing has been turned of or the link provided is broken.");
                }
            }
            else
                Response.Redirect("../Shared/Error?Err=Broken link. The link provided cannot be found please try again.");

            return View(data);
        }
        [HttpPost]
        public ActionResult verifyIdentity(FormCollection collection, string ID, string Type)
        {
            ShareVerifyIdentityModel data = new ShareVerifyIdentityModel();
            data.userName = "Test First Test Last";

            int OTP = 0;
            int.TryParse(Request.Form["OTP"], out OTP);

            if (OTP == handler.GetExternalUserOTP(int.Parse(ID), Type))
            {
                cookie = new HttpCookie("TaxAppGuestUserID");
                cookie.Expires = DateTime.Now.AddHours(3);
                // Set the user id in it.
                cookie["ID"] = Request.Form["userId"];
                // Add it to the current web response.
                Response.Cookies.Add(cookie);

                if (Type == "Job")
                    return RedirectToAction("Job", "Track", new
                    {
                        JobID = ID
                    });
                if (Type == "TAX")
                    return RedirectToAction("TAX", "Track", new
                    {
                        TaxID = ID
                    });
                if (Type == "VAT")
                    return RedirectToAction("VAT", "Track", new
                    {
                        VATID = ID
                    });
            }
            else
            {
                string Err = "Incorrect OTP. We've sent you a new one, please try again";
                return RedirectToAction("verifyIdentity", "Track", new
                {
                    Type,
                    Err,
                    ID
                });
            }

            ViewBag.Message = "An error occurred while processing your request.";
            return View(data);
        }
        #endregion

        public ActionResult Job(string ID)
        {
            string link = getCookie(ID, "Job");
            if (link != "")
                Response.Redirect(link);

            return View();
        }

        public ActionResult TAX(string ID)
        {
            string link = getCookie(ID, "Job");
            if (link != "")
                Response.Redirect(link);

            return View();
        }

        public ActionResult VAT(string ID)
        {
            string link = getCookie(ID, "Job");
            if (link != "")
               Response.Redirect(link);

            return View();
        }
    }
}