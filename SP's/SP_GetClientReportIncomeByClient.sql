USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_GetClientReportIncomeByClient]
	@PID INT,
	@CID INT,
	@SD date,
	@ED date
AS
BEGIN
 SELECT Client.[ClientID], (Client.FirstName + ' ' + Client.LastName) as ClientName,
		Sum(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost) as Income
	FROM   Jobs, Client, Invoice, InvoiceLineItem, JobInvoice
	WHERE  Jobs.ClientID = Client.ClientID
		AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
		AND JobInvoice.JobID = Jobs.JobID
		AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
		AND Invoice.Paid = 1
		AND Invoice.[DateTime] between @SD and @ED
		AND Client.ProfileID = @PID
		AND Jobs.ClientID = @CID
	Group By Client.ClientID, Client.FirstName, Client.LastName, jobs.JobID
	ORDER BY FirstName desc
END 

