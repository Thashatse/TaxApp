-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_GetRepeatGeneralExpenses
AS
BEGIN
	SELECT Expense.ExpenseID, Expense.CategoryID, Expense.[Name], Expense.[Description],
		GeneralExpense.ProfileID, GeneralExpense.[Date], GeneralExpense.Amount, GeneralExpense.[Repeat], GeneralExpense.[Invoice/ReceiptCopy],
		ExpenseCategory.[Name], ExpenseCategory.[Description]
    FROM   Expense, GeneralExpense, ExpenseCategory
    WHERE  Expense.ExpenseID = GeneralExpense.ExpenseID
			AND Expense.CategoryID = ExpenseCategory.CategoryID
			AND GeneralExpense.Repeat = 1
			AND GeneralExpense.Date <= DATEADD(day, -30, getdate())
END
GO
