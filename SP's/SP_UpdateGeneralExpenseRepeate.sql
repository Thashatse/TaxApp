USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_UpdateGeneralExpenseRepeate]
	@EID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    UPDATE [GeneralExpense] 
	Set [Repeat] = 0,
	PrimaryExpenseID = (Select SCOPE_IDENTITY() From GeneralExpense)
    WHERE ExpenseID = @EID

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END