using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Model;

namespace DAL
{
    public class DBAccess : IDBAccess
    {
        #region Profile
        public bool newprofile(Model.Profile User)
        {
            bool Result = false;
            
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
            new SqlParameter("@FN", User.FirstName),
            new SqlParameter("@LN", User.LastName),
            new SqlParameter("@CN", User.CompanyName),
            new SqlParameter("@EM", User.EmailAddress),
            new SqlParameter("@CNum", User.ContactNumber),
            new SqlParameter("@PA", User.PhysicalAddress),
            new SqlParameter("@VATNum", User.VATNumber),
            new SqlParameter("@DR", User.DefaultHourlyRate),
            new SqlParameter("@UN", User.Username),
            new SqlParameter("@Pass", User.Password)
                   };
                
                Result = DBHelper.NonQuery("SP_NewProfile", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }


        public Model.Profile getProfile(Model.Profile User)
        {
            Model.Profile profile = null;

            SqlParameter[] pars = new SqlParameter[]
                {
            new SqlParameter("@PI", User.ProfileID),
            new SqlParameter("@EM", User.EmailAddress),
            new SqlParameter("@UN", User.Username)
                };

            try
            {
                using (DataTable table = DBHelper.ParamSelect("SP_GetProfile",
            CommandType.StoredProcedure, pars))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if(row != null)
                        {
                            profile = new Model.Profile();
                            profile.ProfileID = int.Parse(row[0].ToString());
                            profile.FirstName = row[1].ToString();
                            profile.LastName = row["LastName"].ToString();
                            profile.CompanyName = row["CompanyName"].ToString();
                            profile.EmailAddress = row["EmailAddress"].ToString();
                            profile.ContactNumber = row["ContactNumber"].ToString();
                            profile.PhysicalAddress = row["PhysicalAddress"].ToString();
                            //profile.ProfilePicture = row["ProfilePicture"].ToString();
                            profile.VATNumber = row["VATNumber"].ToString();
                            profile.DefaultHourlyRate = Convert.ToDecimal(row["DefaultHourlyRate"].ToString());
                            profile.Active = Convert.ToBoolean(row["Active"].ToString());
                            profile.Username = row["Username"].ToString();
                            profile.Password = row["Password"].ToString();
                            profile.PassRestCode = row["PassRestCode"].ToString();
                        }
                    }
                }
                return profile;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        #endregion

        #region Bussiness
        public Model.Business GetBussiness()
        {
            Model.Business business = new Model.Business();

            try
            {
                using (DataTable table = DBHelper.Select("SP_GetBussiness",
            CommandType.StoredProcedure))
                {
                        foreach (DataRow row in table.Rows)
                        {
                        business.BusinessID = int.Parse(row["BusinessID"].ToString());
                        business.VATRate = Convert.ToDecimal(row["VATRate"].ToString());
                        business.SMSSid = row["SMSSid"].ToString();
                        business.SMSToken = row["SMSToken"].ToString();
                    }
                }
                return business;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        #endregion

        #region Tax Consultant
        public bool newConsultant(Model.TaxConsultant consultant)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@N", consultant.Name),
                        new SqlParameter("@EA", consultant.EmailAddress),
                        new SqlParameter("@PI", consultant.ProfileID)
                   };

                Result = DBHelper.NonQuery("SP_NewConsultant", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }


        public Model.TaxConsultant getConsultant(Model.TaxConsultant Consultant)
        {
            Model.TaxConsultant consultant = null;

            SqlParameter[] pars = new SqlParameter[]
                {
                        new SqlParameter("@PI", Consultant.ProfileID)
                };

            try
            {
                using (DataTable table = DBHelper.ParamSelect("SP_GetConsultant",
            CommandType.StoredProcedure, pars))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row != null)
                        {
                            consultant = new Model.TaxConsultant();
                            consultant.ProfileID = int.Parse(row[0].ToString());
                            consultant.Name = row[1].ToString();
                            consultant.EmailAddress = row[2].ToString();
                        }
                    }
                }
                return consultant;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        #endregion

        #region Email Settings
        public bool newEmailSettings(Model.EmailSetting Settings)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@PI", Settings.ProfileID),
                        new SqlParameter("@A", Settings.Address),
                        new SqlParameter("@Pass", Settings.Password),
                        new SqlParameter("@H", Settings.Host),
                        new SqlParameter("@P", Settings.Port),
                        new SqlParameter("@ESsl", Settings.EnableSsl),
                        new SqlParameter("@DM", Settings.DeliveryMethod),
                        new SqlParameter("@UDC", Settings.UseDefailtCredentials)
                   };

                Result = DBHelper.NonQuery("SP_NewEmailSettings", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }


        public Model.EmailSetting getEmailSettings(Model.EmailSetting Settings)
        {
            Model.EmailSetting settings = null;

            SqlParameter[] pars = new SqlParameter[]
                {
                        new SqlParameter("@PI", Settings.ProfileID)
                };

            try
            {
                using (DataTable table = DBHelper.ParamSelect("SP_GetEmailSettings",
            CommandType.StoredProcedure, pars))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row != null)
                        {
                            settings = new Model.EmailSetting();
                            settings.ProfileID = int.Parse(row[0].ToString());
                            settings.Address = row[1].ToString();
                            settings.Password = row[2].ToString();
                            settings.Host = row[3].ToString();
                            settings.Port = row[4].ToString();
                            settings.DeliveryMethod = row[6].ToString();
                            settings.EnableSsl = Convert.ToBoolean(row[5].ToString());
                            settings.UseDefailtCredentials = Convert.ToBoolean(row[7].ToString());
                        }
                    }
                }
                return settings;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        #endregion

        #region Job
        public bool newJob(Job job)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@CI", job.ClientID),
                        new SqlParameter("@JT", job.JobTitle),
                        new SqlParameter("@HR", job.HourlyRate),
                        new SqlParameter("@B", job.Budget),
                        new SqlParameter("@SD",job.StartDate),
                   };

                Result = DBHelper.NonQuery("SP_NewJob", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }

        public SP_GetJob_Result getJob(Job Job)
        {
            SP_GetJob_Result job = null;

            SqlParameter[] pars = new SqlParameter[]
                {
                        new SqlParameter("@JID", Job.JobID)
                };

            try
            {
                using (DataTable table = DBHelper.ParamSelect("SP_GetJob",
            CommandType.StoredProcedure, pars))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row != null)
                        {
                            job = new SP_GetJob_Result();
                            job.JobID = int.Parse(row[0].ToString());
                            job.ClientID = int.Parse(row[6].ToString());

                            if (row[8].ToString() != "" && row[8] != null)
                            {
                                job.WorkLogHours = int.Parse(row[8].ToString());
                                int Hour = int.Parse(row[8].ToString()) / 60;
                                int Minute = int.Parse(row[8].ToString()) % 60;
                                job.WorkLogHoursString = Hour +":" + Minute + " ";
                            }
                            else
                            {
                                job.WorkLogHoursString = "None";
                            }

                            job.JobTitle = row[1].ToString();
                            job.ClientFirstName = row[7].ToString();
                            job.StartDate = DateTime.Parse(row[4].ToString());
                            job.StartDateString = String.Format("{0:dddd, dd MMMM yyyy}", job.StartDate);

                            if (row["EndDate"].ToString() != "" && row["EndDate"] != null)
                            {
                                job.EndDate = DateTime.Parse(row[6].ToString());
                                job.EndDateString = String.Format("{0:dddd, dd MMMM yyyy}", job.EndDate);
                                
                            }
                            else
                            {
                                job.EndDateString = "Active";
                            }

                            job.HourlyRate = decimal.Parse(row[2].ToString());
                            job.Budget = decimal.Parse(row[3].ToString());

                            if (row[9].ToString() != "" && row[9] != null)
                            {
                                job.ExpenseTotal = decimal.Parse(row[9].ToString());
                            }
                            else
                            {
                                job.ExpenseTotal = 0;
                            }

                            if (row[10].ToString() != "" && row[10] != null)
                            {
                                job.TotalPaid = decimal.Parse(row[10].ToString());
                            }
                            else
                            {
                                job.TotalPaid = 0;
                            }

                            if (row[11].ToString() != "" && row[11] != null)
                            {
                                job.TotalUnPaid = decimal.Parse(row[11].ToString());
                            }
                            else
                            {
                                job.TotalUnPaid = 0;
                            }

                            if (row[12].ToString() != "" && row[12] != null)
                            {
                                job.TravelLogCostTotal = decimal.Parse(row[12].ToString());
                            }
                            else
                            {
                                job.TravelLogCostTotal = 0;
                            }
                        }
                    }
                }
                return job;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }

        public List<Job> getProfileJobs(Profile profile)
        {
            List<Job> Jobs = new List<Job>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID)
                        //***************************************//
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_GetProfileJobs",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            Job job = new Model.Job();
                            job.JobID = int.Parse(row["JobID"].ToString());
                            job.ClientID = int.Parse(row["ClientID"].ToString());
                            job.JobTitle = row["JobTitle"].ToString();
                            job.StartDate = DateTime.Parse(row["StartDate"].ToString());
                            if (row["EndDate"].ToString() != "" && row["EndDate"] != null)
                            {
                                job.EndDate = DateTime.Parse(row["EndDate"].ToString());
                            }
                            job.HourlyRate = decimal.Parse(row["HourlyRate"].ToString());
                            job.Budget = decimal.Parse(row["Budget"].ToString());
                            Jobs.Add(job);
                        }
                    }
                }
                return Jobs;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }

        #region WorkLog Item
        public bool newWorkLogItem(Model.Worklog logItem, Model.Job job)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@JID", job.JobID),
                        new SqlParameter("@D", logItem.Description),
                        new SqlParameter("@ST", logItem.StartTime),
                        new SqlParameter("@ET", logItem.EndTime)
                   };

                Result = DBHelper.NonQuery("SP_NewWorkLogItem", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }

        public Worklog getLogItem(Model.Worklog logID)
        {
            Worklog logItem = null;

            SqlParameter[] pars = new SqlParameter[]
                {
                        new SqlParameter("@LogID", logID.LogItemID)
                };

            try
            {
                using (DataTable table = DBHelper.ParamSelect("SP_GetLogItem",
            CommandType.StoredProcedure, pars))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row != null)
                        {
                            logItem = new Worklog();
                            logItem.LogItemID = int.Parse(row[0].ToString());
                            logItem.Description = row[1].ToString();
                            logItem.StartTime = DateTime.Parse(row[2].ToString());
                            logItem.EndTime = DateTime.Parse(row[3].ToString());
                        }
                    }
                }
                return logItem;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public List<Worklog> getJobHours(Job JobID)
        {
            List<Worklog> JobWorkLog = new List<Worklog>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@JobID", JobID.JobID)
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_GetJobHours",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            Worklog logItem = new Worklog();
                            logItem.LogItemID = int.Parse(row[0].ToString());
                            logItem.Description = row[1].ToString();
                            logItem.StartTime = DateTime.Parse(row[2].ToString());
                            logItem.EndTime = DateTime.Parse(row[3].ToString());
                            JobWorkLog.Add(logItem);
                        }
                    }
                }
                return JobWorkLog;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        #endregion
        #endregion

        #region Client
        public bool newClient(Client client)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@FN", client.FirstName),
                        new SqlParameter("@LN", client.LastName),
                        new SqlParameter("@CN", client.CompanyName),
                        new SqlParameter("@CNum", client.ContactNumber),
                        new SqlParameter("@EA", client.EmailAddress),
                        new SqlParameter("@PA", client.PhysiclaAddress),
                        new SqlParameter("@PC", client.PreferedCommunicationChannel),
                        new SqlParameter("@PI", client.ProfileID),
                   };

                Result = DBHelper.NonQuery("SP_NewClient", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }

        public Client getClient(Client client)
        {
            Model.Client Client = null;

            SqlParameter[] pars = new SqlParameter[]
                {
                        new SqlParameter("@CID", client.ClientID)
                };

            try
            {
                using (DataTable table = DBHelper.ParamSelect("SP_GetClient",
            CommandType.StoredProcedure, pars))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row != null)
                        {
                            Client = new Model.Client();
                            client.FirstName = row[1].ToString();
                            client.LastName = row[2].ToString();
                            client.CompanyName = row[3].ToString();
                            client.ContactNumber = row[4].ToString();
                            client.EmailAddress = row[5].ToString();
                            client.PreferedCommunicationChannel = row[7].ToString();
                            client.PhysiclaAddress = row[6].ToString();
                            client.ProfileID = int.Parse(row[8].ToString());
                            client.ClientID = int.Parse(row[0].ToString());
                        }
                    }
                }
                return client;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }

        public List<Client> getProfileClients(Client client)
        {
            List<Client> Clients = new List<Client>();
            try
            {
            SqlParameter[] pars = new SqlParameter[]
                {
                        new SqlParameter("@PID", client.ProfileID)
                };

            
                using (DataTable table = DBHelper.ParamSelect("SP_GetProfileClients",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            Model.Client NewClient = new Model.Client();
                            NewClient.ClientID = int.Parse(row[0].ToString());
                            NewClient.FirstName = row[1].ToString();
                                NewClient.LastName = row[2].ToString();
                                NewClient.CompanyName = row[3].ToString();
                                NewClient.ContactNumber = row[4].ToString();
                                NewClient.EmailAddress = row[5].ToString();
                                NewClient.PreferedCommunicationChannel = row[7].ToString();
                                NewClient.PhysiclaAddress = row[6].ToString();
                            NewClient.ProfileID = int.Parse(row[8].ToString());
                            Clients.Add(NewClient);
                        }
                    }
                }
                return Clients;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        #endregion

        #region Expense
        public bool newJobExpense(SP_GetJobExpense_Result newJobExpense)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@CID", newJobExpense.CategoryID),
                        new SqlParameter("@N", newJobExpense.Name),
                        new SqlParameter("@D", newJobExpense.Description),
                        new SqlParameter("@JID", newJobExpense.JobID),
                        new SqlParameter("@Date", newJobExpense.Date),
                        new SqlParameter("@A", newJobExpense.Amount),
                        //new SqlParameter("@IRC", newJobExpense.Invoice_ReceiptCopy),
                   };

                Result = DBHelper.NonQuery("SP_NewJobExpense", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        public SP_GetJobExpense_Result getJobExpense(Expense expenseID)
        {
            SP_GetJobExpense_Result expense = null;

            SqlParameter[] pars = new SqlParameter[]
                {
                        new SqlParameter("@EID", expenseID.ExpenseID)
                };

            try
            {
                using (DataTable table = DBHelper.ParamSelect("SP_GetJobExpense",
            CommandType.StoredProcedure, pars))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row != null)
                        {
                            expense = new SP_GetJobExpense_Result();
                            expense.ExpenseID = int.Parse(row[0].ToString());
                            expense.CategoryID = int.Parse(row[1].ToString());
                            expense.Name = row[2].ToString();
                            expense.Description = row[3].ToString();
                            expense.JobID = int.Parse(row[4].ToString());
                            expense.Date = DateTime.Parse(row[5].ToString());
                            expense.Amount = decimal.Parse(row[6].ToString());
                            //expense.Invoice_ReceiptCopy = row[7].ToString();
                            expense.CatName = row[8].ToString();
                            expense.CatDescription = row[9].ToString();
                        }
                    }
                }
                return expense;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public List<SP_GetJobExpense_Result> getJobExpenses(Job jobID)
        {
            List<SP_GetJobExpense_Result> Expenses = new List<SP_GetJobExpense_Result>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@JID", jobID.JobID)
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_GetProfileClients",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJobExpense_Result expense = new SP_GetJobExpense_Result();
                            expense.ExpenseID = int.Parse(row[0].ToString());
                            expense.CategoryID = int.Parse(row[1].ToString());
                            expense.Name = row[2].ToString();
                            expense.Description = row[3].ToString();
                            expense.JobID = int.Parse(row[4].ToString());
                            expense.Date = DateTime.Parse(row[5].ToString());
                            expense.Amount = decimal.Parse(row[6].ToString());
                            //expense.Invoice_ReceiptCopy = row[7].ToString();
                            expense.CatName = row[8].ToString();
                            expense.CatDescription = row[9].ToString();
                            Expenses.Add(expense);
                        }
                    }
                }
                return Expenses;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public bool newGeneralExpense(SP_GetGeneralExpense_Result newGeneralExpense)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@CID", newGeneralExpense.CategoryID),
                        new SqlParameter("@N", newGeneralExpense.Name),
                        new SqlParameter("@D", newGeneralExpense.Description),
                        new SqlParameter("@PID", newGeneralExpense.ProfileID),
                        new SqlParameter("@Date", newGeneralExpense.Date),
                        new SqlParameter("@A", newGeneralExpense.Amount),
                        new SqlParameter("@R", newGeneralExpense.Repeat),
                        //new SqlParameter("@IRC", newGeneralExpense.Invoice_ReceiptCopy),
                   };

                Result = DBHelper.NonQuery("SP_NewGeneralExpense", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        public SP_GetGeneralExpense_Result getGeneralExpense(Expense expenseID)
        {
            SP_GetGeneralExpense_Result expense = null;

            SqlParameter[] pars = new SqlParameter[]
                {
                        new SqlParameter("@EID", expenseID.ExpenseID)
                };

            try
            {
                using (DataTable table = DBHelper.ParamSelect("SP_GetGeneralExpense",
            CommandType.StoredProcedure, pars))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row != null)
                        {
                            expense = new SP_GetGeneralExpense_Result();
                            expense.ExpenseID = int.Parse(row[0].ToString());
                            expense.CategoryID = int.Parse(row[1].ToString());
                            expense.Name = row[2].ToString();
                            expense.Description = row[3].ToString();
                            expense.ProfileID = int.Parse(row[4].ToString());
                            expense.Date = DateTime.Parse(row[5].ToString());
                            expense.Amount = decimal.Parse(row[6].ToString());
                            expense.Repeat = bool.Parse(row[7].ToString());
                            //expense.Invoice_ReceiptCopy = row[8].ToString();
                            expense.CatName = row[9].ToString();
                            expense.CatDescription = row[10].ToString();
                        }
                    }
                }
                return expense;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public List<SP_GetGeneralExpense_Result> getGeneralExpenses(Profile profileID)
        {
            List<SP_GetGeneralExpense_Result> Expenses = new List<SP_GetGeneralExpense_Result>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profileID.ProfileID)
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_GetProfileClients",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetGeneralExpense_Result expense = new SP_GetGeneralExpense_Result();
                            expense.ExpenseID = int.Parse(row[0].ToString());
                            expense.CategoryID = int.Parse(row[1].ToString());
                            expense.Name = row[2].ToString();
                            expense.Description = row[3].ToString();
                            expense.ProfileID = int.Parse(row[4].ToString());
                            expense.Date = DateTime.Parse(row[5].ToString());
                            expense.Amount = decimal.Parse(row[6].ToString());
                            expense.Repeat = bool.Parse(row[7].ToString());
                            //expense.Invoice_ReceiptCopy = row[8].ToString();
                            expense.CatName = row[9].ToString();
                            expense.CatDescription = row[10].ToString();
                            Expenses.Add(expense);
                        }
                    }
                }
                return Expenses;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public List<ExpenseCategory> getExpenseCatagories()
        {
            List<ExpenseCategory> ExpensesCats = new List<ExpenseCategory>();
            try
            {
                using (DataTable table = DBHelper.Select("SP_GetExpenseCategorys",
            CommandType.StoredProcedure))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            ExpenseCategory cat = new ExpenseCategory();
                            cat.CategoryID = int.Parse(row["CategoryID"].ToString());
                            cat.Name = row["Name"].ToString();
                            cat.Description = row["Description"].ToString();
                            ExpensesCats.Add(cat);
                        }
                    }
                }
                return ExpensesCats;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        #endregion
    }
}                  
