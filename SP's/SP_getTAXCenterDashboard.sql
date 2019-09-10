USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_getTAXCenterDashboard
	@PID int,
	@SD  date,
	@ED date,
	@TR decimal(4,2) = 0
AS
BEGIN
	Select (Select (Sum(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost))
	From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
	Where Invoice.[Datetime] Between @SD and @ED
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as IncomeRECEIVED,
	
	(select Sum(GeneralExpense.Amount)
    FROM   Expense, GeneralExpense, ExpenseCategory
    WHERE  GeneralExpense.ProfileID = @PID
			AND Expense.ExpenseID = GeneralExpense.ExpenseID
			AND Expense.CategoryID = ExpenseCategory.CategoryID
			and GeneralExpense.[Date] between @SD and @ED) as IncomeRECEIVEDGeneral,

	(select Sum ((Vehicle.SARSFuelCost * (TravelLog.ClosingKMs - TravelLog.OpeningKMs))
		+(Vehicle.SARSMaintenceCost * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)))
	From TravelLog, Client, [Profile], jobs, Vehicle
	Where TravelLog.JobID = Jobs.JobID
		 AND Jobs.ClientID = Client.ClientID
		 AND Client.ProfileID = Profile.ProfileID
		 AND TravelLog.VehicleID = Vehicle.VehicleID
		 AND Profile.ProfileID = @PID
			and [TravelLog].[Date] between @SD and @ED) as IncomeRECEIVEDTravel,

	(select sum(JobExpense.Amount)
    FROM   Expense, JobExpense, ExpenseCategory, Jobs, Client
    WHERE  Expense.ExpenseID = JobExpense.ExpenseID
			AND Expense.CategoryID = ExpenseCategory.CategoryID
			AND JobExpense.JobID = Jobs.JobID
			AND Jobs.ClientID = Client.ClientID
			AND Client.ProfileID = @PID
			AND JobExpense.[Date] between @SD and @ED) as IncomeRECEIVEDJob,



	(Select sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost))/100)*@TR)  
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between @SD and @ED
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as TaxOwed,



	(Select ((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost))
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between DATEADD(DAY, (SELECT DATEDIFF(DAY, @ED, @SD)), @SD) and @SD
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as IncomeRECEIVEDPastPeriod,

	(select Sum(GeneralExpense.Amount)
    FROM   Expense, GeneralExpense, ExpenseCategory
    WHERE  GeneralExpense.ProfileID = @PID
			AND Expense.ExpenseID = GeneralExpense.ExpenseID
			AND Expense.CategoryID = ExpenseCategory.CategoryID
			and GeneralExpense.[Date] between @SD and @ED)  as IncomeRECEIVEDPastPeriodGeneral,

	(select Sum ((Vehicle.SARSFuelCost * (TravelLog.ClosingKMs - TravelLog.OpeningKMs))
		+(Vehicle.SARSMaintenceCost * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)))
	From TravelLog, Client, [Profile], jobs, Vehicle
	Where TravelLog.JobID = Jobs.JobID
		 AND Jobs.ClientID = Client.ClientID
		 AND Client.ProfileID = Profile.ProfileID
		 AND TravelLog.VehicleID = Vehicle.VehicleID
		 AND Profile.ProfileID = @PID
			and [TravelLog].[Date] between @SD and @ED) as IncomeRECEIVEDPastPeriodTravel,

	(select sum(JobExpense.Amount)
    FROM   Expense, JobExpense, ExpenseCategory, Jobs, Client
    WHERE  Expense.ExpenseID = JobExpense.ExpenseID
			AND Expense.CategoryID = ExpenseCategory.CategoryID
			AND JobExpense.JobID = Jobs.JobID
			AND Jobs.ClientID = Client.ClientID
			AND Client.ProfileID = @PID
			AND JobExpense.[Date] between @SD and @ED) as IncomeRECEIVEDPastPeriodJob,



	(Select sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost))/100)*@TR)  
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between DATEADD(DAY, (SELECT DATEDIFF(DAY, @ED, @SD)), @SD) and @SD
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as TaxOwedPastPeriod
END
GO
