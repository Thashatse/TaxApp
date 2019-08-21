using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Model;
using System.Globalization;
using System.IO;

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
                            else if (dashboardIncomeExpense.IncomePast30Days < dashboardIncomeExpense.IncomePast60to30DaysPercent
                                && dashboardIncomeExpense.IncomePast30Days !=0)
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

                            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                            nfi.NumberGroupSeparator = " ";
                            dashboardIncomeExpense.IncomePast30DaysString = dashboardIncomeExpense.IncomePast30Days.ToString("#,0.00", nfi);
                            dashboardIncomeExpense.ExpensePast30DaysString = dashboardIncomeExpense.ExpensePast30Days.ToString("#,0.00", nfi);
                            dashboardIncomeExpense.IncomePast60to30DaysPercentString = dashboardIncomeExpense.IncomePast60to30DaysPercent.ToString("#,0.00", nfi);
                            dashboardIncomeExpense.ExpensePast60to30DaysPercentString = dashboardIncomeExpense.ExpensePast60to30DaysPercent.ToString("#,0.00", nfi);
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
        public bool editprofile(Model.Profile User)
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
            new SqlParameter("@PID", User.ProfileID),
            new SqlParameter("@Pass", User.Password)
                   };
                
                Result = DBHelper.NonQuery("SP_EditProfile", CommandType.StoredProcedure, pars);

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
        public bool EditTaxConsultant(TaxConsultant consultant)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@N", consultant.Name),
                        new SqlParameter("@EA", consultant.EmailAddress),
                        new SqlParameter("@PID", consultant.ProfileID)
                   };

                Result = DBHelper.NonQuery("SP_EditTaxConsultant", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
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
        public string newJob(Job job)
        {
            string Result = "";

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@CI", job.ClientID),
                        new SqlParameter("@JT", job.JobTitle),
                        new SqlParameter("@HR", job.HourlyRate),
                        new SqlParameter("@B", job.Budget),
                        new SqlParameter("@SD",job.StartDate),
                        new SqlParameter("@S",job.Share)
                   };

                using (DataTable table = DBHelper.ParamSelect("SP_NewJob",
            CommandType.StoredProcedure, pars))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row != null)
                        {
                            Result = row[0].ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        public bool editJob(Job job)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@JT", job.JobTitle),
                        new SqlParameter("@HR", job.HourlyRate),
                        new SqlParameter("@B", job.Budget),
                        new SqlParameter("@JID",job.JobID),
                   };

                Result = DBHelper.NonQuery("SP_EditJob", CommandType.StoredProcedure, pars);

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
                            job.Share = bool.Parse(row["Share"].ToString());

                            job.Noti95 = bool.Parse(row["Noti75"].ToString());
                            job.Noti90 = bool.Parse(row["Noti90"].ToString());
                            job.Noti75 = bool.Parse(row["Noti95"].ToString());
                            job.noti100 = bool.Parse(row["Noti100"].ToString());

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

                            if ((row[3].ToString() != "" || row[3].ToString() != null)
                                && decimal.Parse(row["Budget"].ToString()) != 0)
                            {
                                job.Budget = decimal.Parse(row[3].ToString());
                                job.BudgetPercent = ((job.ExpenseTotal + job.TravelLogCostTotal +
                                    (job.WorkLogHours/60 * job.HourlyRate)) / job.Budget) * 100;
                            }
                            else
                            {
                                job.Budget = 0;
                                job.BudgetPercent = 0;
                            }

                            job.AllExpenseTotal = job.ExpenseTotal;

                            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                            nfi.NumberGroupSeparator = " ";
                            job.AllExpenseTotalString = job.AllExpenseTotal.ToString("#,0.00", nfi);
                            job.TotalPaidString = job.TotalPaid.ToString("#,0.00", nfi);
                            job.BudgetString = job.Budget.ToString("#,0.00", nfi);
                            job.TravelLogCostTotalString = job.TravelLogCostTotal.ToString("#,0.00", nfi);
                            job.TotalUnPaidString = job.TotalUnPaid.ToString("#,0.00", nfi);
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
                #region jobHours
                List<SP_GetJob_Result> JobHours = new List<SP_GetJob_Result>();

                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@SD", DateTime.Now.AddYears(-100)),
                        new SqlParameter("@ED", DateTime.Now.AddYears(+100)),
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_WorklogHours",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJob_Result jobHours = new Model.SP_GetJob_Result();

                            jobHours.JobID = int.Parse(row["JobID"].ToString());

                            if (row["WorkLogHours"].ToString() != "" && row["WorkLogHours"] != null)
                            {
                                jobHours.WorkLogHours = int.Parse(row["WorkLogHours"].ToString());
                            }
                            else
                            {
                                jobHours.WorkLogHours = 0;
                            }

                            JobHours.Add(jobHours);
                        }
                    }
                }
                #endregion

                #region Job Income outstanding
                List<SP_GetJob_Result> TotalUnPaids = new List<SP_GetJob_Result>();

                pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID)
                        //***************************************//
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_TotalUnPaid",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJob_Result TotalUnPaid = new Model.SP_GetJob_Result();

                            TotalUnPaid.JobID = int.Parse(row["JobID"].ToString());

                            if (row["TotalUnPaid"].ToString() != "" && row["TotalUnPaid"] != null)
                            {
                                TotalUnPaid.TotalUnPaid = decimal.Parse(row["TotalUnPaid"].ToString());
                            }
                            else
                            {
                                TotalUnPaid.TotalUnPaid = 0;
                            }

                            TotalUnPaids.Add(TotalUnPaid);
                        }
                    }
                }
                #endregion

                #region Job Income
                List<SP_GetJob_Result> TotalPaids = new List<SP_GetJob_Result>();

                pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID)
                        //***************************************//
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_TotalPaid",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJob_Result TotalPaid = new Model.SP_GetJob_Result();

                            TotalPaid.JobID = int.Parse(row["JobID"].ToString());

                            if (row["TotalPaid"].ToString() != "" && row["TotalPaid"] != null)
                            {
                                TotalPaid.TotalPaid = decimal.Parse(row["TotalPaid"].ToString());
                            }
                            else
                            {
                                TotalPaid.TotalPaid = 0;
                            }

                            TotalPaids.Add(TotalPaid);
                        }
                    }
                }
                #endregion

                pars = new SqlParameter[]
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
                            job.JobID = int.Parse(row["JobID"].ToString());
                            job.ClientID = int.Parse(row["ClientID"].ToString());

                            job.WorkLogHours = 0;
                            job.WorkLogHoursString = "None";
                            foreach (SP_GetJob_Result hours in JobHours)
                            {
                                if (hours.JobID == job.JobID)
                                {
                                    job.WorkLogHours = hours.WorkLogHours;
                                    int Hour = hours.WorkLogHours / 60;
                                    int Minute = hours.WorkLogHours % 60;
                                    job.WorkLogHoursString = Hour + ":" + Minute + " ";
                                }
                            }

                            job.JobTitle = row["JobTitle"].ToString();
                            job.ClientFirstName = row["FirstName"].ToString();
                            job.StartDate = DateTime.Parse(row["StartDate"].ToString());
                            job.StartDateString = String.Format("{0:dddd, dd MMMM yyyy}", job.StartDate);
                            job.Share = bool.Parse(row["Share"].ToString());

                            if (row["EndDate"].ToString() != "" && row["EndDate"] != null)
                            {
                                job.EndDate = DateTime.Parse(row["EndDate"].ToString());
                                job.EndDateString = String.Format("{0:dddd, dd MMMM yyyy}", job.EndDate);

                            }
                            else
                            {
                                job.EndDateString = "Active";
                            }

                            job.HourlyRate = decimal.Parse(row["HourlyRate"].ToString());

                            if (row["ExpenseTotal"].ToString() != "" && row["ExpenseTotal"] != null)
                            {
                                job.ExpenseTotal = decimal.Parse(row["ExpenseTotal"].ToString());
                            }
                            else
                            {
                                job.ExpenseTotal = 0;
                            }

                            job.TotalPaid = 0;
                            foreach (SP_GetJob_Result item in TotalPaids)
                            {
                                if (job.JobID == item.JobID)
                                {
                                    job.TotalPaid = item.TotalPaid;
                                }
                            }

                            job.TotalUnPaid = 0;
                            foreach (SP_GetJob_Result item in TotalUnPaids)
                            {
                                if (job.JobID == item.JobID)
                                {
                                    job.TotalUnPaid = item.TotalUnPaid;
                                }
                            }

                            if (row["TravelLogCostTotal"].ToString() != "" && row["TravelLogCostTotal"] != null)
                            {
                                job.TravelLogCostTotal = decimal.Parse(row["TravelLogCostTotal"].ToString());
                            }
                            else
                            {
                                job.TravelLogCostTotal = 0;
                            }
                            Jobs.Add(job);

                            if ((row["Budget"].ToString() != "" || row["Budget"].ToString() != null)
                                && decimal.Parse(row["Budget"].ToString()) != 0)
                            {
                                job.Budget = decimal.Parse(row["Budget"].ToString());
                                job.BudgetPercent = ((job.ExpenseTotal + job.TravelLogCostTotal +
                                    ((job.WorkLogHours / 60) * job.HourlyRate)) / job.Budget) * 100;
                            }
                            else
                            {
                                job.Budget = 0;
                                job.BudgetPercent = 0;
                            }

                            job.AllExpenseTotal = job.ExpenseTotal + job.TravelLogCostTotal;

                            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                            nfi.NumberGroupSeparator = " ";
                            job.AllExpenseTotalString = job.AllExpenseTotal.ToString("#,0.00", nfi);
                            job.TotalPaidString = job.TotalPaid.ToString("#,0.00", nfi);
                            job.BudgetString = job.Budget.ToString("#,0.00", nfi);
                            job.TravelLogCostTotalString = job.TravelLogCostTotal.ToString("#,0.00", nfi);
                            job.TotalUnPaidString = job.TotalUnPaid.ToString("#,0.00", nfi);

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
        public List<SP_GetJob_Result> getProfileJobsPast(Profile profile, DateTime sDate, DateTime eDate)
        {
            List<SP_GetJob_Result> Jobs = new List<SP_GetJob_Result>();
            try
            {
                #region jobHours
                List<SP_GetJob_Result> JobHours = new List<SP_GetJob_Result>();

                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1)),
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_WorklogHoursPast",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJob_Result jobHours = new Model.SP_GetJob_Result();

                            jobHours.JobID = int.Parse(row["JobID"].ToString());

                            if (row["WorkLogHours"].ToString() != "" && row["WorkLogHours"] != null)
                            {
                                jobHours.WorkLogHours = int.Parse(row["WorkLogHours"].ToString());
                            }
                            else
                            {
                                jobHours.WorkLogHours = 0;
                            }

                            JobHours.Add(jobHours);
                        }
                    }
                }
                #endregion

                #region Job Income outstanding
                List<SP_GetJob_Result> TotalUnPaids = new List<SP_GetJob_Result>();

                pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1))
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_TotalUnPaidPast",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJob_Result TotalUnPaid = new Model.SP_GetJob_Result();

                            TotalUnPaid.JobID = int.Parse(row["JobID"].ToString());

                            if (row["TotalUnPaid"].ToString() != "" && row["TotalUnPaid"] != null)
                            {
                                TotalUnPaid.TotalUnPaid = decimal.Parse(row["TotalUnPaid"].ToString());
                            }
                            else
                            {
                                TotalUnPaid.TotalUnPaid = 0;
                            }

                            TotalUnPaids.Add(TotalUnPaid);
                        }
                    }
                }
                #endregion

                #region Job Income
                List<SP_GetJob_Result> TotalPaids = new List<SP_GetJob_Result>();

                pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1))
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_TotalPaidPast",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJob_Result TotalPaid = new Model.SP_GetJob_Result();

                            TotalPaid.JobID = int.Parse(row["JobID"].ToString());

                            if (row["TotalPaid"].ToString() != "" && row["TotalPaid"] != null)
                            {
                                TotalPaid.TotalPaid = decimal.Parse(row["TotalPaid"].ToString());
                            }
                            else
                            {
                                TotalPaid.TotalPaid = 0;
                            }

                            TotalPaids.Add(TotalPaid);
                        }
                    }
                }
                #endregion

                pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1)),
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_GetProfileJobsPast",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJob_Result job = new Model.SP_GetJob_Result();
                            job.JobID = int.Parse(row["JobID"].ToString());
                            job.ClientID = int.Parse(row["ClientID"].ToString());

                            job.WorkLogHours = 0;
                            job.WorkLogHoursString = "None";
                            foreach (SP_GetJob_Result hours in JobHours)
                            {
                                if (hours.JobID == job.JobID)
                                {
                                    job.WorkLogHours = hours.WorkLogHours;
                                    int Hour = hours.WorkLogHours / 60;
                                    int Minute = hours.WorkLogHours % 60;
                                    job.WorkLogHoursString = Hour + ":" + Minute + " ";
                                }
                            }

                            job.JobTitle = row["JobTitle"].ToString();
                            job.ClientFirstName = row["FirstName"].ToString();
                            job.StartDate = DateTime.Parse(row["StartDate"].ToString());
                            job.StartDateString = String.Format("{0:dddd, dd MMMM yyyy}", job.StartDate);
                            job.Share = bool.Parse(row["Share"].ToString());

                            if (row["EndDate"].ToString() != "" && row["EndDate"] != null)
                            {
                                job.EndDate = DateTime.Parse(row["EndDate"].ToString());
                                job.EndDateString = String.Format("{0:dddd, dd MMMM yyyy}", job.EndDate);

                            }
                            else
                            {
                                job.EndDateString = "Active";
                            }

                            job.HourlyRate = decimal.Parse(row["HourlyRate"].ToString());

                            if (row["ExpenseTotal"].ToString() != "" && row["ExpenseTotal"] != null)
                            {
                                job.ExpenseTotal = decimal.Parse(row["ExpenseTotal"].ToString());
                            }
                            else
                            {
                                job.ExpenseTotal = 0;
                            }

                            job.TotalPaid = 0;
                            foreach (SP_GetJob_Result item in TotalPaids)
                            {
                                if (job.JobID == item.JobID)
                                {
                                    job.TotalPaid = item.TotalPaid;
                                }
                            }

                            job.TotalUnPaid = 0;
                            foreach (SP_GetJob_Result item in TotalUnPaids)
                            {
                                if (job.JobID == item.JobID)
                                {
                                    job.TotalUnPaid = item.TotalUnPaid;
                                }
                            }

                            if (row["TravelLogCostTotal"].ToString() != "" && row["TravelLogCostTotal"] != null)
                            {
                                job.TravelLogCostTotal = decimal.Parse(row["TravelLogCostTotal"].ToString());
                            }
                            else
                            {
                                job.TravelLogCostTotal = 0;
                            }
                            Jobs.Add(job);

                            if ((row["Budget"].ToString() != "" || row["Budget"].ToString() != null)
                                && decimal.Parse(row["Budget"].ToString()) != 0)
                            {
                                job.Budget = decimal.Parse(row["Budget"].ToString());
                                job.BudgetPercent = ((job.ExpenseTotal + job.TravelLogCostTotal +
                                    (job.WorkLogHours/60 * job.HourlyRate)) / job.Budget) * 100;
                            }
                            else
                            {
                                job.Budget = 0;
                                job.BudgetPercent = 0;
                            }

                            job.AllExpenseTotal = job.ExpenseTotal + job.TravelLogCostTotal;

                            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                            nfi.NumberGroupSeparator = " ";
                            job.AllExpenseTotalString = job.AllExpenseTotal.ToString("#,0.00", nfi);
                            job.TotalPaidString = job.TotalPaid.ToString("#,0.00", nfi);
                            job.BudgetString = job.Budget.ToString("#,0.00", nfi);
                            job.TravelLogCostTotalString = job.TravelLogCostTotal.ToString("#,0.00", nfi);
                            job.TotalUnPaidString = job.TotalUnPaid.ToString("#,0.00", nfi);

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
                #region jobHours
            List<SP_GetJob_Result> JobHours = new List<SP_GetJob_Result>();

                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@SD", DateTime.Now.AddYears(-100)),
                        new SqlParameter("@ED", DateTime.Now.AddYears(+100)),
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_WorklogHours",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJob_Result jobHours = new Model.SP_GetJob_Result();

                            jobHours.JobID = int.Parse(row["JobID"].ToString());

                            if (row["WorkLogHours"].ToString() != "" && row["WorkLogHours"] != null)
                            {
                                jobHours.WorkLogHours = int.Parse(row["WorkLogHours"].ToString());
                            }
                            else
                            {
                                jobHours.WorkLogHours = 0;
                            }

                            JobHours.Add(jobHours);
                        }
                    }
                }
                #endregion

                #region Job Income outstanding
                List<SP_GetJob_Result> TotalUnPaids = new List<SP_GetJob_Result>();

                pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID)
                        //***************************************//
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_TotalUnPaid",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJob_Result TotalUnPaid = new Model.SP_GetJob_Result();

                            TotalUnPaid.JobID = int.Parse(row["JobID"].ToString());

                            if (row["TotalUnPaid"].ToString() != "" && row["TotalUnPaid"] != null)
                            {
                                TotalUnPaid.TotalUnPaid = decimal.Parse(row["TotalUnPaid"].ToString());
                            }
                            else
                            {
                                TotalUnPaid.TotalUnPaid = 0;
                            }

                            TotalUnPaids.Add(TotalUnPaid);
                        }
                    }
                }
                #endregion

                #region Job Income
                List<SP_GetJob_Result> TotalPaids = new List<SP_GetJob_Result>();

                pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID)
                        //***************************************//
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_TotalPaid",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJob_Result TotalPaid = new Model.SP_GetJob_Result();

                            TotalPaid.JobID = int.Parse(row["JobID"].ToString());

                            if (row["TotalPaid"].ToString() != "" && row["TotalPaid"] != null)
                            {
                                TotalPaid.TotalPaid = decimal.Parse(row["TotalPaid"].ToString());
                            }
                            else
                            {
                                TotalPaid.TotalPaid = 0;
                            }

                            TotalPaids.Add(TotalPaid);
                        }
                    }
                }
                #endregion

                pars = new SqlParameter[]
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
                            job.JobID = int.Parse(row["JobID"].ToString());
                            job.ClientID = int.Parse(row["ClientID"].ToString());

                            job.WorkLogHours = 0;
                            job.WorkLogHoursString = "None";
                            foreach (SP_GetJob_Result hours in JobHours)
                            {
                                if (hours.JobID == job.JobID)
                                {
                                    job.WorkLogHours = hours.WorkLogHours;
                                    int Hour = hours.WorkLogHours / 60;
                                    int Minute = hours.WorkLogHours % 60;
                                    job.WorkLogHoursString = Hour + ":" + Minute + " ";
                                }
                            }
                            
                            job.JobTitle = row["JobTitle"].ToString();
                            job.ClientFirstName = row["FirstName"].ToString();
                            job.StartDate = DateTime.Parse(row["StartDate"].ToString());
                            job.StartDateString = String.Format("{0:dddd, dd MMMM yyyy}", job.StartDate);
                            job.Share = bool.Parse(row["Share"].ToString());

                            if (row["EndDate"].ToString() != "" && row["EndDate"] != null)
                            {
                                job.EndDate = DateTime.Parse(row["EndDate"].ToString());
                                job.EndDateString = String.Format("{0:dddd, dd MMMM yyyy}", job.EndDate);

                            }
                            else
                            {
                                job.EndDateString = "Active";
                            }

                            job.HourlyRate = decimal.Parse(row["HourlyRate"].ToString());

                            if (row["ExpenseTotal"].ToString() != "" && row["ExpenseTotal"] != null)
                            {
                                job.ExpenseTotal = decimal.Parse(row["ExpenseTotal"].ToString());
                            }
                            else
                            {
                                job.ExpenseTotal = 0;
                            }

                            job.TotalPaid = 0;
                            foreach(SP_GetJob_Result item in TotalPaids)
                            {
                                if (job.JobID == item.JobID)
                                {
                                    job.TotalPaid = item.TotalPaid;
                                }
                            }

                            job.TotalUnPaid = 0;
                            foreach(SP_GetJob_Result item in TotalUnPaids)
                            {
                                if (job.JobID == item.JobID)
                                {
                                    job.TotalUnPaid = item.TotalUnPaid;
                                }
                            }

                            if (row["TravelLogCostTotal"].ToString() != "" && row["TravelLogCostTotal"] != null)
                            {
                                job.TravelLogCostTotal = decimal.Parse(row["TravelLogCostTotal"].ToString());
                            }
                            else
                            {
                                job.TravelLogCostTotal = 0;
                            }
                            Jobs.Add(job);

                            if ((row["Budget"].ToString() != "" || row["Budget"].ToString() != null)
                                && decimal.Parse(row["Budget"].ToString()) != 0)
                            {
                                job.Budget = decimal.Parse(row["Budget"].ToString());
                                job.BudgetPercent = ((job.ExpenseTotal + job.TravelLogCostTotal +
                                    ((job.WorkLogHours/60) * job.HourlyRate)) / job.Budget) * 100;
                            }
                            else
                            {
                                job.Budget = 0;
                                job.BudgetPercent = 0;
                            }

                            job.AllExpenseTotal = job.ExpenseTotal + job.TravelLogCostTotal;

                            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                            nfi.NumberGroupSeparator = " ";
                            job.AllExpenseTotalString = job.AllExpenseTotal.ToString("#,0.00", nfi);
                            job.TotalPaidString = job.TotalPaid.ToString("#,0.00", nfi);
                            job.BudgetString = job.Budget.ToString("#,0.00", nfi);
                            job.TravelLogCostTotalString = job.TravelLogCostTotal.ToString("#,0.00", nfi);
                            job.TotalUnPaidString = job.TotalUnPaid.ToString("#,0.00", nfi);

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
        public bool UpdateShareJob(Job JobID)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@JID", JobID.JobID),
                   };

                Result = DBHelper.NonQuery("SP_UpdateSharePeriodJob", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        #endregion

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
        public bool EditWorkLogItem(Model.Worklog logItem)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@ID", logItem.LogItemID),
                        new SqlParameter("@D", logItem.Description),
                        new SqlParameter("@ST", logItem.StartTime),
                        new SqlParameter("@ET", logItem.EndTime)
                   };

                Result = DBHelper.NonQuery("SP_EditWorkLogItem", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        public bool DeleteWorkLogItem(Model.Worklog logItem)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@ID", logItem.LogItemID)
                   };

                Result = DBHelper.NonQuery("SP_DeleteWorkLogItem", CommandType.StoredProcedure, pars);

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
                            logItem.Description = row["Description"].ToString();
                            logItem.StartTime = DateTime.Parse(row[2].ToString());
                            logItem.StartTimeString = logItem.StartTime.ToString("HH:mm");
                            logItem.EndTime = DateTime.Parse(row[3].ToString());
                            logItem.EndTimeString = logItem.EndTime.ToString("HH:mm");
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
        public bool editClient(Client client)
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
                        new SqlParameter("@CID", client.ClientID),
                   };

                Result = DBHelper.NonQuery("SP_EditClient", CommandType.StoredProcedure, pars);

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
                            Client.FirstName = row["FirstName"].ToString();
                            Client.LastName = row["LastName"].ToString();

                            if (row["CompanyName"] != null)
                            {
                                if (row["CompanyName"].ToString() != "")
                                {
                                    Client.CompanyName = row["CompanyName"].ToString();
                                }
                                else
                                {
                                    Client.CompanyName = "None";
                                }
                            }
                            else
                            {
                                Client.CompanyName = "None";
                            }
                            Client.ContactNumber = row["ContactNumber"].ToString();
                            Client.EmailAddress = row["EmailAddress"].ToString();
                            Client.PreferedCommunicationChannel = row["PreferedCommunicationChannel"].ToString();
                            Client.PhysiclaAddress = row["PhysiclaAddress"].ToString();
                            Client.ProfileID = int.Parse(row["ProfileID"].ToString());
                            Client.ClientID = int.Parse(row["ClientID"].ToString());
                        }
                    }
                }
                return Client;
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
                            if (row["CompanyName"] != null)
                            {
                                if (row["CompanyName"].ToString() != "")
                                {
                                    NewClient.CompanyName = row["CompanyName"].ToString();
                                }
                                else
                                {
                                    NewClient.CompanyName = "None";
                                }
                            }
                            else
                            {
                                NewClient.CompanyName = "None";
                            }
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
                            if (row["Invoice/ReceiptCopy"] != null && row["Invoice/ReceiptCopy"].ToString() != "")
                            {
                                expense.Invoice_ReceiptCopy = Encoding.ASCII.GetBytes(row["Invoice/ReceiptCopy"].ToString());
                            }
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
                            if (row["Invoice/ReceiptCopy"] != null && row["Invoice/ReceiptCopy"].ToString() != "")
                            {
                                expense.Invoice_ReceiptCopy = Encoding.ASCII.GetBytes(row["Invoice/ReceiptCopy"].ToString());
                            }
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
        public List<SP_GetJobExpense_Result> getAllJobExpense(Profile profileID, DateTime SD, DateTime ED)
        {
            List<SP_GetJobExpense_Result> Expenses = new List<SP_GetJobExpense_Result>();
            SqlParameter[] pars = new SqlParameter[]
                {
                        new SqlParameter("@PID", profileID.ProfileID),
                        new SqlParameter("@SD", SD),
                        new SqlParameter("@ED", ED)
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
                        new SqlParameter("@PEID", newGeneralExpense.PrimaryExpenseID),
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
                            if(row["Invoice/ReceiptCopy"] != null && row["Invoice/ReceiptCopy"].ToString() != "")
                            {
                            expense.Invoice_ReceiptCopy = Encoding.ASCII.GetBytes(row["Invoice/ReceiptCopy"].ToString());
                            }
                            expense.CatName = row[9].ToString();
                            expense.CatDescription = row[10].ToString();
                            expense.DateString = expense.Date.ToString("dddd, dd MMMM yyyy");
                            expense.RepeatOccurrences = getGeneralExpenseRepeatOccurrence(expenseID);
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
        public List<SP_GetGeneralExpense_Result> getGeneralExpenses(Profile profileID, DateTime sDate, DateTime eDate)
        {
            List<SP_GetGeneralExpense_Result> Expenses = new List<SP_GetGeneralExpense_Result>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profileID.ProfileID),
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1))
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
                            if (row["Invoice/ReceiptCopy"] != null && row["Invoice/ReceiptCopy"].ToString() != "")
                            {
                                expense.Invoice_ReceiptCopy = Encoding.ASCII.GetBytes(row["Invoice/ReceiptCopy"].ToString());
                            }
                            expense.CatName = row[9].ToString();
                            expense.CatDescription = row[10].ToString();
                            expense.DateString = expense.Date.ToString("dddd, dd MMMM yyyy");
                            expense.PrimaryExpenseID = int.Parse(row["PrimaryExpenseID"].ToString());
                            Expense expenseID = new Expense();
                            expenseID.ExpenseID = expense.ExpenseID;
                            expense.RepeatOccurrences = getGeneralExpenseRepeatOccurrence(expenseID);
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
        public List<SP_GetGeneralExpense_Result> getGeneralExpenseRepeatOccurrence(Expense expenseID)
        {
            List<SP_GetGeneralExpense_Result> Expenses = new List<SP_GetGeneralExpense_Result>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@EID", expenseID.ExpenseID)
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_GetGeneralExpensesRepeatOccurrence",
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
                            if (row["Invoice/ReceiptCopy"] != null && row["Invoice/ReceiptCopy"].ToString() != "")
                            {
                                expense.Invoice_ReceiptCopy = Encoding.ASCII.GetBytes(row["Invoice/ReceiptCopy"].ToString());
                            }
                            expense.CatName = row[9].ToString();
                            expense.CatDescription = row[10].ToString();
                            expense.DateString = expense.Date.ToString("dddd, dd MMMM yyyy");
                            expense.PrimaryExpenseID = int.Parse(row["PrimaryExpenseID"].ToString());
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
        public List<SP_GetGeneralExpense_Result> getRepeatGeneralExpenses()
        {
            List<SP_GetGeneralExpense_Result> Expenses = new List<SP_GetGeneralExpense_Result>();
            try
            {
                using (DataTable table = DBHelper.Select("SP_GetRepeatGeneralExpenses",
            CommandType.StoredProcedure))
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
                            expense.Invoice_ReceiptCopy = null;
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
        public bool UpdateGeneralExpenseRepeate(SP_GetGeneralExpense_Result expense)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@EID", expense.ExpenseID)
                   };

                Result = DBHelper.NonQuery("SP_UpdateGeneralExpenseRepeate", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
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
        public List<TravelLog> getProfileTravelLog(Profile getProfileTravelLog, DateTime sDate, DateTime eDate)
        {
            List<TravelLog> TravelLog = new List<TravelLog>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@PID", getProfileTravelLog.ProfileID),
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1))
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
                            travelLogItem.JobTitle = row["JobTitle"].ToString();
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
                            travelLogItem.ClientCharge = decimal.Parse(row["ClientCharge"].ToString());
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

                var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                nfi.NumberGroupSeparator = " ";

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
                                + Hour.Description +" at R"+ Hour.UnitCost.ToString("#,0.00", nfi) + " per Hour";
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
                            Travel.DisplayString = Travel.Description + " - "+ Travel.UnitCount.ToString("0.00") + "KM at R" 
                                + Travel.UnitCost.ToString("#,0.00", nfi) + " Per KM";
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
                                + Expense.Description + " at R" + Expense.UnitCost.ToString("#,0.00", nfi) + " each";
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
                            Invoice.DateTimeString = Invoice.DateTime.ToString("hh:mm dd MMM yyyy");
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
        public List<SP_GetInvoice_Result> getInvoicesOutsatanding(Profile profileID)
        {
            List<SP_GetInvoice_Result> JobInvoices = new List<SP_GetInvoice_Result>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profileID.ProfileID)
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_GetInvoicesOutstanding",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetInvoice_Result Invoice = new SP_GetInvoice_Result();
                            Invoice.InvoiceNum = row[0].ToString();
                            Invoice.DateTime = DateTime.Parse(row[1].ToString());
                            Invoice.DateTimeString = Invoice.DateTime.ToString("hh:mm dd MMM yyyy");
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
        public List<SP_GetInvoice_Result> getInvoicesPast(Profile profileID, DateTime sDate, DateTime eDate)
        {
            List<SP_GetInvoice_Result> JobInvoices = new List<SP_GetInvoice_Result>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profileID.ProfileID),
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1))
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_GetInvoicesPaid",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetInvoice_Result Invoice = new SP_GetInvoice_Result();
                            Invoice.InvoiceNum = row[0].ToString();
                            Invoice.DateTime = DateTime.Parse(row[1].ToString());
                            Invoice.DateTimeString = Invoice.DateTime.ToString("hh:mm dd MMM yyyy");
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
                            InvoiceLineItem.UnitCostString = InvoiceLineItem.UnitCost.ToString("#.00");
                            if(InvoiceLineItem.Type == 'H')
                            {
                                InvoiceLineItem.UnitCountString += " h";
                            }
                            else if (InvoiceLineItem.Type == 'T')
                            {
                                InvoiceLineItem.UnitCountString += " KMs";
                            }

                            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                            nfi.NumberGroupSeparator = " ";
                            InvoiceLineItem.VATRateString = InvoiceLineItem.VATRate.ToString("#,0.00", nfi);
                            InvoiceLineItem.TotalCostString = InvoiceLineItem.TotalCost.ToString("#,0.00", nfi);
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

        #region Income Dashboard
        public DashboardIncome getIncomeDashboard(Profile profile)
        {
            DashboardIncome dashboardIncome = null;
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID)
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_IncomeDashboard",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            dashboardIncome = new Model.DashboardIncome();

                            if (row["TotalIncomePast30Days"].ToString() != "")
                            {
                                dashboardIncome.IncomePast30Days = decimal.Parse(row["TotalIncomePast30Days"].ToString());
                            }
                            else
                            {
                                dashboardIncome.IncomePast30Days = 0;
                            }

                            if (row["TotalIncomePast60To30Days"].ToString() != "")
                            {
                                dashboardIncome.IncomePast60to30DaysPercent = decimal.Parse(row["TotalIncomePast60To30Days"].ToString());
                            }
                            else
                            {
                                dashboardIncome.IncomePast60to30DaysPercent = 0;
                            }

                            if (dashboardIncome.IncomePast30Days == 0
                                && dashboardIncome.IncomePast60to30DaysPercent == 0)
                            {
                                dashboardIncome.IncomePast60to30DaysUporDown = 'U';
                            }
                            else if (dashboardIncome.IncomePast30Days > dashboardIncome.IncomePast60to30DaysPercent
                                && dashboardIncome.IncomePast60to30DaysPercent != 0)
                            {
                                dashboardIncome.IncomePast60to30DaysUporDown = 'U';
                                dashboardIncome.IncomePast60to30DaysPercent =
                                    (dashboardIncome.IncomePast30Days / dashboardIncome.IncomePast60to30DaysPercent) * 100;
                            }
                            else if (dashboardIncome.IncomePast30Days < dashboardIncome.IncomePast60to30DaysPercent
                                && dashboardIncome.IncomePast30Days != 0)
                            {
                                dashboardIncome.IncomePast60to30DaysUporDown = 'D';
                                dashboardIncome.IncomePast60to30DaysPercent =
                                    (dashboardIncome.IncomePast60to30DaysPercent / dashboardIncome.IncomePast30Days) * 100;
                            }
                            else
                            {
                                dashboardIncome.IncomePast60to30DaysUporDown = 'U';
                                dashboardIncome.IncomePast60to30DaysPercent = -999999999;
                            }


                            if (row["TotalOutIncome"].ToString() != "")
                            {
                                dashboardIncome.TotalOutIncome = decimal.Parse(row["TotalOutIncome"].ToString());
                            }
                            else
                            {
                                dashboardIncome.TotalOutIncome = 0;
                            }

                            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                            nfi.NumberGroupSeparator = " ";
                            dashboardIncome.IncomePast30DaysString = dashboardIncome.IncomePast30Days.ToString("#,0.00", nfi);
                            dashboardIncome.TotalOutIncomeString = dashboardIncome.TotalOutIncome.ToString("#,0.00", nfi);
                            dashboardIncome.IncomePast60to30DaysPercentString = dashboardIncome.IncomePast60to30DaysPercent.ToString("#,0.00", nfi);
                        }
                    }
                }
                return dashboardIncome;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        #endregion

        #region Tax And Vat Periods
        public List<TaxAndVatPeriods> getTaxOrVatPeriodForProfile(Profile profileID, char type)
        {
            List<TaxAndVatPeriods> Periods = new List<TaxAndVatPeriods>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profileID.ProfileID),
                        new SqlParameter("@T", type)
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_GetProfileTaxAndVatPeriods",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            TaxAndVatPeriods Period = new TaxAndVatPeriods();
                            Period.ProfileID = int.Parse(row["ProfileID"].ToString());
                            Period.PeriodID = int.Parse(row["PeriodID"].ToString());
                            Period.VATRate = decimal.Parse(row["VATRate"].ToString());
                            Period.StartDate = DateTime.Parse(row["StartDate"].ToString());
                            Period.EndDate = DateTime.Parse(row["EndDate"].ToString());
                            Period.Type = row["Type"].ToString()[0];
                            Period.Share = bool.Parse(row["Share"].ToString());
                            Period.PeriodString = Period.StartDate.ToString("dd MMM yyyy") + " - " + Period.EndDate.ToString("dd MMM yyyy");
                            Periods.Add(Period);
                        }
                    }
                }
                return Periods;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public bool newTaxOrVatPeriod(TaxAndVatPeriods newPeriod)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@PID", newPeriod.ProfileID),
                        new SqlParameter("@SD", newPeriod.StartDate.AddDays(-1)),
                        new SqlParameter("@ED", newPeriod.EndDate.AddDays(+1)),
                        new SqlParameter("@T", newPeriod.Type),
                        new SqlParameter("@S", newPeriod.Share)
                   };

                Result = DBHelper.NonQuery("SP_NewTaxOrVatPeriod", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        public Tuple<TaxAndVatPeriods, TaxConsultant> UpdateShareTaxorVatPeriod(TaxAndVatPeriods PeriodID)
        {
            TaxAndVatPeriods Period = null;
            TaxConsultant consultant = null;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@PID", PeriodID.PeriodID),
                   };

                using (DataTable table = DBHelper.ParamSelect("SP_UpdateSharePeriodTaxorVatPeriod",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            Period = new TaxAndVatPeriods();
                            Period.ProfileID = int.Parse(row["ProfileID"].ToString());
                            Period.PeriodID = int.Parse(row["PeriodID"].ToString());
                            Period.VATRate = decimal.Parse(row["VATRate"].ToString());
                            Period.StartDate = DateTime.Parse(row["StartDate"].ToString());
                            Period.EndDate = DateTime.Parse(row["EndDate"].ToString());
                            Period.Type = row["Type"].ToString()[0];
                            Period.Share = bool.Parse(row["Share"].ToString());
                            Period.PeriodString = "From " + Period.StartDate.ToString("dd MMM yyyy") + " to " + Period.EndDate.ToString("dd MMM yyyy");

                            consultant = new Model.TaxConsultant();
                            consultant.ProfileID = int.Parse(row["ProfileID"].ToString());
                            consultant.Name = row["Name"].ToString();
                            consultant.EmailAddress = row["EmailAddress"].ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Tuple.Create(Period, consultant);
        }
        public TaxAndVatPeriods SP_GetLatestTaxAndVatPeriodID()
        {
            TaxAndVatPeriods PeriodID = null;
            try
            {
                using (DataTable table = DBHelper.Select("SP_GetLatestTaxAndVatPeriodID",
            CommandType.StoredProcedure))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            PeriodID = new Model.TaxAndVatPeriods();

                            if (row["PeriodID"].ToString() != "")
                            {
                                PeriodID.PeriodID = int.Parse(row["PeriodID"].ToString());
                            }
                            else
                            {
                                PeriodID.PeriodID = 0;
                            }
                        }
                    }
                }
                return PeriodID;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        #endregion

        #region Tax Period Brakets
        public List<TaxPeriodRates> getTaxPeriodBrakets(TaxAndVatPeriods getBrakets)
        {
            List<TaxPeriodRates> Brakets = new List<TaxPeriodRates>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", getBrakets.PeriodID)
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_GetTaxPeriodBrakets",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            TaxPeriodRates Braket = new TaxPeriodRates();
                            Braket.PeriodID = int.Parse(row["PeriodID"].ToString());
                            Braket.RateID = int.Parse(row["RateID"].ToString());
                            Braket.Rate = decimal.Parse(row["Rate"].ToString());
                            Braket.Threashold = decimal.Parse(row["Threshold"].ToString());
                            Braket.Type = row["Type"].ToString()[0];
                            Brakets.Add(Braket);
                        }
                    }
                }
                return Brakets;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public bool newPeriodTaxBraket(TaxPeriodRates newBraket)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@PID", newBraket.PeriodID),
                        new SqlParameter("@R", newBraket.Rate),
                        new SqlParameter("@T", newBraket.Threashold),
                        new SqlParameter("@TY", newBraket.Type)
                   };

                Result = DBHelper.NonQuery("SP_NewPeriodTaxBraket", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        #endregion

        #region VAT Center
        public VATDashboard getVatCenterDashboard(Profile profile, TaxAndVatPeriods period)
        {
            VATDashboard dashboard = null;
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        new SqlParameter("@SD", period.StartDate.AddDays(-1)),
                        new SqlParameter("@PDID", period.PeriodID),
                        new SqlParameter("@VR", period.VATRate),
                        new SqlParameter("@ED", period.EndDate.AddDays(+1))
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_getVatCenterDashboard",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            dashboard = new VATDashboard();

                            if (row["VATRECEIVED"].ToString() != "")
                            {
                                dashboard.VATRECEIVED = decimal.Parse(row["VATRECEIVED"].ToString());
                            }
                            else
                            {
                                dashboard.VATRECEIVED = 0;
                            }

                            if (row["VATRECEIVEDPastPeriod"].ToString() != "")
                            {
                                dashboard.VATRECEIVEDPercent = decimal.Parse(row["VATRECEIVEDPastPeriod"].ToString());
                            }
                            else
                            {
                                dashboard.VATRECEIVEDPercent = 0;
                            }

                            if (row["VATPAID"].ToString() != "")
                            {
                                dashboard.VATPAID = decimal.Parse(row["VATPAID"].ToString());
                            }
                            else
                            {
                                dashboard.VATPAID = 0;
                            }

                            if (row["VATPAIDPreviousPeriod"].ToString() != "")
                            {
                                dashboard.VATRECEIVEDPercent = decimal.Parse(row["VATPAIDPreviousPeriod"].ToString());
                            }
                            else
                            {
                                dashboard.VATRECEIVEDPercent = 0;
                            }

                            dashboard.VATPAIDOutstandingEst = dashboard.VATRECEIVED - dashboard.VATPAID;
                            dashboard.VATPAIDOutstandingEstPercent = dashboard.VATRECEIVEDPercent - dashboard.VATPAIDPercent;

                            if (dashboard.VATRECEIVED == 0
                                && dashboard.VATRECEIVEDPercent == 0)
                            {
                                dashboard.VATRECEIVEDUporDown = 'U';
                            }
                            else if (dashboard.VATRECEIVED > dashboard.VATRECEIVEDPercent
                                && dashboard.VATRECEIVEDPercent != 0)
                            {
                                dashboard.VATRECEIVEDUporDown = 'U';
                                dashboard.VATRECEIVEDPercent =
                                    (dashboard.VATRECEIVED / dashboard.VATRECEIVEDPercent) * 100;
                            }
                            else if (dashboard.VATRECEIVED < dashboard.VATRECEIVEDPercent)
                            {
                                dashboard.VATRECEIVEDUporDown = 'D';
                                dashboard.VATRECEIVEDPercent =
                                    (dashboard.VATRECEIVEDPercent / dashboard.VATRECEIVED) * 100;
                            }
                            else
                            {
                                dashboard.VATRECEIVEDUporDown = 'U';
                                dashboard.VATRECEIVEDPercent = -999999999;
                            }

                            if (dashboard.VATPAID == 0
                                && dashboard.VATPAIDPercent == 0)
                            {
                                dashboard.VATPAIDUporDown = 'U';
                            }
                            else if (dashboard.VATPAID > dashboard.VATPAIDPercent
                                && dashboard.VATPAIDPercent != 0)
                            {
                                dashboard.VATPAIDUporDown = 'U';
                                dashboard.VATPAIDPercent =
                                    (dashboard.VATPAID / dashboard.VATPAIDPercent) * 100;
                            }
                            else if (dashboard.VATPAID < dashboard.VATPAIDPercent)
                            {
                                dashboard.VATPAIDUporDown = 'D';
                                dashboard.VATPAIDPercent =
                                    (dashboard.VATPAIDPercent / dashboard.VATPAID) * 100;
                            }
                            else
                            {
                                dashboard.VATPAIDUporDown = 'U';
                                dashboard.VATPAIDPercent = -999999999;
                            }

                            if (dashboard.VATPAIDOutstandingEst == 0
                                && dashboard.VATPAIDOutstandingEstPercent == 0)
                            {
                                dashboard.VATPAIDOutstandingEstUporDown = 'U';
                            }
                            else if (dashboard.VATPAIDOutstandingEst > dashboard.VATPAIDOutstandingEstPercent
                                && dashboard.VATPAIDOutstandingEstPercent != 0)
                            {
                                dashboard.VATPAIDOutstandingEstUporDown = 'U';
                                dashboard.VATPAIDOutstandingEstPercent =
                                    (dashboard.VATPAIDOutstandingEst / dashboard.VATPAIDOutstandingEstPercent) * 100;
                            }
                            else if (dashboard.VATPAIDOutstandingEst < dashboard.VATPAIDOutstandingEstPercent)
                            {
                                dashboard.VATPAIDOutstandingEstUporDown = 'D';
                                dashboard.VATPAIDOutstandingEstPercent =
                                    (dashboard.VATPAIDOutstandingEstPercent / dashboard.VATPAIDOutstandingEst) * 100;
                            }
                            else
                            {
                                dashboard.VATPAIDOutstandingEstUporDown = 'U';
                                dashboard.VATPAIDOutstandingEstPercent = -999999999;
                            }

                            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                            nfi.NumberGroupSeparator = " ";
                            dashboard.VATPAIDString = dashboard.VATPAID.ToString("#,0.00", nfi);
                            dashboard.VATRECEIVEDString = dashboard.VATRECEIVED.ToString("#,0.00", nfi);
                            dashboard.VATPAIDOutstandingEstString = dashboard.VATPAIDOutstandingEst.ToString("#,0.00", nfi);
                            dashboard.VATPAIDPercentString = dashboard.VATPAIDPercent.ToString("#,0.00", nfi);
                            dashboard.VATRECEIVEDPercentString = dashboard.VATRECEIVEDPercent.ToString("#,0.00", nfi);
                            dashboard.VATPAIDOutstandingEstPercentString = dashboard.VATPAIDOutstandingEstPercent.ToString("#,0.00", nfi);
                        }
                    }
                }
                return dashboard;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public List<TAXorVATRecivedList> getVATRecivedList(Profile profile, TaxAndVatPeriods period)
        {
            List<TAXorVATRecivedList> List = new List<TAXorVATRecivedList>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        new SqlParameter("@SD", period.StartDate.AddDays(-1)),
                        new SqlParameter("@ED", period.EndDate.AddDays(+1))
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_GetVATRecivedList",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            TAXorVATRecivedList item = new TAXorVATRecivedList();
                            item.JobID = int.Parse(row["JobID"].ToString());
                            item.clientID = int.Parse(row["ClientID"].ToString());
                            item.InvoiceDate = DateTime.Parse(row["DateTime"].ToString());
                            item.InvoiceDateString = item.InvoiceDate.ToString("dd MMM yyyy");
                            item.Total = decimal.Parse(row["Total"].ToString());
                            item.VATorTAX = decimal.Parse(row["VAT"].ToString());
                            item.clientName = row["Client"].ToString();
                            item.JobTitle = row["JobTitle"].ToString();
                            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                            nfi.NumberGroupSeparator = " ";
                            item.TotalString = item.Total.ToString("#,0.00", nfi);
                            item.VATorTAXString = item.VATorTAX.ToString("#,0.00", nfi);
                            List.Add(item);
                        }
                    }
                }
                return List;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        #endregion

        #region Tax Center
        public TaxDashboard getTaxCenterDashboard(Profile profile, TaxAndVatPeriods period)
        {
            TaxDashboard dashboard = null;
            try
            {
                var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                nfi.NumberGroupSeparator = " ";

                List<TaxPeriodRates> brakets = getTaxPeriodBrakets(period);
                brakets = brakets.OrderBy(x => x.Threashold).ToList();
                string endbraketrange = "and above";
                TaxPeriodRates braket = new TaxPeriodRates();

                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        new SqlParameter("@SD", period.StartDate.AddDays(-1)),
                        new SqlParameter("@ED", period.EndDate.AddDays(+1)),
                        new SqlParameter("@TR", 0)
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_getTAXCenterDashboard",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            dashboard = new TaxDashboard();

                            if (row["IncomeRECEIVEDPastPeriod"].ToString() != "")
                            {
                                dashboard.IncomePercent = decimal.Parse(row["IncomeRECEIVEDPastPeriod"].ToString());
                            }
                            else
                            {
                                dashboard.IncomePercent = 0;
                            }

                            bool getUpperRange = false;
                            foreach (TaxPeriodRates item in brakets)
                            {
                                if (getUpperRange == true)
                                {
                                    endbraketrange = " - R" + (item.Threashold - 1).ToString("#,0.00", nfi);
                                    getUpperRange = false;
                                }
                                if (item.Threashold < dashboard.IncomePercent)
                                {
                                    braket = item;
                                    getUpperRange = true;
                                }
                            }
                            if (getUpperRange == true)
                            {
                                endbraketrange = " and above";
                            }

                            if (row["TaxOwedPastPeriod"].ToString() != "")
                            {
                                dashboard.TAXOwedPercent = (dashboard.IncomePercent / 100) * braket.Rate; ;
                            }
                            else
                            {
                                dashboard.TAXOwedPercent = 0;
                            }


                            if (row["IncomeRECEIVED"].ToString() != "")
                            {
                                dashboard.Income = decimal.Parse(row["IncomeRECEIVED"].ToString());
                            }
                            else
                            {
                                dashboard.Income = 2;
                            }

                            getUpperRange = false;
                            foreach (TaxPeriodRates item in brakets)
                            {
                                if (getUpperRange == true)
                                {
                                    endbraketrange = " - R" + (item.Threashold - 1).ToString("#,0.00", nfi);
                                    getUpperRange = false;
                                }
                                if (item.Threashold < dashboard.Income)
                                {
                                    braket = item;
                                    getUpperRange = true;
                                }
                            }
                            if (getUpperRange == true)
                            {
                                endbraketrange = " and above";
                            }

                            if (row["TaxOwed"].ToString() != "")
                            {
                                dashboard.TAXOwed = (dashboard.Income / 100) * braket.Rate;
                            }
                            else
                            {
                                dashboard.TAXOwed = 0;
                            }

                            if (dashboard.Income == 0
                                && dashboard.IncomePercent == 0)
                            {
                                dashboard.IncomeUporDown = 'U';
                            }
                            else if (dashboard.Income > dashboard.IncomePercent
                                && dashboard.IncomePercent != 0)
                            {
                                dashboard.IncomeUporDown = 'U';
                                dashboard.IncomePercent =
                                    (dashboard.Income / dashboard.IncomePercent) * 100;
                            }
                            else if (dashboard.Income < dashboard.IncomePercent)
                            {
                                dashboard.IncomeUporDown = 'D';
                                dashboard.IncomePercent =
                                    (dashboard.IncomePercent / dashboard.Income) * 100;
                            }
                            else
                            {
                                dashboard.IncomeUporDown = 'U';
                                dashboard.IncomePercent = -999999999;
                            }

                            if (dashboard.TAXOwed == 0
                                && dashboard.TAXOwedPercent == 0)
                            {
                                dashboard.TAXOwedUporDown = 'U';
                            }
                            else if (dashboard.TAXOwed > dashboard.TAXOwedPercent
                                && dashboard.TAXOwedPercent != 0)
                            {
                                dashboard.TAXOwedUporDown = 'U';
                                dashboard.TAXOwedPercent =
                                    (dashboard.TAXOwed / dashboard.TAXOwedPercent) * 100;
                            }
                            else if (dashboard.TAXOwed < dashboard.TAXOwedPercent)
                            {
                                dashboard.TAXOwedUporDown = 'D';
                                dashboard.TAXOwedPercent =
                                    (dashboard.TAXOwedPercent / dashboard.TAXOwed) * 100;
                            }
                            else
                            {
                                dashboard.TAXOwedUporDown = 'U';
                                dashboard.TAXOwedPercent = -999999999;
                            }
                            
                            dashboard.TaxBraketString = braket.Rate.ToString("#,0.00", nfi) + "% | R" +
                            braket.Threashold.ToString("#,0.00", nfi) + endbraketrange;

                            dashboard.IncomePercentString = dashboard.IncomePercent.ToString("#,0.00", nfi);
                            dashboard.TAXOwedPercentString = dashboard.TAXOwedPercent.ToString("#,0.00", nfi);
                            dashboard.IncomeSTRING = dashboard.Income.ToString("#,0.00", nfi);
                            dashboard.TAXOwedSTRING = dashboard.TAXOwed.ToString("#,0.00", nfi);

                            dashboard.TAXRate = braket.Rate;
                        }
                    }
                }

                return dashboard;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public List<TAXorVATRecivedList> getTAXRecivedList(Profile profile, TaxAndVatPeriods period, TaxPeriodRates rate)
        {
            List<TAXorVATRecivedList> List = new List<TAXorVATRecivedList>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        new SqlParameter("@SD", period.StartDate.AddDays(-1)),
                        new SqlParameter("@RID", rate.Rate),
                        new SqlParameter("@ED", period.EndDate.AddDays(+1))
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_GetIncomeRecivedList",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            TAXorVATRecivedList item = new TAXorVATRecivedList();
                            item.JobID = int.Parse(row["JobID"].ToString());
                            item.clientID = int.Parse(row["ClientID"].ToString());
                            item.InvoiceDate = DateTime.Parse(row["DateTime"].ToString());
                            item.InvoiceDateString = item.InvoiceDate.ToString("dd MMM yyyy");
                            item.Total = decimal.Parse(row["Total"].ToString());
                            item.VATorTAX = decimal.Parse(row["TAX"].ToString());
                            item.clientName = row["Client"].ToString();
                            item.JobTitle = row["JobTitle"].ToString();
                            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                            nfi.NumberGroupSeparator = " ";
                            item.TotalString = item.Total.ToString("#,0.00", nfi);
                            item.VATorTAXString = item.VATorTAX.ToString("#,0.00", nfi);
                            List.Add(item);
                        }
                    }
                }
                return List;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        #endregion

        #region File Upload Download
        public bool addGeneralExpenseFile(Model.InvoiceAndReciptesFile newFile)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@IRC", newFile.fileByteArray),
                        new SqlParameter("@EID", newFile.ID),
                        new SqlParameter("@FN", newFile.fileName),
                   };

                Result = DBHelper.NonQuery("SP_addGeneralExpenseFile", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        public InvoiceAndReciptesFile getGeneralExpenseFile(Model.InvoiceAndReciptesFile getFile)
        {
            Model.InvoiceAndReciptesFile file = null;

            SqlParameter[] pars = new SqlParameter[]
                {
                        new SqlParameter("@EID", getFile.ID)
                };

            try
            {
                using (DataTable table = DBHelper.ParamSelect("SP_getGeneralExpenseFile",
            CommandType.StoredProcedure, pars))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row != null)
                        {
                            file = new Model.InvoiceAndReciptesFile();
                            file.fileByteArray = (byte[])row["Invoice/ReceiptCopy"];
                            file.fileName = row["FileName"].ToString();
                            file.ID = getFile.ID;
                        }
                    }
                }
                return file;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public bool addJobExpenseFile(Model.InvoiceAndReciptesFile newFile)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@IRC", newFile.fileByteArray),
                        new SqlParameter("@EID", newFile.ID),
                        new SqlParameter("@FN", newFile.fileName),
                   };

                Result = DBHelper.NonQuery("SP_addJobExpenseFile", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        public InvoiceAndReciptesFile getJobExpenseFile(Model.InvoiceAndReciptesFile getFile)
        {
            Model.InvoiceAndReciptesFile file = null;

            SqlParameter[] pars = new SqlParameter[]
                {
                        new SqlParameter("@EID", getFile.ID)
                };

            try
            {
                using (DataTable table = DBHelper.ParamSelect("SP_getJobExpenseFile",
            CommandType.StoredProcedure, pars))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row != null)
                        {
                            file = new Model.InvoiceAndReciptesFile();
                            file.fileByteArray = (byte[])row["Invoice/ReceiptCopy"];
                            file.fileName = row["FileName"].ToString();
                            file.ID = getFile.ID;
                        }
                    }
                }
                return file;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public List<InvoiceAndReciptesFile> getInvoiceAndReciptesFiles(Profile profileID, DateTime SD, DateTime ED)
        {
            List<InvoiceAndReciptesFile> files = new List<InvoiceAndReciptesFile>();

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profileID.ProfileID),
                        new SqlParameter("@SD", SD),
                        new SqlParameter("@ED", ED)
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_GetInvoiceAndReciptesJobs",
            CommandType.StoredProcedure, pars))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row != null)
                        {
                            InvoiceAndReciptesFile file = new InvoiceAndReciptesFile();
                            file.fileByteArray = (byte[])row["Invoice/ReceiptCopy"];
                            file.fileName = row["FileName"].ToString();
                            file.expenseName = row["Name"].ToString();
                            file.ID = int.Parse(row["ExpenseID"].ToString());
                            file.ProfileID = int.Parse(row["ProfileID"].ToString());
                            file.expenseDate = DateTime.Parse(row["Date"].ToString());
                            file.expenseDateString = file.expenseDate.ToString("dd MMM yyyy");
                            file.Type = "JE";
                            files.Add(file);
                        }
                    }
                }

                pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profileID.ProfileID),
                        new SqlParameter("@SD", SD),
                        new SqlParameter("@ED", ED)
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_GetInvoiceAndReciptesGeneral",
            CommandType.StoredProcedure, pars))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row != null)
                        {
                            InvoiceAndReciptesFile file = new InvoiceAndReciptesFile();
                            file.fileByteArray = (byte[])row["Invoice/ReceiptCopy"];
                            file.fileName = row["FileName"].ToString();
                            file.expenseName = row["Name"].ToString();
                            file.ID = int.Parse(row["ExpenseID"].ToString());
                            file.ProfileID = int.Parse(row["ProfileID"].ToString());
                            file.expenseDate = DateTime.Parse(row["Date"].ToString());
                            file.expenseDateString = file.expenseDate.ToString("dd MMM yyyy");
                            file.Type = "GE";
                            files.Add(file);
                        }
                    }
                }

                files = files.OrderBy(x => x.expenseDate).ToList();
                return files;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        #endregion

        #region Vehicle
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
        public bool editVehicle(Vehicle editVehicle)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@N", editVehicle.Name),
                        new SqlParameter("@FC", editVehicle.SARSFuelCost),
                        new SqlParameter("@CC", editVehicle.ClientCharge),
                        new SqlParameter("@MC", editVehicle.SARSMaintenceCost),
                        new SqlParameter("@FxC", editVehicle.SARSFixedCost),
                        new SqlParameter("@VID", editVehicle.VehicleID),
                   };

                Result = DBHelper.NonQuery("SP_EditVehicle", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        public Vehicle getVehicle(Vehicle getVehicle)
        {
            Vehicle Vehicle = null;
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@VID", getVehicle.VehicleID)
                   };

                using (DataTable table = DBHelper.ParamSelect("SP_GetVehicle",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            Vehicle = new Vehicle();
                            Vehicle.VehicleID = int.Parse(row["VehicleID"].ToString());
                            Vehicle.ProfielID = int.Parse(row["ProfileID"].ToString());
                            Vehicle.SARSFixedCost = decimal.Parse(row["SARSFixedCost"].ToString());
                            Vehicle.SARSFuelCost = decimal.Parse(row["SARSFuelCost"].ToString());
                            Vehicle.SARSMaintenceCost = decimal.Parse(row["SARSMaintenceCost"].ToString());
                            Vehicle.ClientCharge = decimal.Parse(row["ClientCharge"].ToString());
                            Vehicle.Name = row["Name"].ToString();
                        }
                    }
                }
                return Vehicle;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        #endregion

        #region Search
        public List<SearchViewModel> getSearchResults(string term, int ProfileID, DateTime sDate, DateTime eDate, string cat)
        {
            List<SearchViewModel> results = new List<SearchViewModel>();

            try
            {
                #region Jobs
                if(cat == "A" || cat == "J")
                {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@ST", term),
                        new SqlParameter("@PID", ProfileID),
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1))
                   };

                using (DataTable table = DBHelper.ParamSelect("SP_SearchJobs",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SearchViewModel result = new SearchViewModel();
                            result.ResultID = int.Parse(row["JobID"].ToString());
                            result.ResultTitle = row["JobTitle"].ToString() + " (Job)";
                            result.ResultDetails = row["Name"].ToString();
                            result.ResultDate = DateTime.Parse(row["StartDate"].ToString());
                            result.ResultDateString = result.ResultDate.ToString("dd MMM yyyy");
                                result.ResultLink = "../Job/Job?ID=" + result.ResultID;
                                results.Add(result);
                        }
                    }
                }
                }
                #endregion

                #region Work Log
                if(cat == "A" || cat == "WL")
                {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@ST", term),
                        new SqlParameter("@PID", ProfileID),
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1))
                   };

                using (DataTable table = DBHelper.ParamSelect("SP_SearchWorkLog",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SearchViewModel result = new SearchViewModel();
                            result.ResultID = int.Parse(row["LogItemID"].ToString());
                            result.ResultTitle = row["Description"].ToString() + " (Work Log Item)";
                            result.ResultDetails = row["JobClient"].ToString();
                            result.ResultDate = DateTime.Parse(row["StartTime"].ToString());
                            result.ResultDateString = result.ResultDate.ToString("hh:mm dd MMM yyyy");
                                result.ResultLink = "../Job/JobWorkLogItem?ID=" + result.ResultID;
                                results.Add(result);
                        }
                    }
                }
                }
                #endregion

                #region Tax Consultant
                if (cat == "A" || cat == "TC")
                {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@ST", term),
                        new SqlParameter("@PID", ProfileID)
                   };

                using (DataTable table = DBHelper.ParamSelect("SP_SearchTaxConsultant",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SearchViewModel result = new SearchViewModel();
                            result.ResultID = ProfileID;
                            result.ResultTitle = row["Name"].ToString() + " (Tax Consultant)";
                                result.ResultLink = "../Tax/Consultant";
                                results.Add(result);
                        }
                    }
                }
                }
                #endregion

                #region Invoice
                if(cat == "A" || cat == "I")
                {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@ST", term),
                        new SqlParameter("@PID", ProfileID),
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1))
                   };

                using (DataTable table = DBHelper.ParamSelect("SP_SearchInvoice",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SearchViewModel result = new SearchViewModel();
                            result.ResultID = long.Parse(row["InvoiceNum"].ToString());
                            result.ResultTitle = row["JobTitle"].ToString() + " (Invoice)";
                            result.ResultDetails = row["Name"].ToString();
                            result.ResultDate = DateTime.Parse(row["StartDate"].ToString());
                            result.ResultDateString = result.ResultDate.ToString("dd MMM yyyy");
                                result.ResultLink = "../Invoice/Invoice?ID=" + result.ResultID;
                                results.Add(result);
                        }
                    }
                }
                }
                #endregion

                #region Travel Log
                if(cat == "A" || cat == "TL")
                {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@ST", term),
                        new SqlParameter("@PID", ProfileID),
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1))
                   };

                using (DataTable table = DBHelper.ParamSelect("SP_SearchTravelLog",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SearchViewModel result = new SearchViewModel();
                            result.ResultID = int.Parse(row["ExpenseID"].ToString());
                            result.ResultTitle = row["Reason"].ToString() + " (Travel Log Item)";
                            result.ResultDetails = row["Details"].ToString();
                            result.ResultDate = DateTime.Parse(row["Date"].ToString());
                            result.ResultDateString = result.ResultDate.ToString("dd MMM yyyy");
                                result.ResultLink = "../Expense/TravleLogItem?ID=" + result.ResultID;
                                results.Add(result);
                        }
                    }
                }
                }
                #endregion

                #region Job Expenses
                if(cat == "A" || cat == "JE")
                {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@ST", term),
                        new SqlParameter("@PID", ProfileID),
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1))
                   };

                using (DataTable table = DBHelper.ParamSelect("SP_SearchJobExpenses",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SearchViewModel result = new SearchViewModel();
                            result.ResultID = int.Parse(row["ExpenseID"].ToString());
                            result.ResultTitle = row["Name"].ToString() + " (Job Expense)";
                            result.ResultDetails = row["Description"].ToString();
                            result.ResultDate = DateTime.Parse(row["Date"].ToString());
                            result.ResultDateString = result.ResultDate.ToString("dd MMM yyyy");
                                result.ResultLink = "../Expense/JobExpense?ID=" + result.ResultID;
                                results.Add(result);
                        }
                    }
                }
                }
                #endregion

                #region Clients
                if(cat == "A" || cat == "C")
                {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@ST", term),
                        new SqlParameter("@PID", ProfileID)
                   };

                using (DataTable table = DBHelper.ParamSelect("SP_SearchClients",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SearchViewModel result = new SearchViewModel();
                            result.ResultID = int.Parse(row["ClientID"].ToString());
                            result.ResultTitle = row["Name"].ToString() + " (Client)";
                            result.ResultDetails = row["CompanyName"].ToString();
                                result.ResultLink = "../Client/ClientDetails?ID=" + result.ResultID;
                                results.Add(result);
                        }
                    }
                }
                }
                #endregion

                #region General Expenses
                if(cat == "A" || cat == "GE")
                {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@ST", term),
                        new SqlParameter("@PID", ProfileID),
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1))
                   };

                using (DataTable table = DBHelper.ParamSelect("SP_SearchGeneralExpenses",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SearchViewModel result = new SearchViewModel();
                            result.ResultID = int.Parse(row["ExpenseID"].ToString());
                            result.ResultTitle = row["Name"].ToString() + " (General Expense)";
                            result.ResultDetails = row["Description"].ToString();
                            result.ResultDate = DateTime.Parse(row["Date"].ToString());
                            result.ResultDateString = result.ResultDate.ToString("dd MMM yyyy");
                                result.ResultLink = "../Expense/GeneralExpense?ID="+result.ResultID;
                            results.Add(result);
                        }
                    }
                }
                }
                #endregion

                results = results.OrderBy(x => x.ResultTitle).ToList();
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return results;
        }
        #endregion

        #region Notifications
        public bool newNotification(Notifications newNotification)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@PID", newNotification.ProfileID),
                        new SqlParameter("@D", newNotification.date),
                        new SqlParameter("@Dets", newNotification.Details),
                        new SqlParameter("@L", newNotification.Link)
                   };

                Result = DBHelper.NonQuery("SP_NewNotification", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        public List<Notifications> getNotifications(Notifications getNotifications)
        {
            List<Notifications> Notifications = new List<Notifications>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@PID", getNotifications.ProfileID)
                   };

                using (DataTable table = DBHelper.ParamSelect("SP_GetNotifications",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            Notifications notification = new Notifications();
                            notification.notificationID = int.Parse(row["NotificationsID"].ToString());
                            notification.ProfileID = int.Parse(row["ProfileID"].ToString());
                            notification.date = DateTime.Parse(row["Date"].ToString());
                            notification.Details = row["Details"].ToString();
                            notification.Link = row["Link"].ToString();

                            TimeSpan span = DateTime.Now.Subtract(notification.date);
                            notification.timeSince = ((int)span.TotalDays).ToString();

                            Notifications.Add(notification);
                        }
                    }
                }

                Notifications = Notifications.OrderBy(x => x.date).ToList();
                return Notifications;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public Notifications dismissNotifications(Notifications dismissNotification)
        {
            Notifications link = null;
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@NID", dismissNotification.notificationID)
                   };

                using (DataTable table = DBHelper.ParamSelect("SP_DeleteNotification",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            link = new Notifications();
                            link.Link = row["Link"].ToString();
                        }
                    }
                }
                return link;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public bool UpdateJobNotiStatus(SP_GetJob_Result job)
        {
            bool Result = false;

            try
            {
                SqlParameter[] pars = new SqlParameter[]
                   {
                        new SqlParameter("@75", job.Noti75),
                        new SqlParameter("@90", job.Noti90),
                        new SqlParameter("@95", job.Noti95),
                        new SqlParameter("@100", job.noti100),
                        new SqlParameter("@JID",job.JobID)
                   };

                Result = DBHelper.NonQuery("SP_EditJobNotiStatus", CommandType.StoredProcedure, pars);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            return Result;
        }
        #endregion

        #region Reports
        public List<SP_GetJob_Result> getJobsReport(Profile profile, DateTime sDate, DateTime eDate)
        {
            List<SP_GetJob_Result> Jobs = new List<SP_GetJob_Result>();
            try
            {
                #region jobHours
                List<SP_GetJob_Result> JobHours = new List<SP_GetJob_Result>();

                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1)),
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_WorklogHoursPast",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJob_Result jobHours = new Model.SP_GetJob_Result();

                            jobHours.JobID = int.Parse(row["JobID"].ToString());

                            if (row["WorkLogHours"].ToString() != "" && row["WorkLogHours"] != null)
                            {
                                jobHours.WorkLogHours = int.Parse(row["WorkLogHours"].ToString());
                            }
                            else
                            {
                                jobHours.WorkLogHours = 0;
                            }

                            JobHours.Add(jobHours);
                        }
                    }
                }
                #endregion

                #region Job Income outstanding
                List<SP_GetJob_Result> TotalUnPaids = new List<SP_GetJob_Result>();

                pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1))
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_TotalUnPaidPast",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJob_Result TotalUnPaid = new Model.SP_GetJob_Result();

                            TotalUnPaid.JobID = int.Parse(row["JobID"].ToString());

                            if (row["TotalUnPaid"].ToString() != "" && row["TotalUnPaid"] != null)
                            {
                                TotalUnPaid.TotalUnPaid = decimal.Parse(row["TotalUnPaid"].ToString());
                            }
                            else
                            {
                                TotalUnPaid.TotalUnPaid = 0;
                            }

                            TotalUnPaids.Add(TotalUnPaid);
                        }
                    }
                }
                #endregion

                #region Job Income
                List<SP_GetJob_Result> TotalPaids = new List<SP_GetJob_Result>();

                pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1))
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_TotalPaidPast",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJob_Result TotalPaid = new Model.SP_GetJob_Result();

                            TotalPaid.JobID = int.Parse(row["JobID"].ToString());

                            if (row["TotalPaid"].ToString() != "" && row["TotalPaid"] != null)
                            {
                                TotalPaid.TotalPaid = decimal.Parse(row["TotalPaid"].ToString());
                            }
                            else
                            {
                                TotalPaid.TotalPaid = 0;
                            }

                            TotalPaids.Add(TotalPaid);
                        }
                    }
                }
                #endregion

                pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1)),
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_GetJobsReport",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJob_Result job = new Model.SP_GetJob_Result();
                            job.JobID = int.Parse(row["JobID"].ToString());
                            job.ClientID = int.Parse(row["ClientID"].ToString());

                            job.WorkLogHours = 0;
                            job.WorkLogHoursString = "None";
                            foreach (SP_GetJob_Result hours in JobHours)
                            {
                                if (hours.JobID == job.JobID)
                                {
                                    job.WorkLogHours = hours.WorkLogHours;
                                    int Hour = hours.WorkLogHours / 60;
                                    int Minute = hours.WorkLogHours % 60;
                                    job.WorkLogHoursString = Hour + ":" + Minute + " ";
                                }
                            }

                            job.JobTitle = row["JobTitle"].ToString();
                            job.ClientFirstName = row["FirstName"].ToString();
                            job.StartDate = DateTime.Parse(row["StartDate"].ToString());
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

                            job.HourlyRate = decimal.Parse(row["HourlyRate"].ToString());

                            if (row["ExpenseTotal"].ToString() != "" && row["ExpenseTotal"] != null)
                            {
                                job.ExpenseTotal = decimal.Parse(row["ExpenseTotal"].ToString());
                            }
                            else
                            {
                                job.ExpenseTotal = 0;
                            }

                            job.TotalPaid = 0;
                            foreach (SP_GetJob_Result item in TotalPaids)
                            {
                                if (job.JobID == item.JobID)
                                {
                                    job.TotalPaid = item.TotalPaid;
                                }
                            }

                            job.TotalUnPaid = 0;
                            foreach (SP_GetJob_Result item in TotalUnPaids)
                            {
                                if (job.JobID == item.JobID)
                                {
                                    job.TotalUnPaid = item.TotalUnPaid;
                                }
                            }

                            if (row["TravelLogCostTotal"].ToString() != "" && row["TravelLogCostTotal"] != null)
                            {
                                job.TravelLogCostTotal = decimal.Parse(row["TravelLogCostTotal"].ToString());
                            }
                            else
                            {
                                job.TravelLogCostTotal = 0;
                            }
                            Jobs.Add(job);

                            if ((row["Budget"].ToString() != "" || row["Budget"].ToString() != null)
                                && decimal.Parse(row["Budget"].ToString()) != 0)
                            {
                                job.Budget = decimal.Parse(row["Budget"].ToString());
                                job.BudgetPercent = ((job.ExpenseTotal + job.TravelLogCostTotal +
                                    (job.WorkLogHours / 60 * job.HourlyRate)) / job.Budget) * 100;
                            }
                            else
                            {
                                job.Budget = 0;
                                job.BudgetPercent = 0;
                            }

                            job.AllExpenseTotal = job.ExpenseTotal + job.TravelLogCostTotal;

                            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                            nfi.NumberGroupSeparator = " ";
                            job.AllExpenseTotalString = job.AllExpenseTotal.ToString("#,0.00", nfi);
                            job.TotalPaidString = job.TotalPaid.ToString("#,0.00", nfi);
                            job.BudgetString = job.Budget.ToString("#,0.00", nfi);
                            job.TravelLogCostTotalString = job.TravelLogCostTotal.ToString("#,0.00", nfi);
                            job.TotalUnPaidString = job.TotalUnPaid.ToString("#,0.00", nfi);

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

        public ReportViewModel getClientReport(Profile profile, DateTime sDate, DateTime eDate)
        {
            ReportViewModel report = null;
            try
            {
                report = new ReportViewModel();

                report.reportTitle = "Client Income and Expenses";
                report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                report.column1Name = "Client";
                report.column2Name = "Expenses by client (R)";
                report.column3Name = "Income by client (Excl. VAT) [R]";
                report.column2DataAlignRight = true;
                report.column3DataAlignRight = true;

                var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                nfi.NumberGroupSeparator = " ";

                report.ReportDataList = new List<ReportDataList>();

                decimal c2Total = 0;
                decimal c3Total = 0;

                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1)),
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_GetClientReportIncome",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {

                        foreach (DataRow row in table.Rows)
                        {
                        ReportDataList Data = new ReportDataList();
                                Data.column1Data = row["ClientName"].ToString();
                                Data.column4Data = row["ClientID"].ToString();
                                Data.column3Data = decimal.Parse(row["Income"].ToString()).ToString("#,0.00", nfi);
                            Data.column2Data = decimal.Parse("0").ToString("#,0.00", nfi);

                            report.ReportDataList.Add(Data);

                                c3Total += decimal.Parse(row["Income"].ToString());
                            }

                    }
            }

                pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1)),
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_GetClientReportTravelExpense",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            int i = 0;
                            bool added = false;

                            foreach (ReportDataList item in report.ReportDataList)
                            {
                                if(item.column4Data == row["ClientID"].ToString())
                                {
                                    report.ReportDataList[i].column2Data = decimal.Parse(row["Expenses"].ToString()).ToString("#,0.00", nfi);
                                    report.ReportDataList[i].column5Data = row["Expenses"].ToString();
                                    added = true;
                                }
                                    i++;
                            }

                            if(added == false)
                            {
                                ReportDataList Data = new ReportDataList();
                                Data.column1Data = row["ClientName"].ToString();
                                Data.column4Data = row["ClientID"].ToString();
                                Data.column2Data = decimal.Parse(row["Expenses"].ToString()).ToString("#,0.00", nfi);
                                Data.column3Data = decimal.Parse("0").ToString("#,0.00", nfi);
                                Data.column5Data = row["Expenses"].ToString();

                                report.ReportDataList.Add(Data);
                            }

                            c2Total += decimal.Parse(row["Expenses"].ToString());
                        }
                    }
                }

                pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1)),
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_GetClientReportJobExpense",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            int i = 0;
                            bool added = false;

                            foreach (ReportDataList item in report.ReportDataList)
                            {
                                if(item.column4Data == row["ClientID"].ToString())
                                {
                                    report.ReportDataList[i].column2Data = 
                                        (decimal.Parse(report.ReportDataList[i].column5Data) + 
                                        decimal.Parse(row["Expenses"].ToString())).ToString("#,0.00", nfi);
                                    added = true;
                                }
                                    i++;
                            }

                            if(added == false)
                            {
                                ReportDataList Data = new ReportDataList();
                                Data.column1Data = row["ClientName"].ToString();
                                Data.column2Data = decimal.Parse(row["Expenses"].ToString()).ToString("#,0.00", nfi);
                                Data.column3Data = decimal.Parse("0").ToString("#,0.00", nfi);
                                report.ReportDataList.Add(Data);
                            }

                            c2Total += decimal.Parse(row["Expenses"].ToString());
                        }
                    }
                }

                report.column2Total = (c2Total.ToString("#,0.00", nfi));
                report.column3Total = (c3Total.ToString("#,0.00", nfi));

                return report;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public ReportViewModel getIncomeByClientReport(Profile profile, DateTime sDate, DateTime eDate)
        {
            ReportViewModel report = null;
            try
            {
                report = new ReportViewModel();

                report.reportTitle = "Income by Client";
                report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                report.column1Name = "Client";
                report.column2Name = "Income by client (Excl. VAT) [R]";
                report.column2DataAlignRight = true;

                var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                nfi.NumberGroupSeparator = " ";

                report.ReportDataList = new List<ReportDataList>();

                decimal c2Total = 0;

                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1)),
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_GetClientReportIncome",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {

                        foreach (DataRow row in table.Rows)
                        {
                        ReportDataList Data = new ReportDataList();
                                Data.column1Data = row["ClientName"].ToString();
                                Data.column3Data = row["ClientID"].ToString();
                                Data.column2Data = decimal.Parse(row["Income"].ToString()).ToString("#,0.00", nfi);

                            report.ReportDataList.Add(Data);

                                c2Total += decimal.Parse(row["Income"].ToString());
                            }

                    }
            }

                report.column2Total = (c2Total.ToString("#,0.00", nfi));

                return report;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public ReportViewModel getExpensesByClientReport(Profile profile, DateTime sDate, DateTime eDate)
        {
            ReportViewModel report = null;
            try
            {
                report = new ReportViewModel();

                report.reportTitle = "Expenses by Client";
                report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                report.column1Name = "Client";
                report.column2Name = "Expenses by client (R)";
                report.column2DataAlignRight = true;

                var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                nfi.NumberGroupSeparator = " ";

                report.ReportDataList = new List<ReportDataList>();

                decimal c2Total = 0;

                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1)),
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_GetClientReportTravelExpense",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                                ReportDataList Data = new ReportDataList();
                                Data.column1Data = row["ClientName"].ToString();
                                Data.column4Data = row["ClientID"].ToString();
                                Data.column2Data = decimal.Parse(row["Expenses"].ToString()).ToString("#,0.00", nfi);
                                Data.column5Data = row["Expenses"].ToString();

                                report.ReportDataList.Add(Data);

                            c2Total += decimal.Parse(row["Expenses"].ToString());
                        }
                    }
                }

                pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1)),
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_GetClientReportJobExpense",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            int i = 0;
                            bool added = false;

                            foreach (ReportDataList item in report.ReportDataList)
                            {
                                if(item.column4Data == row["ClientID"].ToString())
                                {
                                    report.ReportDataList[i].column2Data = 
                                        (decimal.Parse(report.ReportDataList[i].column5Data) + 
                                        decimal.Parse(row["Expenses"].ToString())).ToString("#,0.00", nfi);
                                    added = true;
                                }
                                    i++;
                            }

                            if(added == false)
                            {
                                ReportDataList Data = new ReportDataList();
                                Data.column1Data = row["ClientName"].ToString();
                                Data.column2Data = decimal.Parse(row["Expenses"].ToString()).ToString("#,0.00", nfi);
                                report.ReportDataList.Add(Data);
                            }

                            c2Total += decimal.Parse(row["Expenses"].ToString());
                        }
                    }
                }

                report.column2Total = (c2Total.ToString("#,0.00", nfi));

                return report;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public ReportViewModel getClientReport(Profile profile, DateTime sDate, DateTime eDate, string DropDownID)
        {
            ReportViewModel report = null;
            try
            {
                report = new ReportViewModel();

                report.reportTitle = "Client Income and Expenses";
                report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                report.column1Name = "Client";
                report.column2Name = "Expenses by client (R)";
                report.column3Name = "Income by client (Excl. VAT) [R]";
                report.column2DataAlignRight = true;
                report.column3DataAlignRight = true;

                var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                nfi.NumberGroupSeparator = " ";

                report.ReportDataList = new List<ReportDataList>();

                decimal c2Total = 0;
                decimal c3Total = 0;

                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1)),
                        new SqlParameter("@CID", DropDownID)
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_GetClientReportIncomeByClient",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {

                        foreach (DataRow row in table.Rows)
                        {
                        ReportDataList Data = new ReportDataList();
                                Data.column1Data = row["ClientName"].ToString();
                                Data.column4Data = row["ClientID"].ToString();
                                Data.column3Data = decimal.Parse(row["Income"].ToString()).ToString("#,0.00", nfi);
                            Data.column2Data = decimal.Parse("0").ToString("#,0.00", nfi);
                            report.reportCondition = "For " + Data.column1Data + " From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                
                            report.ReportDataList.Add(Data);

                                c3Total += decimal.Parse(row["Income"].ToString());
                            }

                    }
            }

                pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1)),
                        new SqlParameter("@CID", DropDownID)
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_GetClientReportTravelExpenseByClient",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            int i = 0;
                            bool added = false;

                            foreach (ReportDataList item in report.ReportDataList)
                            {
                                if(item.column4Data == row["ClientID"].ToString())
                                {
                                    report.ReportDataList[i].column2Data = decimal.Parse(row["Expenses"].ToString()).ToString("#,0.00", nfi);
                                    report.ReportDataList[i].column5Data = row["Expenses"].ToString();
                                    added = true;
                                }
                                    i++;
                            }

                            if(added == false)
                            {
                                ReportDataList Data = new ReportDataList();
                                Data.column1Data = row["ClientName"].ToString();
                                Data.column4Data = row["ClientID"].ToString();
                                Data.column2Data = decimal.Parse(row["Expenses"].ToString()).ToString("#,0.00", nfi);
                                Data.column3Data = decimal.Parse("0").ToString("#,0.00", nfi);
                                Data.column5Data = row["Expenses"].ToString();
                                report.reportCondition = "For " + Data.column1Data + " From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");


                                report.ReportDataList.Add(Data);
                            }

                            c2Total += decimal.Parse(row["Expenses"].ToString());
                        }
                    }
                }

                pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1)),
                        new SqlParameter("@CID", DropDownID)
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_GetClientReportJobExpenseByClient",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            int i = 0;
                            bool added = false;

                            foreach (ReportDataList item in report.ReportDataList)
                            {
                                if(item.column4Data == row["ClientID"].ToString())
                                {
                                    report.ReportDataList[i].column2Data = 
                                        (decimal.Parse(report.ReportDataList[i].column5Data) + 
                                        decimal.Parse(row["Expenses"].ToString())).ToString("#,0.00", nfi);
                                    added = true;
                                }
                                    i++;
                            }

                            if(added == false)
                            {
                                ReportDataList Data = new ReportDataList();
                                Data.column1Data = row["ClientName"].ToString();
                                Data.column2Data = decimal.Parse(row["Expenses"].ToString()).ToString("#,0.00", nfi);
                                Data.column3Data = decimal.Parse("0").ToString("#,0.00", nfi);
                                report.ReportDataList.Add(Data);
                                report.reportCondition = "For " + Data.column1Data + " From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");

                            }

                            c2Total += decimal.Parse(row["Expenses"].ToString());
                        }
                    }
                }

                report.column2Total = (c2Total.ToString("#,0.00", nfi));
                report.column3Total = (c3Total.ToString("#,0.00", nfi));

                return report;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public ReportViewModel getIncomeByClientReport(Profile profile, DateTime sDate, DateTime eDate, string DropDownID)
        {
            ReportViewModel report = null;
            try
            {
                report = new ReportViewModel();

                report.reportTitle = "Income by Client";
                report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                report.column1Name = "Client";
                report.column2Name = "Income by client (Excl. VAT) [R]";
                report.column2DataAlignRight = true;

                var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                nfi.NumberGroupSeparator = " ";

                report.ReportDataList = new List<ReportDataList>();

                decimal c2Total = 0;

                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1)),
                        new SqlParameter("@CID", DropDownID)
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_GetClientReportIncomeByClient",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {

                        foreach (DataRow row in table.Rows)
                        {
                        ReportDataList Data = new ReportDataList();
                                Data.column1Data = row["ClientName"].ToString();
                                Data.column3Data = row["ClientID"].ToString();
                                Data.column2Data = decimal.Parse(row["Income"].ToString()).ToString("#,0.00", nfi);

                            report.ReportDataList.Add(Data);
                            report.reportCondition = "For " + Data.column1Data + " From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");


                            c2Total += decimal.Parse(row["Income"].ToString());
                            }

                    }
            }

                report.column2Total = (c2Total.ToString("#,0.00", nfi));

                return report;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        public ReportViewModel getExpensesByClientReport(Profile profile, DateTime sDate, DateTime eDate, string DropDownID)
        {
            ReportViewModel report = null;
            try
            {
                report = new ReportViewModel();

                report.reportTitle = "Expenses by Client";
                report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                report.column1Name = "Client";
                report.column2Name = "Expenses by client (R)";
                report.column2DataAlignRight = true;

                var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                nfi.NumberGroupSeparator = " ";

                report.ReportDataList = new List<ReportDataList>();

                decimal c2Total = 0;

                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1)),
                        new SqlParameter("@CID", DropDownID)
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_GetClientReportTravelExpenseByClient",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                                ReportDataList Data = new ReportDataList();
                                Data.column1Data = row["ClientName"].ToString();
                                Data.column4Data = row["ClientID"].ToString();
                                Data.column2Data = decimal.Parse(row["Expenses"].ToString()).ToString("#,0.00", nfi);
                                Data.column5Data = row["Expenses"].ToString();

                                report.ReportDataList.Add(Data);
                            report.reportCondition = "For " + Data.column1Data + " From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");


                            c2Total += decimal.Parse(row["Expenses"].ToString());
                        }
                    }
                }

                pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1)),
                        new SqlParameter("@CID", DropDownID)
                    };

                using (DataTable table = DBHelper.ParamSelect("SP_GetClientReportJobExpenseByClient",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            int i = 0;
                            bool added = false;

                            foreach (ReportDataList item in report.ReportDataList)
                            {
                                if(item.column4Data == row["ClientID"].ToString())
                                {
                                    report.ReportDataList[i].column2Data = 
                                        (decimal.Parse(report.ReportDataList[i].column5Data) + 
                                        decimal.Parse(row["Expenses"].ToString())).ToString("#,0.00", nfi);
                                    added = true;
                                }
                                    i++;
                            }

                            if(added == false)
                            {
                                ReportDataList Data = new ReportDataList();
                                Data.column1Data = row["ClientName"].ToString();
                                Data.column2Data = decimal.Parse(row["Expenses"].ToString()).ToString("#,0.00", nfi);
                                report.ReportDataList.Add(Data);
                                report.reportCondition = "For " + Data.column1Data + " From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");

                            }

                            c2Total += decimal.Parse(row["Expenses"].ToString());
                        }
                    }
                }

                report.column2Total = (c2Total.ToString("#,0.00", nfi));

                return report;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }

        public List<SP_GetGeneralExpense_Result> getGeneralExpensesReport(Profile profileID, DateTime sDate, DateTime eDate)
        {
            List<SP_GetGeneralExpense_Result> Expenses = new List<SP_GetGeneralExpense_Result>();
            try
            {
                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profileID.ProfileID),
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1))
                    };


                using (DataTable table = DBHelper.ParamSelect("SP_GetGeneralExpensesReport",
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
                            if (row["Invoice/ReceiptCopy"] != null && row["Invoice/ReceiptCopy"].ToString() != "")
                            {
                                expense.Invoice_ReceiptCopy = Encoding.ASCII.GetBytes(row["Invoice/ReceiptCopy"].ToString());
                            }
                            expense.CatName = row[9].ToString();
                            expense.CatDescription = row[10].ToString();
                            expense.DateString = expense.Date.ToString("dddd, dd MMMM yyyy");
                            expense.PrimaryExpenseID = int.Parse(row["PrimaryExpenseID"].ToString());
                            Expense expenseID = new Expense();
                            expenseID.ExpenseID = expense.ExpenseID;
                            expense.RepeatOccurrences = getGeneralExpenseRepeatOccurrence(expenseID);
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

        public ReportViewModel getJobEarningPerHourReport(Profile profile, DateTime sDate, DateTime eDate)
        {
            ReportViewModel report = null;
            try
            {
                report = new ReportViewModel();

                report.reportTitle = "Job Earnings per Hour";
                report.reportSubHeading = "After expenses";
                report.reportCondition = "From " + sDate.ToString("dd MMM yyyy") + " to " + eDate.ToString("dd MMM yyyy");
                report.reportStartDate = sDate.ToString("yyyy-MM-dd");
                report.reportEndDate = eDate.ToString("yyyy-MM-dd");

                report.column1Name = "Start Date";
                report.column2Name = "Job Title";
                report.column3Name = "Job Hourly Rate (R)";
                report.column4Name = "Income per Hour (Excl. VAT) [R]";
                report.column3DataAlignRight = true;
                report.column4DataAlignRight = true;

                var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                nfi.NumberGroupSeparator = " ";

                report.ReportDataList = new List<ReportDataList>();

                SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1)),
                    };

                #region Job Income
                List<SP_GetJob_Result> TotalPaids = new List<SP_GetJob_Result>();

                using (DataTable table = DBHelper.ParamSelect("SP_JobEarnings",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            SP_GetJob_Result TotalPaid = new Model.SP_GetJob_Result();

                            TotalPaid.JobID = int.Parse(row["JobID"].ToString());

                            if (row["TotalPaid"].ToString() != "" && row["TotalPaid"] != null)
                            {
                                TotalPaid.TotalPaid = decimal.Parse(row["TotalPaid"].ToString());
                            }
                            else
                            {
                                TotalPaid.TotalPaid = 0;
                            }

                            if (row["ExpenseTotal"].ToString() != "" && row["ExpenseTotal"] != null)
                            {
                                TotalPaid.TotalPaid -= decimal.Parse(row["ExpenseTotal"].ToString());
                            }

                            if (row["TravelLogCostTotal"].ToString() != "" && row["TravelLogCostTotal"] != null)
                            {
                                TotalPaid.TotalPaid -= decimal.Parse(row["TravelLogCostTotal"].ToString());
                            }
                            else
                            {
                                TotalPaid.TotalPaid = 0;
                            }

                            TotalPaids.Add(TotalPaid);
                        }
                    }
                }
                #endregion

                pars = new SqlParameter[]
                    {
                        new SqlParameter("@PID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@CID", profile.ProfileID),
                        //***************************************//
                        new SqlParameter("@SD", sDate.AddDays(-1)),
                        new SqlParameter("@ED", eDate.AddDays(+1)),
                    };

                #region jobHours
                using (DataTable table = DBHelper.ParamSelect("SP_WorklogHours",
            CommandType.StoredProcedure, pars))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            bool add = false;
                            ReportDataList Data = new ReportDataList();
                            Data.column2Data = row["JobTitle"].ToString();
                            Data.column1Data = DateTime.Parse(row["StartDate"].ToString()).ToString("dd MMM yyyy");

                            if (row["HourlyRate"].ToString() != "" && row["HourlyRate"] != null)
                            {
                                Data.column3Data = decimal.Parse(row["HourlyRate"].ToString()).ToString("#,0.00", nfi);
                            }
                            else
                            {
                                Data.column3Data = "N/A";
                            }

                            foreach (SP_GetJob_Result item in TotalPaids)
                            {
                                if (item.JobID == int.Parse(row["JobID"].ToString()))
                                {

                                if (row["WorkLogHours"].ToString() != "" && row["WorkLogHours"] != null)
                                {
                                    Data.column4Data = (item.TotalPaid/(decimal.Parse(row["WorkLogHours"].ToString())/60)).ToString("#,0.00", nfi);
                                }
                                else
                                {
                                    Data.column4Data = "N/A";
                                }

                                report.ReportDataList.Add(Data);
                                    add = true;
                                }
                            }

                            if (add == false)
                            {
                                Data.column4Data = "0";

                                report.ReportDataList.Add(Data);
                            }
                        }
                    }
                }
                #endregion

                return report;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
        #endregion
    }
}                  