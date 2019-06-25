USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_NewGeneralExpense]
	@CID int,
	@N varchar(max),
	@D varchar(max),
	@PID int,
	@Date datetime,
	@A money,
	@R bit
    /**@IRC varbinary(max)**/
AS
BEGIN
    INSERT  INTO [Expense] (CategoryID, [Name], [Description])
    VALUES (@CID, @N, @D);

	INSERT  INTO [GeneralExpense] (ProfileID, ExpenseID, [Date], Amount, [Repeat], [Invoice/ReceiptCopy])
    VALUES (@PID, (SELECT @@IDENTITY
						From Expense), 
			@Date, @A, @R, null);
END

