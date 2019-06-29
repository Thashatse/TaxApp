USE [TaxApp]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetLogItem]    Script Date: 2019/06/29 19:13:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_GetLogItem]
@LogID INT
AS
BEGIN
    SELECT *,
			(SELECT Sum(DATEDIFF(MINUTE, StartTime, EndTime))
            FROM   Worklog
            WHERE  Worklog.LogItemID = @LogID) AS WorkLogHours
    FROM   Worklog
    WHERE  Worklog.LogItemID = @LogID;
END

