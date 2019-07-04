SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_GetJobInvoices
	@JID int
AS
BEGIN
	select Invoice.InvoiceNum, [DateTime], VATRate, Paid,
	   Jobs.JobID, Jobs.JobTitle,
	   Client.ClientID, Client.FirstName +' '+ Client.LastName as [ClientName], Client.CompanyName,
		Client.EmailAddress, Client.PhysiclaAddress,
		(select Sum(InvoiceLineItem.UnitCount * InvoiceLineItem.unitCost) 
		from InvoiceLineItem
		Where InvoiceLineItem.InvoiceNum = Invoice.InvoiceNum)as TotalCost
	from Invoice, JobInvoice, Jobs, Client
	where Invoice.InvoiceNum = JobInvoice.InvoiceNum
		AND JobInvoice.JobID = Jobs.JobID
		AND Jobs.ClientID = Client.ClientID
		AND JobInvoice.JobID = @JID
END
GO
