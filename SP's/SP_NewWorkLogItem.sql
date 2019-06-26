USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_NewWorkLogItem]
	@JID int,
	@D varchar(Max),
	@ST datetime,
	@ET datetime
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
    INSERT  INTO [Worklog] ([Description], StartTime, EndTime)
    VALUES (@D, @ST, @ET);

	INSERT  INTO [JobHours] (JobID, LogItemID)
    VALUES (@JID, (SELECT @@IDENTITY
						From Worklog));

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END

