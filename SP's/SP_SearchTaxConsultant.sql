SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_SearchTaxConsultant
	@ST varchar(max),
	@PID int
AS
BEGIN
	select TaxConsultant.[Name]
From TaxConsultant
where (TaxConsultant.[Name] like '%'+@ST+'%'
	or TaxConsultant.EmailAddress like '%'+@ST+'%')
	And TaxConsultant.ProfileID = @PID
END
GO
