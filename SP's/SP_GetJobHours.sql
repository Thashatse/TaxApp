USE [TaxApp]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetJobHours]    Script Date: 2019/06/29 13:54:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_GetJobHours]
@JobID INT
AS
BEGIN
    SELECT *, 
			(SELECT Sum(DATEDIFF(MINUTE, StartTime, EndTime))
            FROM   Worklog, JobHours
            WHERE  Worklog.LogItemID = JobHours.LogItemID
                   AND JobHours.JobID = @JobID) AS WorkLogHours
    FROM   Worklog, JobHours
    WHERE  Worklog.LogItemID = JobHours.LogItemID
           AND JobHours.JobID = @JobID
END

