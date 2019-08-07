SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_SearchClients
	@ST varchar(max),
	@PID int
AS
BEGIN

select ClientID, Client.FirstName + ' ' + Client.LastName as [Name], CompanyName
From Client
where (Client.CompanyName like '%'+@ST+'%'
	or Client.FirstName like '%'+@ST+'%'
	or Client.LastName like '%'+@ST+'%'
	or Client.EmailAddress like '%'+@ST+'%'
	or Client.PhysiclaAddress like '%'+@ST+'%')
	And Client.ProfileID = @PID
	
END
GO
