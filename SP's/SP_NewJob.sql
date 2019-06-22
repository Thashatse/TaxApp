USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[SP_NewJob]
	@CI int,
	@JT Varchar(Max),
	@HR money,
	@B money,
	@SD datetime
AS
BEGIN
    INSERT  INTO [Jobs] (ClientID, JobTitle, HourlyRate, Budget, StartDate)
    VALUES (@CI, @JT, @HR, @B, @SD);
END

