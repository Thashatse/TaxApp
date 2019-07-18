USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO 
alter PROCEDURE [dbo].[SP_GetProfileJobsPast]
	@PID INT,
	@CID INT
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
                   AND JobHours.JobID = Jobs.JobID) AS WorkLogHours,
           (SELECT Sum(Amount)
            FROM   JobExpense
            WHERE  JobExpense.JobID = Jobs.JobID) AS ExpenseTotal,
           (SELECT (((Sum(UnitCost * UnitCount)/100)*Invoice.VATRate)+Sum(UnitCost * UnitCount))
            FROM   JobInvoice, Invoice, InvoiceLineItem, Jobs, Client
			WHERE  JobInvoice.InvoiceNum = Invoice.InvoiceNum
				   AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
				   AND JobInvoice.JobID = Jobs.JobID
				   AND Jobs.ClientID = Client.ClientID
				   AND (Client.ProfileID = @PID or Jobs.ClientID = @CID)
                   AND Invoice.Paid = 1
			GROUP BY Invoice.VATRate) AS TotalPaid,
           (SELECT (((Sum(UnitCost * UnitCount)/100)*Invoice.VATRate)+Sum(UnitCost * UnitCount))
            FROM   JobInvoice, Invoice, InvoiceLineItem, Jobs, Client
			WHERE  JobInvoice.InvoiceNum = Invoice.InvoiceNum
				   AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
				   AND JobInvoice.JobID = Jobs.JobID
				   AND Jobs.ClientID = Client.ClientID
				   AND (Client.ProfileID = @PID or Jobs.ClientID = @CID)
                   AND Invoice.Paid = 0
			GROUP BY Invoice.VATRate) AS TotalUnPaid,
           (SELECT Sum((SARSMaintenceCost + SARSFuelCost) * (ClosingKMs - OpeningKMs))
            FROM   TravelLog, Vehicle
            WHERE  TravelLog.JobID = Jobs.JobID
                   AND TravelLog.VehicleID = Vehicle.VehicleID) AS TravelLogCostTotal
    FROM   [TaxApp].[dbo].[Jobs], [TaxApp].[dbo].Client AS C
    WHERE  Jobs.ClientID = C.ClientID AND
			(ProfileID = @PID
		or Jobs.ClientID = @CID) 
		and C.ClientID = Jobs.ClientID 
		and Jobs.EndDate IS NOT NULL
	ORDER BY Jobs.StartDate desc
END 

