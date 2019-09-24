SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_WorklogHours 
	@PID INT, @SD datetime, @ED datetime
AS
BEGIN
	select Jobs.JobID, Jobs.StartDate, Jobs.JobTitle + ' for ' + Client.FirstName + ' '+Client.LastName as JobTitle, (sum(DATEDIFF(MINUTE, StartTime, EndTime))) As WorkLogHours, Jobs.HourlyRate
		   from Worklog, JobHours, Jobs, Client
		   Where JobHours.LogItemID = Worklog.LogItemID
				AND JobHours.JobID = Jobs.JobID
				AND Jobs.ClientID = Client.ClientID
				And (Client. ProfileID = @PID)
				AND Jobs.StartDate between @SD and @ED
			Group by Jobs.JobID, Jobs.JobTitle, Jobs.StartDate, Jobs.HourlyRate, Client.FirstName, Client.LastName
END
GO
