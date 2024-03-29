USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_JobEarnings] 
	@PID INT, 
	@SD Datetime, 
	@ED Datetime
AS
BEGIN
	SELECT Jobs.JobID, Jobs.StartDate, Jobs.JobTitle, (Sum(UnitCost * UnitCount)) as TotalPaid,
	(SELECT Sum(Amount)
            FROM   JobExpense
            WHERE  JobExpense.JobID = Jobs.JobID) AS ExpenseTotal,
           (SELECT Sum((SARSMaintenceCost + SARSFuelCost) * (ClosingKMs - OpeningKMs))
            FROM   TravelLog, Vehicle
            WHERE  TravelLog.JobID = Jobs.JobID
                   AND TravelLog.VehicleID = Vehicle.VehicleID) AS TravelLogCostTotal
            FROM   [TaxApp].[dbo].[JobInvoice], [TaxApp].[dbo].Invoice, [TaxApp].[dbo].InvoiceLineItem, [TaxApp].[dbo].Jobs, Client
			WHERE  JobInvoice.InvoiceNum = Invoice.InvoiceNum
				   AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
                   AND Invoice.Paid = 1
					AND Jobs.JobID = JobInvoice.JobID
				   AND Jobs.ClientID = Client.ClientID
				   AND Client.ProfileID = @PID
				   AND Invoice.[DateTime] between @SD and @ED
			Group by Jobs.JobID, Jobs.JobTitle, Jobs.StartDate, Invoice.VATRate
END
