USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_GetJobExpenses]
	@JID INT
AS
BEGIN
    SELECT Expense.ExpenseID, Expense.CategoryID, Expense.[Name], Expense.[Description],
		JobExpense.JobID, JobExpense.[Date], JobExpense.Amount, JobExpense.[Invoice/ReceiptCopy],
		ExpenseCategory.[Name], ExpenseCategory.[Description]
    FROM   Expense, JobExpense, ExpenseCategory
    WHERE  JobExpense.JobID = @JID
			AND Expense.ExpenseID = JobExpense.ExpenseID
			AND Expense.CategoryID = ExpenseCategory.CategoryID
END 

