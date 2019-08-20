USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetIncomeRecivedList
	@PID int,
	@SD  date,
	@ED date,
	@RID decimal(4,2)
AS
BEGIN
	Select Jobs.JobID, Jobs.JobTitle, Client.FirstName + ' ' + Client.LastName as Client, Client.ClientID, Invoice.[DateTime], 
	sum((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)) As Total,
	sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost))/100)*@RID) As TAX
From Jobs, Invoice, InvoiceLineItem, JobInvoice, Client
WHERE Jobs.JobID = JobInvoice.JobID
	AND JobInvoice.InvoiceNum = Invoice.InvoiceNum
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID
	AND Invoice.[Datetime] Between @SD and @ED
	AND Invoice.Paid = 1
Group by Jobs.JobTitle, Invoice.[DateTime], Jobs.JobID, Client.FirstName, Client.LastName, Client.ClientID
Order by Invoice.[DateTime]
END
GO
