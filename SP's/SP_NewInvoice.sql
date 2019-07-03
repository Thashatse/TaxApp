USE [TaxApp]
GO
/****** Object:  StoredProcedure [dbo].[SP_NewInvoice]    Script Date: 2019/07/01 16:28:25 ******/
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
								(select VATRate from Business), 0);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

