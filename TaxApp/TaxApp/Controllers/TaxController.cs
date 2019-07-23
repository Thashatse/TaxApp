using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Model;

namespace TaxApp.Controllers
{
    public class TaxController : Controller
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
        // GET: Tax
        public ActionResult TaxCenter()
        {
            return View();
        }

        #region TaxVatPeriod
        public ActionResult TaxVatPeriod(string type)
        {
            if(type == null || type == "")
            {
                Response.Redirect("../Shared/Error");
            }
            else
            {
                if (type == "V")
                {
                    ViewBag.Type = "VAT";
                }
                else if (type == "T")
                {
                    ViewBag.Type = "Tax";
                }
                else
                {
                    Response.Redirect("../Shared/Error");
                }

                return View();
            }

            return Redirect("../Shared/Error");
        }

        [HttpPost]
        public ActionResult TaxVatPeriod(FormCollection collection, string type)
        {
            getCookie();
            try
            {
                if (type == "V")
                {
                    ViewBag.Type = "VAT";
                }
                else if (type == "T")
                {
                    ViewBag.Type = "Tax";
                }

                if (type == null || type == "")
                {
                    Response.Redirect("../Shared/Error?Err=Error creating TAX or VAT Period");
                }
                else
                {
                    TaxAndVatPeriods period = new TaxAndVatPeriods();

                    period.StartDate = DateTime.Parse(Request.Form["StartDate"]);
                    period.EndDate = DateTime.Parse(Request.Form["EndDate"]);
                    period.Type = type[0];
                    period.ProfileID = int.Parse(cookie["ID"]);

                    bool result = handler.newTaxOrVatPeriod(period);

                    if(result == true)
                    {
                        if (type == "V")
                        {
                            Response.Redirect("../Vat/VatCenter");
                        }
                        else if (type == "T")
                        {
                            Response.Redirect("../Tax/TaxCenter");
                        }
                        else
                        {
                            Response.Redirect("../Shared/Error?Err=Error creating TAX or VAT Period");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Shared/Error?Err=Error creating TAX or VAT Period");
                    }
                }

                return Redirect("../Shared/Error");
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error creating TAX or VAT Period");
                return Redirect("../Shared/Error?Err=Error creating TAX or VAT Period");
            }
        }
        #endregion
    }
}