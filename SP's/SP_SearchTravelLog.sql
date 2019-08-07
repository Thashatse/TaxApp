USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_SearchTravelLog
	@ST varchar(max),
	@PID int,
	@SD DateTime,
	@ED Datetime
AS
BEGIN
	select TravelLog.ExpenseID, TravelLog.Reason, 'From: '+ TravelLog.[From] + ' To: '+ TravelLog.[To] as Details, TravelLog.[Date]
From TravelLog, Jobs, Client, Vehicle
where (TravelLog.Reason like '%'+@ST+'%'
	or TravelLog.[From] like '%'+@ST+'%'
	or Vehicle.[Name] like '%'+@ST+'%')
	AND Client.ProfileID = @PID
	AND TravelLog.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	AND TravelLog.VehicleID = Vehicle.VehicleID
	AND TravelLog.[Date] between @SD and @ED
END
GO
