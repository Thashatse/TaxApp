SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_getGeneralExpenseFile
	@EID int
AS
BEGIN
	Select [Invoice/ReceiptCopy], [FileName]
	From GeneralExpense
	Where ExpenseID = @EID
END
GO
