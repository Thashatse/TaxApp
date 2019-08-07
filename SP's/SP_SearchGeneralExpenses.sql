SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_SearchGeneralExpenses
	@ST varchar(max),
	@PID int,
	@SD DateTime,
	@ED Datetime
AS
BEGIN
	select Expense.ExpenseID, Expense.[Name], Expense.[Description], GeneralExpense.[Date]
From Expense, ExpenseCategory, GeneralExpense
where (Expense.[Description] like '%'+@ST+'%'
	or Expense.[Name] like '%'+@ST+'%'
	or ExpenseCategory.[Description] like '%'+@ST+'%'
	or ExpenseCategory.[Name] like '%'+@ST+'%')
	AND Expense.CategoryID = ExpenseCategory.CategoryID
	And Expense.ExpenseID = GeneralExpense.ExpenseID
	AND GeneralExpense.ProfileID = @PID
	AND GeneralExpense.[Date] between @SD and @ED
END
GO
