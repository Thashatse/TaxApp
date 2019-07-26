USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE SP_getTAXCenterDashboard
	@PID int,
	@SD  date,
	@ED date,
	@VR decimal(4,2),
	@TR decimal(4,2) = 0
AS
BEGIN
	Select (Select Sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)+(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost))  
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between @SD and @ED
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as IncomeRECEIVED,

	(Select sum((((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)+(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost))/100)*@TR)  
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between @SD and @ED
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as TaxOwed,

	(Select Sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)+(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost))  
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between DATEADD(DAY, (SELECT DATEDIFF(DAY, @ED, @SD)), @SD) and @SD
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as IncomeRECEIVEDPastPeriod,

	(Select sum((((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)+(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost))/100)*@TR)  
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
