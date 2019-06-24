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

                        if (handler.getProfile(checkProfile) == null)
                        {
                            Response.Redirect("/Landing/Welcome");
                        }
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

        // GET: Job
        public ActionResult Jobs()
        {
            getCookie();
            Model.Profile getJobs = new Model.Profile();
            getJobs.ProfileID = int.Parse(cookie["ID"].ToString());
            List<Model.Job> Jobs = handler.getProfileJobs(getJobs);
            return View(Jobs);
        }
        
        #region New Job
        // GET: Landing/NewProfile
        public ActionResult NewJob()
        {
            getCookie();
            Model.Client getClients = new Model.Client();
            getClients.ProfileID = int.Parse(cookie["ID"].ToString());
            List<Model.Client> Clients = handler.getProfileClients(getClients);
            ViewBag.ClientList = new SelectList(Clients, "ClientID", "FirstName");
            return View();
        }

        // POST: Landing/NewProfile
        [HttpPost]
        public ActionResult NewJob(FormCollection collection)
        {
            try
            {
                Model.Job newJob = new Model.Job();

                newJob.ClientID = int.Parse(Request.Form["ClientList"].ToString());
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
        
        #region New Work Log
        // GET: Landing/NewProfile
        public ActionResult NewWorkLogItem()
        {
            getCookie();
            return View();
        }

        // POST: Landing/NewProfile
        [HttpPost]
        public ActionResult NewWorkLogItem(FormCollection collection, string JobID)
        {
            try
            {
                Model.Job jobID = new Model.Job();
                jobID.JobID = int.Parse(JobID);

                Model.Worklog logItem = new Model.Worklog();
                logItem.Description = Request.Form["Description"].ToString();
                logItem.StartTime = DateTime.Parse(Request.Form["StartTime"].ToString());
                logItem.EndTime = DateTime.Parse(Request.Form["EndTime"].ToString());

                bool result = handler.newWorkLogItem(logItem, jobID);

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