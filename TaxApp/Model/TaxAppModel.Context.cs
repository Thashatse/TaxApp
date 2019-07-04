﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TaxAppEntities : DbContext
    {
        public TaxAppEntities()
            : base("name=TaxAppEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Business> Businesses { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<EmailSetting> EmailSettings { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public virtual DbSet<GeneralExpense> GeneralExpenses { get; set; }
        public virtual DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }
        public virtual DbSet<JobExpense> JobExpenses { get; set; }
        public virtual DbSet<JobHour> JobHours { get; set; }
        public virtual DbSet<JobInvoice> JobInvoices { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<TaxConsultant> TaxConsultants { get; set; }
        public virtual DbSet<TravelLog> TravelLogs { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<Worklog> Worklogs { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
    
        public virtual ObjectResult<SP_GetGeneralExpense_Result> SP_GetGeneralExpense(Nullable<int> eID)
        {
            var eIDParameter = eID.HasValue ?
                new ObjectParameter("EID", eID) :
                new ObjectParameter("EID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetGeneralExpense_Result>("SP_GetGeneralExpense", eIDParameter);
        }
    
        public virtual ObjectResult<SP_GetInvoice_Result> SP_GetInvoice(string iN)
        {
            var iNParameter = iN != null ?
                new ObjectParameter("IN", iN) :
                new ObjectParameter("IN", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetInvoice_Result>("SP_GetInvoice", iNParameter);
        }
    
        public virtual ObjectResult<SP_GetJob_Result> SP_GetJob(Nullable<int> jID)
        {
            var jIDParameter = jID.HasValue ?
                new ObjectParameter("JID", jID) :
                new ObjectParameter("JID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetJob_Result>("SP_GetJob", jIDParameter);
        }
    
        public virtual ObjectResult<SP_GetJobExpense_Result> SP_GetJobExpense(Nullable<int> eID)
        {
            var eIDParameter = eID.HasValue ?
                new ObjectParameter("EID", eID) :
                new ObjectParameter("EID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetJobExpense_Result>("SP_GetJobExpense", eIDParameter);
        }
    }
}
