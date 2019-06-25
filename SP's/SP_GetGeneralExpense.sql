USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_GetGeneralExpense]
	@EID INT
AS
BEGIN
    SELECT Expense.ExpenseID, Expense.CategoryID, Expense.[Name], Expense.[Description],
		GeneralExpense.ProfileID, GeneralExpense.[Date], GeneralExpense.Amount, GeneralExpense.[Repeat], GeneralExpense.[Invoice/ReceiptCopy],
		ExpenseCategory.[Name], ExpenseCategory.[Description]
    FROM   Expense, GeneralExpense, ExpenseCategory
    WHERE  Expense.ExpenseID = @EID
			AND Expense.ExpenseID = GeneralExpense.ExpenseID
			AND Expense.CategoryID = ExpenseCategory.CategoryID
END 

