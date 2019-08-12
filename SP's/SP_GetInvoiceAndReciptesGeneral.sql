USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetInvoiceAndReciptesGeneral
	@PID int,
	@SD datetime,
	@ED datetime
AS
BEGIN
	select Expense.ExpenseID, Expense.[Name], [Date], [Invoice/ReceiptCopy], [FileName], ProfileID 
	From [TaxApp].[dbo].Expense, [TaxApp].[dbo].GeneralExpense
	where Expense.ExpenseID = GeneralExpense.ExpenseID
		And [Invoice/ReceiptCopy] is not NULL
		AND [Date] between @SD and @ED
		AND ProfileID = @PID
	Order by [Date]
END
GO
