SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_WorklogHours 
	@PID INT,
	@CID INT
AS
BEGIN
	select sum(DATEDIFF(MINUTE, StartTime, EndTime))
		   from Worklog, JobHours, Jobs, Client
		   Where JobHours.LogItemID = Worklog.LogItemID
				AND JobHours.JobID = Jobs.JobID
				AND Jobs.ClientID = Client.ClientID
				And (Client. ProfileID = @PID or
					Client.ClientID = @CID)
		   group by JobHours.JobID
END
GO
