SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetInvoice
	@IN nchar(11)
AS
BEGIN
	select Invoice.InvoiceNum, [DateTime], VATRate, Paid, [Type],
	   LineItemID, [Name], UnitCount, UnitCost, (UnitCount*UnitCost) as TotalCost,
	   Jobs.JobID, Jobs.JobTitle,
	   Client.ClientID, Client.FirstName +' '+ Client.LastName as [ClientName], Client.CompanyName,
		Client.EmailAddress, Client.PhysiclaAddress
	from Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
	where Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
		AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
		AND JobInvoice.JobID = Jobs.JobID
		AND Jobs.ClientID = Client.ClientID
		AND Invoice.InvoiceNum = @IN
END
GO
