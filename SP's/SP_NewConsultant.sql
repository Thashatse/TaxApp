USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_NewConsultant]
	@EA VARCHAR (50), 
	@PI INT, 
	@N varchar (max)
	AS
BEGIN
    INSERT  INTO [TaxConsultant] (Name, EmailAddress, ProfileID)
    VALUES                (@N, @EA, @PI);
END

