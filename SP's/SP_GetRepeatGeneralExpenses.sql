SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetRepeatGeneralExpenses
AS
BEGIN
	SELECT Expense.ExpenseID, Expense.CategoryID, Expense.[Name], Expense.[Description],
		GeneralExpense.ProfileID, GeneralExpense.[Date], GeneralExpense.Amount, GeneralExpense.[Repeat], GeneralExpense.[Invoice/ReceiptCopy],
		ExpenseCategory.[Name], ExpenseCategory.[Description], GeneralExpense.PrimaryExpenseID
    FROM   Expense, GeneralExpense, ExpenseCategory
    WHERE  Expense.ExpenseID = GeneralExpense.ExpenseID
			AND Expense.CategoryID = ExpenseCategory.CategoryID
			AND GeneralExpense.Repeat = 1
			AND GeneralExpense.Date <= DATEADD(day, -30, getdate())
END
GO
