SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE SP_TotalPaid 
	@PID INT,
	@CID INT
AS
BEGIN
	SELECT Jobs.JobID,(((Sum(UnitCost * UnitCount)/100)*Invoice.VATRate)+Sum(UnitCost * UnitCount)) AS TotalPaid
            FROM   JobInvoice, Invoice, InvoiceLineItem, Jobs, Client
			WHERE  JobInvoice.InvoiceNum = Invoice.InvoiceNum
				   AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
				   AND JobInvoice.JobID = Jobs.JobID
				   AND Jobs.ClientID = Client.ClientID
				   AND (Client.ProfileID = @PID or Jobs.ClientID = @CID)
                   AND Invoice.Paid = 1
			GROUP BY Invoice.VATRate, Jobs.JobID
END
GO
