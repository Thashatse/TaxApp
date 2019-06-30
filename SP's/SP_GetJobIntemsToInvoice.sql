SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_GetJobIntemsToInvoice 
	@JID int
AS
BEGIN
	SET NOCOUNT ON;

	Select Worklog.LogItemID, Worklog.[Description], Jobs.HourlyRate, 
	(SELECT Sum(DATEDIFF(MINUTE, StartTime, EndTime))
     FROM   Worklog) AS WorkLogHours
From Worklog, JobHours, Jobs
WHERE  Worklog.LogItemID = JobHours.LogItemID
	And JobHours.JobID = Jobs.JobID
	AND Invoiced = 0
	AND JobHours.JobID = '100000002'

select ExpenseID, Reason, (ClosingKMs - OpeningKMs) as UnitKMs, (Vehicle.FuelCost + Vehicle.MaintenceCost) as CostPerKM 
from TravelLog, Vehicle
Where Invoiced = 0
	AND TravelLog.VehicleID = Vehicle.VehicleID
	AND TravelLog.JobID = '100000002'

select Expense.ExpenseID, Expense.[Name], Amount 
from JobExpense, Expense
Where Expense.ExpenseID = JobExpense.ExpenseID
	AND Invoiced = 0
	AND JobExpense.JobID = '100000002'

END
GO
