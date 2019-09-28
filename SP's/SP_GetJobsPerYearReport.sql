SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetJobsPerYearReport
	@PID int
AS
BEGIN
	Select Count(Jobs.JobID) as NoOfJobs,
	DATENAME(yyyy,Jobs.StartDate) as 'Year'
	From Jobs, Client
	Where Jobs.ClientID = Client.ClientID
		AND Client.ProfileID = @PID 
	Group By DATENAME(yyyy,Jobs.StartDate)
END
GO
