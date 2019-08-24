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
        NotificationsFunctions notiFunctions = new NotificationsFunctions();
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
                    "Error in welcome method of LandingControles");
                Redirect("/Shared/Error");
            }
        }

        #region Tax Center
        public ActionResult TaxCenter(string view, string period)
        {
            getCookie();

            try
            {
                TaxCenter viewModel = new TaxCenter();

                TaxDashboard dashboard = null;
                List<TAXorVATRecivedList> TAXRecived = null;

                Profile profileID = new Profile();
                profileID.ProfileID = int.Parse(cookie["ID"]);

                List<TaxAndVatPeriods> taxPeriod = handler.getTaxOrVatPeriodForProfile(profileID, 'T');

                if (taxPeriod == null || taxPeriod.Count == 0)
                {
                    Response.Redirect("../Tax/TaxVatPeriod?Type=T");
                }
                else
                {
                    ViewBag.TaxPeriodList = new SelectList(taxPeriod, "PeriodID", "PeriodString");
                    ViewBag.View = view;

                    ViewBag.TaxPeriod = null;

                    if (period == null || period == "")
                    {
                        Response.Redirect("../Tax/TaxCenter?period=" + taxPeriod[0].PeriodID + "&view=" + view);
                    }

                    foreach (TaxAndVatPeriods item in taxPeriod)
                    {
                        if (item.PeriodID.ToString() == period)
                        {
                            ViewBag.TaxPeriod = item.PeriodString;

                            dashboard = handler.getTaxCenterDashboard(profileID, item);

                            TaxPeriodRates rate = new TaxPeriodRates();
                            rate.Rate = dashboard.TAXRate;
                            TAXRecived = handler.getTAXRecivedList(profileID, item, rate);

                            viewModel.period = item;
                        }
                    }

                    if (ViewBag.TaxPeriod == null)
                    {
                        Response.Redirect("../Shared/Error?Err=An error occurred loading data for tax period");
                    }

                    viewModel.TAXDashboard = dashboard;
                    viewModel.TAXRecivedList = TAXRecived;

                    return View(viewModel);
                }

                return Redirect("../Shared/Error?Err=An error occurred loading the Tax center");
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Tax Center");
                return Redirect("../Shared/Error?Err=An error occurred loading the Tax center");
            }
        }
        [HttpPost]
        public ActionResult TaxCenter(FormCollection collection, string view, string period)
        {
            getCookie();

            try
            {
                Profile profileID = new Profile();
                profileID.ProfileID = int.Parse(cookie["ID"]);

                List<TaxAndVatPeriods> taxPeriod = handler.getTaxOrVatPeriodForProfile(profileID, 'V');

                Response.Redirect("../Tax/TaxCenter?period=" + Request.Form["TaxPeriodList"].ToString());

                return Redirect("../Shared/Error?Err=An error occurred updating the Tax period");
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Tax Center");
                return Redirect("../Shared/Error?Err=An error occurred loading the Tax center");
            }
        }
        #endregion

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
                            Response.Redirect("../Tax/TaxBrakets?ID="+handler.SP_GetLatestTaxAndVatPeriodID().PeriodID+ "&period="+
                                period.StartDate.ToString("dd MMM yyyy")+" - "+ period.EndDate.ToString("dd MMM yyyy"));
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

        #region Tax Consultant
        public ActionResult Consultant()
        {
            try
            {
                getCookie();

                TaxConsultant consultant = new TaxConsultant();
                consultant.ProfileID = int.Parse(cookie["ID"]);
                consultant = handler.getConsumtant(consultant);

                if(consultant == null)
                {
                    Response.Redirect("../Landing/TaxConsultant?ID=" + cookie["ID"] + "&Return=Consultant");
                }

                return View(consultant);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Tax Consultant");
                return Redirect("../Shared/Error?Err=An error occurred loading Tax Consultant");
            }
        }
        public ActionResult EditConsultant()
        {
            try
            {
                getCookie();

                TaxConsultant consultant = new TaxConsultant();
                consultant.ProfileID = int.Parse(cookie["ID"]);
                consultant = handler.getConsumtant(consultant);

                if(consultant == null)
                {
                    Response.Redirect("../Shared/Error?Err=An error occurred loading Tax Consultant for edit");
                }

                return View(consultant);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Tax Consultant");
                return Redirect("../Shared/Error?Err=An error occurred loading Tax Consultant for edit");
            }
        }
        [HttpPost]
        public ActionResult EditConsultant(FormCollection collection)
        {
            try
            {
                getCookie();

                if(Request.Form["Name"] != null && Request.Form["Name"] != ""
                    && Request.Form["EmailAddress"] != null && Request.Form["EmailAddress"] != "")
                {
                TaxConsultant consultant = new TaxConsultant();
                consultant.ProfileID = int.Parse(cookie["ID"]);
                consultant = handler.getConsumtant(consultant);

                if(consultant == null)
                {
                    function.logAnError("Error loding Tax Consultant for edit");
                    Response.Redirect("../Tax/Consultant");
                }

                consultant = new Model.TaxConsultant();

                consultant.Name = Request.Form["Name"];
                consultant.EmailAddress = Request.Form["EmailAddress"];
                consultant.ProfileID = int.Parse(cookie["ID"]);

                bool result = handler.EditTaxConsultant(consultant);

                if (result == true)
                {
                    return Redirect("../Tax/Consultant");
                }
                else
                {
                    return RedirectToAction("../Shared/Error?Err=An error occurred loading Tax Consultant for edit");
                }
                }
                else
                {
                    ViewBag.Err = "Please enter a name and email address";
                    return View();
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Tax Consultant for edit");
                return Redirect("../Shared/Error?Err=An error occurred loading Tax Consultant for edit");
            }
        }
        #endregion

        #region Tax Brakets
        public ActionResult TaxBrakets(string ID, string period)
        {
            TaxBraketsView view = new TaxBraketsView();
            view.getRate = null;
            view.setRate = null;

            try
            {
                ViewBag.Period = period;
                ViewBag.ID = ID;

                TaxAndVatPeriods PeriodID = new TaxAndVatPeriods();
                PeriodID.PeriodID = int.Parse(ID);
                List<TaxPeriodRates> brakets = handler.getTaxPeriodBrakets(PeriodID);

                view.getRate = brakets;
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding new Tax Brakets");
                return Redirect("../Shared/Error?Err=An error occurred loading the new tax braket page");
            }

            return View(view);
        }

        [HttpPost]
        public ActionResult TaxBrakets(FormCollection collection, string ID, string period)
        {
            TaxBraketsView view = new TaxBraketsView();
            view.getRate = null;
            view.setRate = null;

            try
            {
                TaxAndVatPeriods PeriodID = new TaxAndVatPeriods();
                PeriodID.PeriodID = int.Parse(ID);
                List<TaxPeriodRates> brakets = handler.getTaxPeriodBrakets(PeriodID);

                TaxPeriodRates newRate = new TaxPeriodRates();

                newRate.Rate = decimal.Parse(Request.Form["setRate.Rate"].ToString());
                newRate.Threashold = decimal.Parse(Request.Form["setRate.Threashold"].ToString());
                if (brakets.Count<1)
                {
                newRate.Type =  Request.Form["Type"].ToString()[0];
                }
                else
                {
                newRate.Type =  'I';
                }
                newRate.PeriodID = int.Parse(ID);

                bool result = handler.newPeriodTaxBraket(newRate);

                if(result == true && newRate.Type == 'I')
                {
                    ViewBag.Period = period;
                    ViewBag.ID = ID;

                    brakets = handler.getTaxPeriodBrakets(PeriodID);

                    view.getRate = brakets;
                }
                else if (result == true && newRate.Type == 'C')
                {
                    Response.Redirect("../Tax/TaxCenter?period=" + ID);
                }
                else
                {
                    function.logAnError("Error creating new Tax Braket");
                    return Redirect("../Shared/Error?Err=An error occurred creating new tax braket");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding new Tax Brakets");
                return Redirect("../Shared/Error?Err=An error occurred loading the new tax braket page");
            }

            return View(view);
        }
        #endregion

        #region Update Period Share
        public ActionResult Share(string ID, string type)
        {
            getCookie();

            Tuple<TaxAndVatPeriods, TaxConsultant> tuple = null;

            try
            {
                TaxAndVatPeriods periodID = new TaxAndVatPeriods();
                periodID.PeriodID = int.Parse(ID);


                tuple = handler.UpdateShareTaxorVatPeriod(periodID);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error Updating Tax or Vat Period share");
            }

            string period = ID;
            string view = "DS";

            if(tuple != null)
            {
if(tuple.Item1 != null && tuple.Item2 != null)
            {
                string subject = "";
                string body = "";
                if (tuple.Item1.Type == 'T')
                {
                    subject = ViewBag.ProfileName + " has shared information from their Tax period with you.";
                    body = "Hello " + tuple.Item2.Name + " \n \n " +
                        ViewBag.ProfileName + " has shared information from their Tax period dated " + tuple.Item1.PeriodString
                        + " with you using Tax App. \n\n" +
                        "Use the link bellow to gain access: \n http://localhost:54533/Track/verifyIdentity?ID=" + tuple.Item1.PeriodID + "&Type=TAX";
                }
                else if (tuple.Item1.Type == 'V')
                {
                    subject = ViewBag.ProfileName + " has shared information from their VAT period with you.";
                    body = "Hello " + tuple.Item2.Name + " \n \n " +
                        ViewBag.ProfileName + " has shared information from their VAT period dated " + tuple.Item1.PeriodString
                        + " with you using Tax App. \n\n" +
                        "Use the link bellow to gain access: \n http://localhost:54533/Track/verifyIdentity?ID=" + tuple.Item1.PeriodID + "&Type=VAT";
                }

                bool result = function.sendEmail(tuple.Item2.EmailAddress,
                    tuple.Item2.Name,
                    subject,
                    body,
                    ViewBag.ProfileName,
                    int.Parse(cookie["ID"]));
            }
            }
            

            if (type == "T")
            {
                return RedirectToAction("TaxCenter", "Tax", new
                {
                    period,
                    view
                });
            }
            else
            {
                return RedirectToAction("VATCenter", "VAT", new
                {
                    period,
                    view
                });
            }
        }
        #endregion
    }
}