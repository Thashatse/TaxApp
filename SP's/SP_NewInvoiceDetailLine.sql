USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_NewInvoiceDetailLine]
	@INum nchar(11),
	@ID int,
	@Name nchar(50),
	@UnitCount decimal(18, 2),
	@UnitCost decimal(18, 2),
	@T char
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT  INTO [InvoiceLineItem] (InvoiceNum, [Name], UnitCount, UnitCost, [Type])
        VALUES       (@INum, @Name, @UnitCount, @UnitCost, @T);

		Update [JobExpense]
		Set invoiced = 1
		from JobExpense, Expense
		Where JobExpense.ExpenseID = @ID
				And Expense.ExpenseID = JobExpense.ExpenseID
				And Expense.[Name] = @Name

		Update [TravelLog]
		Set invoiced = 1
		Where TravelLog.ExpenseID = @ID
			And TravelLog.Reason = @Name

		Update [WorkLog]
		Set invoiced = 1
		Where Worklog.LogItemID = @ID
			And Worklog.[Description] = @Name

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

