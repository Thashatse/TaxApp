USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_DeleteWorkLogItem] 
	@ID int
AS
BEGIN
		Begin Transaction;
			Begin Try
				DELETE FROM Worklog
				Where (Worklog.LogItemID = @ID)

				DELETE FROM JobHours
				Where (JobHours.LogItemID = @ID)
			End try
		Begin Catch
			if @@TRANCOUNT > 0
				Begin
					ROLLBACK TRANSACTION
				End
		End Catch
commit Transaction
END
