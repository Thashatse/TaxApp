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
    SELECT [JobID],
           [JobTitle],
           [HourlyRate],
           [Budget],
           [StartDate],
           [EndDate],
           C.[ClientID],
           C.FirstName,
           (SELECT Sum(DATEDIFF(MINUTE, StartTime, EndTime))
            FROM   Worklog, JobHours
            WHERE  Worklog.LogItemID = JobHours.LogItemID
                   AND JobHours.JobID = @JID) AS WorkLogHours,
           (SELECT Sum(Amount)
            FROM   JobExpense
            WHERE  JobExpense.JobID = @JID) AS ExpenseTotal,
           (SELECT (((Sum(UnitCost * UnitCount)/100)*Invoice.VATRate)+Sum(UnitCost * UnitCount)) as TotalUnPaid
            FROM   [TaxApp].[dbo].[JobInvoice], [TaxApp].[dbo].Invoice, [TaxApp].[dbo].InvoiceLineItem
			WHERE  JobInvoice.JobID = @JID
                   AND JobInvoice.InvoiceNum = Invoice.InvoiceNum
				   AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
                   AND Invoice.Paid = 1
			GROUP BY Invoice.VATRate) AS TotalPaid,
           (SELECT (((Sum(InvoiceLineItem.UnitCost * UnitCount)/100)*Invoice.VATRate)+Sum(UnitCost * UnitCount)) as TotalUnPaid
            FROM   [TaxApp].[dbo].[JobInvoice], [TaxApp].[dbo].Invoice, [TaxApp].[dbo].InvoiceLineItem
			WHERE  JobInvoice.JobID = @JID
                   AND JobInvoice.InvoiceNum = Invoice.InvoiceNum
				   AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
                   AND Invoice.Paid = 0
			GROUP BY Invoice.VATRate) AS TotalUnPaid,
           (SELECT Sum((SARSMaintenceCost + SARSFuelCost) * (ClosingKMs - OpeningKMs))
            FROM   TravelLog, Vehicle
            WHERE  TravelLog.JobID = @JID
                   AND TravelLog.VehicleID = Vehicle.VehicleID) AS TravelLogCostTotal
    FROM   [TaxApp].[dbo].[Jobs], [TaxApp].[dbo].Client AS C
    WHERE  Jobs.JobID = @JID
           AND Jobs.ClientID = C.ClientID;
END

