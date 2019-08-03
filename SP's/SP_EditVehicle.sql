USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_EditVehicle]
	@N varchar(30),
	@FC money,
	@MC money,
	@FxC money,
	@VID int,
	@CC money
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    UPDATE [Vehicle] 
	Set [Name] = @N,
		SARSFuelCost = @FC,
		SARSMaintenceCost = @MC,
		SARSFixedCost = @FxC,
		ClientCharge = @CC
    WHERE VehicleID = @VID

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END