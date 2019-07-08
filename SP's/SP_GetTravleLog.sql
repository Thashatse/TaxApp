SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Alter PROCEDURE SP_GetTravleLog
	@PID int
AS
BEGIN
	Select TravelLog.JobID, TravelLog.[From], TravelLog.[To], TravelLog.Reason, 
			TravelLog.OpeningKMs, TravelLog.ClosingKMs, TravelLog.VehicleID,
			TravelLog.JobID, TravelLog.Invoiced
	From TravelLog, Client, [Profile], jobs
	Where TravelLog.JobID = Jobs.JobID
		 AND Jobs.ClientID = Client.ClientID
		 AND Client.ProfileID = Profile.ProfileID
		 AND Profile.ProfileID = @PID
END
GO
