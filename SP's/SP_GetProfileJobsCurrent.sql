USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_GetProfileJobsCurrent]
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
		   Share,
           (C.FirstName + ' ' + C.LastName) as FirstName,
		   (SELECT Sum(Amount)
            FROM   JobExpense
            WHERE  JobExpense.JobID = Jobs.JobID) AS ExpenseTotal,
           (SELECT Sum((SARSMaintenceCost + SARSFuelCost) * (ClosingKMs - OpeningKMs))
            FROM   TravelLog, Vehicle
            WHERE  TravelLog.JobID = Jobs.JobID
                   AND TravelLog.VehicleID = Vehicle.VehicleID) AS TravelLogCostTotal,
			Noti100, Noti75, Noti90, Noti95
    FROM   [TaxApp].[dbo].[Jobs], [TaxApp].[dbo].Client AS C
    WHERE  Jobs.ClientID = C.ClientID AND
			(ProfileID = @PID
		or Jobs.ClientID = @CID)
		and C.ClientID = Jobs.ClientID
		and Jobs.EndDate IS NULL
	ORDER BY Jobs.StartDate desc
END 

