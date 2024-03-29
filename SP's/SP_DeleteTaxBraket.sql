USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_DeleteTaxBraket] 
	@RID int
AS
BEGIN
		Begin Transaction;
			Begin Try
				DELETE FROM TaxPeriodRates
				Where (RateID = @RID)
			End try
		Begin Catch
			if @@TRANCOUNT > 0
				Begin
					ROLLBACK TRANSACTION
				End
		End Catch
commit Transaction
END
