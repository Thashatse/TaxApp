USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_GetClientReportTravelExpense]
	@PID INT,
	@CID INT,
	@SD date,
	@ED date
AS
BEGIN
 SELECT Client.[ClientID], (Client.FirstName + ' ' + Client.LastName) as ClientName,
		Sum(((ClientCharge) * (ClosingKMs - OpeningKMs))) as Expenses
	FROM   Jobs, Client, TravelLog, Vehicle
	WHERE  Jobs.ClientID = Client.ClientID
		AND TravelLog.JobID = Jobs.JobID
		AND TravelLog.VehicleID = Vehicle.VehicleID
		AND TravelLog.[Date] between @SD and @ED
		AND Client.ProfileID = @PID
	Group By Client.ClientID, Client.FirstName, Client.LastName, jobs.ClientID
	ORDER BY FirstName desc
END 

