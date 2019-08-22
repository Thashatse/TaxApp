SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetTaxOrVATOTP 
	@ID int
AS
BEGIN
	Select OTP, OTPDate
	From TaxAndVatPeriods, TaxConsultant
	Where TaxAndVatPeriods.ProfileID = TaxConsultant.ProfileID
		AND Share = 1
		And OTPDate between dateadd(minute,-10,getdate()) and getdate()
		AND TaxAndVatPeriods.PeriodID = @ID
END
GO