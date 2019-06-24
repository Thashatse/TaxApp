USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_GetProfileJobs]
	@PID INT
AS
BEGIN
    SELECT *
    FROM   Jobs, Client
    WHERE  ProfileID = @PID
		and Client.ClientID
		= Jobs.ClientID 
END 

