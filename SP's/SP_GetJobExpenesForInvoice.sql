SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetJobExpenesForInvoice
	@JID int
AS
BEGIN
	select Expense.ExpenseID, Expense.[Name], Amount/((([Profile].VatRate)/100)+1)
from JobExpense, Expense, Jobs, Client, [Profile]
Where Expense.ExpenseID = JobExpense.ExpenseID
	AND Invoiced = 0
	AND JobExpense.JobID = @JID
	AND JobExpense.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	AND Client.ProfileID = [Profile].ProfileID
END
GO
