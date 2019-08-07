SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_SearchWorkLog
	@ST varchar(max),
	@PID int,
	@SD DateTime,
	@ED Datetime
AS
BEGIN
	select WorkLog.LogItemID, Worklog.[Description], Jobs.JobTitle +' For: '+ Client.FirstName + ' ' + Client.LastName as [JobClient],  Worklog.StartTime 
From Jobs, Client, JobHours, Worklog
where (JobTitle like '%'+@ST+'%'
	or Client.CompanyName like '%'+@ST+'%'
	or Client.FirstName like '%'+@ST+'%'
	or Client.LastName like '%'+@ST+'%'
	or Client.EmailAddress like '%'+@ST+'%'
	or Client.PhysiclaAddress like '%'+@ST+'%')
	And Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID
	AND Jobs.JobID = JobHours.JobID
	And JobHours.LogItemID = Worklog.LogItemID
	And Worklog.StartTime between @SD and @ED
Group by  WorkLog.LogItemID, Worklog.[Description], Jobs.JobTitle, Client.FirstName, Client.LastName,  Worklog.StartTime
END
GO
