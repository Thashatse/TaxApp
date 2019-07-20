SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetJobTravleLog
	@JID int
AS
BEGIN
	Select *, (TravelLog.ClosingKMs - TravelLog.OpeningKMs) as TotalKMs,
			 (Vehicle.SARSFuelCost * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)) as SARSFuelCost,
			 (Vehicle.SARSMaintenceCost * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)) as SARSMaintenceCost,
			 (Vehicle.ClientCharge * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)) as ClientCharge
	From TravelLog, Vehicle
	Where JobID = @JID
		AND TravelLog.VehicleID = Vehicle.VehicleID
END
GO
