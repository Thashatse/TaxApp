 SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
CREATE PROCEDURE SP_GetProfile 
	@EM varchar(50),
	@PI int,
	@UN varbinary(50)
AS
BEGIN
	Select *
	From Profile
	Where ProfileID = @PI
		OR EmailAddress = @EM
		OR Username = @UN	
END
GO
