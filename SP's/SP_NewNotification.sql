SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_NewNotification
	@PID int,
	@D DateTime,
	@Dets Varchar(Max),
	@L Varchar(max)
AS
BEGIN
	BEGIN TRY
        BEGIN TRANSACTION;
        INSERT  INTO [Notifications](ProfileID, [Date], Details, Link)
        VALUES       (@PID, @D, @Dets, @L);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END
GO
