SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_GetJobExpenesForInvoice
	@JID int
AS
BEGIN
	select Expense.ExpenseID, Expense.[Name], Amount 
from JobExpense, Expense
Where Expense.ExpenseID = JobExpense.ExpenseID
	AND Invoiced = 0
	AND JobExpense.JobID = @JID
END
GO
