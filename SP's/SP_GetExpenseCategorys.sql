USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_GetExpenseCategorys]
AS
BEGIN
    SELECT *
    FROM   ExpenseCategory
	order by Name
END

 