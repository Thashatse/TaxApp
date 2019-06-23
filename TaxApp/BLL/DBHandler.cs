using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;

namespace BLL
{
    public class DBHandler : IDBHandler
    {
        private IDBAccess db;

        public DBHandler()
        {
            db = new DBAccess();
        }

        #region Profile
        public bool newprofile(Model.Profile user)
        {
            return db.newprofile(user);
        }
        public Model.Profile getProfile(Model.Profile User)
        {
            return db.getProfile(User);
        }
        #endregion

        #region Bussiness
        public Model.Business GetBussiness()
            {
            return db.GetBussiness();
        }
        #endregion

        #region Tax Consultanat
        public bool newConsultant(Model.TaxConsultant consultant)
        {
            return db.newConsultant(consultant);
        }
        public Model.TaxConsultant getConsumtant(Model.TaxConsultant consultant)
        {
            return db.getConsultant(consultant);
        }
        #endregion

        #region Email Settings
        public bool newEmailSettings(Model.EmailSetting Settings)
        {
            return db.newEmailSettings(Settings);
        }
        public Model.EmailSetting getEmailSettings(Model.EmailSetting Settings)
        {
            return db.getEmailSettings(Settings);
        }
        #endregion
        
        #region Job
        public bool newJob(Model.Job job)
        {
            return db.newJob(job);
        }
        public Model.Job getJob(Model.Job job)
        {
            return db.getJob(job);
        }
        #endregion

        #region Client
        public bool newClient(Model.Client client)
        {
            return db.newClient(client);
        }
        public Model.Client getClient (Model.Client client)
        {
            return db.getClient(client);
        }
        public List<Client> getProfileClients(Client client)
        {
            return db.getProfileClients(client);
        }
        #endregion
    }
}
