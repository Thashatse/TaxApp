SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_IncomeDashboard
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

(Select sum(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)  
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between DATEADD(Month, -1, getdate()) and getdate()
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 0
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as TotalOutIncome 
END
GO
