USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_GetJobExpensesAllProfile]
	@PID INT
AS
BEGIN
    SELECT Expense.ExpenseID, Expense.CategoryID, Expense.[Name], Expense.[Description],
		JobExpense.JobID, JobExpense.[Date], JobExpense.Amount, JobExpense.[Invoice/ReceiptCopy],
		ExpenseCategory.[Name], ExpenseCategory.[Description], Jobs.JobTitle
    FROM   Expense, JobExpense, ExpenseCategory, Jobs, Client
    WHERE  Expense.ExpenseID = JobExpense.ExpenseID
			AND Expense.CategoryID = ExpenseCategory.CategoryID
			AND JobExpense.JobID = Jobs.JobID
			AND Jobs.ClientID = Client.ClientID
			AND Client.ProfileID = @PID
	Order by JobExpense.[Date] desc
END 