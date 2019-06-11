USE [master]
GO
/****** Object:  Database [TaxApp]    Script Date: 2019/06/11 17:11:37 ******/
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
/****** Object:  Table [dbo].[Business]    Script Date: 2019/06/11 17:11:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Business](
	[BusinessID] [nchar](10) NOT NULL,
	[VATRate] [decimal](3, 2) NOT NULL,
	[SMSSid] [varchar](max) NOT NULL,
	[SMSToken] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Business] PRIMARY KEY CLUSTERED 
(
	[BusinessID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 2019/06/11 17:11:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[ClientID] [nchar](10) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[CompanyName] [varbinary](50) NOT NULL,
	[ContactNumber] [varchar](50) NOT NULL,
	[EmailAddress] [varchar](50) NOT NULL,
	[PhysiclaAddress] [varchar](50) NOT NULL,
	[PreferedCommunicationChannel] [nchar](3) NOT NULL,
	[ProfileID] [nchar](10) NOT NULL,
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[ClientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailSettings]    Script Date: 2019/06/11 17:11:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailSettings](
	[ProfileID] [nchar](10) NOT NULL,
	[Address] [varchar](max) NOT NULL,
	[Password] [varbinary](50) NOT NULL,
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
/****** Object:  Table [dbo].[Expense]    Script Date: 2019/06/11 17:11:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Expense](
	[ExpenseID] [nchar](10) NOT NULL,
	[CategoryID] [nchar](10) NOT NULL,
	[Name] [varchar](max) NOT NULL,
	[Description] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Expense] PRIMARY KEY CLUSTERED 
(
	[ExpenseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExpenseCategory]    Script Date: 2019/06/11 17:11:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExpenseCategory](
	[CategoryID] [nchar](10) NOT NULL,
	[Name] [varchar](max) NOT NULL,
	[Description] [varchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GeneralExpense]    Script Date: 2019/06/11 17:11:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeneralExpense](
	[ProfileID] [nchar](10) NOT NULL,
	[ExpenseID] [nchar](10) NOT NULL,
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
/****** Object:  Table [dbo].[Invoice]    Script Date: 2019/06/11 17:11:37 ******/
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
/****** Object:  Table [dbo].[JobExpense]    Script Date: 2019/06/11 17:11:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobExpense](
	[JobID] [nchar](10) NOT NULL,
	[ExpenseID] [nchar](10) NOT NULL,
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
/****** Object:  Table [dbo].[JobHours]    Script Date: 2019/06/11 17:11:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobHours](
	[JobID] [nchar](10) NOT NULL,
	[LogItemID] [nchar](10) NOT NULL,
 CONSTRAINT [PK_JobHours] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC,
	[LogItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobInvoice]    Script Date: 2019/06/11 17:11:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobInvoice](
	[JobID] [nchar](10) NOT NULL,
	[InvoiceNum] [nchar](10) NOT NULL,
 CONSTRAINT [PK_JobInvoice] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC,
	[InvoiceNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Jobs]    Script Date: 2019/06/11 17:11:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Jobs](
	[JobID] [nchar](10) NOT NULL,
	[ClientID] [nchar](10) NOT NULL,
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
/****** Object:  Table [dbo].[Profile]    Script Date: 2019/06/11 17:11:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profile](
	[ProfileID] [nchar](10) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[CompanyName] [varchar](50) NULL,
	[EmailAddress] [varchar](50) NOT NULL,
	[ContactNumber] [nchar](10) NOT NULL,
	[PhysicalAddress] [nchar](10) NULL,
	[ProfilePicture] [varbinary](max) NOT NULL,
	[VATNumber] [nchar](30) NOT NULL,
	[DefaultHourlyRate] [money] NOT NULL,
	[Active] [bit] NOT NULL,
	[Username] [varbinary](50) NOT NULL,
	[Password] [varbinary](50) NOT NULL,
	[PassRestCode] [varchar](30) NULL,
 CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaxConsultant]    Script Date: 2019/06/11 17:11:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaxConsultant](
	[ProfileID] [nchar](10) NOT NULL,
	[Name] [varchar](max) NOT NULL,
	[EmailAddress] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TaxConsultant] PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Worklog]    Script Date: 2019/06/11 17:11:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Worklog](
	[LogItemID] [nchar](10) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [TaxApp] SET  READ_WRITE 
GO
