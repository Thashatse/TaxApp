USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[SP_GetJob]
	@JID INT,
	@CID INT
AS
BEGIN
    SELECT *
    FROM   Jobs
    WHERE  JobID = @JID
		or ClientID = @CID;
END

