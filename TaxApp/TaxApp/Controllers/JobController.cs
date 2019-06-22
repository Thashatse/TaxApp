using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaxApp.Controllers
{
    public class JobController : Controller
    {
        IDBHandler handler = new DBHandler();
        HttpCookie cookie;
        Functions function = new Functions();

        // GET: Job
        public ActionResult Jobs()
        {
            return View();
        }
        
        #region New Job
        // GET: Landing/NewProfile
        public ActionResult NewJob()
        {
            return View();
        }

        // POST: Landing/NewProfile
        [HttpPost]
        public ActionResult NewJob(FormCollection collection)
        {
            try
            {
                Model.Job newJob = new Model.Job();

                newJob.ClientID = int.Parse(Request.Form["ClientID"].ToString());
                newJob.JobTitle = Request.Form["JobTitle"].ToString();
                newJob.HourlyRate = decimal.Parse(Request.Form["HourlyRate"].ToString());
                newJob.Budget = decimal.Parse(Request.Form["Budget"].ToString());
                newJob.StartDate = DateTime.Parse(Request.Form["StartDate"].ToString());

                bool result = handler.newJob(newJob);

                if (result == true)
                {
                    return Redirect("/job/jobs");
                }
                else
                {
                    return RedirectToAction("../Shared/Error");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in email settings method of LandingControles");
                return View();
            }
        }
        #endregion
    }
}