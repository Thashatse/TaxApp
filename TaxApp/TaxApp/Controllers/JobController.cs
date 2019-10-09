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


                if (sDate.ToString("yyyy") == eDate.ToString("yyyy"))
                    ViewBag.DateRange = "From " + sDate.ToString("dd MMM") + " to " + eDate.ToString("dd MMM yyyy");
                else
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
                return RedirectToAction("Error", "Shared");
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
                return RedirectToAction("Error", "Shared");
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

                ViewBag.CurrentSession = false;
                foreach (Worklog item in JobHours)
                {
                    if(item.EndTime == item.StartTime)
                    {
                        ViewBag.CurrentSession = true;
                        ViewBag.CurrentSessionID = item.LogItemID;
                    }
                }

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
                return RedirectToAction("Error", "Shared", new { err = "Error Loading Job" });
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
            
                ViewBag.CurrentSession = false;
                foreach (Worklog item in JobHours)
                {
                    if (item.EndTime == item.StartTime)
                    {
                        ViewBag.CurrentSession = true;
                        ViewBag.CurrentSessionID = item.LogItemID;
                    }
                }

                return View(JobHours);
        }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding job Hours List");
                return RedirectToAction("Error", "Shared");
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
                return RedirectToAction("Error", "Shared");
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
                    return RedirectToAction("NewClient", "Client");

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
                return RedirectToAction("Error", "Shared");
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
                        "Use the link bellow to gain access: \n http://sict-iis.nmmu.ac.za/taxapp/Track/verifyIdentity?ID=" + resultID + "&Type=J";

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
                    return RedirectToAction("jobs","job");
                }
                else
                {
                    return RedirectToAction("Error", "Shared");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in email settings method of LandingControles");
                return RedirectToAction("Error", "Shared", new { err = "An error occurred while creating new job." });
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
                return RedirectToAction("Error", "Shared", new { err = "An error occurred while loading job for edit." });
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
                        return RedirectToAction("Error", "Shared", new { err ="An error occurred while saving job edits." });
                    }
                }

                return View(editJob);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding edit job");
                        return RedirectToAction("Error", "Shared", new { err ="An error occurred while loading job for edit." });
            }
        }
        #endregion
        
        #region New Work Log
        public ActionResult NewWorkLogItem(string ID, string Session ="false")
        {
            if(ID == null || ID == "")
                return RedirectToAction("Error", "Shared");

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

                ViewBag.StratTime = "09:00";
                ViewBag.EndTime = "17:00";
                ViewBag.Session = bool.Parse(Session);

                return View(defaultData);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loading new work log item view - NewWorkLogItem Job Controller");
                return RedirectToAction("Error", "Shared", new { err = " " });
            }
        }

        [HttpPost]
        public ActionResult NewWorkLogItem(FormCollection collection, string ID, string Session = "false")
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

                    if (bool.Parse(Session) == false)
                    {
                        if (DateTime.Parse(Request.Form["startTimeDate"].ToString() + " " + Request.Form["startTime"].ToString()) >
                        DateTime.Parse(Request.Form["endTimeDate"].ToString() + " " + Request.Form["endTime"].ToString()))
                        {
                            ViewBag.Err = "Please enter a start time and date before end date and time"; check = false;
                        }
                    }

                    if(check == true)
                    {
                        Model.Worklog logItem = new Model.Worklog();
                        logItem.Description = Request.Form["Description"].ToString();
                        if(bool.Parse(Session) == false)
                        {
                        logItem.StartTime = DateTime.Parse(Request.Form["startTimeDate"].ToString() + " " + Request.Form["startTime"].ToString());
                        logItem.EndTime = DateTime.Parse(Request.Form["endTimeDate"].ToString() + " " + Request.Form["endTime"].ToString());
                        }
                        else
                        {
                            logItem.StartTime = DateTime.Now;
                            logItem.EndTime = DateTime.Now;
                        }

                        bool result = handler.newWorkLogItem(logItem, jobID);

                        if (result == true)
                        {
                            return Redirect(Url.Action("job","Job")+"?ID=" + ID);
                        }
                        else
                        {
                            return RedirectToAction("Error", "Shared");
                        }
                    }

                    Model.SP_GetJob_Result Job = handler.getJob(jobID);
                    ViewBag.JobTitle = Job.JobTitle;
                    ViewBag.JobID = Job.JobID;

                    Worklog defaultData = new Worklog();
                    if (Request.Form["Description"] != null)
                        defaultData.Description = Request.Form["Description"].ToString();
                    ViewBag.StratTime = DateTime.Parse(Request.Form["startTimeDate"]).ToString("hh:mm");
                    ViewBag.EndTime = DateTime.Parse(Request.Form["endTimeDate"]).ToString("hh:mm");
                    ViewBag.Session = bool.Parse(Session);
                    defaultData.DefultDate = DateTime.Now.ToString("yyyy-MM-dd");
                    defaultData.MaxDateStart = DateTime.Parse(Request.Form["startTime"]).ToString("yyyy-MM-dd");
                    defaultData.MaxDateEnd = DateTime.Parse(Request.Form["endTime"]).ToString("yyyy-MM-dd");

                    return View(defaultData);
                }
                else
                {
                    return RedirectToAction("Error", "Shared");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in email settings method of LandingControles");
                return RedirectToAction("Error", "Shared");
            }
        }
        #endregion

        #region Edit Work Log
        public ActionResult EditJobWorkLog(string ID, string JobID)
        {
            if(ID == null || ID == "")
                return RedirectToAction("Error", "Shared");

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
                return RedirectToAction("Error", "Shared");
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
                            return Redirect(Url.Action("job","Job")+"?ID=" + JobID);
                        }
                        else
                        {
                            return RedirectToAction("Error", "Shared");
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
                    return RedirectToAction("Error", "Shared");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in email settings method of LandingControles");
                return RedirectToAction("Error", "Shared");
            }
        }
        #endregion

        #region Delete Work Log
        public ActionResult DeleteJobWorkLog(string ID, string JobID)
        {
            if(ID == null || ID == "")
                return RedirectToAction("Error", "Shared");

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
                return RedirectToAction("Error", "Shared");
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
                        return Redirect(Url.Action("job","Job")+"?ID=" + JobID);
                    }
                    else
                    {
                        return RedirectToAction("Error", "Shared");
                    }
                }
                else
                {
                    return RedirectToAction("Error", "Shared");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error deleting work log item");
                return RedirectToAction("Error", "Shared");
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
                if (currentJobs.Count == 0)
                    return RedirectToAction("NewJob", "Job");
                
                ViewBag.JobList = new SelectList(currentJobs, "JobID", "JobTitle");

                return View(currentJobs);
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding job on Job selector");
                return RedirectToAction("Error", "Shared");
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
                    Response.Redirect(Url.Action("NewJobExpense","Expense")+"?ID="+ Request.Form["JobList"].ToString());
                }
                else if (Dest == "New*Travel*Expense")
                {
                    Response.Redirect(Url.Action("NewTravelExpense","Expense")+"?ID=" + Request.Form["JobList"].ToString());
                }
                else if (Dest == "New*Invoice")
                {
                    Response.Redirect(Url.Action("NewInvoice", "Invoice")+"?ID=" + Request.Form["JobList"].ToString());
                }

                    function.logAnError("selecting job. Dest Value: '" + Dest + "'. In Job controler");
                return RedirectToAction("Error", "Shared");
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "selecting job. Dest Value: '"+Dest+"'. In Job controler");
                return RedirectToAction("Error", "Shared");
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
                        Response.Redirect(Url.Action("Job","Job")+"?ID=" + ID);
                    }
                    else
                    {
                        function.logAnError("Error marking Job As Complete Job controller");
                        return RedirectToAction("Error", "Shared", new { err ="Error marking invoice as paid"});
                    }
                }
                else
                {
                    function.logAnError("Error marking Job As Complete Job controller - no JobID supplied");
                        return RedirectToAction("Error", "Shared", new { err ="Error marking Job as Complete"});
                }

                return View();
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error marking Job As Complete Job controller");
                return RedirectToAction("Error", "Shared");
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
                        "Use the link bellow to gain access: \n http://sict-iis.nmmu.ac.za/taxapp/Track/verifyIdentity?ID=" + jobdetails.JobID+"&Type=Job";

                bool result = function.sendEmail(clientDetails.EmailAddress,
                    clientDetails.FirstName + " " + clientDetails.LastName,
                    subject,
                    body,
                    ViewBag.ProfileName,
                    int.Parse(cookie["ID"]));

                if(!result)
                    function.logAnError("Error emailing Job share");
            }

            return Redirect(Url.Action("Job","Job")+"?ID=" + ID);
        }
        #endregion

        #region end current session
        public ActionResult EndJobWorkLogSession(string ID, string JobID)
        {
            try
            {
                if (ID != null)
                {
                        Model.Worklog logItem = new Model.Worklog();
                        logItem.LogItemID = int.Parse(ID);
                    logItem = handler.getLogItem(logItem);
                        logItem.EndTime = DateTime.Now;

                        bool result = handler.EditWorkLogItem(logItem);

                        if (result == true)
                        {
                            return Redirect(Url.Action("job","Job")+"?ID=" + JobID);
                        }
                        else
                        {
                            return RedirectToAction("Error", "Shared", new { err = "Error ending work log session" });
                        }
                }
                else
                {
                    return RedirectToAction("Error", "Shared", new { err = "Error ending work log session" });
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error ending work log session");
                return RedirectToAction("Error", "Shared", new { err = "Error ending work log session" });
            }
        }
        #endregion
    }
}