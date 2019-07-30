USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_EditTaxConsultant]
	@N varchar(Max),
	@EA varchar(50),
	@PID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    UPDATE [TaxConsultant] 
	Set [Name] = @N,
		EmailAddress = @EA
    WHERE ProfileID = @PID

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END