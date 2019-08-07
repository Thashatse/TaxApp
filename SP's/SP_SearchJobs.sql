SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_SearchJobs
	@ST varchar(max),
	@PID int,
	@SD DateTime,
	@ED Datetime
AS
BEGIN
	select JobID, JobTitle, Client.FirstName + ' ' + Client.LastName as [Name],  StartDate 
From Jobs, Client
where (JobTitle like '%'+@ST+'%'
	or Client.CompanyName like '%'+@ST+'%'
	or Client.FirstName like '%'+@ST+'%'
	or Client.LastName like '%'+@ST+'%'
	or Client.EmailAddress like '%'+@ST+'%'
	or Client.PhysiclaAddress like '%'+@ST+'%')
	And Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID
	AND Jobs.StartDate between @SD and @ED
END
GO
