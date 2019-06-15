using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class DBHandler : IDBHandler
    {
        private IDBAccess db;

        public DBHandler()
        {
            db = new DBAccess();
        }

        #region Bussiness
        public Model.Business GetBussiness()
            {
            return db.GetBussiness();
        }
        #endregion
    }
}
