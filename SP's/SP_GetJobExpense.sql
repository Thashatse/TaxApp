USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_GetJobExpense]
	@EID INT
AS
BEGIN
    SELECT Expense.ExpenseID, Expense.CategoryID, Expense.[Name], Expense.[Description],
		JobExpense.JobID, JobExpense.[Date], JobExpense.Amount, JobExpense.[Invoice/ReceiptCopy],
		ExpenseCategory.[Name], ExpenseCategory.[Description]
    FROM   Expense, JobExpense, ExpenseCategory
    WHERE  Expense.ExpenseID = @EID
			AND Expense.ExpenseID = JobExpense.ExpenseID
			AND Expense.CategoryID = ExpenseCategory.CategoryID
END 

