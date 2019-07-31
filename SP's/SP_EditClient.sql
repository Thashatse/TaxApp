USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_EditClient]
	@FN varchar(50),
	@LN Varchar(50),
	@CN varchar(50),
	@CNum nchar(10),
	@EA varchar(50),
	@PA varchar(50),
	@CID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    UPDATE [Client] 
	Set FirstName = @FN,
		LastName = @LN,
		CompanyName = @CN,
		ContactNumber = @CNum,
		EmailAddress = @EA,
		PhysiclaAddress = @PA
    WHERE ClientID = @CID

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END