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

        #region View Jobs (List/Details)
        // GET: Jobs
        public ActionResult Jobs()
        {
            try
            {
                getCookie();
            Model.Profile getJobs = new Model.Profile();
            getJobs.ProfileID = int.Parse(cookie["ID"].ToString());
            List<Model.SP_GetJob_Result> currentJobs = handler.getProfileJobs(getJobs);
            List<Model.SP_GetJob_Result> pastJobs = handler.getProfileJobsPast(getJobs);

                if(pastJobs.Count == 0)
                {
                    pastJobs = null;
                }

                if (currentJobs.Count == 0)
                {
                    currentJobs = null;
                }

                var viewModel = new Model.JobViewModel();
                viewModel.curentJobs = currentJobs;
                viewModel.pastJobs = pastJobs;

                return View(viewModel);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding job details");
                return RedirectToAction("../Shared/Error");
            }
        }

        // GET: Job
        public ActionResult Job(string ID)
        {
            try
            {
                getCookie();
            Model.Job getJob = new Model.Job();
            getJob.JobID = int.Parse(ID);

                Model.SP_GetJob_Result Job = handler.getJob(getJob);

                ViewBag.JobTitle = Job.JobTitle;
                ViewBag.JobID = Job.JobID;

                if(Job.EndDate != null)
                {
                    ViewBag.Complete = "Done";
                }
                else
                {
                    ViewBag.Complete = "NotDone";
                }

                return View(Job);
        }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding job details");
                return Redirect("../Shared/Error?Err=Error Loading Job");
    }

}
        #endregion

        #region Worklog Details and list
        public ActionResult JobWorkLog(string ID)
        {
            try
            {
                getCookie();
            Model.Job getJob = new Model.Job();
            getJob.JobID = int.Parse(ID);
            List<Model.Worklog> JobHours = handler.getJobHours(getJob);
                ViewBag.JobID = ID;

                Model.SP_GetJob_Result Job = handler.getJob(getJob);
                ViewBag.JobTitle = Job.JobTitle;
                ViewBag.JobID = Job.JobID;

                if (Job.EndDate != null)
                {
                    ViewBag.Complete = "Done";
                }
                else
                {
                    ViewBag.Complete = "NotDone";
                }

                return View(JobHours);
        }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding job Hours List");
                return Redirect("/job/jobs");
    }
}
        public ActionResult JobWorkLogItem(string ID, string JobID)
        {
            try
            {
                getCookie();
            Model.Worklog LogID = new Model.Worklog();
            LogID.LogItemID = int.Parse(ID);
            Model.Worklog LogItem = handler.getLogItem(LogID);
                ViewBag.JobID = ID;

                Model.Job getJob = new Model.Job();
                getJob.JobID = int.Parse(JobID);
                Model.SP_GetJob_Result Job = handler.getJob(getJob);
                ViewBag.JobTitle = Job.JobTitle;
                ViewBag.JobID = Job.JobID;

                if (Job.EndDate != null)
                {
                    ViewBag.Complete = "Done";
                }
                else
                {
                    ViewBag.Complete = "NotDone";
                }


                return View(LogItem);
        }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Log Item Details");
                return Redirect("/job/jobs");
    }
}
        #endregion

        #region New Job
        // GET: Landing/NewProfile
        public ActionResult NewJob()
        {
            try { 
            getCookie();
            Model.Client getClients = new Model.Client();
            getClients.ProfileID = int.Parse(cookie["ID"].ToString());
            List<Model.Client> Clients = handler.getProfileClients(getClients);
            ViewBag.ClientList = new SelectList(Clients, "ClientID", "FirstName");
            return View();
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding new job");
                return Redirect("/job/jobs");
            }
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
                newJob.StartDate = DateTime.Parse(Request.Form["StartDate"]);

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
        public ActionResult NewWorkLogItem(string ID)
        {
            getCookie();

            Model.Job getJob = new Model.Job();
            getJob.JobID = int.Parse(ID);
            Model.SP_GetJob_Result Job = handler.getJob(getJob);
            ViewBag.JobTitle = Job.JobTitle;
            ViewBag.JobID = Job.JobID;

            return View();
        }

        // POST: Landing/NewProfile
        [HttpPost]
        public ActionResult NewWorkLogItem(FormCollection collection, string ID)
        {
            try
            {
                if(ID != null) {
                Model.Job jobID = new Model.Job();
                jobID.JobID = int.Parse(ID);

                Model.Worklog logItem = new Model.Worklog();
                logItem.Description = Request.Form["Description"].ToString();
                logItem.StartTime = DateTime.Parse(Request.Form["startTimeDate"].ToString() + " " + Request.Form["startTime"].ToString());
                logItem.EndTime = DateTime.Parse(Request.Form["endTimeDate"].ToString() + " " + Request.Form["endTime"].ToString());

                bool result = handler.newWorkLogItem(logItem, jobID);

                if (result == true)
                {
                    return Redirect("/job/JobWorkLog?ID=" + ID);
                }
                else
                {
                    return RedirectToAction("../Shared/Error");
                }
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

        #region Job Selector
        public ActionResult JobSelector(string Dest)
        {
            getCookie();

            try
            {
                ViewBag.Title = "Job For "+Dest.Replace('*', ' ');

                Model.Profile getJobs = new Model.Profile();
                getJobs.ProfileID = int.Parse(cookie["ID"].ToString());

                List<Model.SP_GetJob_Result> currentJobs = handler.getProfileJobs(getJobs);
                
                ViewBag.JobList = new SelectList(currentJobs, "JobID", "JobTitle");

                return View(currentJobs);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding job on Job selector");
                return RedirectToAction("../Shared/Error");
            }
        }

        [HttpPost]
        public ActionResult JobSelector(FormCollection collection, string Dest)
        {
            getCookie();

            try
            {
                Model.Profile getJobs = new Model.Profile();
                getJobs.ProfileID = int.Parse(cookie["ID"].ToString());

                List<Model.SP_GetJob_Result> currentJobs = handler.getProfileJobs(getJobs);

                ViewBag.JobList = new SelectList(currentJobs, "JobID", "JobTitle");

                if (Dest == "New*Job*Expense")
                {
                    Response.Redirect("../Expense/NewJobExpense?ID="+ Request.Form["JobList"].ToString());
                }
                else if (Dest == "New*Travel*Expense")
                {
                    Response.Redirect("../Expense/NewTravelExpense?ID=" + Request.Form["JobList"].ToString());
                }

                    function.logAnError("selecting job. Dest Value: '" + Dest + "'. In Job controler");
                    return View("../Shared/Error");
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "selecting job. Dest Value: '"+Dest+"'. In Job controler");
                return View("../Shared/Error");
            }
        }
        #endregion

        #region Mark Job As Complete
        // GET: Invoice/Create
        public ActionResult MarkAsComplete(string ID = "0")
        {
            try
            {
                getCookie();

                if (ID != "0")
                {
                    Model.Job job = new Model.Job();
                    job.JobID = int.Parse(ID);
                    bool result = handler.MarkJobAsComplete(job);

                    if (result == true)
                    {
                        Response.Redirect("/Job/Job?ID=" + ID);
                    }
                    else
                    {
                        function.logAnError("Error marking Job As Complete Job controller");
                        Response.Redirect("../Shared/Error?Err=Error marking invoice as paid");
                    }
                }
                else
                {
                    function.logAnError("Error marking Job As Complete Job controller - no JobID supplied");
                    Response.Redirect("../Shared/Error?Err=Error marking Job as Complete");
                }

                return View();
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error marking Job As Complete Job controller");
                return Redirect("/job/job?ID=" + ID);
            }
        }
        #endregion
    }
}