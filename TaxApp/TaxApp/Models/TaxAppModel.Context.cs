﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaxApp.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TaxAppEntities1 : DbContext
    {
        public TaxAppEntities1()
            : base("name=TaxAppEntities1")
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
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<JobExpense> JobExpenses { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TaxConsultant> TaxConsultants { get; set; }
        public virtual DbSet<Worklog> Worklogs { get; set; }
    }
}
