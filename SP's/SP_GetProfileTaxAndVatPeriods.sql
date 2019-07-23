SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetProfileTaxAndVatPeriods
	@PID int,
	@T char(1)
AS
BEGIN
	Select *
	From TaxAndVatPeriods
	Where ProfileID = @PID
		AND [Type] = @T
	Order by StartDate desc
END
GO
