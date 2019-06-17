USE [TaxApp]
GO
/****** Object:  StoredProcedure [dbo].[SP_NewProfile]    Script Date: 2019/06/17 16:58:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_NewProfile]
@FN VARCHAR (50), @LN VARCHAR (50), @CN VARCHAR (50), @EM VARCHAR (50), @CNum NCHAR (10), @PA VARCHAR (1000), @VATNum NCHAR (30), @DR MONEY, @UN VARCHAR (50), @Pass VARCHAR (Max)
AS
BEGIN
    INSERT  INTO [Profile] (FirstName, LastName, CompanyName, EmailAddress, ContactNumber, PhysicalAddress, VATNumber, DefaultHourlyRate, Active, Username, [Password])
    VALUES                (@FN, @LN, @CN, @EM, @CNum, @PA, @VATNum, @DR, 1, @UN, @Pass);
END

