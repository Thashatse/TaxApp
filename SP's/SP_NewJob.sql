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
	@SD datetime,
	@S Bit
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
    INSERT  INTO [Jobs] (ClientID, JobTitle, HourlyRate, Budget, StartDate, Noti100, Noti75, Noti90, Noti95, Share)
    VALUES (@CI, @JT, @HR, @B, @SD, 0,0,0,0, @S);

	SELECT SCOPE_IDENTITY()
COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END

