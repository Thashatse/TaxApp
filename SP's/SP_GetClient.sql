USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_GetClient]
	@CID INT
AS
BEGIN
    SELECT *
    FROM   Client
    WHERE  ClientID = @CID
END

