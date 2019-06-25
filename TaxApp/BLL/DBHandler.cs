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

        public List<Job> getProfileJobs(Profile profile)
        {
            return db.getProfileJobs(profile);
        }

        #region WorkLog Item
        public bool newWorkLogItem(Model.Worklog logItem, Model.Job job)
        {
            return db.newWorkLogItem(logItem, job);
        }
        #endregion
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

        #region Expense
        public bool newJobExpense(SP_GetJobExpense_Result newJobExpense)
        {
            return db.newJobExpense(newJobExpense);
        }
        public SP_GetJobExpense_Result getJobExpense(Expense expenseID)
        {
            return db.getJobExpense(expenseID);
        }
        public List<SP_GetJobExpense_Result> getJobExpenses(Job jobID)
        {
            return db.getJobExpenses(jobID);
        }
        public bool newGeneralExpense(SP_GetGeneralExpense_Result newGeneralExpense)
        {
            return db.newGeneralExpense(newGeneralExpense);
        }
        public SP_GetGeneralExpense_Result getGeneralExpense(Expense expenseID)
        {
            return db.getGeneralExpense(expenseID);
        }
        public List<SP_GetGeneralExpense_Result> getGeneralExpenses(Profile profileID)
        {
            return db.getGeneralExpenses(profileID);
        }
        public List<ExpenseCategory> getExpenseCatagories()
        {
            return db.getExpenseCatagories();
        }
        #endregion
    }
}
