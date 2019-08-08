SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_GetNotifications
	@PID int
AS
BEGIN
	select *
	from Notifications
	Where ProfileID = @PID
END
GO
