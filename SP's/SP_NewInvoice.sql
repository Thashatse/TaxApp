USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_NewInvoice] 
	@JID int,
	@INum nchar(11)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT  INTO [JobInvoice] (JobID, InvoiceNum)
        VALUES                (@JID, @INum);

        INSERT  INTO [Invoice] (InvoiceNum, [DateTime], VATRate, Paid)
        VALUES                 (@INum, (select GETDATE()),
								(select VATRate 
									from [Profile], Jobs, Client 
									where [Profile].ProfileID = Client.ProfileID
										AND Client.ClientID = Jobs.ClientID
										AND Jobs.JobID = @JID)
								, 0);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

