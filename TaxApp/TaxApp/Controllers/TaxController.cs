using System;
using System.Collections.Generic;
using System.Globalization;
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

        #region Tax Center
        public ActionResult TaxCenter(string view, string period)
        {
            getCookie();

            try
            {
                TaxCenter viewModel = new TaxCenter();

                TaxDashboard dashboard = null;
                List<TAXorVATRecivedList> TAXRecived = null;

                var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                nfi.NumberGroupSeparator = " ";

                Profile profileID = new Profile();
                profileID.ProfileID = int.Parse(cookie["ID"]);

                List<TaxAndVatPeriods> taxPeriod = handler.getTaxOrVatPeriodForProfile(profileID, 'T');

                if (taxPeriod == null || taxPeriod.Count == 0)
                {
                    return RedirectToAction("TaxVatPeriod", "Tax", new
                    {
                        Type = "T"
                    });
                }
                else
                {
                    ViewBag.TaxPeriodList = new SelectList(taxPeriod, "PeriodID", "PeriodString");
                    ViewBag.View = view;

                    ViewBag.TaxPeriod = null;

                    if (period == null || period == "")
                    {
                        return RedirectToAction("TaxCenter", "Tax", new { period = taxPeriod[0].PeriodID, view });
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
                            ViewBag.PeriodID = item.PeriodID;
                        }
                    }

                    if (ViewBag.TaxPeriod == null)
                    {
                        return RedirectToAction("Error", "Shared", new { err = "An error occurred loading data for tax period" });
                    }

                    decimal totalAmount = 0;
                    decimal totalAmountTAX = 0;
                    foreach (TAXorVATRecivedList taxItem in TAXRecived)
                    {
                        totalAmount += taxItem.Total;
                        totalAmountTAX += taxItem.VATorTAX;
                    }
                    ViewBag.totalAmount = totalAmount.ToString("#,0.00", nfi);
                    ViewBag.totalAmountTax = totalAmountTAX.ToString("#,0.00", nfi);

                    viewModel.TAXDashboard = dashboard;
                    viewModel.TAXRecivedList = TAXRecived;

                    return View(viewModel);
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Tax Center");
                return RedirectToAction("Error", "Shared", new { err = "An error occurred loading the Tax center" });
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

                return RedirectToAction("TaxCenter", "Tax", new { period = Request.Form["TaxPeriodList"].ToString() });
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Tax Center");
                return RedirectToAction("Error", "Shared", new { err = "An error occurred loading the Tax center" });
            }
        }
        #endregion

        #region TaxVatPeriod
        public ActionResult TaxVatPeriod(string type, string Period = "0")
        {
            getCookie();
            if(type == null || type == "")
            {
                return RedirectToAction("Error", "Shared");
            }
            else
            {
                if (type == "V")
                {
                        ViewBag.Type = "New VAT"; 
                }
                else if (type == "T")
                {
                        ViewBag.Type = "New Tax";
                }
                else
                {
                    return RedirectToAction("Error", "Shared");
                }

                return View();
            }
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
                    return RedirectToAction("Error", "Shared", new { Err="Error creating TAX or VAT Period"});
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
                            return RedirectToAction("VatCenter", "Vat");
                        }
                        else if (type == "T")
                        {
                            return RedirectToAction("TaxBrakets", "Tax", new{ID=handler.SP_GetLatestTaxAndVatPeriodID().PeriodID, period=
                                period.StartDate.ToString("dd MMM yyyy")+" to "+ period.EndDate.ToString("dd MMM yyyy")});
                        }
                        else
                        {
                            return RedirectToAction("Error", "Shared", new { Err = "Error creating TAX or VAT Period" });
                        }
                    }
                    else
                    {
                        return RedirectToAction("Error", "Shared", new { Err = "Error creating TAX or VAT Period" });
                    }
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error creating TAX or VAT Period");
                return RedirectToAction("Error", "Shared", new { Err = "Error creating TAX or VAT Period" });
            }
        }
        #endregion

        #region EditTaxVatPeriod
        public ActionResult EditTaxVatPeriod(string type, string Period = "0", string ReturnToConsultant = "false")
        {
            getCookie();
            if(type == null || type == "")
            {
                return RedirectToAction("Error", "Shared");
            }
            else
            {
                if (type == "V")
                {
                    if(Period != "0")
                    {
                        ViewBag.Type = "Edit VAT";
                        try
                        {
                            Profile profileID = new Profile();
                            profileID.ProfileID = int.Parse(cookie["ID"]);
                            List<TaxAndVatPeriods> taxPeriod = handler.getTaxOrVatPeriodForProfile(profileID, 'V');
                            foreach (TaxAndVatPeriods item in taxPeriod)
                            {
                                if (item.PeriodID.ToString() == Period)
                                {
                                    item.StartDateString = item.StartDate.ToString("yyyy-MM-dd");
                                    item.EndDateString = item.EndDate.ToString("yyyy-MM-dd");
                                    ViewBag.PeriodID = item.PeriodID;
                                    return View(item);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            function.logAnError(e.ToString() +
                                "Error loding Vat period for edit");
                            return RedirectToAction("Error", "Shared", new { Err = "An error occurred loading the Vat period" });
                        }
                    }
                }
                else if (type == "T")
                {
                    if (Period != "0")
                    {
                                ViewBag.Type = "Edit Tax";
                        try
                        {
                            Profile profileID = new Profile();
                            profileID.ProfileID = int.Parse(cookie["ID"]);
                            List<TaxAndVatPeriods> taxPeriod = handler.getTaxOrVatPeriodForProfile(profileID, 'T');
                            foreach (TaxAndVatPeriods item in taxPeriod)
                            {
                                if (item.PeriodID.ToString() == Period)
                                {
                                    item.StartDateString = item.StartDate.ToString("yyyy-MM-dd");
                                    item.EndDateString = item.EndDate.ToString("yyyy-MM-dd");
                                    ViewBag.PeriodID = item.PeriodID;
                                    return View(item);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            function.logAnError(e.ToString() +
                                "Error loding Tax period for edit");
                            return RedirectToAction("Error", "Shared", new { Err = "An error occurred loading the Tax period" });
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Error", "Shared");
                }

                return View();
            }

            return RedirectToAction("Error", "Shared", new { Err = "Error creating TAX or VAT Period" });
        }

        [HttpPost]
        public ActionResult EditTaxVatPeriod(FormCollection collection, string type, string Period = "0", string ReturnToConsultant = "false")
        {
            getCookie();
            try
            {
                TaxAndVatPeriods period = new TaxAndVatPeriods();
                if (type == null || type == "")
                {
                    return RedirectToAction("Error", "Shared", new { Err = "Error updating TAX or VAT Period" });
                }
                else
                {
                if (type == "V")
                {
                    if (Period != "0")
                    {
                        ViewBag.Type = "Edit VAT";
                    }
                }
                else if (type == "T")
                {
                    if (Period != "0")
                    {
                        ViewBag.Type = "Edit Tax";
                    }
                }

                    period.StartDate = DateTime.Parse(Request.Form["StartDate"]);
                    period.EndDate = DateTime.Parse(Request.Form["EndDate"]);
                    period.PeriodID = int.Parse(Period);

                    bool result = handler.editTaxOrVatPeriod(period);

                    if(result == true)
                    {
                        if (type == "V")
                        {
                            return RedirectToAction("VatCenter", "Vat", new { period = period.PeriodID });
                        }
                        else if (type == "T")
                        {
                            return RedirectToAction("TaxBrakets", "Tax", new
                            {
                                ID = period.PeriodID,
                                period = period.StartDate.ToString("dd MMM yyyy") + " to " + period.EndDate.ToString("dd MMM yyyy")
                                + "&ReturnToConsultant=" + ReturnToConsultant
                            });
                        }
                        else
                        {
                            return RedirectToAction("Error", "Shared", new { Err = "Error updating TAX or VAT Period" });
                        }
                    }
                    else
                    {
                        return RedirectToAction("Error", "Shared", new { Err = "Error updating TAX or VAT Period" });
                    }
                }
            }
            catch (Exception e)
            {
                function.logAnError("Error updating TAX or VAT Period" + e.ToString());
                return RedirectToAction("Error", "Shared", new { Err = "Error updating TAX or VAT Period" });
            }
        }
        #endregion

        #region Tax Consultant
        public ActionResult Consultant()
        {
            try
            {
                getCookie();

                ConsultantViewModel viewModel = new ConsultantViewModel();

                viewModel.Consultant = new TaxConsultant();
                viewModel.Consultant.ProfileID = int.Parse(cookie["ID"]);
                viewModel.Consultant = handler.getConsumtant(viewModel.Consultant);

                if (viewModel.Consultant == null)
                    return RedirectToAction("TaxConsultant", "Landing", new { ID = cookie["ID"], Return = "Consultant" });

                Profile profileID = new Profile();
                profileID.ProfileID = int.Parse(cookie["ID"]);

                viewModel.taxPeriod = handler.getTaxOrVatPeriodForProfile(profileID, 'T');
                viewModel.vatPeriod = handler.getTaxOrVatPeriodForProfile(profileID, 'V');

                return View(viewModel);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Tax Consultant");
                return RedirectToAction("Error", "Shared", new { Err = "An error occurred loading Tax Consultant" });
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
                    return RedirectToAction("Error", "Shared", new { Err = "An error occurred loading Tax Consultant for edit" });

                return View(consultant);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Tax Consultant");
                return RedirectToAction("Error", "Shared", new { Err = "An error occurred loading Tax Consultant for edit" });
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
                    return RedirectToAction("Consultant", "Tax");
                }

                consultant = new Model.TaxConsultant();

                consultant.Name = Request.Form["Name"];
                consultant.EmailAddress = Request.Form["EmailAddress"];
                consultant.ProfileID = int.Parse(cookie["ID"]);

                bool result = handler.EditTaxConsultant(consultant);

                if (result == true)
                        return RedirectToAction("Consultant", "Tax");
                    else
                        return RedirectToAction("Error", "Shared", new { Err = "An error occurred loading Tax Consultant for edit" });
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
                return RedirectToAction("Error", "Shared", new { Err = "An error occurred loading Tax Consultant for edit" });
            }
        }
        #endregion

        #region Tax Brakets
        public ActionResult TaxBrakets(string ID, string period, string ReturnToConsultant = "false")
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
                return RedirectToAction("Error", "Shared", new { Err = "An error occurred loading the new tax braket page" });
            }

            return View(view);
        }

        [HttpPost]
        public ActionResult TaxBrakets(FormCollection collection, string ID, string period, string ReturnToConsultant = "false")
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
                else if (bool.Parse(ReturnToConsultant))
                {
                    return RedirectToAction("Consultant", "Tax");
                }
                else if (result == true && newRate.Type == 'C')
                {
                    return RedirectToAction("TaxCenter", "Tax", new { period = ID });
                }
                else
                {
                    function.logAnError("Error creating new Tax Braket");
                    return RedirectToAction("Error", "Shared", new { Err = "An error occurred creating new tax braket" });
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding new Tax Brakets");
                return RedirectToAction("Error", "Shared", new { Err = "An error occurred loading the new tax braket page" });
            }

            return View(view);
        }

        public ActionResult DeleteTaxBraket(FormCollection collection, string RateID, string period, string ID)
        {
            try
            {
                TaxPeriodRates rID = new TaxPeriodRates();
                rID.RateID = int.Parse(RateID);
                handler.deletePeriodTaxBraket(rID);
            }
            catch (Exception e)
            {
                function.logAnError("Error deleting Tax Braket. Braket ID: "+RateID + " Error: "+e.ToString());
            }

            return Redirect(Url.Action("TaxBrakets", "Tax")+"?ID="+ID+"&period="+period);
        }
        #endregion

        #region Update Period Share
        public ActionResult Share(string ID, string type, string ReturnToConsultant = "false")
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
                return RedirectToAction("Error", "Shared");
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
                        "Use the link bellow to gain access: \n http://sict-iis.nmmu.ac.za/taxapp/Track/verifyIdentity?ID=" + tuple.Item1.PeriodID + "&Type=TAX";
                }
                else if (tuple.Item1.Type == 'V')
                {
                    subject = ViewBag.ProfileName + " has shared information from their VAT period with you.";
                    body = "Hello " + tuple.Item2.Name + " \n \n " +
                        ViewBag.ProfileName + " has shared information from their VAT period dated " + tuple.Item1.PeriodString
                        + " with you using Tax App. \n\n" +
                        "Use the link bellow to gain access: \n http://sict-iis.nmmu.ac.za/taxapp/Track/verifyIdentity?ID=" + tuple.Item1.PeriodID + "&Type=VAT";
                }

                bool result = function.sendEmail(tuple.Item2.EmailAddress,
                    tuple.Item2.Name,
                    subject,
                    body,
                    ViewBag.ProfileName,
                    int.Parse(cookie["ID"]));
            }
            }

           if (bool.Parse(ReturnToConsultant))
                {
                return RedirectToAction("Consultant", "Tax", new
                {
                    period,
                    view
                });
            }
            else if (type == "T")
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