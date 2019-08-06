SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE SP_TotalUnPaidPast 
	@PID INT,
	@CID INT,
	@SD date,
	@ED date
AS
BEGIN
	SELECT Jobs.JobID,(((Sum(UnitCost * UnitCount)/100)*Invoice.VATRate)+Sum(UnitCost * UnitCount)) AS TotalUnPaid
            FROM   JobInvoice, Invoice, InvoiceLineItem, Jobs, Client
			WHERE  JobInvoice.InvoiceNum = Invoice.InvoiceNum
				   AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
				   AND JobInvoice.JobID = Jobs.JobID
				   AND Jobs.ClientID = Client.ClientID
				   AND (Client.ProfileID = @PID or Jobs.ClientID = @CID)
                   AND Invoice.Paid = 0
				and Jobs.StartDate between @SD and @ED
			GROUP BY Invoice.VATRate, Jobs.JobID
END
GO