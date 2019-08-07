SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_SearchJobExpenses
	@ST varchar(max),
	@PID int,
	@SD DateTime,
	@ED Datetime
AS
BEGIN
	select Expense.ExpenseID, Expense.[Name], Expense.[Description], JobExpense.[Date]
From Expense, ExpenseCategory, JobExpense, Jobs, Client
where (Expense.[Description] like '%'+@ST+'%'
	or Expense.[Name] like '%'+@ST+'%'
	or ExpenseCategory.[Description] like '%'+@ST+'%'
	or ExpenseCategory.[Name] like '%'+@ST+'%')
	AND Expense.CategoryID = ExpenseCategory.CategoryID
	And Expense.ExpenseID = JobExpense.ExpenseID
	AND Client.ProfileID = @PID
	AND JobExpense.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	and JobExpense.[Date] between @SD and @ED
END
GO
