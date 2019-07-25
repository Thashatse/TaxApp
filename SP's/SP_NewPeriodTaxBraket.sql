USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_NewPeriodTaxBraket]
	@PID INT, @R Decimal(4,2), @T Decimal(18,2), @Ty Char(1)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT  INTO [TaxPeriodRates] (PeriodID, Rate, Threshold, [Type])
        VALUES                (@PID, @R, @T, @TY);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

