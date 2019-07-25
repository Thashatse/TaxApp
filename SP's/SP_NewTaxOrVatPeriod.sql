USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_NewTaxOrVatPeriod]
	@PID int,
	@SD Date,
	@ED Date,
	@T char(1)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
    INSERT  INTO [TaxAndVatPeriods] (ProfileID, StartDate, EndDate, [Type], VATRate)
    VALUES (@PID, @SD, @ED, @T, (Select VATRate
								FROM [Profile]
								Where ProfileID = @PID));
COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END

