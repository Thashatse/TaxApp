SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_GetJobHours
	@JobID int
AS
BEGIN
	Select *
	From Worklog, JobHours
	Where Worklog.LogItemID = JobHours.LogItemID
	AND JobHours.JobID = @JobID
END
GO
