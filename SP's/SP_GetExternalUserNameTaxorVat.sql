SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetExternalUserNameTaxorVat
	@ID int
AS
BEGIN
	Select [Name] as userName, EmailAddress, TaxConsultant.ProfileID as ID
	From TaxAndVatPeriods, TaxConsultant
	Where TaxAndVatPeriods.ProfileID = TaxConsultant.ProfileID
		AND Share = 1
		AND TaxAndVatPeriods.PeriodID = @ID
		And OTPDate between dateadd(minute,-10,getdate()) and getdate()
END
GO
