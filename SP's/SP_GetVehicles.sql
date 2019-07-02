USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_GetVehicles]
	@PID INT
AS
BEGIN
    SELECT *
    FROM   Vehicle
	Where ProfileID = @PID
	order by [Name]
END