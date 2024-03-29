USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_NewGeneralExpense]
	@CID INT, @N VARCHAR (MAX), @D VARCHAR (MAX), @PID INT, @Date DATETIME, @A MONEY, @R BIT, @PEID int
AS
BEGIN
BEGIN TRY
		BEGIN TRANSACTION
			INSERT  INTO [Expense] (CategoryID, [Name], [Description])
			VALUES                (@CID, @N, @D);

			declare @NPEID int;
			set @NPEID = (Select SCOPE_IDENTITY())

			INSERT  INTO [GeneralExpense] (ProfileID, ExpenseID, [Date], Amount, [Repeat], [Invoice/ReceiptCopy], PrimaryExpenseID)
			VALUES       (@PID, @NPEID, @Date, @A, @R, NULL, @NPEID);

			UPDATE [GeneralExpense] 
			Set PrimaryExpenseID = @NPEID,
				Repeat = 0
			WHERE PrimaryExpenseID = @PEID
					or ExpenseID = @PEID
COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
END

