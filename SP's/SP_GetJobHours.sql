USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_GetJobHours]
@JobID INT
AS
BEGIN
    SELECT *, DATEDIFF(MINUTE, StartTime, EndTime) AS WorkLogHours
    FROM   Worklog, JobHours
    WHERE  Worklog.LogItemID = JobHours.LogItemID
           AND JobHours.JobID = @JobID
	Order by Worklog.StartTime desc
END

