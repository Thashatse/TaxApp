SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_GetInvoiceCount
AS
BEGIN
	select count(*)
	from Invoice
	Where [DateTime] between dateadd(day, datediff(day,1, GETDATE()),0) 
		and dateadd(day, datediff(day,-1, GETDATE()),0)
END
GO
