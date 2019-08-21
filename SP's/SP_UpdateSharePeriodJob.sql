SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_UpdateSharePeriodJob 
	@JID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		IF ((select Share from Jobs where Jobs.JobID = @JID)  = 1)
			UPDATE [Jobs] 
			Set [Share] = 0
			WHERE Jobs.JobID = @JID
	   ELSE 
		   UPDATE [Jobs] 
			Set [Share] = 1
			WHERE Jobs.JobID = @JID


		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
END
GO
