USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_editGeneralExpense]
	@EID int, @CID INT, @N VARCHAR (MAX), @D VARCHAR (MAX), @Date DATETIME, @A MONEY, @R BIT
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    UPDATE [GeneralExpense] 
	Set Repeat = @R,
	Date = @Date,
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