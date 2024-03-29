USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_NewConsultant]
	@EA VARCHAR (50), 
	@PI INT, 
	@N varchar (max)
	AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
    INSERT  INTO [TaxConsultant] (Name, EmailAddress, ProfileID)
    VALUES                (@N, @EA, @PI);

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END

