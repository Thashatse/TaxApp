USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[SP_GetEmailSettings]
	@PI INT
AS
BEGIN
    SELECT *
    FROM   EmailSettings
    WHERE  ProfileID = @PI;
END

