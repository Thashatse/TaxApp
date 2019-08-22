SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetJobsOTP 
	@ID int
AS
BEGIN
	Select OTP, OTPDate
	From Jobs, Client
	Where Jobs.ClientID = Client.CLientID
		AND Share = 1
		And OTPDate between dateadd(minute,-10,getdate()) and getdate()
		AND Jobs.JobID = @ID
END
GO