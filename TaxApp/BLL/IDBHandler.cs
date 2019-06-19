using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IDBHandler
    {
        #region Profile
        bool newprofile(Model.Profile user);
        Model.Profile getProfile(Model.Profile User);
        #endregion

        #region Bussiness
        Model.Business GetBussiness();
        #endregion

        #region Tax Consultanat
        bool newConsultant(Model.TaxConsultant consultant);
        Model.TaxConsultant getConsumtant(Model.TaxConsultant consultant);
        #endregion

        #region Email Settings
        bool newEmailSettings(Model.EmailSetting Settings);
        Model.EmailSetting getEmailSettings(Model.EmailSetting Settings);
        #endregion
    }
}

