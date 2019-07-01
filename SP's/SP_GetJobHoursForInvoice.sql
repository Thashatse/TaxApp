SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE SP_GetJobHoursForInvoice 
	@JID int
AS
BEGIN
	SET NOCOUNT ON;
	Select Worklog.LogItemID, Worklog.[Description], Jobs.HourlyRate, 
		(SELECT Sum(DATEDIFF(MINUTE, StartTime, EndTime))
		FROM   Worklog) AS WorkLogHours
	From Worklog, JobHours, Jobs
	WHERE  Worklog.LogItemID = JobHours.LogItemID
	And JobHours.JobID = Jobs.JobID
	AND Invoiced = 0
	AND JobHours.JobID = @JID
END
GO
