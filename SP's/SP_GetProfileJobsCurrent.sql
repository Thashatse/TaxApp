USE [TaxApp]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetProfileJobsCurrent]    Script Date: 2019/07/15 14:11:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_GetProfileJobsCurrent]
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
            FROM   JobInvoice, Invoice, InvoiceLineItem
			WHERE  JobInvoice.JobID = '100000002'
                   AND JobInvoice.InvoiceNum = Invoice.InvoiceNum
                   AND Invoice.Paid = 1
			GROUP BY Invoice.VATRate) AS TotalPaid,
           (SELECT (((Sum(UnitCost * UnitCount)/100)*Invoice.VATRate)+Sum(UnitCost * UnitCount))
            FROM   JobInvoice, Invoice, InvoiceLineItem
			WHERE  JobInvoice.JobID = '100000002'
                   AND JobInvoice.InvoiceNum = Invoice.InvoiceNum
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
		and Jobs.EndDate IS NULL
	Order by Jobs.StartDate desc
END 

