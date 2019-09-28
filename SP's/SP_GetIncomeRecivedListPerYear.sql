USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetIncomeRecivedListPerYear
	@PID int
AS
BEGIN
	Select DATENAME(yyyy,Invoice.[DateTime]) as 'Year', 
	sum((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)) As Total
From Jobs, Invoice, InvoiceLineItem, JobInvoice, Client
WHERE Jobs.JobID = JobInvoice.JobID
	AND JobInvoice.InvoiceNum = Invoice.InvoiceNum
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID
	AND Invoice.Paid = 1
Group by DATENAME(yyyy,Invoice.[DateTime])
Order by DATENAME(yyyy,Invoice.[DateTime])
END
GO
