USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_GetProfileClients]
	@PID INT
AS
BEGIN
    SELECT *
    FROM   Client
    WHERE  ProfileID = @PID
	Order by FirstName
END

