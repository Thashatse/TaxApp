USE [master]
GO
/****** Object:  Database [TaxApp]    Script Date: 2019/06/28 16:38:44 ******/
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
/****** Object:  Table [dbo].[Business]    Script Date: 2019/06/28 16:38:44 ******/
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
/****** Object:  Table [dbo].[Client]    Script Date: 2019/06/28 16:38:44 ******/
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
/****** Object:  Table [dbo].[EmailSettings]    Script Date: 2019/06/28 16:38:44 ******/
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
/****** Object:  Table [dbo].[Expense]    Script Date: 2019/06/28 16:38:44 ******/
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
/****** Object:  Table [dbo].[ExpenseCategory]    Script Date: 2019/06/28 16:38:44 ******/
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
/****** Object:  Table [dbo].[GeneralExpense]    Script Date: 2019/06/28 16:38:44 ******/
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
/****** Object:  Table [dbo].[Invoice]    Script Date: 2019/06/28 16:38:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoice](
	[InvoiceNum] [nchar](10) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[SubTotal] [money] NOT NULL,
	[VAT] [money] NOT NULL,
	[Paid] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobExpense]    Script Date: 2019/06/28 16:38:44 ******/
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
 CONSTRAINT [PK_JobExpense] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC,
	[ExpenseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobHours]    Script Date: 2019/06/28 16:38:44 ******/
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
/****** Object:  Table [dbo].[JobInvoice]    Script Date: 2019/06/28 16:38:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobInvoice](
	[JobID] [int] NOT NULL,
	[InvoiceNum] [nchar](10) NOT NULL,
 CONSTRAINT [PK_JobInvoice] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC,
	[InvoiceNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Jobs]    Script Date: 2019/06/28 16:38:44 ******/
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
/****** Object:  Table [dbo].[Profile]    Script Date: 2019/06/28 16:38:44 ******/
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
 CONSTRAINT [PK_Profile_1] PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaxConsultant]    Script Date: 2019/06/28 16:38:44 ******/
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
/****** Object:  Table [dbo].[TravelLog]    Script Date: 2019/06/28 16:38:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TravelLog](
	[ExpenseID] [int] NOT NULL,
	[From] [nchar](1000) NOT NULL,
	[To] [nchar](1000) NOT NULL,
	[Reason] [varchar](max) NULL,
	[OpeningKMs] [int] NOT NULL,
	[ClosingKMs] [int] NULL,
	[VehicleID] [int] NOT NULL,
	[JobID] [int] NULL,
 CONSTRAINT [PK_TravelExpense] PRIMARY KEY CLUSTERED 
(
	[ExpenseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vehicle]    Script Date: 2019/06/28 16:38:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehicle](
	[VehicleID] [int] IDENTITY(1000000000,1) NOT NULL,
	[Name] [nchar](30) NOT NULL,
	[FuelCost] [money] NOT NULL,
	[MaintenceCost] [money] NOT NULL,
 CONSTRAINT [PK_Vehicle] PRIMARY KEY CLUSTERED 
(
	[VehicleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Worklog]    Script Date: 2019/06/28 16:38:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Worklog](
	[LogItemID] [int] IDENTITY(1000000000,1) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NULL,
 CONSTRAINT [PK_Worklog] PRIMARY KEY CLUSTERED 
(
	[LogItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetBussiness]    Script Date: 2019/06/28 16:38:45 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_GetClient]    Script Date: 2019/06/28 16:38:45 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_GetConsultant]    Script Date: 2019/06/28 16:38:45 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_GetEmailSettings]    Script Date: 2019/06/28 16:38:45 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_GetExpenseCategorys]    Script Date: 2019/06/28 16:38:45 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_GetGeneralExpense]    Script Date: 2019/06/28 16:38:45 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_GetGeneralExpenses]    Script Date: 2019/06/28 16:38:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetGeneralExpenses]
@PID INT
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
    WHERE  GeneralExpense.ProfileID = @PID
           AND Expense.ExpenseID = GeneralExpense.ExpenseID
           AND Expense.CategoryID = ExpenseCategory.CategoryID;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetJob]    Script Date: 2019/06/28 16:38:45 ******/
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
           (SELECT Sum(SubTotal + vat)
            FROM   JobInvoice, Invoice
            WHERE  JobInvoice.JobID = @JID
                   AND JobInvoice.InvoiceNum = Invoice.InvoiceNum
                   AND Paid = 1) AS TotalPaid,
           (SELECT Sum(SubTotal + vat)
            FROM   JobInvoice, Invoice
            WHERE  JobInvoice.JobID = @JID
                   AND JobInvoice.InvoiceNum = Invoice.InvoiceNum
                   AND Paid = 1) AS TotalUnPaid,
           (SELECT Sum((MaintenceCost + FuelCost) * (ClosingKMs - OpeningKMs))
            FROM   TravelLog, Vehicle
            WHERE  TravelLog.JobID = @JID
                   AND TravelLog.VehicleID = Vehicle.VehicleID) AS TravelLogCostTotal
    FROM   [TaxApp].[dbo].[Jobs], [TaxApp].[dbo].Client AS C
    WHERE  Jobs.JobID = @JID
           AND Jobs.ClientID = C.ClientID;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetJobExpense]    Script Date: 2019/06/28 16:38:45 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_GetJobExpenses]    Script Date: 2019/06/28 16:38:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetJobExpenses]
@JID INT
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
    WHERE  JobExpense.JobID = @JID
           AND Expense.ExpenseID = JobExpense.ExpenseID
           AND Expense.CategoryID = ExpenseCategory.CategoryID;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetJobHours]    Script Date: 2019/06/28 16:38:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetJobHours]
@JobID INT
AS
BEGIN
    SELECT *
    FROM   Worklog, JobHours
    WHERE  Worklog.LogItemID = JobHours.LogItemID
           AND JobHours.JobID = @JobID;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetLogItem]    Script Date: 2019/06/28 16:38:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetLogItem]
@LogID INT
AS
BEGIN
    SELECT *
    FROM   Worklog
    WHERE  Worklog.LogItemID = @LogID;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetProfile]    Script Date: 2019/06/28 16:38:45 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_GetProfileClients]    Script Date: 2019/06/28 16:38:45 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_GetProfileJobs]    Script Date: 2019/06/28 16:38:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetProfileJobs]
@PID INT, @CID INT
AS
BEGIN
    SELECT *
    FROM   Jobs, Client
    WHERE  (ProfileID = @PID
            OR Jobs.ClientID = @CID)
           AND Client.ClientID = Jobs.ClientID;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_NewClient]    Script Date: 2019/06/28 16:38:45 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_NewConsultant]    Script Date: 2019/06/28 16:38:45 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_NewEmailSettings]    Script Date: 2019/06/28 16:38:45 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_NewGeneralExpense]    Script Date: 2019/06/28 16:38:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_NewGeneralExpense]
@CID INT, @N VARCHAR (MAX), @D VARCHAR (MAX), @PID INT, @Date DATETIME, @A MONEY, @R BIT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT  INTO [Expense] (CategoryID, [Name], [Description])
        VALUES                (@CID, @N, @D);
        INSERT  INTO [GeneralExpense] (ProfileID, ExpenseID, [Date], Amount, [Repeat], [Invoice/ReceiptCopy])
        VALUES                       (@PID, (SELECT SCOPE_IDENTITY()), @Date, @A, @R, NULL);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[SP_NewJob]    Script Date: 2019/06/28 16:38:45 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_NewJobExpense]    Script Date: 2019/06/28 16:38:45 ******/
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
        INSERT  INTO [JobExpense] (JobID, ExpenseID, [Date], Amount, [Invoice/ReceiptCopy])
        VALUES                   (@JID, (SELECT SCOPE_IDENTITY()), @Date, @A, NULL);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[SP_NewProfile]    Script Date: 2019/06/28 16:38:45 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_NewWorkLogItem]    Script Date: 2019/06/28 16:38:45 ******/
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
        INSERT  INTO [Worklog] ([Description], StartTime, EndTime)
        VALUES                (@D, @ST, @ET);
        INSERT  INTO [JobHours] (JobID, LogItemID)
        VALUES                 (@JID, (SELECT @@IDENTITY
                                       FROM   Worklog));
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
    END CATCH
END

GO
USE [master]
GO
ALTER DATABASE [TaxApp] SET  READ_WRITE 
GO
