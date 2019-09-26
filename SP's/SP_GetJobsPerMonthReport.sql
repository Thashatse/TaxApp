SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_GetJobsPerMonthReport
	@PID int,
	@Y nchar(4)
AS
BEGIN
	Select Count(Jobs.JobID) as NoOfJobs,
	DATENAME(mm,Jobs.StartDate) as 'Month'
	From Jobs, Client
	Where year(Jobs.StartDate) = @Y
		AND Jobs.ClientID = Client.ClientID
		AND Client.ProfileID = @PID 
	Group By DATENAME(mm,Jobs.StartDate)
END
GO
