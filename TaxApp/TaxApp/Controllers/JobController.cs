using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;

namespace TaxApp.Controllers
{
    public class JobController : Controller
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

        #region View Jobs (List/Details)
        // GET: Jobs
        public ActionResult Jobs(string view, string PastJobsDisplayCount, string StartDateRange, string EndDateRange)
        {
            try
            {
                getCookie();

                ViewBag.cat = 'J';
                ViewBag.view = view;
                ViewBag.SeeMore = false;
                int year = DateTime.Now.Year;
                DateTime sDate = DateTime.Now.AddMonths(-6);
                DateTime eDate = DateTime.Now;

                if (StartDateRange != null && EndDateRange != null
                    && DateTime.TryParse(StartDateRange, out sDate) && DateTime.TryParse(EndDateRange, out eDate)) { }

                if (sDate > eDate)
                {
                    DateTime temp = sDate;
                    sDate = eDate;
                    eDate = temp;
                }

                ViewBag.DateRange = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                ViewBag.StartDateRange = sDate.ToString("yyyy-MM-dd");
                ViewBag.EndDateRange = eDate.ToString("yyyy-MM-dd");

                Model.Profile getJobs = new Model.Profile();
            getJobs.ProfileID = int.Parse(cookie["ID"].ToString());
                Client client = new Model.Client();
                client.ClientID = 0;
                List<Model.SP_GetJob_Result> pastJobs = handler.getProfileJobsPast(getJobs, client, sDate, eDate);
            List<Model.SP_GetJob_Result> currentJobs = handler.getProfileJobs(getJobs, client);

                if(pastJobs.Count == 0)
                {
                    pastJobs = null;
                }
                else if(pastJobs.Count > 3)
                {
                    int x;
                    if (PastJobsDisplayCount != null && PastJobsDisplayCount != "" && function.IsDigitsOnly(PastJobsDisplayCount))
                        x = int.Parse(PastJobsDisplayCount);
                    else
                        x = 6;

                    if(x < pastJobs.Count)
                    {
                        pastJobs = pastJobs.GetRange(0, x);
                        ViewBag.SeeMore = true;
                    }
                    else
                    {
                        pastJobs = pastJobs.GetRange(0, pastJobs.Count);
                        ViewBag.SeeMore = false;
                    }

                    ViewBag.X = x + 6;
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

        [HttpPost]
        public ActionResult Jobs(FormCollection collection, string view, string PastJobsDisplayCount, string StartDateRange, string EndDateRange)
        {
            try
            {
                ViewBag.cat = 'J';
                int year = DateTime.Now.Year;
                DateTime sDate = DateTime.Now.AddMonths(-6);
                DateTime eDate = DateTime.Now;

                DateTime.TryParse(Request.Form["StartDate"], out sDate);
                DateTime.TryParse(Request.Form["EndDate"], out eDate);

                StartDateRange = sDate.ToString("yyyy-MM-dd");
                EndDateRange = eDate.ToString("yyyy-MM-dd");

                return RedirectToAction("Jobs", "Job", new {
                    view,
                    PastJobsDisplayCount,
                    StartDateRange,
                    EndDateRange
                });
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error updating date range for jobs page");
                return RedirectToAction("../Shared/Error");
            }
        }

        // GET: Job
        public ActionResult Job(string ID)
        {
            try
            {
                ViewBag.cat = 'J';
                getCookie();
            Model.Job getJob = new Model.Job();
            getJob.JobID = int.Parse(ID);

                Model.SP_GetJob_Result Job = handler.getJob(getJob);

                List<Model.Worklog> JobHours = handler.getJobHours(getJob);

                List<Model.SP_GetJobExpense_Result> JobExpenses = handler.getJobExpenses(getJob);

                List<Model.TravelLog> JobTravelLog = handler.getJobTravelLog(getJob);

                List<SP_GetInvoice_Result> invoiceDetails = handler.getJobInvoices(getJob);

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

                List<List<SP_GetJobIntemsToInvoice_Result>> JobItemsForInvoice = handler.getJobItemsForInvoice(getJob);
                if (JobItemsForInvoice.ElementAt(0).Count == 0
                    && JobItemsForInvoice.ElementAt(1).Count == 0
                    && JobItemsForInvoice.ElementAt(2).Count == 0)
                    ViewBag.NoInvoice = "True";

                TrackJobViewModel view = new TrackJobViewModel();
                view.jobDetails = Job;
                view.Worklog = JobHours;
                view.JobExpenses = JobExpenses;
                view.JobTravelLog = JobTravelLog;
                view.invoices = invoiceDetails;
                return View(view);
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
                ViewBag.cat = "WL";
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
                ViewBag.cat = "WL";
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

                if (Clients.Count == 0)
                    Response.Redirect("../Client/NewClient");

            ViewBag.ClientList = new SelectList(Clients, "ClientID", "FirstName");

                Profile getProfile = new Profile();
                getProfile.ProfileID = int.Parse(cookie["ID"].ToString());
                getProfile.EmailAddress = "";
                getProfile.Username = "";
                getProfile = handler.getProfile(getProfile);
                Job defaultData = new Job();
                defaultData.HourlyRate = getProfile.DefaultHourlyRate;
                defaultData.DefultDate = DateTime.Now.ToString("yyyy-MM-dd");
                defaultData.MinDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            return View(defaultData);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding new job");
                return Redirect("../Shared/Error");
            }
        }

        // POST: Landing/NewProfile
        [HttpPost]
        public ActionResult NewJob(FormCollection collection)
        {
            try
            {
                getCookie();

                Model.Client getClients = new Model.Client();
                getClients.ProfileID = int.Parse(cookie["ID"].ToString());
                List<Model.Client> Clients = handler.getProfileClients(getClients);
                ViewBag.ClientList = new SelectList(Clients, "ClientID", "FirstName");

                Model.Job newJob = new Model.Job();

                newJob.ClientID = int.Parse(Request.Form["ClientList"].ToString());
                newJob.JobTitle = Request.Form["JobTitle"].ToString();
                newJob.HourlyRate = decimal.Parse(Request.Form["HourlyRate"].ToString());
                newJob.Share = false;
                if(Request.Form["Share"].ToString() == "true,false")
                    newJob.Share = true;
                if (Request.Form["Budget"].ToString() != "")
                    newJob.Budget = decimal.Parse(Request.Form["Budget"].ToString());
                else
                    newJob.Budget = 0;
                newJob.StartDate = DateTime.Parse(Request.Form["StartDate"]);

                string resultID = handler.newJob(newJob);

                Client clientDetails = null;

                Client getclient = new Client();
                getclient.ClientID = newJob.ClientID;
                clientDetails = handler.getClient(getclient);

                bool result;
                if (resultID == null && resultID == "")
                    result = false;
                else if (newJob.Share)
                    result = false;
                else
                    result = true;

                if (resultID != null && clientDetails != null && resultID != "" && newJob.Share)
            {
                    string subject = "";
                    string body = "";

                    subject = ViewBag.ProfileName + " has shared information about " + newJob.JobTitle + ".";
                    body = "Hello " + clientDetails.FirstName + " " + clientDetails.LastName + " \n \n " +
                        ViewBag.ProfileName + " has shared information about " + newJob.JobTitle
                        + " with you using Tax App. \n" +
                        "Use the link bellow to gain access: \n http://localhost:54533/Track/verifyIdentity?ID=" + resultID + "&Type=J";

                    result = function.sendEmail(clientDetails.EmailAddress,
                        clientDetails.FirstName + " " + clientDetails.LastName,
                        subject,
                        body,
                        ViewBag.ProfileName,
                        int.Parse(cookie["ID"]));

                    if (!result)
                        function.logAnError("Error emailing Job share");

                    if (resultID != "" && resultID != null)
                        result = true;
                }


                if (result == true)
                {
                    return Redirect("/job/jobs");
                }
                else
                {
                    return Redirect("../Shared/Error");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in email settings method of LandingControles");
                return Redirect("../Shared/Error?Err=An error occurred while creating new job.");
            }
        }
        #endregion

        #region edit Job
        public ActionResult EditJob(string ID)
        {
            try {
                getCookie();

                Model.Job getJob = new Model.Job();
                getJob.JobID = int.Parse(ID);

                Model.SP_GetJob_Result Job = handler.getJob(getJob);

                Job editJob = new Job();
                editJob.JobID = Job.JobID;
                editJob.JobTitle = Job.JobTitle;
                editJob.HourlyRate = Job.HourlyRate;
                editJob.Budget = Job.Budget;

                ViewBag.JobTitle = Job.JobTitle;
                ViewBag.JobID = Job.JobID;

                return View(editJob);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding edit job");
                return Redirect("../Shared/Error?Err=An error occurred while loading job for edit.");
            }
        }

        [HttpPost]
        public ActionResult EditJob(string ID, FormCollection collection)
        {
            try
            {
                getCookie();
                
                Job editJob = new Job();
                editJob.JobID = int.Parse(ID);
                SP_GetJob_Result Job = handler.getJob(editJob);
                editJob.JobID = Job.JobID;
                editJob.JobTitle = Job.JobTitle;
                editJob.HourlyRate = Job.HourlyRate;
                editJob.Budget = Job.Budget;

                bool err = false;

                if (Request.Form["HourlyRate"] == null || Request.Form["HourlyRate"] == "")
                {
                    ViewBag.Err = "Please enter a hourly rate";
                    err = true;
                }
                else
                {
                    editJob.HourlyRate = decimal.Parse(Request.Form["HourlyRate"].ToString());
                }

                if(Request.Form["JobTitle"] == null || Request.Form["JobTitle"] == "")
                {
                    ViewBag.Err = "Please enter a job title";
                    err = true;
                }
                else
                {
                    editJob.JobTitle = Request.Form["JobTitle"].ToString();
                }

                if(Request.Form["Budget"].ToString() != "")
                    editJob.Budget = decimal.Parse(Request.Form["Budget"].ToString());

                if(err == false)
                {
                    bool result = handler.editJob(editJob);

                    if (result == true)
                    {
                        return Redirect("/job/job?ID=" + editJob.JobID);
                    }
                    else
                    {
                        return Redirect("../Shared/Error?Err=An error occurred while saving job edits.");
                    }
                }

                return View(editJob);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding edit job");
                return Redirect("../Shared/Error?Err=An error occurred while loading job for edit.");
            }
        }
        #endregion
        
        #region New Work Log
        // GET: Landing/NewProfile
        public ActionResult NewWorkLogItem(string ID)
        {
            if(ID == null || ID == "")
                Response.Redirect("../Shared/Error");

            try
            {
                getCookie();

            Model.Job getJob = new Model.Job();
            getJob.JobID = int.Parse(ID);
            Model.SP_GetJob_Result Job = handler.getJob(getJob);
            ViewBag.JobTitle = Job.JobTitle;
            ViewBag.JobID = Job.JobID;

            Worklog defaultData = new Worklog();
            defaultData.DefultDate = DateTime.Now.ToString("yyyy-MM-dd");
            defaultData.MaxDateStart = DateTime.Now.ToString("yyyy-MM-dd");
            defaultData.MaxDateEnd = DateTime.Now.ToString("yyyy-MM-dd");

                return View(defaultData);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loading new work log item view - NewWorkLogItem Job Controller");
                return View();
            }
        }

        // POST: Landing/NewProfile
        [HttpPost]
        public ActionResult NewWorkLogItem(FormCollection collection, string ID)
        {
            try
            {
                if (ID != null)
                {
                    Model.Job jobID = new Model.Job();
                    jobID.JobID = int.Parse(ID);

                    bool check = true;

                    if (Request.Form["Description"] == null || Request.Form["Description"].ToString() == "")
                    {
                        ViewBag.Err = "Please enter a description"; check = false;
                    }

                    if (DateTime.Parse(Request.Form["startTimeDate"].ToString() + " " + Request.Form["startTime"].ToString()) >
                        DateTime.Parse(Request.Form["endTimeDate"].ToString() + " " + Request.Form["endTime"].ToString()))
                    {
                        ViewBag.Err = "Please enter a start time and date before end date and time"; check = false;
                    }

                    if(check == true)
                    {
                        Model.Worklog logItem = new Model.Worklog();
                        logItem.Description = Request.Form["Description"].ToString();
                        logItem.StartTime = DateTime.Parse(Request.Form["startTimeDate"].ToString() + " " + Request.Form["startTime"].ToString());
                        logItem.EndTime = DateTime.Parse(Request.Form["endTimeDate"].ToString() + " " + Request.Form["endTime"].ToString());

                        bool result = handler.newWorkLogItem(logItem, jobID);

                        if (result == true)
                        {
                            return Redirect("/job/Job?ID=" + ID);
                        }
                        else
                        {
                            return RedirectToAction("../Shared/Error");
                        }
                    }

                    Model.SP_GetJob_Result Job = handler.getJob(jobID);
                    ViewBag.JobTitle = Job.JobTitle;
                    ViewBag.JobID = Job.JobID;

                    Worklog defaultData = new Worklog();
                    defaultData.DefultDate = DateTime.Now.ToString("yyyy-MM-dd");
                    defaultData.MaxDateStart = DateTime.Now.ToString("yyyy-MM-dd");
                    defaultData.MaxDateEnd = DateTime.Now.ToString("yyyy-MM-dd");

                    return View(defaultData);
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

        #region Edit Work Log
        public ActionResult EditJobWorkLog(string ID, string JobID)
        {
            if(ID == null || ID == "")
                Response.Redirect("../Shared/Error");

            try
            {
                getCookie();

                Worklog Item = new Worklog();
                Item.LogItemID = int.Parse(ID);
                Item = handler.getLogItem(Item);

                Item.DateString = Item.StartTime.ToString("yyyy-MM-dd");
                Item.DefultDate = Item.EndTime.ToString("yyyy-MM-dd");
                Item.MaxDateStart = DateTime.Now.ToString("yyyy-MM-dd");
                Item.MaxDateEnd = DateTime.Now.ToString("yyyy-MM-dd");

                ViewBag.JobID = JobID;

                return View(Item);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loading edit work log item view - NewWorkLogItem Job Controller");
                return View();
            }
        }

        [HttpPost]
        public ActionResult EditJobWorkLog(FormCollection collection, string ID, string JobID)
        {
            try
            {
                if (ID != null)
                {
                    if (Request.Form["Description"] != null && Request.Form["Description"].ToString() != "")
                    {
                        Model.Worklog logItem = new Model.Worklog();
                        logItem.LogItemID = int.Parse(ID);
                        logItem.Description = Request.Form["Description"].ToString();
                        logItem.StartTime = DateTime.Parse(Request.Form["startTimeDate"].ToString() + " " + Request.Form["startTime"].ToString());
                        logItem.EndTime = DateTime.Parse(Request.Form["endTimeDate"].ToString() + " " + Request.Form["endTime"].ToString());

                        bool result = handler.EditWorkLogItem(logItem);

                        if (result == true)
                        {
                            return Redirect("/job/Job?ID=" + JobID);
                        }
                        else
                        {
                            return RedirectToAction("../Shared/Error");
                        }
                    }
                    else
                    {
                        Worklog Item = new Worklog();
                        Item.LogItemID = int.Parse(ID);
                        Item = handler.getLogItem(Item);

                        ViewBag.Err = "Please enter a description";

                        return View(Item);
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

        #region Delete Work Log
        public ActionResult DeleteJobWorkLog(string ID, string JobID)
        {
            if(ID == null || ID == "")
                Response.Redirect("../Shared/Error");

            try
            {
                getCookie();

                Worklog Item = new Worklog();
                Item.LogItemID = int.Parse(ID);
                Item = handler.getLogItem(Item);

                ViewBag.JobID = JobID;

                return View(Item);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loading delete work log item view - NewWorkLogItem Job Controller");
                return View();
            }
        }

        // POST: Landing/NewProfile
        [HttpPost]
        public ActionResult DeleteJobWorkLog(FormCollection collection, string ID, string JobID)
        {
            try
            {
                if (ID != null)
                {
                    Model.Worklog logItem = new Model.Worklog();
                    logItem.LogItemID = int.Parse(ID);

                    bool result = handler.DeleteWorkLogItem(logItem);

                    if (result == true)
                    {
                        return Redirect("/job/Job?ID=" + JobID);
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
                    "Error deleting work log item");
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

                Client client = new Model.Client();
                client.ClientID = 0;

                List<Model.SP_GetJob_Result> currentJobs = handler.getProfileJobs(getJobs, client);
                
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

                Client client = new Model.Client();
                client.ClientID = 0;

                List<Model.SP_GetJob_Result> currentJobs = handler.getProfileJobs(getJobs, client);

                ViewBag.JobList = new SelectList(currentJobs, "JobID", "JobTitle");

                if (Dest == "New*Job*Expense")
                {
                    Response.Redirect("../Expense/NewJobExpense?ID="+ Request.Form["JobList"].ToString());
                }
                else if (Dest == "New*Travel*Expense")
                {
                    Response.Redirect("../Expense/NewTravelExpense?ID=" + Request.Form["JobList"].ToString());
                }
                else if (Dest == "New*Invoice")
                {
                    Response.Redirect("../Invoice/NewInvoice?ID=" + Request.Form["JobList"].ToString());
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

        #region Update Job Share
        public ActionResult Share(string ID)
        {
            getCookie();

            Job getJob = new Job();
            getJob.JobID = int.Parse(ID);

            SP_GetJob_Result jobdetails = null;
            Client clientDetails = null;

            try
            {
                Job jobID = new Job();
                jobID.JobID = int.Parse(ID);

                if (!handler.UpdateShareJob(jobID))
                    function.logAnError("Error Updating Job share");

                jobdetails = handler.getJob(getJob);

                Client getclient = new Client();
                getclient.ClientID = jobdetails.ClientID;
                clientDetails = handler.getClient(getclient);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error Updating job share");
            }

            if (jobdetails != null && clientDetails != null && jobdetails.Share)
            {
                string subject = "";
                string body = "";

                    subject = ViewBag.ProfileName + " has shared information about "+jobdetails.JobTitle +".";
                    body = "Hello " + clientDetails.FirstName + " " + clientDetails.LastName + " \n \n " +
                        ViewBag.ProfileName + " has shared information about " + jobdetails.JobTitle
                        + " with you using Tax App. \n \n" +
                        "Use the link bellow to gain access: \n http://localhost:54533/Track/verifyIdentity?ID=" + jobdetails.JobID+"&Type=Job";

                bool result = function.sendEmail(clientDetails.EmailAddress,
                    clientDetails.FirstName + " " + clientDetails.LastName,
                    subject,
                    body,
                    ViewBag.ProfileName,
                    int.Parse(cookie["ID"]));

                if(!result)
                    function.logAnError("Error emailing Job share");
            }

            Response.Redirect("../Job/Job?ID=" + ID);
                return RedirectToAction("Job", "Job", new
                {
                    ID
                });
        }
        #endregion
    }
}