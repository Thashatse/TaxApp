USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_GetProfileJobs]
	@PID INT,
	@CID INT
AS
BEGIN
    SELECT *
    FROM   Jobs, Client
    WHERE  (ProfileID = @PID
		or Jobs.ClientID = @CID)
		and Client.ClientID
		= Jobs.ClientID 
END 

