using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;

namespace TaxApp.Controllers
{
    public class ReportController : Controller
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

        // GET: Report
        public ActionResult Reports()
        {
            getCookie();

            Profile profile = new Model.Profile();
            profile.ProfileID = int.Parse(cookie["ID"].ToString());

            DashboardIncomeExpense IncomeExpense = handler.getDashboardIncomeExpense(profile);

            List<TaxAndVatPeriods> taxAndvatPeriod = handler.getTaxOrVatPeriodForProfile(profile, 'T');
            TaxDashboard Tax = handler.getTaxCenterDashboard(profile, taxAndvatPeriod[0]);

            taxAndvatPeriod = handler.getTaxOrVatPeriodForProfile(profile, 'V');
            VATDashboard Vat = handler.getVatCenterDashboard(profile, taxAndvatPeriod[0]);

            ReportsViewModel viewModel = new ReportsViewModel();
            viewModel.DashboardIncomeExpense = IncomeExpense;
            viewModel.TAXDashboard = Tax;
            viewModel.VATDashboard = Vat;

            return View(viewModel);
        }
    }
}