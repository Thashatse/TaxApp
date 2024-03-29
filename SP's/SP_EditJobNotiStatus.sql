USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_EditJobNotiStatus]
	@75 bit,
	@90 bit,
	@95 bit,
	@100 bit,
	@JID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    UPDATE [Jobs] 
	Set Noti100 = @100,
		Noti75 = @75,
		Noti90 = @90,
		Noti95 = @95
    WHERE JobID = @JID

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END