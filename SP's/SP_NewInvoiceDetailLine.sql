USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[SP_NewInvoiceDetailLine]
	@INum nchar(11),
	@Name nchar(50),
	@UnitCount int,
	@UnitCost int
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT  INTO [InvoiceLineItem] (InvoiceNum, [Name], UnitCount, UnitCost)
        VALUES       (@INum, @Name, @UnitCount, @UnitCost);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

