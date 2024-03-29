USE [TaxApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_GetClientReportJobExpense]
	@PID INT,
	@CID INT,
	@SD date,
	@ED date
AS
BEGIN
SELECT Client.[ClientID], (Client.FirstName + ' ' + Client.LastName) as ClientName,
		Sum(Amount) as Expenses
	FROM   JobExpense, Jobs, Client
	WHERE  JobExpense.JobID = Jobs.JobID
		AND JobExpense.[Date] between @SD and @ED
		AND Jobs.ClientID = Client.ClientID	
		AND Client.ProfileID = @PID
	Group By Client.ClientID, Client.FirstName, Client.LastName, jobs.ClientID
	ORDER BY FirstName desc
END 

