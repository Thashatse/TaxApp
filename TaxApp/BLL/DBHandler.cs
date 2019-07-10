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
        public SP_GetJob_Result getJob(Model.Job job)
        {
            return db.getJob(job);
        }

        public List<SP_GetJob_Result> getProfileJobs(Profile profile)
        {
            return db.getProfileJobs(profile);
        }
        public List<SP_GetJob_Result> getProfileJobsPast(Profile profile)
        {
            return db.getProfileJobsPast(profile);
        }

        #region WorkLog Item
        public bool newWorkLogItem(Model.Worklog logItem, Model.Job job)
        {
            return db.newWorkLogItem(logItem, job);
        }
        public Worklog getLogItem(Model.Worklog logID)
        {
            return db.getLogItem(logID);
        }
        public List<Worklog> getJobHours(Job JobID)
        {
            return db.getJobHours(JobID);
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
        public bool NewTravelExpense(TravelLog newTravelLogExpense)
        {
            return db.NewTravelExpense(newTravelLogExpense);
        }
        public bool newVehicle(Vehicle newVehicle)
        {
            return db.newVehicle(newVehicle);
        }
        public List<Vehicle> getVehicles(Profile getProfileVehicles)
        {
            return db.getVehicles(getProfileVehicles);
        }
        public List<TravelLog> getProfileTravelLog(Profile getProfileTravelLog)
        {
            return db.getProfileTravelLog(getProfileTravelLog);
        }
        public List<TravelLog> getJobTravelLog(Job getJobTravelLog)
        {
            return db.getJobTravelLog(getJobTravelLog);
        }
        #endregion

        #region Invoice
        public int getInvoiceTodaysCount()
        {
            return db.getInvoiceTodaysCount();
        }

        public List<List<SP_GetJobIntemsToInvoice_Result>> getJobItemsForInvoice(Job jobID)
        {
            return db.getJobItemsForInvoice(jobID);
        }

        public bool newInvoiceDetailLine(InvoiceLineItem newInvoiceLineItem)
        {
            return db.newInvoiceDetailLine(newInvoiceLineItem);
        }
        public bool newInvoice(Invoice newInvoice, Job jobID)
        {
            return db.newInvoice(newInvoice, jobID);
        }
        public List<SP_GetInvoice_Result> getJobInvoices(Job jobID)
        {
            return db.getJobInvoices(jobID);
        }
        public List<SP_GetInvoice_Result> getInvoices(Profile profileID)
        {
            return db.getInvoices(profileID);
        }
        public List<SP_GetInvoice_Result> getInvoiceDetails(Invoice invoiceNum)
        {
            return db.getInvoiceDetails(invoiceNum);
        }
        #endregion
    }
}
