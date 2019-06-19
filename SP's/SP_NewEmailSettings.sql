USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[SP_NewEmailSettings]
	@PI int,
	@A VARCHAR (Max),
	@Pass VARCHAR (MAx),
	@H VARCHAR (50),
	@P VARCHAR (50),
	@ESsl BIT,
	@DM Varchar (50),
	@UDC bit
AS
BEGIN
    INSERT  INTO [EmailSettings] (ProfileID, Address, Password, Host, Port, EnableSsl, DeliveryMethod, UseDefailtCredentials)
    VALUES                (@PI, @A, @Pass, @H, @P, @ESsl, @DM, @UDC);
END

