USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_addGeneralExpenseFile]
	@EID int,
	@IRC varbinary(MAX),
	@FN varchar(MAX)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    UPDATE [GeneralExpense] 
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