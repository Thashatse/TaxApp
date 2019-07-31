USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_NewJob]
	@CI int,
	@JT Varchar(Max),
	@HR money,
	@B money = 0,
	@SD datetime
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
    INSERT  INTO [Jobs] (ClientID, JobTitle, HourlyRate, Budget, StartDate)
    VALUES (@CI, @JT, @HR, @B, @SD);
COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END

