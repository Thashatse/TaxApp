USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_addJobExpenseFile]
	@EID int,
	@IRC varbinary(MAX),
	@FN varchar(MAX)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    UPDATE [JobExpense] 
	Set [Invoice/ReceiptCopy] = @IRC,
		[FileName] = @FN
    WHERE ExpenseID = @EID

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END