USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_GetJob]
@JID INT
AS
BEGIN
    SELECT [JobID],[JobTitle],[HourlyRate],[Budget],[StartDate],[EndDate],
	C.[ClientID], C.FirstName, 
	(Select Sum(DATEDIFF(MINUTE, StartTime, EndTime))
		from Worklog, JobHours
		Where Worklog.LogItemID = JobHours.LogItemID
		And JobHours.JobID = @JID) as WorkLogHours,
	(Select Sum(Amount)
		from JobExpense
		Where JobExpense.JobID = @JID) as ExpenseTotal,
	(Select Sum(SubTotal + vat)
		from JobInvoice, Invoice
		Where JobInvoice.JobID = @JID
		AND JobInvoice.InvoiceNum = Invoice.InvoiceNum
		AND Paid = 1) as TotalPaid,
	(Select Sum(SubTotal + vat)
		from JobInvoice, Invoice
		Where JobInvoice.JobID = @JID
		AND JobInvoice.InvoiceNum = Invoice.InvoiceNum
		AND Paid = 1) as TotalUnPaid,
	(Select Sum((MaintenceCost+FuelCost)*(ClosingKMs-OpeningKMs))
		from TravelLog, Vehicle
		Where TravelLog.JobID = @JID
		AND TravelLog.VehicleID = Vehicle.VehicleID) as TravelLogCostTotal
  FROM [TaxApp].[dbo].[Jobs], [TaxApp].[dbo].Client C
  WHERE Jobs.JobID = @JID
	AND Jobs.ClientID = C.ClientID;
END

