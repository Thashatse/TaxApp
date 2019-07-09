USE [TaxApp]
GO
/****** Object:  StoredProcedure [dbo].[SP_NewWorkLogItem]    Script Date: 2019/06/30 15:44:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_NewWorkLogItem]
@JID INT, @D VARCHAR (MAX), @ST DATETIME, @ET DATETIME
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT  INTO [Worklog] ([Description], StartTime, EndTime, Invoiced)
        VALUES                (@D, @ST, @ET, 0);

        INSERT  INTO [JobHours] (JobID, LogItemID)
        VALUES                 (@JID, (SELECT @@IDENTITY));

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

