USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[SP_GetConsultant]
	@PI INT
AS
BEGIN
    SELECT *
    FROM   TaxConsultant
    WHERE  ProfileID = @PI
END

