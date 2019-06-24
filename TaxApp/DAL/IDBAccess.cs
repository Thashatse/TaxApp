using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL
{
    public interface IDBAccess
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
        Model.TaxConsultant getConsultant(Model.TaxConsultant consultant);
        #endregion

        #region Email Settings
        bool newEmailSettings(Model.EmailSetting Settings);
        Model.EmailSetting getEmailSettings(Model.EmailSetting Settings);
        #endregion

        #region Job
        bool newJob(Model.Job job);
        Model.Job getJob(Model.Job job);
        List<Job> getProfileJobs(Profile profile);
        
        #region WorkLog Item
        bool newWorkLogItem(Model.Worklog logItem, Model.Job job);
        #endregion
        #endregion

        #region Client
        bool newClient(Model.Client client);
        Model.Client getClient(Model.Client client);

        List<Client> getProfileClients(Client client);
        #endregion
    }
}
