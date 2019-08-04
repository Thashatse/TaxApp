using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BLL
{
    public interface IDBHandler
    {
        #region Dashboard Details
        DashboardIncomeExpense getDashboardIncomeExpense(Profile profile);
        #endregion

        #region Profile
        bool newprofile(Model.Profile user);
        bool editprofile(Model.Profile User);
        Model.Profile getProfile(Model.Profile User);
        #endregion

        #region Bussiness
        Model.Business GetBussiness();
        #endregion

        #region Tax Consultanat
        bool newConsultant(Model.TaxConsultant consultant);
        Model.TaxConsultant getConsumtant(Model.TaxConsultant consultant);
        bool EditTaxConsultant(TaxConsultant consultant);
        #endregion

        #region Email Settings
        bool newEmailSettings(Model.EmailSetting Settings);
        Model.EmailSetting getEmailSettings(Model.EmailSetting Settings);
        #endregion

        #region Job
        bool newJob(Model.Job job);
        bool editJob(Job job);
        SP_GetJob_Result getJob(Model.Job job);
        List<SP_GetJob_Result> getProfileJobs(Profile profile);
        List<SP_GetJob_Result> getProfileJobsPast(Profile profile);
        List<SP_GetJob_Result> getProfileJobsDashboard(Profile profile);
        bool MarkJobAsComplete(Job job);

        #endregion

        #region WorkLog Item
        bool newWorkLogItem(Model.Worklog logItem, Model.Job job);
        Worklog getLogItem(Model.Worklog logID);
        List<Worklog> getJobHours(Job JobID);
        bool EditWorkLogItem(Model.Worklog logItem);
        bool DeleteWorkLogItem(Model.Worklog logItem);
        #endregion

        #region Client
        bool newClient(Model.Client client);
        Model.Client getClient(Model.Client client);
        List<Client> getProfileClients(Client client);
        bool editClient(Client client);
        #endregion

        #region Expense
        bool newJobExpense(SP_GetJobExpense_Result newJobExpense);
        SP_GetJobExpense_Result getJobExpense(Expense expenseID);
        List<SP_GetJobExpense_Result> getJobExpenses(Job jobID);
        List<SP_GetJobExpense_Result> getAllJobExpense(Profile profileID);
        bool newGeneralExpense(SP_GetGeneralExpense_Result newGeneralExpense);
        SP_GetGeneralExpense_Result getGeneralExpense(Expense expenseID);
        List<SP_GetGeneralExpense_Result> getGeneralExpenses(Profile profileID);
        List<ExpenseCategory> getExpenseCatagories();
        bool NewTravelExpense(TravelLog newTravelLogExpense);
        bool DeleteTravelExpense(TravelLog TravelLogExpense);
        bool EditTravelExpense(TravelLog newTravelLogExpense);
        bool newVehicle(Vehicle newVehicle);
        List<Vehicle> getVehicles(Profile getProfileVehicles);
        List<TravelLog> getProfileTravelLog(Profile getProfileTravelLog);
        List<TravelLog> getJobTravelLog(Job getJobTravelLog);
        TravelLog getTravelLogItem(TravelLog getTravelLogItem);
        bool UpdateGeneralExpenseRepeate(SP_GetGeneralExpense_Result expense);
        List<SP_GetGeneralExpense_Result> getRepeatGeneralExpenses();
        #endregion

        #region Invoice
        int getInvoiceTodaysCount();
        List<List<SP_GetJobIntemsToInvoice_Result>> getJobItemsForInvoice(Job jobID);
        bool newInvoiceDetailLine(InvoiceLineItem newInvoiceLineItem);
        bool newInvoice(Invoice newInvoice, Job jobID);
        List<SP_GetInvoice_Result> getJobInvoices(Job jobID);
        List<SP_GetInvoice_Result> getInvoices(Profile profileID);
        List<SP_GetInvoice_Result> getInvoiceDetails(Invoice invoiceNum);
        List<SP_GetInvoice_Result> getInvoicesOutsatanding(Profile profileID);
        List<SP_GetInvoice_Result> getInvoicesPast(Profile profileID);
        bool MarkInvoiceAsPaid(Invoice invoice);
        DashboardIncome getIncomeDashboard(Profile profile);
        #endregion

        #region TaxAndVatPeriods
        List<TaxAndVatPeriods> getTaxOrVatPeriodForProfile(Profile profileID, char type);
        bool newTaxOrVatPeriod(TaxAndVatPeriods newPeriod);
        #endregion

        #region Tax Period Brakets
        List<TaxPeriodRates> getTaxPeriodBrakets(TaxAndVatPeriods getBrakets);
        bool newPeriodTaxBraket(TaxPeriodRates newBraket);
        #endregion

        #region VAT Center
        VATDashboard getVatCenterDashboard(Profile profile, TaxAndVatPeriods period);
        List<TAXorVATRecivedList> getVATRecivedList(Profile profile, TaxAndVatPeriods period);
        TaxAndVatPeriods SP_GetLatestTaxAndVatPeriodID();
        #endregion

        #region Tax Center
        TaxDashboard getTaxCenterDashboard(Profile profile, TaxAndVatPeriods period);
        List<TAXorVATRecivedList> getTAXRecivedList(Profile profile, TaxAndVatPeriods period, TaxPeriodRates rate);
        #endregion

        #region File Upload Download
        bool addGeneralExpenseFile(FileUpload newFile);
        FileUpload getGeneralExpenseFile(FileUpload getFile);
        bool addJobExpenseFile(FileUpload newFile);
        FileUpload getJobExpenseFile(FileUpload getFile);
        #endregion

        #region Vehicle
        bool editVehicle(Vehicle editVehicle);
        Vehicle getVehicle(Vehicle getVehicle);
        #endregion
    }
}

