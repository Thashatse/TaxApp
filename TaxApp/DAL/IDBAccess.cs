﻿using System;
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
        Model.SP_GetJob_Result getJob(Model.Job job);
        List<Job> getProfileJobs(Profile profile);
        #endregion

        #region WorkLog Item
        bool newWorkLogItem(Model.Worklog logItem, Model.Job job);
        Worklog getLogItem(Model.Worklog logID);
        List<Worklog> getJobHours(Job JobID);
        #endregion

        #region Client
        bool newClient(Model.Client client);
        Model.Client getClient(Model.Client client);

        List<Client> getProfileClients(Client client);
        #endregion

        #region Expense
        bool newJobExpense(SP_GetJobExpense_Result newJobExpense);
        SP_GetJobExpense_Result getJobExpense(Expense expenseID);
        List<SP_GetJobExpense_Result> getJobExpenses(Job jobID);
        bool newGeneralExpense(SP_GetGeneralExpense_Result newGeneralExpense);
        SP_GetGeneralExpense_Result getGeneralExpense(Expense expenseID);
        List<SP_GetGeneralExpense_Result> getGeneralExpenses(Profile profileID);
        List<ExpenseCategory> getExpenseCatagories();
        bool NewTravelExpense(TravelLog newTravelLogExpense);
        bool newVehicle(Vehicle newVehicle);
        List<Vehicle> getVehicles(Profile getProfileVehicles);
        #endregion

        #region Invoice
        int getInvoiceTodaysCount();

        List<List<SP_GetJobIntemsToInvoice_Result>> getJobItemsForInvoice(Job jobID);

        bool newInvoiceDetailLine(InvoiceLineItem newInvoiceLineItem);
        bool newInvoice(Invoice newInvoice, Job jobID);
        #endregion
    }
}
