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
        #region Dashboard Details
        public DashboardIncomeExpense getDashboardIncomeExpense(Profile profile)
        {
            DashboardIncomeExpense dashboardIncomeExpense = null;
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID)
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_DashboardIncomeExpense",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            dashboardIncomeExpense = new Model.DashboardIncomeExpense();

                            if (row["TotalIncomePast30Days"].ToString() != "")
                            {
                                dashboardIncomeExpense.IncomePast30Days = decimal.Parse(row["TotalIncomePast30Days"].ToString());
                            }
                            else
                            {
                                dashboardIncomeExpense.IncomePast30Days = 0;
                            }

                            if (row["TotalIncomePast60To30Days"].ToString() != "")
                            {
                                dashboardIncomeExpense.IncomePast60to30DaysPercent = decimal.Parse(row["TotalIncomePast60To30Days"].ToString());
                            }
                            else
                            {
                                dashboardIncomeExpense.IncomePast60to30DaysPercent = 0;
                            }

                            if (dashboardIncomeExpense.IncomePast30Days == 0
                                && dashboardIncomeExpense.IncomePast60to30DaysPercent == 0)
                            {
                                dashboardIncomeExpense.IncomePast60to30DaysUporDown = 'U';
                            }
                            else if (dashboardIncomeExpense.IncomePast30Days > dashboardIncomeExpense.IncomePast60to30DaysPercent
                                && dashboardIncomeExpense.IncomePast60to30DaysPercent != 0)
                            {
                                dashboardIncomeExpense.IncomePast60to30DaysUporDown = 'U';
                                dashboardIncomeExpense.IncomePast60to30DaysPercent =
                                    (dashboardIncomeExpense.IncomePast30Days / dashboardIncomeExpense.IncomePast60to30DaysPercent) * 100;
                            }
                            else if (dashboardIncomeExpense.IncomePast30Days < dashboardIncomeExpense.IncomePast60to30DaysPercent)
                            {
                                dashboardIncomeExpense.IncomePast60to30DaysUporDown = 'D';
                                dashboardIncomeExpense.IncomePast60to30DaysPercent =
                                    (dashboardIncomeExpense.IncomePast60to30DaysPercent / dashboardIncomeExpense.IncomePast30Days) * 100;
                            }
                            else
                            {
                                dashboardIncomeExpense.IncomePast60to30DaysUporDown = 'U';
                                dashboardIncomeExpense.IncomePast60to30DaysPercent = -999999999;
                            }


                            if (row["TotalExpensePast30Days"].ToString() != "")
                            {
                                dashboardIncomeExpense.ExpensePast30Days = decimal.Parse(row["TotalExpensePast30Days"].ToString());
                            }
                            else
                            {
                                dashboardIncomeExpense.ExpensePast30Days = 0;
                            }

                            if (row["TotalExpensePast60To30Days"].ToString() != "")
                            {
                                dashboardIncomeExpense.ExpensePast60to30DaysPercent = decimal.Parse(row["TotalExpensePast60To30Days"].ToString());
                            }
                            else
                            {
                                dashboardIncomeExpense.ExpensePast60to30DaysPercent = 0;
                            }

                            if(dashboardIncomeExpense.ExpensePast30Days ==0 
                                && dashboardIncomeExpense.ExpensePast60to30DaysPercent ==0)
                                {
                                dashboardIncomeExpense.ExpensePast60to30DaysUporDown = 'U';
                            }
                        else if(dashboardIncomeExpense.ExpensePast30Days > dashboardIncomeExpense.ExpensePast60to30DaysPercent
                                && dashboardIncomeExpense.ExpensePast60to30DaysPercent != 0)
                            {
                                dashboardIncomeExpense.ExpensePast60to30DaysUporDown = 'U';
                                dashboardIncomeExpense.ExpensePast60to30DaysPercent = 
                                    (dashboardIncomeExpense.ExpensePast30Days / dashboardIncomeExpense.ExpensePast60to30DaysPercent) * 100;
                            }
                            else if (dashboardIncomeExpense.ExpensePast30Days < dashboardIncomeExpense.ExpensePast60to30DaysPercent)
                            {
                                dashboardIncomeExpense.ExpensePast60to30DaysUporDown = 'D';
                                dashboardIncomeExpense.ExpensePast60to30DaysPercent =
                                    (dashboardIncomeExpense.ExpensePast60to30DaysPercent / dashboardIncomeExpense.ExpensePast60to30DaysPercent)*100;
                            }
                            else
                            {
                                dashboardIncomeExpense.ExpensePast60to30DaysUporDown = 'U';
                                dashboardIncomeExpense.ExpensePast60to30DaysPercent = -999999999;
                                    }

                            dashboardIncomeExpense.IncomePast30DaysString = dashboardIncomeExpense.IncomePast30Days.ToString("0.##"); ;
                            dashboardIncomeExpense.ExpensePast30DaysString = dashboardIncomeExpense.ExpensePast30Days.ToString("0.##"); ;
                        }
                    }
                }
                return dashboardIncomeExpense;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        #endregion

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
                                job.WorkLogHours = 0;
                                job.WorkLogHoursString = "None";
                            }

                            job.JobTitle = row[1].ToString();
                            job.ClientFirstName = row[7].ToString();
                            job.StartDate = DateTime.Parse(row[4].ToString());
                            job.StartDateString = String.Format("{0:dddd, dd MMMM yyyy}", job.StartDate);

                            if (row["EndDate"].ToString() != "" && row["EndDate"] != null)
                            {
                                job.EndDate = DateTime.Parse(row["EndDate"].ToString());
                                job.EndDateString = String.Format("{0:dddd, dd MMMM yyyy}", job.EndDate);
                                
                            }
                            else
                            {
                                job.EndDateString = "Active";
                            }

                            job.HourlyRate = decimal.Parse(row[2].ToString());

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

                            if (row["TotalUnPaid"].ToString() != "" && row["TotalUnPaid"] != null)
                            {
                                job.TotalUnPaid = decimal.Parse(row["TotalUnPaid"].ToString());
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

                            if (row[3].ToString() != "" || row[3].ToString() != null)
                            {
                                job.Budget = decimal.Parse(row[3].ToString());
                                job.BudgetPercent = ((job.ExpenseTotal + job.TravelLogCostTotal + 
                                    (job.WorkLogHours * job.HourlyRate)) / job.Budget) *100;
                            }
                            else
                            {
                                job.Budget = 0;
                                job.BudgetPercent = 0;
                            }

                            job.AllExpenseTotal = job.ExpenseTotal + job.TravelLogCostTotal;
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

        public List<SP_GetJob_Result> getProfileJobs(Profile profile)
        {
            List<SP_GetJob_Result> Jobs = new List<SP_GetJob_Result>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID)
                        //***************************************//
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_GetProfileJobsCurrent",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJob_Result job = new Model.SP_GetJob_Result();
                            job.JobID = int.Parse(row[0].ToString());
                            job.ClientID = int.Parse(row[6].ToString());

                            if (row[8].ToString() != "" && row[8] != null)
                            {
                                job.WorkLogHours = int.Parse(row[8].ToString());
                                int Hour = int.Parse(row[8].ToString()) / 60;
                                int Minute = int.Parse(row[8].ToString()) % 60;
                                job.WorkLogHoursString = Hour + ":" + Minute + " ";
                            }
                            else
                            {
                                job.WorkLogHours = 0;
                                job.WorkLogHoursString = "None";
                            }

                            job.JobTitle = row[1].ToString();
                            job.ClientFirstName = row[7].ToString();
                            job.StartDate = DateTime.Parse(row[4].ToString());
                            job.StartDateString = String.Format("{0:dddd, dd MMMM yyyy}", job.StartDate);

                            if (row["EndDate"].ToString() != "" && row["EndDate"] != null)
                            {
                                job.EndDate = DateTime.Parse(row["EndDate"].ToString());
                                job.EndDateString = String.Format("{0:dddd, dd MMMM yyyy}", job.EndDate);

                            }
                            else
                            {
                                job.EndDateString = "Active";
                            }

                            job.HourlyRate = decimal.Parse(row[2].ToString());

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
                            Jobs.Add(job);

                            if (row[3].ToString() != "" || row[3].ToString() != null)
                            {
                                job.Budget = decimal.Parse(row[3].ToString());
                                job.BudgetPercent = ((job.ExpenseTotal + job.TravelLogCostTotal +
                                    (job.WorkLogHours * job.HourlyRate)) / job.Budget) * 100;
                            }
                            else
                            {
                                job.Budget = 0;
                                job.BudgetPercent = 0;
                            }

                            job.AllExpenseTotal = job.ExpenseTotal + job.TravelLogCostTotal;
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
        public List<SP_GetJob_Result> getProfileJobsPast(Profile profile)
        {
            List<SP_GetJob_Result> Jobs = new List<SP_GetJob_Result>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID)
                        //***************************************//
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_GetProfileJobsPast",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJob_Result job = new Model.SP_GetJob_Result();
                            job.JobID = int.Parse(row[0].ToString());
                            job.ClientID = int.Parse(row[6].ToString());

                            if (row[8].ToString() != "" && row[8] != null)
                            {
                                job.WorkLogHours = int.Parse(row[8].ToString());
                                int Hour = int.Parse(row[8].ToString()) / 60;
                                int Minute = int.Parse(row[8].ToString()) % 60;
                                job.WorkLogHoursString = Hour + ":" + Minute + " ";
                            }
                            else
                            {
                                job.WorkLogHours = 0;
                                job.WorkLogHoursString = "None";
                            }

                            job.JobTitle = row[1].ToString();
                            job.ClientFirstName = row[7].ToString();
                            job.StartDate = DateTime.Parse(row[4].ToString());
                            job.StartDateString = String.Format("{0:dddd, dd MMMM yyyy}", job.StartDate);

                            if (row["EndDate"].ToString() != "" && row["EndDate"] != null)
                            {
                                job.EndDate = DateTime.Parse(row["EndDate"].ToString());
                                job.EndDateString = String.Format("{0:dddd, dd MMMM yyyy}", job.EndDate);

                            }
                            else
                            {
                                job.EndDateString = "Active";
                            }

                            job.HourlyRate = decimal.Parse(row[2].ToString());

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
                            Jobs.Add(job);

                            if (row[3].ToString() != "" || row[3].ToString() != null)
                            {
                                job.Budget = decimal.Parse(row[3].ToString());
                                job.BudgetPercent = ((job.ExpenseTotal + job.TravelLogCostTotal +
                                    (job.WorkLogHours * job.HourlyRate)) / job.Budget) * 100;
                            }
                            else
                            {
                                job.Budget = 0;
                                job.BudgetPercent = 0;
                            }

                            job.AllExpenseTotal = job.ExpenseTotal + job.TravelLogCostTotal;
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
        public List<SP_GetJob_Result> getProfileJobsDashboard(Profile profile)
        {
            List<SP_GetJob_Result> Jobs = new List<SP_GetJob_Result>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID)
                        //***************************************//
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_GetProfileJobsDashboard",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJob_Result job = new Model.SP_GetJob_Result();
                            job.JobID = int.Parse(row[0].ToString());
                            job.ClientID = int.Parse(row[6].ToString());

                            if (row[8].ToString() != "" && row[8] != null)
                            {
                                job.WorkLogHours = int.Parse(row[8].ToString());
                                int Hour = int.Parse(row[8].ToString()) / 60;
                                int Minute = int.Parse(row[8].ToString()) % 60;
                                job.WorkLogHoursString = Hour + ":" + Minute + " ";
                            }
                            else
                            {

                                job.WorkLogHours = 0;
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

                            if (row[9].ToString() != "" && row[9] != null)
                            {
                                job.ExpenseTotal = decimal.Parse(row[9].ToString());
                            }
                            else
                            {
                                job.ExpenseTotal = 0;
                            }

                            if (row["TotalPaid"].ToString() != "" && row["TotalPaid"] != null)
                            {
                                job.TotalPaid = decimal.Parse(row["TotalPaid"].ToString());
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
                            Jobs.Add(job);

                            if (row[3].ToString() != "" || row[3].ToString() != null)
                            {
                                job.Budget = decimal.Parse(row[3].ToString());
                                job.BudgetPercent = ((job.ExpenseTotal + job.TravelLogCostTotal +
                                    (job.WorkLogHours * job.HourlyRate)) / job.Budget) * 100;
                            }
                            else
                            {
                                job.Budget = 0;
                                job.BudgetPercent = 0;
                            }

                            job.AllExpenseTotal = job.ExpenseTotal + job.TravelLogCostTotal;
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
        public bool MarkJobAsComplete(Job job)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@JID", job.JobID)
                   };

                Result = DBHelper.NonQuery("SP_MarkJobAsComplete", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
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
                            logItem.StartTimeString = logItem.StartTime.ToString("hh:mm tt");
                            logItem.EndTime = DateTime.Parse(row[3].ToString());
                            logItem.EndTimeString = logItem.EndTime.Value.ToString("hh:mm tt");
                            logItem.DateString = logItem.StartTime.ToString("dddd, dd MMMM yyyy");
                            int Hour = int.Parse(row["WorkLogHours"].ToString()) / 60;
                            int Minute = int.Parse(row["WorkLogHours"].ToString()) % 60;
                            logItem.WorkLogHoursString = Hour + ":" + Minute + " ";
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
                            logItem.DateString = logItem.StartTime.ToString("dddd, dd MMMM yyyy");
                            int Hour = int.Parse(row["WorkLogHours"].ToString()) / 60;
                            int Minute = int.Parse(row["WorkLogHours"].ToString()) % 60;
                            logItem.WorkLogHoursString = Hour + ":" + Minute + " ";
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
                            if(expense.Description == "" || expense.Description == null)
                            {
                                expense.Description = "None";
                            }
                            expense.JobID = int.Parse(row[4].ToString());
                            expense.Date = DateTime.Parse(row[5].ToString());
                            expense.DateString = expense.Date.ToString("dddd, dd MMMM yyyy");
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


                using (DataTable table = DBHelper.ParamSelect("SP_GetJobExpenses",
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
                            if (expense.Description == "" || expense.Description == null)
                            {
                                expense.Description = "None";
                            }
                            expense.JobID = int.Parse(row[4].ToString());
                            expense.Date = DateTime.Parse(row[5].ToString());
                            expense.DateString = expense.Date.ToString("dddd, dd MMMM yyyy");
                            expense.Amount = decimal.Parse(row[6].ToString());
                            //expense.Invoice_ReceiptCopy = row[7].ToString();
                            expense.CatName = row[8].ToString();
                            expense.CatDescription = row[9].ToString();
                            expense.JobTitle = row["JobTitle"].ToString();
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
        public List<SP_GetJobExpense_Result> getAllJobExpense(Profile profileID)
        {
            List<SP_GetJobExpense_Result> Expenses = new List<SP_GetJobExpense_Result>();
            SqlParameter[] pars = new SqlParameter[]
                {
                        new SqlParameter("@PID", profileID.ProfileID)
                };

            try
            {
                using (DataTable table = DBHelper.ParamSelect("SP_GetJobExpensesAllProfile",
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
                            if (expense.Description == "" || expense.Description == null)
                            {
                                expense.Description = "None";
                            }
                            expense.JobID = int.Parse(row[4].ToString());
                            expense.Date = DateTime.Parse(row[5].ToString());
                            expense.DateString = expense.Date.ToString("dddd, dd MMMM yyyy");
                            expense.Amount = decimal.Parse(row[6].ToString());
                            //expense.Invoice_ReceiptCopy = row[7].ToString();
                            expense.CatName = row[8].ToString();
                            expense.CatDescription = row[9].ToString();
                            expense.JobTitle = row["JobTitle"].ToString();
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
                            expense.DateString = expense.Date.ToString("dddd, dd MMMM yyyy");
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


                using (DataTable table = DBHelper.ParamSelect("SP_GetGeneralExpenses",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetGeneralExpense_Result expense = new SP_GetGeneralExpense_Result();
                            expense.ExpenseID = int.Parse(row[0].ToString());
                            expense.CategoryID = int.Parse(row["CategoryID"].ToString());
                            expense.Name = row[2].ToString();
                            expense.Description = row[3].ToString();
                            expense.ProfileID = int.Parse(row[4].ToString());
                            expense.Date = DateTime.Parse(row[5].ToString());
                            expense.Amount = decimal.Parse(row[6].ToString());
                            expense.Repeat = bool.Parse(row[7].ToString());
                            //expense.Invoice_ReceiptCopy = row[8].ToString();
                            expense.CatName = row[9].ToString();
                            expense.CatDescription = row[10].ToString();
                            expense.DateString = expense.Date.ToString("dddd, dd MMMM yyyy");
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
        public bool NewTravelExpense(TravelLog newTravelLogExpense)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@F", newTravelLogExpense.From),
                        new SqlParameter("@T", newTravelLogExpense.To),
                        new SqlParameter("@R", newTravelLogExpense.Reason),
                        new SqlParameter("@OKM", newTravelLogExpense.OpeningKMs),
                        new SqlParameter("@CKM", newTravelLogExpense.ClosingKMs),
                        new SqlParameter("@VID", newTravelLogExpense.VehicleID),
                        new SqlParameter("@JID", newTravelLogExpense.JobID),
                        new SqlParameter("@D", newTravelLogExpense.Date),
                   };

                Result = DBHelper.NonQuery("SP_NewTravelExpense", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        public bool EditTravelExpense(TravelLog newTravelLogExpense)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@F", newTravelLogExpense.From),
                        new SqlParameter("@T", newTravelLogExpense.To),
                        new SqlParameter("@R", newTravelLogExpense.Reason),
                        new SqlParameter("@OKM", newTravelLogExpense.OpeningKMs),
                        new SqlParameter("@CKM", newTravelLogExpense.ClosingKMs),
                        new SqlParameter("@VID", newTravelLogExpense.VehicleID),
                        new SqlParameter("@D", newTravelLogExpense.Date),
                        new SqlParameter("@EID", newTravelLogExpense.ExpenseID),
                   };

                Result = DBHelper.NonQuery("SP_EditTravelExpense", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        public bool DeleteTravelExpense(TravelLog TravelLogExpense)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@EID", TravelLogExpense.ExpenseID)
                   };

                Result = DBHelper.NonQuery("SP_DeleteTravelLogItem", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        public bool newVehicle(Vehicle newVehicle)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@N", newVehicle.Name),
                        new SqlParameter("@FC", newVehicle.SARSFuelCost),
                        new SqlParameter("@CC", newVehicle.ClientCharge),
                        new SqlParameter("@MC", newVehicle.SARSMaintenceCost),
                        new SqlParameter("@FxC", newVehicle.SARSFixedCost),
                        new SqlParameter("@PID", newVehicle.ProfielID),
                   };

                Result = DBHelper.NonQuery("SP_NewVehicle", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        public List<Vehicle> getVehicles(Profile getProfileVehicles)
        {
            List<Vehicle> Vehicles = new List<Vehicle>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@PID", getProfileVehicles.ProfileID)
                   };

                using (DataTable table = DBHelper.ParamSelect("SP_GetVehicles",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            Vehicle Vehicle = new Vehicle();
                            Vehicle.VehicleID = int.Parse(row["VehicleID"].ToString());
                            Vehicle.ProfielID = int.Parse(row["ProfileID"].ToString());
                            Vehicle.SARSFixedCost = decimal.Parse(row["SARSFixedCost"].ToString());
                            Vehicle.SARSFuelCost = decimal.Parse(row["SARSFuelCost"].ToString());
                            Vehicle.SARSMaintenceCost = decimal.Parse(row["SARSMaintenceCost"].ToString());
                            Vehicle.ClientCharge = decimal.Parse(row["ClientCharge"].ToString());
                            Vehicle.Name = row["Name"].ToString();
                            Vehicles.Add(Vehicle);
                        }
                    }
                }
                return Vehicles;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public List<TravelLog> getProfileTravelLog(Profile getProfileTravelLog)
        {
            List<TravelLog> TravelLog = new List<TravelLog>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@PID", getProfileTravelLog.ProfileID)
                   };

                using (DataTable table = DBHelper.ParamSelect("SP_GetTravleLog",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            TravelLog travelLogItem = new TravelLog();
                            travelLogItem.JobID = int.Parse(row["JobID"].ToString());
                            travelLogItem.ExpenseID = int.Parse(row["ExpenseID"].ToString());
                            travelLogItem.VehicleID = int.Parse(row["VehicleID"].ToString());
                            travelLogItem.From = row["From"].ToString();
                            travelLogItem.To = row["To"].ToString();
                            travelLogItem.Reason = row["Reason"].ToString();
                            travelLogItem.OpeningKMs = double.Parse(row["OpeningKMs"].ToString());
                            travelLogItem.ClosingKMs = double.Parse(row["ClosingKMs"].ToString());
                            travelLogItem.TotalKMs = double.Parse(row["TotalKMs"].ToString());
                            travelLogItem.SARSFuelCost = decimal.Parse(row["SARSFuelCost"].ToString());
                            travelLogItem.SARSMaintenceCost = decimal.Parse(row["SARSMaintenceCost"].ToString());
                            travelLogItem.ClientCharge = decimal.Parse(row["ClientCharge"].ToString());
                            travelLogItem.Date = DateTime.Parse(row["Date"].ToString());
                            travelLogItem.DateString = travelLogItem.Date.ToString("dddd, dd MMMM yyyy");
                            travelLogItem.Invoiced = bool.Parse(row["Invoiced"].ToString());
                            TravelLog.Add(travelLogItem);
                        }
                    }
                }
                return TravelLog;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public TravelLog getTravelLogItem(TravelLog getTravelLogItem)
        {
                            TravelLog travelLogItem = null;
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@EID", getTravelLogItem.ExpenseID)
                   };

                using (DataTable table = DBHelper.ParamSelect("SP_GetTravleLogItem",
            CommandType.StoredProcedure, pars))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row != null)
                        {
                            travelLogItem = new TravelLog();
                            travelLogItem.JobID = int.Parse(row["JobID"].ToString());
                            travelLogItem.ExpenseID = int.Parse(row["ExpenseID"].ToString());
                            travelLogItem.VehicleID = int.Parse(row["VehicleID"].ToString());
                            travelLogItem.From = row["From"].ToString();
                            travelLogItem.FromGoogleURL = travelLogItem.From.Replace(" ", "%2C+'");
                            travelLogItem.To = row["To"].ToString();
                            travelLogItem.ToGoogleURL = travelLogItem.To.Replace(" ", "%2C+'");
                            travelLogItem.ToGoogleURL = travelLogItem.ToGoogleURL.Replace(",", "");
                            travelLogItem.Reason = row["Reason"].ToString();
                            travelLogItem.OpeningKMs = double.Parse(row["OpeningKMs"].ToString());
                            travelLogItem.ClosingKMs = double.Parse(row["ClosingKMs"].ToString());
                            travelLogItem.Invoiced = bool.Parse(row["Invoiced"].ToString());
                            travelLogItem.Date = DateTime.Parse(row["Date"].ToString());
                            travelLogItem.DateString = travelLogItem.Date.ToString("dddd, dd MMMM yyyy");
                            travelLogItem.TotalKMs = double.Parse(row["TotalKMs"].ToString());
                            travelLogItem.SARSFuelCost = decimal.Parse(row["SARSFuelCost"].ToString());
                            travelLogItem.SARSMaintenceCost = decimal.Parse(row["SARSMaintenceCost"].ToString());
                            travelLogItem.ClientCharge = decimal.Parse(row["ClientCharge"].ToString());
                            travelLogItem.VehicleName = row["Name"].ToString();
                            travelLogItem.JobTitle = row["JobTitle"].ToString();
                        }
                    }
                }
                return travelLogItem;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public List<TravelLog> getJobTravelLog(Job getJobTravelLog)
        {
            List<TravelLog> TravelLog = new List<TravelLog>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@JID", getJobTravelLog.JobID)
                   };

                using (DataTable table = DBHelper.ParamSelect("SP_GetJobTravleLog",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            TravelLog travelLogItem = new TravelLog();
                            travelLogItem.JobID = int.Parse(row["JobID"].ToString());
                            travelLogItem.ExpenseID = int.Parse(row["ExpenseID"].ToString());
                            travelLogItem.VehicleID = int.Parse(row["VehicleID"].ToString());
                            travelLogItem.From = row["From"].ToString();
                            travelLogItem.To = row["To"].ToString();
                            travelLogItem.Reason = row["Reason"].ToString();
                            travelLogItem.OpeningKMs = double.Parse(row["OpeningKMs"].ToString());
                            travelLogItem.ClosingKMs = double.Parse(row["ClosingKMs"].ToString());
                            travelLogItem.Invoiced = bool.Parse(row["Invoiced"].ToString());
                            travelLogItem.Date = DateTime.Parse(row["Date"].ToString());
                            travelLogItem.DateString = travelLogItem.Date.ToString("dddd, dd MMMM yyyy");
                            travelLogItem.TotalKMs = double.Parse(row["TotalKMs"].ToString());
                            travelLogItem.SARSFuelCost = decimal.Parse(row["SARSFuelCost"].ToString());
                            travelLogItem.SARSMaintenceCost = decimal.Parse(row["SARSMaintenceCost"].ToString());
                            TravelLog.Add(travelLogItem);
                        }
                    }
                }
                return TravelLog;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        #endregion

        #region Invoice
        public int getInvoiceTodaysCount()
        {
            int todaysCount = -1;

            try
            {
                using (DataTable table = DBHelper.Select("SP_GetInvoiceCount",
            CommandType.StoredProcedure))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row != null)
                        {
                            todaysCount = new int();
                            todaysCount = int.Parse(row[0].ToString());
                        }
                    }
                }
                return todaysCount;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public List<List<SP_GetJobIntemsToInvoice_Result>> getJobItemsForInvoice(Job jobID)
        {
            List<SP_GetJobIntemsToInvoice_Result> Hours = new List<SP_GetJobIntemsToInvoice_Result>();
            List<SP_GetJobIntemsToInvoice_Result> Travels = new List<SP_GetJobIntemsToInvoice_Result>();
            List<SP_GetJobIntemsToInvoice_Result> Expenses = new List<SP_GetJobIntemsToInvoice_Result>();

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@JID", jobID.JobID)
                    };

                SqlParameter[] pars2 = new SqlParameter[]
                    {
                        new SqlParameter("@JID", jobID.JobID)
                    };

                SqlParameter[] pars3 = new SqlParameter[]
                    {
                        new SqlParameter("@JID", jobID.JobID)
                    };


                using (
                    DataTable table = DBHelper.ParamSelect("SP_GetJobHoursForInvoice",
            CommandType.StoredProcedure, pars))
                {
                        if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJobIntemsToInvoice_Result Hour = new SP_GetJobIntemsToInvoice_Result();
                            Hour.ID = int.Parse(row[0].ToString());
                            Hour.Description = row[1].ToString();
                            Hour.UnitCost = decimal.Parse(row[2].ToString());
                            Hour.UnitCount = decimal.Parse(row[3].ToString());
                            int WorkHours = int.Parse(row[3].ToString()) / 60;
                            int Minutes = int.Parse(row[3].ToString()) % 60;
                            Hour.DisplayString = WorkHours + ":" + Minutes + "h of " 
                                + Hour.Description +" at R"+ Hour.UnitCost.ToString("0.##") + " per Hour";
                            Hours.Add(Hour);
                        }
                    }
                }

                using (DataTable table = DBHelper.ParamSelect("SP_GetJobTravelForInvoice",
            CommandType.StoredProcedure, pars2))
                {
                        if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJobIntemsToInvoice_Result Travel = new SP_GetJobIntemsToInvoice_Result();
                            Travel.ID = int.Parse(row[0].ToString());
                            Travel.Description = row[1].ToString();
                            Travel.UnitCost = decimal.Parse(row[3].ToString());
                            Travel.UnitCount = decimal.Parse(row[2].ToString());
                            Travel.DisplayString = Travel.Description + " - "+ Travel.UnitCount.ToString("0.##") + "KM at R" 
                                + Travel.UnitCost.ToString("0.##") + " Per KM";
                            Travels.Add(Travel);
                        }
                    }
                }

                using (DataTable table = DBHelper.ParamSelect("SP_GetJobExpenesForInvoice",
            CommandType.StoredProcedure, pars3))
                {
                        if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJobIntemsToInvoice_Result Expense = new SP_GetJobIntemsToInvoice_Result();
                            Expense.ID = int.Parse(row[0].ToString());
                            Expense.Description = row[1].ToString();
                            Expense.UnitCost = decimal.Parse(row[2].ToString());
                            Expense.UnitCount = 1;
                            Expense.DisplayString = Expense.UnitCount + "* "
                                + Expense.Description + " at R" + Expense.UnitCost.ToString("0.##") + " each";
                            Expenses.Add(Expense);
                        }
                    }
                }

                return new List<List<SP_GetJobIntemsToInvoice_Result>> { Hours, Travels, Expenses };
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public bool newInvoiceDetailLine(InvoiceLineItem newInvoiceLineItem)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@INum", newInvoiceLineItem.InvoiceNum),
                        new SqlParameter("@ID", newInvoiceLineItem.LineItemID),
                        new SqlParameter("@Name", newInvoiceLineItem.Name),
                        new SqlParameter("@UnitCount", newInvoiceLineItem.UnitCount),
                        new SqlParameter("@UnitCost", newInvoiceLineItem.UnitCost),
                        new SqlParameter("@T", newInvoiceLineItem.Type)
                   };

                Result = DBHelper.NonQuery("SP_NewInvoiceDetailLine", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        public bool newInvoice(Invoice newInvoice, Job jobID)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@JID", jobID.JobID),
                        new SqlParameter("@INum", newInvoice.InvoiceNum)
                   };

                Result = DBHelper.NonQuery("SP_NewInvoice", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        public List<SP_GetInvoice_Result> getJobInvoices(Job jobID)
        {
            List<SP_GetInvoice_Result> JobInvoices = new List<SP_GetInvoice_Result>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@JID", jobID.JobID)
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_GetJobInvoices",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetInvoice_Result Invoice = new SP_GetInvoice_Result();
                            Invoice.InvoiceNum = row[0].ToString();
                            Invoice.DateTime = DateTime.Parse(row[1].ToString());
                            Invoice.VATRate = decimal.Parse(row[2].ToString());
                            Invoice.Paid = bool.Parse(row[3].ToString());
                            Invoice.JobID = int.Parse(row[4].ToString());
                            Invoice.JobTitle = row[5].ToString();
                            Invoice.ClientID = int.Parse(row[6].ToString());
                            Invoice.ClientName = row[7].ToString();
                            Invoice.CompanyName = row[8].ToString();
                            Invoice.EmailAddress = row[9].ToString();
                            Invoice.PhysiclaAddress = row[10].ToString();
                            Invoice.TotalCost = decimal.Parse(row["TotalCost"].ToString());
                            Invoice.TotalCost = Invoice.TotalCost + ((Invoice.TotalCost / 100) * 15);
                            JobInvoices.Add(Invoice);
                        }
                    }
                }
                return JobInvoices;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public List<SP_GetInvoice_Result> getInvoices(Profile profileID)
        {
            List<SP_GetInvoice_Result> JobInvoices = new List<SP_GetInvoice_Result>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profileID.ProfileID)
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_GetInvoices",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetInvoice_Result Invoice = new SP_GetInvoice_Result();
                            Invoice.InvoiceNum = row[0].ToString();
                            Invoice.DateTime = DateTime.Parse(row[1].ToString());
                            Invoice.VATRate = decimal.Parse(row[2].ToString());
                            Invoice.Paid = bool.Parse(row[3].ToString());
                            Invoice.JobID = int.Parse(row[4].ToString());
                            Invoice.JobTitle = row[5].ToString();
                            Invoice.ClientID = int.Parse(row[6].ToString());
                            Invoice.ClientName = row[7].ToString();
                            Invoice.CompanyName = row[8].ToString();
                            Invoice.EmailAddress = row[9].ToString();
                            Invoice.PhysiclaAddress = row[10].ToString();
                            Invoice.TotalCost = decimal.Parse(row["TotalCost"].ToString());
                            Invoice.TotalCost = Invoice.TotalCost + ((Invoice.TotalCost / 100) * 15);
                            JobInvoices.Add(Invoice);
                        }
                    }
                }
                return JobInvoices;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public List<SP_GetInvoice_Result> getInvoiceDetails(Invoice invoiceNum)
        {
            List<SP_GetInvoice_Result> InvoiceLineItems = new List<SP_GetInvoice_Result>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@IN", invoiceNum.InvoiceNum)
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_GetInvoice",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetInvoice_Result InvoiceLineItem = new SP_GetInvoice_Result();
                            InvoiceLineItem.InvoiceNum = row["InvoiceNum"].ToString();
                            InvoiceLineItem.Type = row["Type"].ToString()[0];
                            InvoiceLineItem.DateTime = DateTime.Parse(row["DateTime"].ToString());
                            InvoiceLineItem.VATRate = decimal.Parse(row["VATRate"].ToString());
                            InvoiceLineItem.Paid = bool.Parse(row["Paid"].ToString());
                            InvoiceLineItem.LineItemID = int.Parse(row["LineItemID"].ToString());
                            InvoiceLineItem.Name = row["Name"].ToString();
                            InvoiceLineItem.UnitCount = decimal.Parse(row["UnitCount"].ToString());
                            InvoiceLineItem.UnitCountString = row["UnitCount"].ToString();
                            InvoiceLineItem.UnitCost = decimal.Parse(row["UnitCost"].ToString());
                            InvoiceLineItem.TotalCost = decimal.Parse(row["TotalCost"].ToString());
                            InvoiceLineItem.JobID = int.Parse(row["JobID"].ToString());
                            InvoiceLineItem.JobTitle = row["JobTitle"].ToString();
                            InvoiceLineItem.ClientID = int.Parse(row["ClientID"].ToString());
                            InvoiceLineItem.ClientName = row["ClientName"].ToString();
                            InvoiceLineItem.CompanyName = row["CompanyName"].ToString();
                            InvoiceLineItem.EmailAddress = row["EmailAddress"].ToString();
                            InvoiceLineItem.PhysiclaAddress = row["PhysiclaAddress"].ToString();
                            InvoiceLineItem.UnitCostString = InvoiceLineItem.UnitCost.ToString("#.##");
                            if(InvoiceLineItem.Type == 'H')
                            {
                                InvoiceLineItem.UnitCountString += " h";
                            }
                            else if (InvoiceLineItem.Type == 'T')
                            {
                                InvoiceLineItem.UnitCountString += " KMs";
                            }
                            InvoiceLineItem.VATRateString = InvoiceLineItem.VATRate.ToString("0.##");
                            InvoiceLineItem.TotalCostString = InvoiceLineItem.TotalCost.ToString("0.##");
                            InvoiceLineItems.Add(InvoiceLineItem);
                        }
                    }
                }
                return InvoiceLineItems;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public bool MarkInvoiceAsPaid(Invoice invoice)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@INum", invoice.InvoiceNum)
                   };

                Result = DBHelper.NonQuery("SP_MarkInvoiceAsPaid", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        #endregion
    }
}                  