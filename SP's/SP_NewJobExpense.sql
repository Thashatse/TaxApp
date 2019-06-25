USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_NewJobExpense]
	@CID int,
	@N varchar(max),
	@D varchar(max),
	@JID int,
	@Date datetime,
	@A money
	/**@IRC varbinary(max)**/
AS
BEGIN
    INSERT  INTO [Expense] (CategoryID, [Name], [Description])
    VALUES (@CID, @N, @D);

	INSERT  INTO [JobExpense] (JobID, ExpenseID, [Date], Amount, [Invoice/ReceiptCopy])
    VALUES (@JID, (SELECT @@IDENTITY
						From Expense), 
			@Date, @A, null);
END

