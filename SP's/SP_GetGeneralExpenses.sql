USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_GetGeneralExpenses]
	@PID INT,
	@SD date,
	@ED date
AS
BEGIN
    SELECT Expense.ExpenseID, Expense.CategoryID, Expense.[Name], Expense.[Description],
		GeneralExpense.ProfileID, GeneralExpense.[Date], GeneralExpense.Amount, GeneralExpense.[Repeat], GeneralExpense.[Invoice/ReceiptCopy],
		ExpenseCategory.[Name], ExpenseCategory.[Description], PrimaryExpenseID
    FROM   Expense, GeneralExpense, ExpenseCategory
    WHERE  GeneralExpense.ProfileID = @PID
			AND Expense.ExpenseID = GeneralExpense.ExpenseID
			AND Expense.CategoryID = ExpenseCategory.CategoryID
			and GeneralExpense.[Date] between @SD and @ED
			and (GeneralExpense.ExpenseID = GeneralExpense.PrimaryExpenseID
				or GeneralExpense.PrimaryExpenseID = 0)
	Order by GeneralExpense.[Date] desc
END 