USE [master]
GO
/****** Object:  Database [TaxApp]    Script Date: 2019/07/29 20:24:34 ******/
CREATE DATABASE [TaxApp]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TaxApp', FILENAME = N'F:\Google Drive\Visual Studio\TaxApp\TaxApp\TaxApp.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TaxApp_log', FILENAME = N'F:\Google Drive\Visual Studio\TaxApp\TaxApp\TaxApp_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [TaxApp] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TaxApp].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TaxApp] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TaxApp] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TaxApp] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TaxApp] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TaxApp] SET ARITHABORT OFF 
GO
ALTER DATABASE [TaxApp] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TaxApp] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TaxApp] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TaxApp] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TaxApp] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TaxApp] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TaxApp] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TaxApp] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TaxApp] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TaxApp] SET  DISABLE_BROKER 
GO
ALTER DATABASE [TaxApp] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TaxApp] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TaxApp] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TaxApp] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TaxApp] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TaxApp] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TaxApp] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TaxApp] SET RECOVERY FULL 
GO
ALTER DATABASE [TaxApp] SET  MULTI_USER 
GO
ALTER DATABASE [TaxApp] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TaxApp] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TaxApp] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TaxApp] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [TaxApp] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'TaxApp', N'ON'
GO
ALTER DATABASE [TaxApp] SET QUERY_STORE = OFF
GO
USE [TaxApp]
GO
/****** Object:  Table [dbo].[Business]    Script Date: 2019/07/29 20:24:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Business](
	[BussinessID] [int] IDENTITY(1000000000,1) NOT NULL,
	[VATRate] [decimal](4, 2) NOT NULL,
	[SMSSid] [varchar](max) NOT NULL,
	[SMSToken] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Business] PRIMARY KEY CLUSTERED 
(
	[BussinessID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 2019/07/29 20:24:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[ClientID] [int] IDENTITY(1000000000,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[CompanyName] [varchar](50) NOT NULL,
	[ContactNumber] [nchar](10) NOT NULL,
	[EmailAddress] [varchar](50) NOT NULL,
	[PhysiclaAddress] [varchar](50) NOT NULL,
	[PreferedCommunicationChannel] [nchar](3) NOT NULL,
	[ProfileID] [int] NOT NULL,
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[ClientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailSettings]    Script Date: 2019/07/29 20:24:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailSettings](
	[ProfileID] [int] NOT NULL,
	[Address] [varchar](max) NOT NULL,
	[Password] [varchar](max) NOT NULL,
	[Host] [varchar](50) NOT NULL,
	[Port] [varchar](50) NOT NULL,
	[EnableSsl] [bit] NOT NULL,
	[DeliveryMethod] [varchar](50) NOT NULL,
	[UseDefailtCredentials] [bit] NOT NULL,
 CONSTRAINT [PK_EmailSettings] PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Expense]    Script Date: 2019/07/29 20:24:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Expense](
	[ExpenseID] [int] IDENTITY(1000000000,1) NOT NULL,
	[CategoryID] [int] NOT NULL,
	[Name] [varchar](max) NOT NULL,
	[Description] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Expense] PRIMARY KEY CLUSTERED 
(
	[ExpenseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExpenseCategory]    Script Date: 2019/07/29 20:24:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExpenseCategory](
	[Name] [varchar](max) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CategoryID] [int] IDENTITY(1000000000,1) NOT NULL,
 CONSTRAINT [PK_ExpenseCategory] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GeneralExpense]    Script Date: 2019/07/29 20:24:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeneralExpense](
	[ProfileID] [int] NOT NULL,
	[ExpenseID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Amount] [money] NOT NULL,
	[Repeat] [bit] NOT NULL,
	[Invoice/ReceiptCopy] [varbinary](max) NULL,
 CONSTRAINT [PK_GeneralExpense] PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC,
	[ExpenseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invoice]    Script Date: 2019/07/29 20:24:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoice](
	[InvoiceNum] [nchar](11) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[VATRate] [decimal](4, 2) NOT NULL,
	[Paid] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceLineItem]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceLineItem](
	[LineItemID] [int] IDENTITY(100000000,1) NOT NULL,
	[InvoiceNum] [nchar](11) NOT NULL,
	[Name] [nchar](50) NOT NULL,
	[UnitCount] [decimal](18, 2) NOT NULL,
	[UnitCost] [money] NOT NULL,
	[Type] [char](1) NOT NULL,
 CONSTRAINT [PK_InvoiceLineItem_1] PRIMARY KEY CLUSTERED 
(
	[LineItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobExpense]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobExpense](
	[JobID] [int] NOT NULL,
	[ExpenseID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Amount] [money] NOT NULL,
	[Invoice/ReceiptCopy] [varbinary](max) NULL,
	[Invoiced] [bit] NOT NULL,
 CONSTRAINT [PK_JobExpense] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC,
	[ExpenseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobHours]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobHours](
	[JobID] [int] NOT NULL,
	[LogItemID] [int] NOT NULL,
 CONSTRAINT [PK_JobHours] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC,
	[LogItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobInvoice]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobInvoice](
	[JobID] [int] NOT NULL,
	[InvoiceNum] [nchar](11) NOT NULL,
 CONSTRAINT [PK_JobInvoice] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC,
	[InvoiceNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Jobs]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Jobs](
	[JobID] [int] IDENTITY(100000000,1) NOT NULL,
	[ClientID] [int] NOT NULL,
	[JobTitle] [varchar](max) NOT NULL,
	[HourlyRate] [money] NULL,
	[Budget] [money] NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_Jobs] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Profile]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profile](
	[ProfileID] [int] IDENTITY(1000000000,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[CompanyName] [varchar](50) NULL,
	[EmailAddress] [varchar](50) NOT NULL,
	[ContactNumber] [nchar](10) NOT NULL,
	[PhysicalAddress] [varchar](1000) NULL,
	[ProfilePicture] [varbinary](max) NULL,
	[VATNumber] [nchar](30) NOT NULL,
	[DefaultHourlyRate] [money] NOT NULL,
	[Active] [bit] NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Password] [varchar](max) NOT NULL,
	[PassRestCode] [varchar](30) NULL,
	[VATRate] [decimal](4, 2) NULL,
 CONSTRAINT [PK_Profile_1] PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaxAndVatPeriods]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaxAndVatPeriods](
	[PeriodID] [int] IDENTITY(1000000000,1) NOT NULL,
	[ProfileID] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[Type] [char](1) NOT NULL,
	[VATRate] [decimal](4, 2) NULL,
 CONSTRAINT [PK_TaxAndVatPeriods] PRIMARY KEY CLUSTERED 
(
	[PeriodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaxConsultant]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaxConsultant](
	[ProfileID] [int] NOT NULL,
	[Name] [varchar](max) NOT NULL,
	[EmailAddress] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TaxConsultant] PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaxPeriodRates]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaxPeriodRates](
	[PeriodID] [int] NOT NULL,
	[Rate] [decimal](4, 2) NOT NULL,
	[Threshold] [decimal](18, 2) NOT NULL,
	[Type] [char](1) NOT NULL,
	[RateID] [int] IDENTITY(1000000000,1) NOT NULL,
 CONSTRAINT [PK_TaxPeriodRates] PRIMARY KEY CLUSTERED 
(
	[RateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TravelLog]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TravelLog](
	[ExpenseID] [int] IDENTITY(1000000000,1) NOT NULL,
	[From] [varchar](max) NOT NULL,
	[To] [varchar](max) NOT NULL,
	[Reason] [varchar](max) NULL,
	[OpeningKMs] [decimal](12, 2) NOT NULL,
	[ClosingKMs] [decimal](12, 2) NULL,
	[VehicleID] [int] NOT NULL,
	[JobID] [int] NULL,
	[Invoiced] [bit] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_TravelExpense] PRIMARY KEY CLUSTERED 
(
	[ExpenseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vehicle]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehicle](
	[VehicleID] [int] IDENTITY(1000000000,1) NOT NULL,
	[Name] [nchar](30) NOT NULL,
	[SARSFixedCost] [money] NOT NULL,
	[SARSFuelCost] [money] NOT NULL,
	[SARSMaintenceCost] [money] NOT NULL,
	[ClientCharge] [money] NOT NULL,
	[ProfileID] [int] NOT NULL,
 CONSTRAINT [PK_Vehicle_1] PRIMARY KEY CLUSTERED 
(
	[VehicleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Worklog]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Worklog](
	[LogItemID] [int] IDENTITY(1000000000,1) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NULL,
	[Invoiced] [bit] NOT NULL,
 CONSTRAINT [PK_Worklog] PRIMARY KEY CLUSTERED 
(
	[LogItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[SP_addGeneralExpenseFile]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_addGeneralExpenseFile]
	@EID int,
	@IRC varbinary(MAX)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    UPDATE [GeneralExpense] 
	Set [Invoice/ReceiptCopy] = @IRC
    WHERE ExpenseID = @EID

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END
GO
/****** Object:  StoredProcedure [dbo].[SP_DashboardIncomeExpense]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_DashboardIncomeExpense]
	@PID int 
AS
BEGIN

	Select (Select sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)+(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost))  
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between DATEADD(Month, -1, getdate()) and getdate()
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as TotalIncomePast30Days, 

(Select sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)+(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)) 
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between DATEADD(Month, -2, getdate()) and DATEADD(Month, -1, getdate())
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as TotalIncomePast60To30Days,

((Select sum(Amount) 
From Expense, JobExpense, Jobs, Client
Where Expense.ExpenseID = JobExpense.ExpenseID
	AND JobExpense.[Date] Between DATEADD(Month, -1, getdate()) and getdate()
	AND JobExpense.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) +
(Select sum(Amount) 
From Expense, GeneralExpense
Where Expense.ExpenseID = GeneralExpense.ExpenseID
	And GeneralExpense.ProfileID = @PID
	AND GeneralExpense.[Date] Between DATEADD(Month, -1, getdate()) and getdate()) +
(Select Sum((ClosingKMs - OpeningKMs) * (ClientCharge))
From TravelLog, Vehicle
Where TravelLog.VehicleID = Vehicle.VehicleID 
	And Vehicle.ProfileID = @PID
	AND TravelLog.[Date] Between DATEADD(Month, -1, getdate()) and getdate())) as TotalExpensePast30Days,

((Select sum(Amount) 
From Expense, JobExpense, Jobs, Client
Where Expense.ExpenseID = JobExpense.ExpenseID
	AND JobExpense.[Date] Between DATEADD(Month, -2, getdate()) and DATEADD(Month, -1, getdate())
	AND JobExpense.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) +
(Select sum(Amount) 
From Expense, GeneralExpense
Where Expense.ExpenseID = GeneralExpense.ExpenseID
	And GeneralExpense.ProfileID = @PID
	AND GeneralExpense.[Date] Between DATEADD(Month, -2, getdate()) and DATEADD(Month, -1, getdate()) +
(Select Sum((ClosingKMs - OpeningKMs) * (ClientCharge))
From TravelLog, Vehicle
Where TravelLog.VehicleID = Vehicle.VehicleID 
	AND TravelLog.[Date] Between DATEADD(Month, -2, getdate()) and DATEADD(Month, -1, getdate()))))  as TotalExpensePast60To30Days 

END
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteTravelLogItem]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_DeleteTravelLogItem] 
	@EID int
AS
BEGIN
		Begin Transaction;
			Begin Try
				DELETE FROM TravelLog
				Where (TravelLog.ExpenseID = @EID)
			End try
		Begin Catch
			if @@TRANCOUNT > 0
				Begin
					ROLLBACK TRANSACTION
				End
		End Catch
commit Transaction
END
GO
/****** Object:  StoredProcedure [dbo].[SP_EditTaxConsultant]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_EditTaxConsultant]
	@N varchar(Max),
	@EA varchar(50),
	@PID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    UPDATE [TaxConsultant] 
	Set [Name] = @N,
		EmailAddress = @EA
    WHERE ProfileID = @PID

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END
GO
/****** Object:  StoredProcedure [dbo].[SP_EditTravelExpense]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_EditTravelExpense]
	@F varchar(Max),
	@T varchar(Max),
	@R varchar(Max),
	@OKM decimal(12, 2),
	@CKM decimal(12, 2),
	@VID int,
	@D date,
	@EID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    UPDATE [TravelLog] 
	Set [From] = @F,
		[To] = @T, 
		Reason = @R, 
		OpeningKMs = @OKM, 
		ClosingKMs = @CKM, 
		VehicleID = @VID, 
		[Date] = @D
    WHERE ExpenseID = @EID

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetBussiness]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetBussiness]
AS
BEGIN
    SELECT TOP (1000) [BusinessID],
                      [VATRate],
                      [SMSSid],
                      [SMSToken]
    FROM   [TaxApp].[dbo].[Business];
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetClient]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetClient]
@CID INT
AS
BEGIN
    SELECT *
    FROM   Client
    WHERE  ClientID = @CID;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetConsultant]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetConsultant]
@PI INT
AS
BEGIN
    SELECT *
    FROM   TaxConsultant
    WHERE  ProfileID = @PI;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetEmailSettings]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetEmailSettings]
@PI INT
AS
BEGIN
    SELECT *
    FROM   EmailSettings
    WHERE  ProfileID = @PI;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetExpenseCategorys]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetExpenseCategorys]
AS
BEGIN
    SELECT   *
    FROM     ExpenseCategory
    ORDER BY Name;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetGeneralExpense]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetGeneralExpense]
@EID INT
AS
BEGIN
    SELECT Expense.ExpenseID,
           Expense.CategoryID,
           Expense.[Name],
           Expense.[Description],
           GeneralExpense.ProfileID,
           GeneralExpense.[Date],
           GeneralExpense.Amount,
           GeneralExpense.[Repeat],
           GeneralExpense.[Invoice/ReceiptCopy],
           ExpenseCategory.[Name],
           ExpenseCategory.[Description]
    FROM   Expense, GeneralExpense, ExpenseCategory
    WHERE  Expense.ExpenseID = @EID
           AND Expense.ExpenseID = GeneralExpense.ExpenseID
           AND Expense.CategoryID = ExpenseCategory.CategoryID;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetGeneralExpenses]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetGeneralExpenses]
	@PID INT
AS
BEGIN
    SELECT Expense.ExpenseID, Expense.CategoryID, Expense.[Name], Expense.[Description],
		GeneralExpense.ProfileID, GeneralExpense.[Date], GeneralExpense.Amount, GeneralExpense.[Repeat], GeneralExpense.[Invoice/ReceiptCopy],
		ExpenseCategory.[Name], ExpenseCategory.[Description]
    FROM   Expense, GeneralExpense, ExpenseCategory
    WHERE  GeneralExpense.ProfileID = @PID
			AND Expense.ExpenseID = GeneralExpense.ExpenseID
			AND Expense.CategoryID = ExpenseCategory.CategoryID
	Order by GeneralExpense.[Date] desc
END 
GO
/****** Object:  StoredProcedure [dbo].[SP_GetIncomeRecivedList]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetIncomeRecivedList]
	@PID int,
	@SD  date,
	@ED date,
	@RID decimal(4,2)
AS
BEGIN
	Select Jobs.JobID, Jobs.JobTitle, Client.FirstName + ' ' + Client.LastName as Client, Client.ClientID, Invoice.[DateTime], 
	sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)+(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)) As Total,
	sum((((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)+(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost))/100)*@RID) As TAX
From Jobs, Invoice, InvoiceLineItem, JobInvoice, Client
WHERE Jobs.JobID = JobInvoice.JobID
	AND JobInvoice.InvoiceNum = Invoice.InvoiceNum
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID
	AND Invoice.[Datetime] Between @SD and @ED
	AND Invoice.Paid = 1
Group by Jobs.JobTitle, Invoice.[DateTime], Jobs.JobID, Client.FirstName, Client.LastName, Client.ClientID
Order by Invoice.[DateTime]
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetInvoice]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetInvoice]
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
/****** Object:  StoredProcedure [dbo].[SP_GetInvoiceCount]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetInvoiceCount]
AS
BEGIN
	select count(*)
	from Invoice
	Where [DateTime] between dateadd(day, datediff(day,1, GETDATE()),0) 
		and dateadd(day, datediff(day,-1, GETDATE()),0)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetInvoices]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetInvoices]
	@PID int
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
		AND Client.ProfileID = @PID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetInvoicesOutstanding]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_GetInvoicesOutstanding]
	@PID int
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
		AND Client.ProfileID = @PID
		AND Paid = 0
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetInvoicesPaid]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_GetInvoicesPaid]
	@PID int
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
		AND Client.ProfileID = @PID
		AND Paid = 1
	Order by [DateTime] desc
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetJob]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetJob]
@JID INT
AS
BEGIN
    SELECT [JobID],
           [JobTitle],
           [HourlyRate],
           [Budget],
           [StartDate],
           [EndDate],
           C.[ClientID],
           C.FirstName,
           (SELECT Sum(DATEDIFF(MINUTE, StartTime, EndTime))
            FROM   Worklog, JobHours
            WHERE  Worklog.LogItemID = JobHours.LogItemID
                   AND JobHours.JobID = @JID) AS WorkLogHours,
           (SELECT Sum(Amount)
            FROM   JobExpense
            WHERE  JobExpense.JobID = @JID) AS ExpenseTotal,
           (SELECT (((Sum(UnitCost * UnitCount)/100)*Invoice.VATRate)+Sum(UnitCost * UnitCount)) as TotalUnPaid
            FROM   [TaxApp].[dbo].[JobInvoice], [TaxApp].[dbo].Invoice, [TaxApp].[dbo].InvoiceLineItem
			WHERE  JobInvoice.JobID = @JID
                   AND JobInvoice.InvoiceNum = Invoice.InvoiceNum
				   AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
                   AND Invoice.Paid = 1
			GROUP BY Invoice.VATRate) AS TotalPaid,
           (SELECT (((Sum(InvoiceLineItem.UnitCost * UnitCount)/100)*Invoice.VATRate)+Sum(UnitCost * UnitCount)) as TotalUnPaid
            FROM   [TaxApp].[dbo].[JobInvoice], [TaxApp].[dbo].Invoice, [TaxApp].[dbo].InvoiceLineItem
			WHERE  JobInvoice.JobID = @JID
                   AND JobInvoice.InvoiceNum = Invoice.InvoiceNum
				   AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
                   AND Invoice.Paid = 0
			GROUP BY Invoice.VATRate) AS TotalUnPaid,
           (SELECT Sum((SARSMaintenceCost + SARSFuelCost) * (ClosingKMs - OpeningKMs))
            FROM   TravelLog, Vehicle
            WHERE  TravelLog.JobID = @JID
                   AND TravelLog.VehicleID = Vehicle.VehicleID) AS TravelLogCostTotal
    FROM   [TaxApp].[dbo].[Jobs], [TaxApp].[dbo].Client AS C
    WHERE  Jobs.JobID = @JID
           AND Jobs.ClientID = C.ClientID;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetJobExpenesForInvoice]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetJobExpenesForInvoice]
	@JID int
AS
BEGIN
	select Expense.ExpenseID, Expense.[Name], Amount 
from JobExpense, Expense
Where Expense.ExpenseID = JobExpense.ExpenseID
	AND Invoiced = 0
	AND JobExpense.JobID = @JID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetJobExpense]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetJobExpense]
@EID INT
AS
BEGIN
    SELECT Expense.ExpenseID,
           Expense.CategoryID,
           Expense.[Name],
           Expense.[Description],
           JobExpense.JobID,
           JobExpense.[Date],
           JobExpense.Amount,
           JobExpense.[Invoice/ReceiptCopy],
           ExpenseCategory.[Name],
           ExpenseCategory.[Description]
    FROM   Expense, JobExpense, ExpenseCategory
    WHERE  Expense.ExpenseID = @EID
           AND Expense.ExpenseID = JobExpense.ExpenseID
           AND Expense.CategoryID = ExpenseCategory.CategoryID;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetJobExpenses]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetJobExpenses]
	@JID INT
AS
BEGIN
    SELECT Expense.ExpenseID, Expense.CategoryID, Expense.[Name], Expense.[Description],
		JobExpense.JobID, JobExpense.[Date], JobExpense.Amount, JobExpense.[Invoice/ReceiptCopy],
		ExpenseCategory.[Name], ExpenseCategory.[Description], Jobs.JobTitle
    FROM   Expense, JobExpense, ExpenseCategory, Jobs
    WHERE  JobExpense.JobID = @JID
			AND Expense.ExpenseID = JobExpense.ExpenseID
			AND Expense.CategoryID = ExpenseCategory.CategoryID
			AND JobExpense.JobID = Jobs.JobID
	Order by JobExpense.[Date] desc
END 

GO
/****** Object:  StoredProcedure [dbo].[SP_GetJobExpensesAllProfile]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetJobExpensesAllProfile]
	@PID INT
AS
BEGIN
    SELECT Expense.ExpenseID, Expense.CategoryID, Expense.[Name], Expense.[Description],
		JobExpense.JobID, JobExpense.[Date], JobExpense.Amount, JobExpense.[Invoice/ReceiptCopy],
		ExpenseCategory.[Name], ExpenseCategory.[Description], Jobs.JobTitle
    FROM   Expense, JobExpense, ExpenseCategory, Jobs, Client
    WHERE  Expense.ExpenseID = JobExpense.ExpenseID
			AND Expense.CategoryID = ExpenseCategory.CategoryID
			AND JobExpense.JobID = Jobs.JobID
			AND Jobs.ClientID = Client.ClientID
			AND Client.ProfileID = @PID
	Order by JobExpense.[Date] desc
END 
GO
/****** Object:  StoredProcedure [dbo].[SP_GetJobHours]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetJobHours]
@JobID INT
AS
BEGIN
    SELECT *, DATEDIFF(MINUTE, StartTime, EndTime) AS WorkLogHours
    FROM   Worklog, JobHours
    WHERE  Worklog.LogItemID = JobHours.LogItemID
           AND JobHours.JobID = @JobID
	Order by Worklog.StartTime desc
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetJobHoursForInvoice]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetJobHoursForInvoice] 
	@JID int
AS
BEGIN
	SET NOCOUNT ON;
	Select Worklog.LogItemID, Worklog.[Description], Jobs.HourlyRate, DATEDIFF(MINUTE, StartTime, EndTime) AS WorkLogHours
	From Worklog, JobHours, Jobs
	WHERE  Worklog.LogItemID = JobHours.LogItemID
	And JobHours.JobID = Jobs.JobID
	AND Invoiced = 0
	AND JobHours.JobID = @JID
	Order by Worklog.StartTime
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetJobInvoices]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetJobInvoices]
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
/****** Object:  StoredProcedure [dbo].[SP_GetJobTravelForInvoice]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetJobTravelForInvoice] 
	@JID int
AS
BEGIN
	select ExpenseID, Reason, (ClosingKMs - OpeningKMs) as UnitKMs, (ClientCharge) as CostPerKM 
	from TravelLog, Vehicle
	Where Invoiced = 0
	AND TravelLog.VehicleID = Vehicle.VehicleID
	AND TravelLog.JobID = @JID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetJobTravleLog]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetJobTravleLog]
	@JID int
AS
BEGIN
	Select *, (TravelLog.ClosingKMs - TravelLog.OpeningKMs) as TotalKMs,
			 (Vehicle.SARSFuelCost * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)) as SARSFuelCost,
			 (Vehicle.SARSMaintenceCost * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)) as SARSMaintenceCost,
			 (Vehicle.ClientCharge * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)) as ClientCharge
	From TravelLog, Vehicle
	Where JobID = @JID
		AND TravelLog.VehicleID = Vehicle.VehicleID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetLatestTaxAndVatPeriodID]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetLatestTaxAndVatPeriodID]
AS
BEGIN
	SELECT TOP 1 PeriodID FROM TaxAndVatPeriods ORDER BY PeriodID DESC
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetLogItem]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetLogItem]
@LogID INT
AS
BEGIN
    SELECT *,
			(SELECT Sum(DATEDIFF(MINUTE, StartTime, EndTime))
            FROM   Worklog
            WHERE  Worklog.LogItemID = @LogID) AS WorkLogHours
    FROM   Worklog
    WHERE  Worklog.LogItemID = @LogID;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetProfile]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetProfile]
@EM VARCHAR (50), @PI INT, @UN VARCHAR (50)
AS
BEGIN
    SELECT *
    FROM   Profile
    WHERE  ProfileID = @PI
           OR EmailAddress = @EM
           OR Username = @UN;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetProfileClients]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetProfileClients]
@PID INT
AS
BEGIN
    SELECT *
    FROM   Client
    WHERE  ProfileID = @PID;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetProfileJobsCurrent]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetProfileJobsCurrent]
	@PID INT,
	@CID INT
AS
BEGIN
    SELECT [JobID],
           [JobTitle],
           [HourlyRate],
           [Budget],
           [StartDate],
           [EndDate],
           C.[ClientID],
           C.FirstName,
           (SELECT Sum(DATEDIFF(MINUTE, StartTime, EndTime))
            FROM   Worklog, JobHours
            WHERE  Worklog.LogItemID = JobHours.LogItemID
                   AND JobHours.JobID = Jobs.JobID) AS WorkLogHours,
           (SELECT Sum(Amount)
            FROM   JobExpense
            WHERE  JobExpense.JobID = Jobs.JobID) AS ExpenseTotal,
           (SELECT (((Sum(UnitCost * UnitCount)/100)*Invoice.VATRate)+Sum(UnitCost * UnitCount))
            FROM   JobInvoice, Invoice, InvoiceLineItem, Jobs, Client
			WHERE  JobInvoice.InvoiceNum = Invoice.InvoiceNum
				   AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
				   AND JobInvoice.JobID = Jobs.JobID
				   AND Jobs.ClientID = Client.ClientID
				   AND (Client.ProfileID = @PID or Jobs.ClientID = @CID)
                   AND Invoice.Paid = 1
			GROUP BY Invoice.VATRate) AS TotalPaid,
           (SELECT (((Sum(UnitCost * UnitCount)/100)*Invoice.VATRate)+Sum(UnitCost * UnitCount))
            FROM   JobInvoice, Invoice, InvoiceLineItem, Jobs, Client
			WHERE  JobInvoice.InvoiceNum = Invoice.InvoiceNum
				   AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
				   AND JobInvoice.JobID = Jobs.JobID
				   AND Jobs.ClientID = Client.ClientID
				   AND (Client.ProfileID = @PID or Jobs.ClientID = @CID)
                   AND Invoice.Paid = 0
			GROUP BY Invoice.VATRate) AS TotalUnPaid,
           (SELECT Sum((SARSMaintenceCost + SARSFuelCost) * (ClosingKMs - OpeningKMs))
            FROM   TravelLog, Vehicle
            WHERE  TravelLog.JobID = Jobs.JobID
                   AND TravelLog.VehicleID = Vehicle.VehicleID) AS TravelLogCostTotal
    FROM   [TaxApp].[dbo].[Jobs], [TaxApp].[dbo].Client AS C
    WHERE  Jobs.ClientID = C.ClientID AND
			(ProfileID = @PID
		or Jobs.ClientID = @CID)
		and C.ClientID = Jobs.ClientID
		and Jobs.EndDate IS NULL
	ORDER BY Jobs.StartDate desc
END 

GO
/****** Object:  StoredProcedure [dbo].[SP_GetProfileJobsDashboard]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetProfileJobsDashboard]
	@PID INT,
	@CID INT
AS
BEGIN
    SELECT TOP 3
			[JobID],
           [JobTitle],
           [HourlyRate],
           [Budget],
           [StartDate],
           [EndDate],
           C.[ClientID],
           C.FirstName,
           (SELECT Sum(DATEDIFF(MINUTE, StartTime, EndTime))
            FROM   Worklog, JobHours
            WHERE  Worklog.LogItemID = JobHours.LogItemID
                   AND JobHours.JobID = Jobs.JobID) AS WorkLogHours,
           (SELECT Sum(Amount)
            FROM   JobExpense
            WHERE  JobExpense.JobID = Jobs.JobID) AS ExpenseTotal,
           (SELECT (((Sum(UnitCost * UnitCount)/100)*Invoice.VATRate)+Sum(UnitCost * UnitCount))
            FROM   JobInvoice, Invoice, InvoiceLineItem, Jobs, Client
			WHERE  JobInvoice.InvoiceNum = Invoice.InvoiceNum
				   AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
				   AND JobInvoice.JobID = Jobs.JobID
				   AND Jobs.ClientID = Client.ClientID
				   AND (Client.ProfileID = @PID or Jobs.ClientID = @CID)
                   AND Invoice.Paid = 1
			GROUP BY Invoice.VATRate) AS TotalPaid,
           (SELECT (((Sum(UnitCost * UnitCount)/100)*Invoice.VATRate)+Sum(UnitCost * UnitCount))
            FROM   JobInvoice, Invoice, InvoiceLineItem, Jobs, Client
			WHERE  JobInvoice.InvoiceNum = Invoice.InvoiceNum
				   AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
				   AND JobInvoice.JobID = Jobs.JobID
				   AND Jobs.ClientID = Client.ClientID
				   AND (Client.ProfileID = @PID or Jobs.ClientID = @CID)
                   AND Invoice.Paid = 0
			GROUP BY Invoice.VATRate) AS TotalUnPaid,
           (SELECT Sum((SARSMaintenceCost + SARSFuelCost) * (ClosingKMs - OpeningKMs))
            FROM   TravelLog, Vehicle
            WHERE  TravelLog.JobID = Jobs.JobID
                   AND TravelLog.VehicleID = Vehicle.VehicleID) AS TravelLogCostTotal
    FROM   [TaxApp].[dbo].[Jobs], [TaxApp].[dbo].Client AS C
    WHERE  Jobs.ClientID = C.ClientID AND
			(ProfileID = @PID
		or Jobs.ClientID = @CID)
		and C.ClientID = Jobs.ClientID 
		and Jobs.EndDate IS NULL
	Order by Jobs.StartDate desc;
END 

GO
/****** Object:  StoredProcedure [dbo].[SP_GetProfileJobsPast]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetProfileJobsPast]
	@PID INT,
	@CID INT
AS
BEGIN
    SELECT [JobID],
           [JobTitle],
           [HourlyRate],
           [Budget],
           [StartDate],
           [EndDate],
           C.[ClientID],
           C.FirstName,
           (SELECT Sum(DATEDIFF(MINUTE, StartTime, EndTime))
            FROM   Worklog, JobHours
            WHERE  Worklog.LogItemID = JobHours.LogItemID
                   AND JobHours.JobID = Jobs.JobID) AS WorkLogHours,
           (SELECT Sum(Amount)
            FROM   JobExpense
            WHERE  JobExpense.JobID = Jobs.JobID) AS ExpenseTotal,
           (SELECT (((Sum(UnitCost * UnitCount)/100)*Invoice.VATRate)+Sum(UnitCost * UnitCount))
            FROM   JobInvoice, Invoice, InvoiceLineItem, Jobs, Client
			WHERE  JobInvoice.InvoiceNum = Invoice.InvoiceNum
				   AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
				   AND JobInvoice.JobID = Jobs.JobID
				   AND Jobs.ClientID = Client.ClientID
				   AND (Client.ProfileID = @PID or Jobs.ClientID = @CID)
                   AND Invoice.Paid = 1
			GROUP BY Invoice.VATRate) AS TotalPaid,
           (SELECT (((Sum(UnitCost * UnitCount)/100)*Invoice.VATRate)+Sum(UnitCost * UnitCount))
            FROM   JobInvoice, Invoice, InvoiceLineItem, Jobs, Client
			WHERE  JobInvoice.InvoiceNum = Invoice.InvoiceNum
				   AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
				   AND JobInvoice.JobID = Jobs.JobID
				   AND Jobs.ClientID = Client.ClientID
				   AND (Client.ProfileID = @PID or Jobs.ClientID = @CID)
                   AND Invoice.Paid = 0
			GROUP BY Invoice.VATRate) AS TotalUnPaid,
           (SELECT Sum((SARSMaintenceCost + SARSFuelCost) * (ClosingKMs - OpeningKMs))
            FROM   TravelLog, Vehicle
            WHERE  TravelLog.JobID = Jobs.JobID
                   AND TravelLog.VehicleID = Vehicle.VehicleID) AS TravelLogCostTotal
    FROM   [TaxApp].[dbo].[Jobs], [TaxApp].[dbo].Client AS C
    WHERE  Jobs.ClientID = C.ClientID AND
			(ProfileID = @PID
		or Jobs.ClientID = @CID) 
		and C.ClientID = Jobs.ClientID 
		and Jobs.EndDate IS NOT NULL
	ORDER BY Jobs.StartDate desc
END 

GO
/****** Object:  StoredProcedure [dbo].[SP_GetProfileTaxAndVatPeriods]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetProfileTaxAndVatPeriods]
	@PID int,
	@T char(1)
AS
BEGIN
	Select *
	From TaxAndVatPeriods
	Where ProfileID = @PID
		AND [Type] = @T
	Order by StartDate desc
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetRepeatGeneralExpenses]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetRepeatGeneralExpenses]
AS
BEGIN
	SELECT Expense.ExpenseID, Expense.CategoryID, Expense.[Name], Expense.[Description],
		GeneralExpense.ProfileID, GeneralExpense.[Date], GeneralExpense.Amount, GeneralExpense.[Repeat], GeneralExpense.[Invoice/ReceiptCopy],
		ExpenseCategory.[Name], ExpenseCategory.[Description]
    FROM   Expense, GeneralExpense, ExpenseCategory
    WHERE  Expense.ExpenseID = GeneralExpense.ExpenseID
			AND Expense.CategoryID = ExpenseCategory.CategoryID
			AND GeneralExpense.Repeat = 1
			AND GeneralExpense.Date <= DATEADD(day, -30, getdate())
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetTaxAndVatPeriod]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_GetTaxAndVatPeriod]
	@PID int
AS
BEGIN
	Select *
	From TaxAndVatPeriods
	Where PeriodID = @PID
	Order by StartDate desc
END
GO
/****** Object:  StoredProcedure [dbo].[SP_getTAXCenterDashboard]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_getTAXCenterDashboard]
	@PID int,
	@SD  date,
	@ED date,
	@VR decimal(4,2),
	@TR decimal(4,2) = 0
AS
BEGIN
	Select (Select Sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)+(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost))  
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between @SD and @ED
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as IncomeRECEIVED,

	(Select sum((((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)+(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost))/100)*@TR)  
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between @SD and @ED
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as TaxOwed,

	(Select Sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)+(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost))  
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between DATEADD(DAY, (SELECT DATEDIFF(DAY, @ED, @SD)), @SD) and @SD
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as IncomeRECEIVEDPastPeriod,

	(Select sum((((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)+(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost))/100)*@TR)  
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between DATEADD(DAY, (SELECT DATEDIFF(DAY, @ED, @SD)), @SD) and @SD
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as TaxOwedPastPeriod
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetTaxPeriodBrakets]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetTaxPeriodBrakets] 
	@PID int
AS
BEGIN
	Select *
	From TaxPeriodRates
	Where PeriodID = @PID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetTravleLog]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetTravleLog]
	@PID int
AS
BEGIN
	Select TravelLog.ExpenseID, TravelLog.[From], TravelLog.[To], TravelLog.Reason, 
			TravelLog.OpeningKMs, TravelLog.ClosingKMs, TravelLog.VehicleID,
			TravelLog.JobID, TravelLog.Invoiced, TravelLog.[Date],
			(TravelLog.ClosingKMs - TravelLog.OpeningKMs) as TotalKMs,
			 (Vehicle.SARSFuelCost * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)) as SARSFuelCost,
			 (Vehicle.SARSMaintenceCost * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)) as SARSMaintenceCost,
			 (Vehicle.ClientCharge * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)) as ClientCharge,
			 Jobs.JobTitle
	From TravelLog, Client, [Profile], jobs, Vehicle
	Where TravelLog.JobID = Jobs.JobID
		 AND Jobs.ClientID = Client.ClientID
		 AND Client.ProfileID = Profile.ProfileID
		 AND TravelLog.VehicleID = Vehicle.VehicleID
		 AND Profile.ProfileID = @PID
	Order by TravelLog.[Date] desc
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetTravleLogItem]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetTravleLogItem]
	@EID int
AS
BEGIN
	Select TravelLog.ExpenseID, TravelLog.[From], TravelLog.[To], TravelLog.Reason, 
			TravelLog.OpeningKMs, TravelLog.ClosingKMs, TravelLog.VehicleID,
			TravelLog.JobID, TravelLog.Invoiced, TravelLog.[Date],
			(TravelLog.ClosingKMs - TravelLog.OpeningKMs) as TotalKMs,
			 (Vehicle.SARSFuelCost * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)) as SARSFuelCost,
			 (Vehicle.SARSMaintenceCost * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)) as SARSMaintenceCost,
			 (Vehicle.ClientCharge * (TravelLog.ClosingKMs - TravelLog.OpeningKMs)) as ClientCharge,
			 Jobs.JobTitle, Vehicle.[Name]
	From TravelLog, Client, [Profile], jobs, Vehicle
	Where TravelLog.JobID = Jobs.JobID
		 AND Jobs.ClientID = Client.ClientID
		 AND Client.ProfileID = Profile.ProfileID
		 AND TravelLog.VehicleID = Vehicle.VehicleID
		 AND TravelLog.ExpenseID = @EID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_getVatCenterDashboard]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_getVatCenterDashboard]
	@PID int,
	@SD  date,
	@ED date,
	@PDID int,
	@VR decimal(4,2)
AS
BEGIN
	Select (Select sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*@VR))  
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between @SD and @ED
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as VATRECEIVED,

	(Select sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*@VR))  
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between DATEADD(DAY, (SELECT DATEDIFF(DAY, @ED, @SD)), @SD) and @SD
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as VATRECEIVEDPastPeriod,

(((Select sum(Amount) 
From Expense, JobExpense, Jobs, Client
Where Expense.ExpenseID = JobExpense.ExpenseID
	AND JobExpense.[Date] Between @SD and @ED
	AND JobExpense.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) +
((Select sum(Amount) 
From Expense, GeneralExpense
Where Expense.ExpenseID = GeneralExpense.ExpenseID
	And GeneralExpense.ProfileID = @PID
	AND GeneralExpense.[Date] Between @SD and @ED) +
(Select Sum((ClosingKMs - OpeningKMs) * (ClientCharge))
From TravelLog, Vehicle
Where TravelLog.VehicleID = Vehicle.VehicleID 
	And Vehicle.ProfileID = @PID
	AND TravelLog.[Date] Between @SD and @ED))) -

((Select sum(Amount) 
From Expense, JobExpense, Jobs, Client
Where Expense.ExpenseID = JobExpense.ExpenseID
	AND JobExpense.[Date] Between @SD and @ED
	AND JobExpense.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) +
((Select sum(Amount) 
From Expense, GeneralExpense
Where Expense.ExpenseID = GeneralExpense.ExpenseID
	And GeneralExpense.ProfileID = @PID
	AND GeneralExpense.[Date] Between @SD and @ED) +
(Select Sum((ClosingKMs - OpeningKMs) * (ClientCharge))
From TravelLog, Vehicle
Where TravelLog.VehicleID = Vehicle.VehicleID 
	And Vehicle.ProfileID = @PID
	AND TravelLog.[Date] Between @SD and @ED))) / 
	(((@VR)/100)+1))
	 as VATPAID,

	 (((Select sum(Amount) 
From Expense, JobExpense, Jobs, Client
Where Expense.ExpenseID = JobExpense.ExpenseID
	AND JobExpense.[Date] Between DATEADD(DAY, (SELECT DATEDIFF(DAY, @ED, @SD)), @SD) and @SD
	AND JobExpense.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) +
((Select sum(Amount) 
From Expense, GeneralExpense
Where Expense.ExpenseID = GeneralExpense.ExpenseID
	And GeneralExpense.ProfileID = @PID
	AND GeneralExpense.[Date] Between DATEADD(DAY, (SELECT DATEDIFF(DAY, @ED, @SD)), @SD) and @SD) +
(Select Sum((ClosingKMs - OpeningKMs) * (ClientCharge))
From TravelLog, Vehicle
Where TravelLog.VehicleID = Vehicle.VehicleID 
	And Vehicle.ProfileID = @PID
	AND TravelLog.[Date] Between DATEADD(DAY, (SELECT DATEDIFF(DAY, @ED, @SD)), @SD) and @SD))) -

((Select sum(Amount) 
From Expense, JobExpense, Jobs, Client
Where Expense.ExpenseID = JobExpense.ExpenseID
	AND JobExpense.[Date] Between DATEADD(DAY, (SELECT DATEDIFF(DAY, @ED, @SD)), @SD) and @SD
	AND JobExpense.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) +
((Select sum(Amount) 
From Expense, GeneralExpense
Where Expense.ExpenseID = GeneralExpense.ExpenseID
	And GeneralExpense.ProfileID = @PID
	AND GeneralExpense.[Date] Between DATEADD(DAY, (SELECT DATEDIFF(DAY, @ED, @SD)), @SD) and @SD) +
(Select Sum((ClosingKMs - OpeningKMs) * (ClientCharge))
From TravelLog, Vehicle
Where TravelLog.VehicleID = Vehicle.VehicleID 
	And Vehicle.ProfileID = @PID
	AND TravelLog.[Date] Between DATEADD(DAY, (SELECT DATEDIFF(DAY, @ED, @SD)), @SD) and @SD))) / 
	(((@VR)/100)+1))
	 as VATPAIDPreviousPeriod
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetVATRecivedList]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetVATRecivedList]
	@PID int,
	@SD  date,
	@ED date
AS
BEGIN
	Select Jobs.JobID, Jobs.JobTitle, Client.FirstName + ' ' + Client.LastName as Client, Client.ClientID, Invoice.[DateTime], 
	sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)+(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)) As Total,
	sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)) As VAT
From Jobs, Invoice, InvoiceLineItem, JobInvoice, Client
WHERE Jobs.JobID = JobInvoice.JobID
	AND JobInvoice.InvoiceNum = Invoice.InvoiceNum
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID
	AND Invoice.[Datetime] Between @SD and @ED
	AND Invoice.Paid = 1
Group by Jobs.JobTitle, Invoice.[DateTime], Jobs.JobID, Client.FirstName, Client.LastName, Client.ClientID
Order by Invoice.[DateTime]
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetVehicles]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetVehicles]
	@PID INT
AS
BEGIN
    SELECT *
    FROM   Vehicle
	Where ProfileID = @PID
	order by [Name]
END
GO
/****** Object:  StoredProcedure [dbo].[SP_IncomeDashboard]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IncomeDashboard]
	@PID int 
AS
BEGIN

	Select (Select sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)+(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost))  
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between DATEADD(Month, -1, getdate()) and getdate()
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as TotalIncomePast30Days, 

(Select sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)+(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost))
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between DATEADD(Month, -2, getdate()) and DATEADD(Month, -1, getdate())
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 1
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as TotalIncomePast60To30Days,

(Select sum((((InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost)/100)*VATRate)+(InvoiceLineItem.UnitCount * InvoiceLineItem.UnitCost))  
From Invoice, InvoiceLineItem, JobInvoice, Jobs, Client
Where Invoice.[Datetime] Between DATEADD(Month, -1, getdate()) and getdate()
	AND Invoice.InvoiceNum = InvoiceLineItem.InvoiceNum
	AND  Invoice.Paid = 0
	AND Invoice.InvoiceNum = JobInvoice.InvoiceNum
	AND JobInvoice.JobID = Jobs.JobID
	AND Jobs.ClientID = Client.ClientID
	And Client.ProfileID = @PID) as TotalOutIncome 
END
GO
/****** Object:  StoredProcedure [dbo].[SP_MarkInvoiceAsPaid]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_MarkInvoiceAsPaid]
	@INum nchar(11)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    UPDATE [Invoice] 
	Set [Paid] = 1
    WHERE InvoiceNum = @INum

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END
GO
/****** Object:  StoredProcedure [dbo].[SP_MarkJobAsComplete]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_MarkJobAsComplete]
	@JID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    UPDATE [Jobs] 
	Set EndDate = GETDATE()
    WHERE JobID = @JID

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END
GO
/****** Object:  StoredProcedure [dbo].[SP_NewClient]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_NewClient]
@FN VARCHAR (50), @LN VARCHAR (50), @CN VARCHAR (50), @CNum NCHAR (10), @EA VARCHAR (50), @PA VARCHAR (50), @PC NCHAR (3), @PI INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT  INTO [Client] (FirstName, LastName, CompanyName, ContactNumber, EmailAddress, PhysiclaAddress, PreferedCommunicationChannel, ProfileID)
        VALUES               (@FN, @LN, @CN, @CNum, @EA, @PA, @PC, @PI);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[SP_NewConsultant]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_NewConsultant]
@EA VARCHAR (50), @PI INT, @N VARCHAR (MAX)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT  INTO [TaxConsultant] (Name, EmailAddress, ProfileID)
        VALUES                      (@N, @EA, @PI);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[SP_NewEmailSettings]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_NewEmailSettings]
@PI INT, @A VARCHAR (MAX), @Pass VARCHAR (MAX), @H VARCHAR (50), @P VARCHAR (50), @ESsl BIT, @DM VARCHAR (50), @UDC BIT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT  INTO [EmailSettings] (ProfileID, Address, Password, Host, Port, EnableSsl, DeliveryMethod, UseDefailtCredentials)
        VALUES                      (@PI, @A, @Pass, @H, @P, @ESsl, @DM, @UDC);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[SP_NewGeneralExpense]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_NewGeneralExpense]
@CID INT, @N VARCHAR (MAX), @D VARCHAR (MAX), @PID INT, @Date DATETIME, @A MONEY, @R BIT
AS
BEGIN
BEGIN TRY
		BEGIN TRANSACTION
			INSERT  INTO [Expense] (CategoryID, [Name], [Description])
			VALUES                (@CID, @N, @D);

			INSERT  INTO [GeneralExpense] (ProfileID, ExpenseID, [Date], Amount, [Repeat], [Invoice/ReceiptCopy])
			VALUES       (@PID, (Select SCOPE_IDENTITY()), @Date, @A, @R, NULL);
COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
END

GO
/****** Object:  StoredProcedure [dbo].[SP_NewInvoice]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_NewInvoice] 
	@JID int,
	@INum nchar(11)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT  INTO [JobInvoice] (JobID, InvoiceNum)
        VALUES                (@JID, @INum);

        INSERT  INTO [Invoice] (InvoiceNum, [DateTime], VATRate, Paid)
        VALUES                 (@INum, (select GETDATE()),
								(select VATRate 
									from [Profile], Jobs, Client 
									where [Profile].ProfileID = Client.ProfileID
										AND Client.ClientID = Jobs.JobID
										AND Jobs.JobID = @JID)
								, 0);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[SP_NewInvoiceDetailLine]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_NewInvoiceDetailLine]
	@INum nchar(11),
	@ID int,
	@Name nchar(50),
	@UnitCount decimal(18, 2),
	@UnitCost decimal(18, 2),
	@T char
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT  INTO [InvoiceLineItem] (InvoiceNum, [Name], UnitCount, UnitCost, [Type])
        VALUES       (@INum, @Name, @UnitCount, @UnitCost, @T);

		Update [JobExpense]
		Set invoiced = 1
		from JobExpense, Expense
		Where JobExpense.ExpenseID = @ID
				And Expense.ExpenseID = JobExpense.ExpenseID
				And Expense.[Name] = @Name

		Update [TravelLog]
		Set invoiced = 1
		Where TravelLog.ExpenseID = @ID
			And TravelLog.Reason = @Name

		Update [WorkLog]
		Set invoiced = 1
		Where Worklog.LogItemID = @ID
			And Worklog.[Description] = @Name

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[SP_NewJob]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_NewJob]
@CI INT, @JT VARCHAR (MAX), @HR MONEY, @B MONEY, @SD DATETIME
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT  INTO [Jobs] (ClientID, JobTitle, HourlyRate, Budget, StartDate)
        VALUES             (@CI, @JT, @HR, @B, @SD);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[SP_NewJobExpense]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_NewJobExpense]
@CID INT, @N VARCHAR (MAX), @D VARCHAR (MAX), @JID INT, @Date DATETIME, @A MONEY
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT  INTO [Expense] (CategoryID, [Name], [Description])
        VALUES                (@CID, @N, @D);
        INSERT  INTO [JobExpense] (JobID, ExpenseID, [Date], Amount, [Invoice/ReceiptCopy], Invoiced)
        VALUES                   (@JID, (SELECT SCOPE_IDENTITY()), @Date, @A, NULL, 0);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[SP_NewPeriodTaxBraket]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_NewPeriodTaxBraket]
	@PID INT, @R Decimal(4,2), @T Decimal(18,2), @Ty Char(1)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT  INTO [TaxPeriodRates] (PeriodID, Rate, Threshold, [Type])
        VALUES                (@PID, @R, @T, @TY);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[SP_NewProfile]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_NewProfile]
@FN VARCHAR (50), @LN VARCHAR (50), @CN VARCHAR (50), @EM VARCHAR (50), @CNum NCHAR (10), @PA VARCHAR (1000), @VATNum NCHAR (30), @DR MONEY, @UN VARCHAR (50), @Pass VARCHAR (MAX)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT  INTO [Profile] (FirstName, LastName, CompanyName, EmailAddress, ContactNumber, PhysicalAddress, VATNumber, DefaultHourlyRate, Active, Username, [Password])
        VALUES                (@FN, @LN, @CN, @EM, @CNum, @PA, @VATNum, @DR, 1, @UN, @Pass);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[SP_NewTaxOrVatPeriod]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_NewTaxOrVatPeriod]
	@PID int,
	@SD Date,
	@ED Date,
	@T char(1)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
    INSERT  INTO [TaxAndVatPeriods] (ProfileID, StartDate, EndDate, [Type], VATRate)
    VALUES (@PID, @SD, @ED, @T, (Select VATRate
								FROM [Profile]
								Where ProfileID = @PID));
COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END

GO
/****** Object:  StoredProcedure [dbo].[SP_NewTravelExpense]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_NewTravelExpense]
	@F varchar(Max),
	@T varchar(Max),
	@R varchar(Max),
	@OKM decimal(12, 2),
	@CKM decimal(12, 2),
	@VID int,
	@JID int,
	@D date
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    INSERT  INTO [TravelLog] ([From], [To], Reason, OpeningKMs, ClosingKMs, VehicleID, JobID, Invoiced, [Date])
    VALUES (@F, @T, @R, @OKM, @CKM, @VID, @JID, 0, @D);

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END
GO
/****** Object:  StoredProcedure [dbo].[SP_NewVehicle]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_NewVehicle]
	@N varchar(30),
	@FC money,
	@MC money,
	@FxC money,
	@PID int,
	@CC money
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
    INSERT  INTO [Vehicle] ([Name], SARSFuelCost, SARSMaintenceCost, SARSFixedCost, ProfileID, ClientCharge)
    VALUES (@N, @FC, @MC, @FxC, @PID, @CC);

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END
GO
/****** Object:  StoredProcedure [dbo].[SP_NewWorkLogItem]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_NewWorkLogItem]
@JID INT, @D VARCHAR (MAX), @ST DATETIME, @ET DATETIME
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT  INTO [Worklog] ([Description], StartTime, EndTime, Invoiced)
        VALUES                (@D, @ST, @ET, 0);

        INSERT  INTO [JobHours] (JobID, LogItemID)
        VALUES                 (@JID, (SELECT @@IDENTITY));

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateGeneralExpenseRepeate]    Script Date: 2019/07/29 20:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_UpdateGeneralExpenseRepeate]
	@EID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

    UPDATE [GeneralExpense] 
	Set Repeat = 0
    WHERE ExpenseID = @EID

		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRANSACTION
	END CATCH 
	END
GO
USE [master]
GO
ALTER DATABASE [TaxApp] SET  READ_WRITE 
GO
