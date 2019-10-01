USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_updateJobExpense]
	@EID int, @CID INT, @N VARCHAR (MAX), @D VARCHAR (MAX), @Date DATETIME, @A MONEY
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    UPDATE [JobExpense] 
	Set [Date] = @Date,
	Amount = @A
    WHERE ExpenseID = @EID
	
    UPDATE [Expense] 
	Set CategoryID = @CID,
	[Name] = @N,
	[Description] = @D
    WHERE ExpenseID = @EID

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END