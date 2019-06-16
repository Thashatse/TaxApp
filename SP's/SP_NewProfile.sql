SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	Adds new user to profile table
-- =============================================
CREATE PROCEDURE SP_NewProfile 
	-- Add the parameters for the stored procedure here
	@FN varchar(50), 
	@LN varchar(50),
	@CN varchar(50), 
	@EM varchar(50),
	@CNum nchar(10), 
	@PA varchar(1000),
	@VATNum nchar(30), 
	@DR money,
	@UN varbinary(50), 
	@Pass varbinary(50)
AS
BEGIN
	Insert Into [Profile](FirstName, LastName, CompanyName, EmailAddress, ContactNumber, PhysicalAddress, VATNumber, DefaultHourlyRate, Active, Username, [Password])
	Values (@FN, @LN, @CN, @EM, @CNum, @PA, @VATNum, @DR, 1, @UN, @Pass)
END
GO
