﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

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
                        business.BusinessID = row["BusinessID"].ToString();
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
    }
}                  
