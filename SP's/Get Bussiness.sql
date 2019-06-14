SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
CREATE PROCEDURE SP_GetBussiness
AS
BEGIN
	SELECT TOP (1000) [BusinessID]
      ,[VATRate]
      ,[SMSSid]
      ,[SMSToken]
  FROM [TaxApp].[dbo].[Business]
END
GO