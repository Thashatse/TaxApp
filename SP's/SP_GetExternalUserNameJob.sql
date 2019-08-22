SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetExternalUserNameJob
	@ID int
AS
BEGIN
	Select Client.FirstName + ' ' + Client.LastName as userName, EmailAddress, client.ClientID  as ID
	From Jobs, Client
	Where Jobs.ClientID = Client.CLientID
		AND Share = 1
		AND Jobs.JobID = @ID
		And OTPDate between dateadd(minute,-10,getdate()) and getdate()
END
GO
