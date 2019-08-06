SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_WorklogHoursPast
	@PID INT,
	@CID INT,
	@SD date,
	@ED date
AS
BEGIN
	select JobHours.JobID, sum(DATEDIFF(MINUTE, StartTime, EndTime)) as WorkLogHours
		   from Worklog, JobHours, Jobs, Client
		   Where JobHours.LogItemID = Worklog.LogItemID
				AND JobHours.JobID = Jobs.JobID
				AND Jobs.ClientID = Client.ClientID
				And (Client. ProfileID = @PID or
					Client.ClientID = @CID)
				AND Jobs.EndDate is not null
				and Jobs.StartDate between @SD and @ED
		   group by JobHours.JobID
END
GO
