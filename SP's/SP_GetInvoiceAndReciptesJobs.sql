USE [TaxApp]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetInvoiceAndReciptesJobs
	@PID int,
	@SD datetime,
	@ED datetime
AS
BEGIN
	select Expense.ExpenseID, Expense.[Name], [Date], [Invoice/ReceiptCopy], [FileName], ProfileID 
	From [TaxApp].[dbo].Expense, [TaxApp].[dbo].JobExpense, [TaxApp].[dbo].Jobs, [TaxApp].[dbo].Client
	where Expense.ExpenseID = JobExpense.ExpenseID
		And [Invoice/ReceiptCopy] is not NULL
		And Jobs.JobID = JobExpense.JobID
		AND Jobs.ClientID = Client.ClientID
		AND [Date] between @SD and @ED
		AND ProfileID = @PID
	Order by [Date]
END
GO
