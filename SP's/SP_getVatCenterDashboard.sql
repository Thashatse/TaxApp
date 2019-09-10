USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_getVatCenterDashboard
	@PID int,
	@SD  date,
	@ED date,
	@VR decimal(4,2)
AS
BEGIN
	Select (Select sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*@VR))  
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between @SD and @ED
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as VATRECEIVED,

	(Select sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*@VR))  
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between DATEADD(DAY, (SELECT DATEDIFF(DAY, @ED, @SD)), @SD) and @SD
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as VATRECEIVEDPastPeriod,

(Select sum(Amount) 
From Expense, JobExpense, Jobs, Client
Where Expense.ExpenseID = JobExpense.ExpenseID
	AND JobExpense.[Date] Between @SD and @ED
	AND JobExpense.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as VATPAIDJob,

(Select sum(Amount) 
From Expense, GeneralExpense
Where Expense.ExpenseID = GeneralExpense.ExpenseID
	And GeneralExpense.ProfileID = @PID
	AND GeneralExpense.[Date] Between @SD and @ED) as VATPAIDGeneral,

(Select Sum((ClosingKMs - OpeningKMs) * (ClientCharge))
From TravelLog, Vehicle
Where TravelLog.VehicleID = Vehicle.VehicleID 
	And Vehicle.ProfileID = @PID
	AND TravelLog.[Date] Between @SD and @ED) as VATPAIDTravel,

	 (Select sum(Amount) 
From Expense, JobExpense, Jobs, Client
Where Expense.ExpenseID = JobExpense.ExpenseID
	AND JobExpense.[Date] Between DATEADD(DAY, (SELECT DATEDIFF(DAY, @ED, @SD)), @SD) and @SD
	AND JobExpense.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as VATPAIDPreviousPeriodJob,

(Select sum(Amount) 
From Expense, GeneralExpense
Where Expense.ExpenseID = GeneralExpense.ExpenseID
	And GeneralExpense.ProfileID = @PID
	AND GeneralExpense.[Date] Between DATEADD(DAY, (SELECT DATEDIFF(DAY, @ED, @SD)), @SD) and @SD) as VATPAIDPreviousPeriodGeneral,

(Select Sum((ClosingKMs - OpeningKMs) * (ClientCharge))
From TravelLog, Vehicle
Where TravelLog.VehicleID = Vehicle.VehicleID 
	And Vehicle.ProfileID = @PID
	AND TravelLog.[Date] Between DATEADD(DAY, (SELECT DATEDIFF(DAY, @ED, @SD)), @SD) and @SD) as VATPAIDPreviousPeriodTravel
END
GO
