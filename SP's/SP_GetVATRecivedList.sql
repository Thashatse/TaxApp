USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetVATRecivedList
	@PID int,
	@SD  date,
	@ED date,
	@PDID int
AS
BEGIN
	Select Jobs.JobID, Jobs.JobTitle, Client.FirstName + ' ' + Client.LastName as Client, Client.ClientID, Jobs.StartDate, 
	sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)+(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)) As Total,
	sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)) As VAT
From Jobs, Invoice, InvoiceLineItem, JobInvoice, Client
WHERE Jobs.JobID = JobInvoice.JobID
	AND JobInvoice.InvoiceNum = Invoice.InvoiceNum
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID
	AND Invoice.[Datetime] Between @SD and @ED
	AND Invoice.Paid = 1
Group by Jobs.JobTitle, Jobs.StartDate, Jobs.JobID, Client.FirstName, Client.LastName, Client.ClientID
Order by Jobs.StartDate
END
GO
