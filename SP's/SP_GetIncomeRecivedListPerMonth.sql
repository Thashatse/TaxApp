USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetIncomeRecivedListPerMonth
	@PID int,
	@Y nchar(11)
AS
BEGIN
	Select DATENAME(mm,Invoice.[DateTime]) as 'Month', 
	sum((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)) As Total
From Jobs, Invoice, InvoiceLineItem, JobInvoice, Client
WHERE Jobs.JobID = JobInvoice.JobID
	AND JobInvoice.InvoiceNum = Invoice.InvoiceNum
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID
	AND year(Invoice.[DateTime]) = @Y
	AND Invoice.Paid = 1
Group by DATENAME(mm,Invoice.[DateTime])
Order by DATENAME(mm,Invoice.[DateTime])
END
GO
