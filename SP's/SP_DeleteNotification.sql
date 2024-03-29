USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_DeleteNotification] 
	@NID int
AS
BEGIN
		Begin Transaction;
			Begin Try
				Select Link
				From Notifications
				Where (Notifications.NotificationsID = @NID)

				DELETE FROM Notifications
				Where (Notifications.NotificationsID = @NID)
			End try
		Begin Catch
			if @@TRANCOUNT > 0
				Begin
					ROLLBACK TRANSACTION
				End
		End Catch
commit Transaction
END
