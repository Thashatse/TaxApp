SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE SP_getJobExpenseFile
	@EID int
AS
BEGIN
	Select [Invoice/ReceiptCopy], [FileName]
	From JobExpense
	Where ExpenseID = @EID
END
GO
