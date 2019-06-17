USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_GetProfile]
@EM VARCHAR (50), @PI INT, @UN varchar (50)
AS
BEGIN
    SELECT *
    FROM   Profile
    WHERE  ProfileID = @PI
           OR EmailAddress = @EM
           OR Username = @UN;
END

