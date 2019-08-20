SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_WorklogHours 
	@PID INT,
	@CID INT, @SD datetime, @ED datetime
AS
BEGIN
	select Jobs.JobID, Jobs.StartDate, Jobs.JobTitle, (sum(DATEDIFF(MINUTE, StartTime, EndTime))) As WorkLogHours
		   from Worklog, JobHours, Jobs, Client
		   Where JobHours.LogItemID = Worklog.LogItemID
				AND JobHours.JobID = Jobs.JobID
				AND Jobs.ClientID = Client.ClientID
				And (Client. ProfileID = @PID or
					Client.ClientID = @CID)
			Group by Jobs.JobID, Jobs.JobTitle, Jobs.StartDate
END
GO
