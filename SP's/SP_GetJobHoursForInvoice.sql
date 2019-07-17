SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetJobHoursForInvoice 
	@JID int
AS
BEGIN
	SET NOCOUNT ON;
	Select Worklog.LogItemID, Worklog.[Description], Jobs.HourlyRate, DATEDIFF(MINUTE, StartTime, EndTime) AS WorkLogHours
	From Worklog, JobHours, Jobs
	WHERE  Worklog.LogItemID = JobHours.LogItemID
	And JobHours.JobID = Jobs.JobID
	AND Invoiced = 0
	AND JobHours.JobID = @JID
	Order by Worklog.StartTime
END
GO
