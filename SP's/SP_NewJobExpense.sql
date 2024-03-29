USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_NewJobExpense]
@CID INT, @N VARCHAR (MAX), @D VARCHAR (MAX), @JID INT, @Date DATETIME, @A MONEY
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT  INTO [Expense] (CategoryID, [Name], [Description])
        VALUES                (@CID, @N, @D);
        INSERT  INTO [JobExpense] (JobID, ExpenseID, [Date], Amount, [Invoice/ReceiptCopy], Invoiced)
        VALUES                   (@JID, (SELECT SCOPE_IDENTITY()), @Date, @A, NULL, 0);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

