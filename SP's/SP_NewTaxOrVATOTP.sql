
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_NewTaxOrVATOTP 
	@ID int,
	@OTP int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

			UPDATE [TaxConsultant] 
			Set [OTP] = @OTP,
				[OTPDate] = GETDATE()
			Where TaxConsultant.ProfileID = (Select Top 1 TaxAndVatPeriods.ProfileID 
											 From TaxAndVatPeriods
											 Where Share = 1
											 AND TaxAndVatPeriods.PeriodID = @ID)

		COMMIT TRANSACTION 
	END TRY 
		BEGIN CATCH 
			IF @@TRANCOUNT > 0 
				ROLLBACK TRANSACTION
		END CATCH 
END
GO