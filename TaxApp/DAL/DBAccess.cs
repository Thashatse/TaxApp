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
        #region Bussiness
        public TaxApp.Models.Business GetHomePageFeatures()
        {
            TaxApp.Models.Business business = new TaxApp.Models.Business();

            try
            {
                using (DataTable table = DBHelper.Select("SP_GetBussiness",
            CommandType.StoredProcedure))
                {
                        foreach (DataRow row in table.Rows)
                        {
                        business.BusinessID = row["FeatureID"].ToString();
                        business.VATRate = Convert.ToDecimal(row["ItemID"].ToString());
                        business.SMSSid = row["ItemID"].ToString();
                        business.SMSToken = row["ItemID"].ToString();
                    }
                }
                return business;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
    }
}                  
