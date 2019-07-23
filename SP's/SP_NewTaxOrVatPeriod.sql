USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_NewTaxOrVatPeriod]
	@PID int,
	@SD Date,
	@ED Date,
	@T char(1)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
    INSERT  INTO [TaxAndVatPeriods] (ProfileID, StartDate, EndDate, [Type])
    VALUES (@PID, @SD, @ED, @T);
COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END

