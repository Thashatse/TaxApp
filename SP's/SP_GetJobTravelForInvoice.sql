SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_GetJobTravelForInvoice 
	@JID int
AS
BEGIN
	select ExpenseID, Reason, (ClosingKMs - OpeningKMs) as UnitKMs, (Vehicle.FuelCost + Vehicle.MaintenceCost) as CostPerKM 
	from TravelLog, Vehicle
	Where Invoiced = 0
	AND TravelLog.VehicleID = Vehicle.VehicleID
	AND TravelLog.JobID = @JID
END
GO
