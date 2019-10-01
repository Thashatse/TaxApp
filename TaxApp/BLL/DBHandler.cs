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

        #region Dashboard Details
        public DashboardIncomeExpense getDashboardIncomeExpense(Profile profile)
        {
            return db.getDashboardIncomeExpense(profile);
        }
        #endregion

        #region Profile
        public bool newprofile(Model.Profile user)
        {
            return db.newprofile(user);
        }
        public Model.Profile getProfile(Model.Profile User)
        {
            return db.getProfile(User);
        }
        public bool editprofile(Model.Profile User)
        {
            return db.editprofile(User);
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
        public bool EditTaxConsultant(TaxConsultant consultant)
        {
            return db.EditTaxConsultant(consultant);
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
        public string newJob(Model.Job job)
        {
            return db.newJob(job);
        }
        public bool editJob(Job job)
        {
            return db.editJob(job);
        }
        public SP_GetJob_Result getJob(Model.Job job)
        {
            return db.getJob(job);
        }

        public List<SP_GetJob_Result> getProfileJobs(Profile profile, Client client)
        {
            return db.getProfileJobs(profile, client);
        }
        public List<SP_GetJob_Result> getProfileJobsPast(Profile profile, Client client, DateTime sDate, DateTime eDate)
        {
            return db.getProfileJobsPast(profile, client, sDate, eDate);
        }
        public List<SP_GetJob_Result> getProfileJobsDashboard(Profile profile)
        {
            return db.getProfileJobsDashboard(profile);
        }
        public bool MarkJobAsComplete(Job job)
        {
            return db.MarkJobAsComplete(job);
        }
        #endregion

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

        public bool EditWorkLogItem(Model.Worklog logItem)
        {
            return db.EditWorkLogItem(logItem);
        }
        public bool DeleteWorkLogItem(Model.Worklog logItem)
        {
            return db.DeleteWorkLogItem(logItem);
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
        public bool editClient(Client client)
        {
            return db.editClient(client);
        }
        #endregion

        #region Expense
        public bool newJobExpense(SP_GetJobExpense_Result newJobExpense)
        {
            return db.newJobExpense(newJobExpense);
        }
        public bool updateJobExpense(SP_GetJobExpense_Result updateJobExpense)
        {
            return db.updateJobExpense(updateJobExpense);
        }
        public SP_GetJobExpense_Result getJobExpense(Expense expenseID)
        {
            return db.getJobExpense(expenseID);
        }
        public List<SP_GetJobExpense_Result> getAllJobExpense(Profile profileID, DateTime SD, DateTime ED)
        {
            return db.getAllJobExpense(profileID, SD, ED);
        }
        public List<SP_GetJobExpense_Result> getJobExpenses(Job jobID)
        {
            return db.getJobExpenses(jobID);
        }
        public bool newGeneralExpense(SP_GetGeneralExpense_Result newGeneralExpense)
        {
            return db.newGeneralExpense(newGeneralExpense);
        }
        public bool updateGeneralExpense(SP_GetGeneralExpense_Result updateGeneralExpense)
        {
            return db.updateGeneralExpense(updateGeneralExpense);
        }
        public SP_GetGeneralExpense_Result getGeneralExpense(Expense expenseID)
        {
            return db.getGeneralExpense(expenseID);
        }
        public List<SP_GetGeneralExpense_Result> getGeneralExpenses(Profile profileID, DateTime sDate, DateTime eDate)
        {
            return db.getGeneralExpenses(profileID, sDate, eDate);
        }
        public List<SP_GetGeneralExpense_Result> getGeneralExpenseRepeatOccurrence(Expense expenseID)
        {
            return db.getGeneralExpenseRepeatOccurrence(expenseID);
        }
        public List<ExpenseCategory> getExpenseCatagories()
        {
            return db.getExpenseCatagories();
        }
        public bool NewTravelExpense(TravelLog newTravelLogExpense)
        {
            return db.NewTravelExpense(newTravelLogExpense);
        }
        public bool DeleteTravelExpense(TravelLog TravelLogExpense)
        {
            return db.DeleteTravelExpense(TravelLogExpense);
        }
        public bool EditTravelExpense(TravelLog TravelLogExpense)
        {
            return db.EditTravelExpense(TravelLogExpense);
        }
        public bool newVehicle(Vehicle newVehicle)
        {
            return db.newVehicle(newVehicle);
        }
        public List<Vehicle> getVehicles(Profile getProfileVehicles)
        {
            return db.getVehicles(getProfileVehicles);
        }
        public List<TravelLog> getProfileTravelLog(Profile getProfileTravelLog, DateTime sDate, DateTime eDate)
        {
            return db.getProfileTravelLog(getProfileTravelLog, sDate, eDate);
        }
        public List<TravelLog> getProfileTravelLog(Profile getProfileTravelLog, DateTime sDate, DateTime eDate, string DDID)
        {
            return db.getProfileTravelLog(getProfileTravelLog, sDate, eDate, DDID);
        }
        public List<TravelLog> getJobTravelLog(Job getJobTravelLog)
        {
            return db.getJobTravelLog(getJobTravelLog);
        }
        public TravelLog getTravelLogItem(TravelLog getTravelLogItem)
        {
            return db.getTravelLogItem(getTravelLogItem);
        }
        public List<SP_GetGeneralExpense_Result> getRepeatGeneralExpenses()
        {
            return db.getRepeatGeneralExpenses();
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
        public List<SP_GetInvoice_Result> getInvoices(Profile profileID, Client client)
        {
            return db.getInvoices(profileID, client);
        }public List<SP_GetInvoice_Result> getInvoicesOutsatanding(Profile profileID)
        {
            return db.getInvoicesOutsatanding(profileID);
        }
        public List<SP_GetInvoice_Result> getInvoicesPast(Profile profileID, DateTime sDate, DateTime eDate)
        {
            return db.getInvoicesPast(profileID, sDate, eDate);
        }
        public List<SP_GetInvoice_Result> getInvoiceDetails(Invoice invoiceNum)
        {
            return db.getInvoiceDetails(invoiceNum);
        }
        public bool MarkInvoiceAsPaid(Invoice invoice)
        {
            return db.MarkInvoiceAsPaid(invoice);
        }
        public DashboardIncome getIncomeDashboard(Profile profile)
        {
            return db.getIncomeDashboard(profile);
        }
        #endregion
        
        #region TaxAndVatPeriods
        public List<TaxAndVatPeriods> getTaxOrVatPeriodForProfile(Profile profileID, char type)
        {
            return db.getTaxOrVatPeriodForProfile(profileID, type);
        }
        public bool newTaxOrVatPeriod(TaxAndVatPeriods newPeriod)
        {
            return db.newTaxOrVatPeriod(newPeriod);
        }
        public Tuple<TaxAndVatPeriods, TaxConsultant> UpdateShareTaxorVatPeriod(TaxAndVatPeriods PeriodID)
        {
            return db.UpdateShareTaxorVatPeriod(PeriodID);
        }
        public bool UpdateShareJob(Job JobID)
        {
            return db.UpdateShareJob(JobID);
        }

        public
        bool editTaxOrVatPeriod(TaxAndVatPeriods editPeriod)
        {
            return db.editTaxOrVatPeriod(editPeriod);
        }
        public bool deletePeriodTaxBraket(TaxPeriodRates deleteBraket)
        {
            return db.deletePeriodTaxBraket(deleteBraket);
        }
        #endregion

        #region Tax Period Brakets
        public List<TaxPeriodRates> getTaxPeriodBrakets(TaxAndVatPeriods getBrakets)
        {
            return db.getTaxPeriodBrakets(getBrakets);
        }
        public bool newPeriodTaxBraket(TaxPeriodRates newBraket)
        {
            return db.newPeriodTaxBraket(newBraket);
        }
        #endregion

        #region VAT Center
        public VATDashboard getVatCenterDashboard(Profile profile, TaxAndVatPeriods period)
        {
            return db.getVatCenterDashboard(profile, period);
        }
        public List<TAXorVATRecivedList> getVATRecivedList(Profile profile, TaxAndVatPeriods period)
        {
            return db.getVATRecivedList(profile, period);
        }

        public TaxAndVatPeriods SP_GetLatestTaxAndVatPeriodID()
        {
            return db.SP_GetLatestTaxAndVatPeriodID();
        }
        #endregion

        #region Tax Center
        public TaxDashboard getTaxCenterDashboard(Profile profile, TaxAndVatPeriods period)
        {
            return db.getTaxCenterDashboard(profile, period);
        }
        public List<TAXorVATRecivedList> getTAXRecivedList(Profile profile, TaxAndVatPeriods period, TaxPeriodRates rate)
        {
            return db.getTAXRecivedList(profile, period, rate);
        }
        #endregion

        #region File Upload Download
        public bool addGeneralExpenseFile(InvoiceAndReciptesFile newFile)
        {
            return db.addGeneralExpenseFile(newFile);
        }
        public InvoiceAndReciptesFile getGeneralExpenseFile(InvoiceAndReciptesFile getFile)
        {
            return db.getGeneralExpenseFile(getFile);
        }
        public bool addJobExpenseFile(InvoiceAndReciptesFile newFile)
        {
            return db.addJobExpenseFile(newFile);
        }
        public InvoiceAndReciptesFile getJobExpenseFile(InvoiceAndReciptesFile getFile)
        {
            return db.getJobExpenseFile(getFile);
        }
        public List<InvoiceAndReciptesFile> getInvoiceAndReciptesFiles(Profile profileID, DateTime SD, DateTime ED)
        {
            return db.getInvoiceAndReciptesFiles(profileID, SD, ED);
        }
        #endregion

        #region Vehicle
        public bool editVehicle(Vehicle editVehicle)
        {
            return db.editVehicle(editVehicle);
        }
        public Vehicle getVehicle(Vehicle getVehicle)
        {
            return db.getVehicle(getVehicle);
        }
        #endregion

        #region Search
        public List<SearchViewModel> getSearchResults(string term, int ProfileID, DateTime sDate, DateTime eDate, string cat)
        {
            return db.getSearchResults(term, ProfileID, sDate, eDate, cat);
        }
        #endregion

        #region Notifications
        public bool newNotification(Model.Notifications newNotification) { return db.newNotification(newNotification); }
        public List<Model.Notifications> getNotifications(Model.Notifications getNotifications) { return db.getNotifications(getNotifications); }
        public Model.Notifications dismissNotifications(Model.Notifications dismissNotification) { return db.dismissNotifications(dismissNotification); }
        public Model.Notifications getNotificationLink(Model.Notifications dismissNotification) { return db.getNotificationLink(dismissNotification); }
        public bool UpdateJobNotiStatus(SP_GetJob_Result job) { return db.UpdateJobNotiStatus(job); }
        public List<OutstandingInvoiceReminders> getOverdueInvoices() { return db.getOverdueInvoices(); }
        #endregion

        #region Reports
        public List<SP_GetJob_Result> getJobsReport(Profile profile, DateTime sDate, DateTime eDate)
        { return db.getJobsReport(profile, sDate, eDate); }
        public ReportViewModel getClientReport(Profile profile, DateTime sDate, DateTime eDate)
        { return db.getClientReport(profile, sDate, eDate); }
        public ReportViewModel getIncomeByClientReport(Profile profile, DateTime sDate, DateTime eDate)
        { return db.getIncomeByClientReport(profile, sDate, eDate); }
        public ReportViewModel getExpensesByClientReport(Profile profile, DateTime sDate, DateTime eDate)
        { return db.getExpensesByClientReport(profile, sDate, eDate); }
        public ReportViewModel getClientReport(Profile profile, DateTime sDate, DateTime eDate, string DropDownID)
        { return db.getClientReport(profile, sDate, eDate, DropDownID); }
        public ReportViewModel getIncomeByClientReport(Profile profile, DateTime sDate, DateTime eDate, string DropDownID)
        { return db.getIncomeByClientReport(profile, sDate, eDate, DropDownID); }
        public ReportViewModel getExpensesByClientReport(Profile profile, DateTime sDate, DateTime eDate, string DropDownID)
        { return db.getExpensesByClientReport(profile, sDate, eDate, DropDownID); }
        public List<SP_GetGeneralExpense_Result> getGeneralExpensesReport(Profile profileID, DateTime sDate, DateTime eDate)
        { return db.getGeneralExpensesReport(profileID, sDate, eDate); }
        public ReportViewModel getJobEarningPerHourReport(Profile profile, DateTime sDate, DateTime eDate)
        {
            return db.getJobEarningPerHourReport(profile, sDate, eDate);
        }
        public ReportViewModel getJobPerMonthReport(Profile profile, string Year)
        { return db.getJobPerMonthReport(profile, Year); }
        public ReportViewModel getJobPerYearReport(Profile profile)
        { return db.getJobPerYearReport(profile); }
        public ReportViewModel getIncomeRecivedListPerMonth(Profile profile, string Year)
        { return db.getIncomeRecivedListPerMonth(profile, Year); }
        public ReportViewModel getIncomeRecivedListPerYear(Profile profile)
        { return db.getIncomeRecivedListPerYear(profile); }
        #endregion

        #region Verify External User
        public int GetExternalUserOTP(int ID, string Type) { return db.GetExternalUserOTP(ID, Type); }
        public Tuple<bool, string, string, int> NewExternalUserOTP(int ID, int OTP, string Type) { return db.NewExternalUserOTP(ID, OTP, Type); }
        #endregion
    }
}