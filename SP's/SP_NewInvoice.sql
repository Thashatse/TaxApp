USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[SP_NewInvoice]
	@JID int,
	@INum nchar(11),
	@Date datetime,
	@VATRate decimal(4,2)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT  INTO [JobInvoice] (JobID, InvoiceNum)
        VALUES                (@JID, @INum);
        INSERT  INTO [Invoice] (InvoiceNum, [DateTime], VATRate, Paid)
        VALUES                 (@INum, @Date, @VATRate, 0);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

