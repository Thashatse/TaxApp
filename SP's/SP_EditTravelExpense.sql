USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_EditTravelExpense]
	@F varchar(Max),
	@T varchar(Max),
	@R varchar(Max),
	@OKM decimal(12, 2),
	@CKM decimal(12, 2),
	@VID int,
	@D date,
	@EID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    UPDATE [TravelLog] 
	Set [From] = @F,
		[To] = @T, 
		Reason = @R, 
		OpeningKMs = @OKM, 
		ClosingKMs = @CKM, 
		VehicleID = @VID, 
		[Date] = @D
    WHERE ExpenseID = @EID

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END