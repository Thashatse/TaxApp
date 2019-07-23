using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Model;

namespace TaxApp.Controllers
{
    public class VatController : Controller
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

        // GET: VAT
        public ActionResult VatCenter(string view)
        {
            getCookie();

            try
            {
                Profile profileID = new Profile();
                profileID.ProfileID = int.Parse(cookie["ID"]);

                List<TaxAndVatPeriods> vatPeriod = handler.getTaxOrVatPeriodForProfile(profileID, 'V');

                if(vatPeriod == null || vatPeriod.Count == 0)
                {
                    Response.Redirect("../Tax/TaxVatPeriod?Type=V");
                }
                else
                {
                    ViewBag.VatPeriodList = new SelectList(vatPeriod, "PeriodID", "PeriodString");
                    ViewBag.View = view;
                    ViewBag.VatPeriod = vatPeriod[0].PeriodString;
                    return View();
                }

                return Redirect("../Shared/Error");
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Expese Page");
                return Redirect("Home/index");
    }
}
    }
}