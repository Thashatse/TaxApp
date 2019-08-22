SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_NewJobOTP 
	@ID int,
	@OTP int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

			UPDATE [Client] 
			Set [OTP] = @OTP,
				[OTPDate] = GETDATE()
			Where Client.CLientID = (Select Top 1 Jobs.ClientID 
											 From Jobs
											 Where Share = 1
											 AND Jobs.JobID = @ID)

		COMMIT TRANSACTION 
	END TRY 
		BEGIN CATCH 
			IF @@TRANCOUNT > 0 
				ROLLBACK TRANSACTION
		END CATCH  
END
GO