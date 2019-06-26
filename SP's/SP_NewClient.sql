USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_NewClient]
	@FN varchar(50),
	@LN Varchar(50),
	@CN varchar(50),
	@CNum nchar(10),
	@EA varchar(50),
	@PA varchar(50),
	@PC nchar(3),
	@PI int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
    INSERT  INTO [Client] (FirstName, LastName, CompanyName, ContactNumber,
	 EmailAddress, PhysiclaAddress, PreferedCommunicationChannel, ProfileID)
    VALUES (@FN, @LN, @CN, @CNum, @EA, @PA, @PC, @PI);

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END

