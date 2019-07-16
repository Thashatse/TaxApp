USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_NewTravelExpense]
	@F varchar(Max),
	@T varchar(Max),
	@R varchar(Max),
	@OKM decimal(12, 2),
	@CKM decimal(12, 2),
	@VID int,
	@JID int,
	@D date
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    INSERT  INTO [TravelLog] ([From], [To], Reason, OpeningKMs, ClosingKMs, VehicleID, JobID, Invoiced, [Date])
    VALUES (@F, @T, @R, @OKM, @CKM, @VID, @JID, 0, @D);

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END