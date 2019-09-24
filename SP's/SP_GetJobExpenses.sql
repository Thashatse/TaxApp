USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_GetJobExpenses]
	@JID INT
AS
BEGIN
    SELECT Expense.ExpenseID, Expense.CategoryID, Expense.[Name], Expense.[Description],
		JobExpense.JobID, JobExpense.[Date], JobExpense.Amount, JobExpense.[Invoice/ReceiptCopy],
		ExpenseCategory.[Name], ExpenseCategory.[Description], Jobs.JobTitle, Invoiced
    FROM   Expense, JobExpense, ExpenseCategory, Jobs
    WHERE  JobExpense.JobID = @JID
			AND Expense.ExpenseID = JobExpense.ExpenseID
			AND Expense.CategoryID = ExpenseCategory.CategoryID
			AND JobExpense.JobID = Jobs.JobID
	Order by JobExpense.[Date] desc
END 

