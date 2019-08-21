SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_UpdateSharePeriodTaxorVatPeriod 
	@PID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		IF ((select Share from TaxAndVatPeriods where PeriodID = @PID)  = 1)
			UPDATE [TaxAndVatPeriods] 
			Set [Share] = 0
			WHERE PeriodID = @PID
	   ELSE 
		   UPDATE [TaxAndVatPeriods] 
			Set [Share] = 1
			WHERE PeriodID = @PID

	IF ((select Share from TaxAndVatPeriods where PeriodID = @PID)  = 1)
			Select * 
			From TaxAndVatPeriods, TaxConsultant
			WHERE PeriodID = @PID
				AND TaxAndVatPeriods.ProfileID = TaxConsultant.ProfileID;
		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
END
GO
