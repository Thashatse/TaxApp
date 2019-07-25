SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE SP_GetTaxAndVatPeriod
	@PID int
AS
BEGIN
	Select *
	From TaxAndVatPeriods
	Where PeriodID = @PID
	Order by StartDate desc
END
GO
