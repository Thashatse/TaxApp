SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetTravleLog
	@PID int
AS
BEGIN
	Select TravelLog.ExpenseID, TravelLog.[From], TravelLog.[To], TravelLog.Reason, 
			TravelLog.OpeningKMs, TravelLog.ClosingKMs, TravelLog.VehicleID,
			TravelLog.JobID, TravelLog.Invoiced, TravelLog.[Date],
			(TravelLog.ClosingKMs - TravelLog.OpeningKMs) as TotalKMs,
			 (Vehicle.SARSFuelCost * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)) as SARSFuelCost,
			 (Vehicle.SARSMaintenceCost * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)) as SARSMaintenceCost,
			 (Vehicle.ClientCharge * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)) as ClientCharge
	From TravelLog, Client, [Profile], jobs, Vehicle
	Where TravelLog.JobID = Jobs.JobID
		 AND Jobs.ClientID = Client.ClientID
		 AND Client.ProfileID = Profile.ProfileID
		 AND TravelLog.VehicleID = Vehicle.VehicleID
		 AND Profile.ProfileID = @PID
	Order by TravelLog.[Date] desc
END
GO
