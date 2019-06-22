﻿using System;
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

        public Job getJob(Job Job)
        {
            Model.Job job = null;

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
                            job = new Model.Job();
                            job.JobID = int.Parse(row[0].ToString());
                            job.ClientID = int.Parse(row[1].ToString());
                            job.JobTitle = row[2].ToString();
                            job.StartDate = DateTime.Parse(row[5].ToString());
                            if(row[6] != null)
                            {
                                job.EndDate = DateTime.Parse(row[6].ToString());
                            }
                            job.HourlyRate = decimal.Parse(row[3].ToString());
                            job.Budget = decimal.Parse(row[4].ToString());
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
        #endregion
    }
}                  
