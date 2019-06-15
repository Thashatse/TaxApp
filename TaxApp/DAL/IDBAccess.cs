using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDBAccess
    {
        #region Bussiness
        Model.Business GetBussiness();
        #endregion
    }
}
