USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_NewVehicle]
	@N varchar(30),
	@FC money,
	@MC money,
	@FxC money,
	@PID int,
	@CC money
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
    INSERT  INTO [Vehicle] ([Name], SARSFuelCost, SARSMaintenceCost, SARSFixedCost, ProfileID, ClientCharge)
    VALUES (@N, @FC, @MC, @FxC, @PID, @CC);

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END