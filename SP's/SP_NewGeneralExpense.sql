USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_NewGeneralExpense]
@CID INT, @N VARCHAR (MAX), @D VARCHAR (MAX), @PID INT, @Date DATETIME, @A MONEY, @R BIT
AS
BEGIN
BEGIN TRY
		BEGIN TRANSACTION
			INSERT  INTO [Expense] (CategoryID, [Name], [Description])
			VALUES                (@CID, @N, @D);

			INSERT  INTO [GeneralExpense] (ProfileID, ExpenseID, [Date], Amount, [Repeat], [Invoice/ReceiptCopy])
			VALUES       (@PID, (Select SCOPE_IDENTITY()), @Date, @A, @R, NULL);
COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
END

