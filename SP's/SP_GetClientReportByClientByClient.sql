USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_GetClientReportByClientByClient]
	@PID INT,
	@CID INT,
	@SD date,
	@ED date
AS
BEGIN
    SELECT Client.[ClientID], (Client.FirstName + ' ' + Client.LastName) as ClientName,
		Sum(Amount + ((ClientCharge) * (ClosingKMs - OpeningKMs))) as Expenses,
		Sum(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost) as Income
	FROM   JobExpense, Jobs, Client, TravelLog, Vehicle, Invoice, InvoiceLineItem, JobInvoice
	WHERE  JobExpense.JobID = Jobs.JobID
		AND Jobs.ClientID = Client.ClientID
		AND TravelLog.JobID = Jobs.JobID
		AND TravelLog.VehicleID = Vehicle.VehicleID
		AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
		AND JobInvoice.JobID = Jobs.JobID
		AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
		AND Invoice.[DateTime] between @SD and @ED
		AND JobExpense.[Date] between @SD and @ED
		AND TravelLog.[Date] between @SD and @ED
		AND Client.ProfileID = @PID
		AND Jobs.ClientID = @CID
	Group By Client.ClientID, Client.FirstName, Client.LastName, Jobs.JobID
	ORDER BY FirstName desc
END 

