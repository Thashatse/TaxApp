USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_GetOverdueInvoices]
AS
BEGIN
	select Invoice.InvoiceNum, Invoice.DateTime, Jobs.JobTitle, Client.FirstName + ' '+Client.LastName as name, Profile.ProfileID 
	from invoice, JobInvoice, Jobs, Client, Profile
	where Paid = 0 
		AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
		AND JobInvoice.JobID = Jobs.JobID
		AND Jobs.ClientID = Client.ClientID
		AND Client.ProfileID = Profile.ProfileID 
		AND Invoice.DateTime <= DATEADD(DAY, -5, GETDATE())
END
