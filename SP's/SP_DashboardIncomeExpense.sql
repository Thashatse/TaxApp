SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_DashboardIncomeExpense
	@PID int 
AS
BEGIN

	Select (Select sum(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)  
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between DATEADD(Month, -1, getdate()) and getdate()
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as TotalIncomePast30Days, 

(Select sum(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost) as TotalIncomePast30Days 
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between DATEADD(Month, -2, getdate()) and DATEADD(Month, -1, getdate())
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as TotalIncomePast60To30Days,

((Select sum(Amount) 
From Expense, JobExpense, Jobs, Client
Where Expense.ExpenseID = JobExpense.ExpenseID
	AND JobExpense.[Date] Between DATEADD(Month, -1, getdate()) and getdate()
	AND JobExpense.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) +
(Select sum(Amount) 
From Expense, GeneralExpense
Where Expense.ExpenseID = GeneralExpense.ExpenseID
	And GeneralExpense.ProfileID = @PID
	AND GeneralExpense.[Date] Between DATEADD(Month, -1, getdate()) and getdate()) +
(Select Sum((ClosingKMs - OpeningKMs) * (ClientCharge))
From TravelLog, Vehicle
Where TravelLog.VehicleID = Vehicle.VehicleID 
	And Vehicle.ProfileID = @PID
	AND TravelLog.[Date] Between DATEADD(Month, -1, getdate()) and getdate())) as TotalExpensePast30Days,

((Select sum(Amount) 
From Expense, JobExpense, Jobs, Client
Where Expense.ExpenseID = JobExpense.ExpenseID
	AND JobExpense.[Date] Between DATEADD(Month, -2, getdate()) and DATEADD(Month, -1, getdate())
	AND JobExpense.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) +
(Select sum(Amount) 
From Expense, GeneralExpense
Where Expense.ExpenseID = GeneralExpense.ExpenseID
	And GeneralExpense.ProfileID = @PID
	AND GeneralExpense.[Date] Between DATEADD(Month, -2, getdate()) and DATEADD(Month, -1, getdate()) +
(Select Sum((ClosingKMs - OpeningKMs) * (ClientCharge))
From TravelLog, Vehicle
Where TravelLog.VehicleID = Vehicle.VehicleID 
	AND TravelLog.[Date] Between DATEADD(Month, -2, getdate()) and DATEADD(Month, -1, getdate()))))  as TotalExpensePast60To30Days 

END
GO
