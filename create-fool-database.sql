USE [master]
GO
/****** Object:  Database [Fool]     ******/
CREATE DATABASE [Fool]
GO
ALTER DATABASE [Fool] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Fool].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Fool] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Fool] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Fool] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Fool] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Fool] SET ARITHABORT OFF 
GO
ALTER DATABASE [Fool] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Fool] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Fool] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Fool] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Fool] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Fool] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Fool] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Fool] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Fool] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Fool] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Fool] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Fool] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Fool] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Fool] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Fool] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Fool] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Fool] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Fool] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Fool] SET  MULTI_USER 
GO
ALTER DATABASE [Fool] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Fool] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Fool] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Fool] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Fool] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Fool] SET QUERY_STORE = OFF
GO
USE [Fool]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [Fool]
GO
/****** Object:  Table [dbo].[EmailSubscription]     ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailSubscription](
	[EmailSubscriptionId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_EmailSubscription] PRIMARY KEY CLUSTERED 
(
	[EmailSubscriptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Instrument]     ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Instrument](
	[InstrumentId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Symbol] [nvarchar](50) NOT NULL,
	[Exchange] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Instrument] PRIMARY KEY CLUSTERED 
(
	[InstrumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]     ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserEmailSubscription]     ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserEmailSubscription](
	[UserId] [int] NOT NULL,
	[EmailSubscriptionId] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	CONSTRAINT [PK_UserEmailSubscription] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[EmailSubscriptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserEmailSubscription]  WITH CHECK ADD  CONSTRAINT [FK_UserEmailSubscription_EmailSubscription] FOREIGN KEY([EmailSubscriptionId])
REFERENCES [dbo].[EmailSubscription] ([EmailSubscriptionId])
GO
ALTER TABLE [dbo].[UserEmailSubscription] CHECK CONSTRAINT [FK_UserEmailSubscription_EmailSubscription]
GO
ALTER TABLE [dbo].[UserEmailSubscription]  WITH CHECK ADD  CONSTRAINT [FK_UserEmailSubscription_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[UserEmailSubscription] CHECK CONSTRAINT [FK_UserEmailSubscription_User]
GO
GO
/****** Object:  Table [dbo].[UserWatchedInstrument]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserWatchedInstrument](
	[UserId] [int] NOT NULL,
	[InstrumentId] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	CONSTRAINT [PK_UserWatchedInstrument] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[InstrumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserWatchedInstrument]  WITH CHECK ADD  CONSTRAINT [FK_UserWatchedInstrument_Instrument] FOREIGN KEY([InstrumentId])
REFERENCES [dbo].[Instrument] ([InstrumentId])
GO
ALTER TABLE [dbo].[UserWatchedInstrument] CHECK CONSTRAINT [FK_UserWatchedInstrument_Instrument]
GO
ALTER TABLE [dbo].[UserWatchedInstrument]  WITH CHECK ADD  CONSTRAINT [FK_UserWatchedInstrument_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[UserWatchedInstrument] CHECK CONSTRAINT [FK_UserWatchedInstrument_User]
GO
USE [master]
GO
ALTER DATABASE [Fool] SET  READ_WRITE 
GO

-- Test data
USE [Fool]

SET IDENTITY_INSERT EmailSubscription ON
INSERT INTO EmailSubscription (EmailSubscriptionId, Name) VALUES
(1, 'Non Predefined Mailing'),
(2, 'Scorecard Daily'),
(3, 'Breakfast News'),
(4, 'Rule Maker Portfolio'),
(5, 'Hot Topics'),
(6, 'Investing Basics'),
(7, 'FoolWatch Weekly'),
(8, 'Investment Clubs '),
(9, 'Ask FoolU'),
(10, 'Fresh Stuff'),
(11, 'Fool Books'),
(12, 'Daily Watchlist'),
(13, 'Weekly Watchlist');
SET IDENTITY_INSERT EmailSubscription OFF

SET IDENTITY_INSERT Instrument ON
INSERT INTO Instrument (InstrumentId, Name, Symbol, Exchange) VALUES 
(1, 'Amazon','AMZN','NYSE'),
(2, 'Netflix','NFLX','NYSE'),
(3, 'Google','GOOG','NYSE'),
(4, 'Roku','ROKU','NYSE'),
(5, 'Alcoa','AA','NYSE'),
(6, 'Advance Auto Parts','AAP','NYSE'),
(7, 'ABB','ABB','NYSE'),
(8, 'AmerisourceBergen','ABC','NYSE'),
(9, 'Dominion Diamond Corporation','DDC','NYSE'),
(10, 'Asbury Automotive Group','ABG','NYSE');
SET IDENTITY_INSERT Instrument OFF

SET IDENTITY_INSERT [User] ON
INSERT INTO [User] (UserId, FirstName, LastName, Email, DateCreated, DateModified) VALUES 
(1,'Kurt ','Shafer','KurtShafer@email.com', GETDATE(), GETDATE()),
(2,'Jean ','Livingston','JeanLivingston@email.com', GETDATE(), GETDATE()),
(3,'Larry ','Dickinson','LarryDickinson@email.com', GETDATE(), GETDATE()),
(4,'Rhonda ','Cohen','RhondaCohen@email.com', GETDATE(), GETDATE()),
(5,'Balaji ','Naga','BalajiNaga@email.com', GETDATE(), GETDATE()),
(6,'Atul ','Ashar','AtulAshar@email.com', GETDATE(), GETDATE()),
(7,'Bart ','Fernandes','BartFernandes@email.com', GETDATE(), GETDATE()),
(8,'Heather ','Flodstrom','HeatherFlodstrom@email.com', GETDATE(), GETDATE()),
(9,'teresa ','kraemer','teresakraemer@email.com', GETDATE(), GETDATE()),
(10,'mike ','shuster','mikeshuster@email.com', GETDATE(), GETDATE());
SET IDENTITY_INSERT [User] OFF

SET IDENTITY_INSERT [User] ON
INSERT INTO UserEmailSubscription (UserId, EmailSubscriptionId, DateCreated) VALUES
(1, 12, GETDATE()),
(2, 12, GETDATE()),
(3, 12, GETDATE()),
(4, 12, GETDATE()),
(5, 12, GETDATE()),
(6, 12, GETDATE()),
(8, 12, GETDATE()),
(9, 12, GETDATE()),
(10, 12, GETDATE());
SET IDENTITY_INSERT [User] OFF

INSERT INTO UserWatchedInstrument (UserId, InstrumentId, DateCreated) VALUES
(1, 1, GETDATE()),
(1, 2, GETDATE()),
(1, 3, GETDATE()),
(1, 4, GETDATE()),
(1, 5, GETDATE()),
(1, 6, GETDATE()),
(1, 7, GETDATE()),
(1, 8, GETDATE()),
(1, 9, GETDATE()),
(1, 10, GETDATE()),
(2, 1, GETDATE()),
(2, 3, GETDATE()),
(2, 5, GETDATE()),
(2, 7, GETDATE()),
(2, 9, GETDATE()),
(3, 2, GETDATE()),
(3, 4, GETDATE()),
(3, 6, GETDATE()),
(3, 8, GETDATE()),
(3, 10, GETDATE()),
(4, 1, GETDATE()),
(5, 2, GETDATE()),
(6, 3, GETDATE()),
(7, 4, GETDATE()),
(8, 5, GETDATE()),
(9, 6, GETDATE()),
(10, 7, GETDATE());