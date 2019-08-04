USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_EditWorkLogItem]
	@D VARCHAR (MAX), @ST DATETIME, @ET DATETIME,
	@ID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    UPDATE [Worklog] 
	Set [Description] = @D,
		StartTime = @ST,
		EndTime = @ET
    WHERE Worklog.LogItemID = @ID

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END