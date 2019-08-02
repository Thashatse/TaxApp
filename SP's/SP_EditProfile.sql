USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_EditProfile]
	@FN VARCHAR (50), @LN VARCHAR (50), @CN VARCHAR (50), @EM VARCHAR (50), 
	@CNum NCHAR (10), @PA VARCHAR (1000), @VATNum NCHAR (30), @DR MONEY, 
	@UN VARCHAR (50), @PID int, @Pass VARCHAR (Max)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    UPDATE [Profile] 
	Set FirstName = @FN,
		LastName = @LN,
		CompanyName = @CN,
		EmailAddress = @EM,
		ContactNumber = @CNum,
		PhysicalAddress = @PA,
		VATNumber = @VATNum,
		DefaultHourlyRate = @DR,
		Username = @UN,
		[Password] = @Pass
    WHERE ProfileID = @PID

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END