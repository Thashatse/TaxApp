SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetTravleLog
	@PID int,
	@SD date,
	@ED date
AS
BEGIN
	Select TravelLog.ExpenseID, TravelLog.[From], TravelLog.[To], TravelLog.Reason, 
			TravelLog.OpeningKMs, TravelLog.ClosingKMs, TravelLog.VehicleID,
			TravelLog.JobID, TravelLog.Invoiced, TravelLog.[Date],
			(TravelLog.ClosingKMs - TravelLog.OpeningKMs) as TotalKMs,
			 (Vehicle.SARSFuelCost * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)) as SARSFuelCost,
			 (Vehicle.SARSMaintenceCost * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)) as SARSMaintenceCost,
			 (Vehicle.ClientCharge * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)) as ClientCharge,
			 Jobs.JobTitle
	From TravelLog, Client, [Profile], jobs, Vehicle
	Where TravelLog.JobID = Jobs.JobID
		 AND Jobs.ClientID = Client.ClientID
		 AND Client.ProfileID = Profile.ProfileID
		 AND TravelLog.VehicleID = Vehicle.VehicleID
		 AND Profile.ProfileID = @PID
			and [TravelLog].[Date] between @SD and @ED
	Order by TravelLog.[Date] desc
END
GO
