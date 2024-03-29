USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_DeleteTravelLogItem] 
	@EID int
AS
BEGIN
		Begin Transaction;
			Begin Try
				DELETE FROM TravelLog
				Where (TravelLog.ExpenseID = @EID)
			End try
		Begin Catch
			if @@TRANCOUNT > 0
				Begin
					ROLLBACK TRANSACTION
				End
		End Catch
commit Transaction
END
