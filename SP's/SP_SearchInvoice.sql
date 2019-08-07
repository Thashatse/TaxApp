SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_SearchInvoice
	@ST varchar(max),
	@PID int,
	@SD DateTime,
	@ED Datetime
AS
BEGIN
	select Invoice.InvoiceNum, JobTitle, Client.FirstName + ' ' + Client.LastName as [Name],  StartDate 
From Jobs, Client, Invoice, InvoiceLineItem, JobInvoice
where (JobTitle like '%'+@ST+'%'
	or Client.CompanyName like '%'+@ST+'%'
	or Client.FirstName like '%'+@ST+'%'
	or Client.LastName like '%'+@ST+'%'
	or Client.EmailAddress like '%'+@ST+'%'
	or Client.PhysiclaAddress like '%'+@ST+'%'
	or InvoiceLineItem.[Name] like '%'+@ST+'%')
	And Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID
	AND Jobs.JobID = JobInvoice.JobID
	AND JobInvoice.InvoiceNum = Invoice.InvoiceNum
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND StartDate between @SD and @ED
Group by Invoice.InvoiceNum, JobTitle, Client.FirstName, Client.LastName,  StartDate
END
GO
