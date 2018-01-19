
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/16/2017 09:26:08
-- Generated from EDMX file: E:\shuangzan\Model\ShuangZanModelDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ShuangZanDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserInfoRoleInfo_UserInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserInfoRoleInfo] DROP CONSTRAINT [FK_UserInfoRoleInfo_UserInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_UserInfoRoleInfo_RoleInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserInfoRoleInfo] DROP CONSTRAINT [FK_UserInfoRoleInfo_RoleInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_UserInfoR_UserInfo_ActionInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[R_UserInfo_ActionInfo] DROP CONSTRAINT [FK_UserInfoR_UserInfo_ActionInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_ActionInfoR_UserInfo_ActionInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[R_UserInfo_ActionInfo] DROP CONSTRAINT [FK_ActionInfoR_UserInfo_ActionInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleInfoActionInfo_RoleInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleInfoActionInfo] DROP CONSTRAINT [FK_RoleInfoActionInfo_RoleInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleInfoActionInfo_ActionInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleInfoActionInfo] DROP CONSTRAINT [FK_RoleInfoActionInfo_ActionInfo];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserInfo];
GO
IF OBJECT_ID(N'[dbo].[ActionInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ActionInfo];
GO
IF OBJECT_ID(N'[dbo].[RoleInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleInfo];
GO
IF OBJECT_ID(N'[dbo].[R_UserInfo_ActionInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[R_UserInfo_ActionInfo];
GO
IF OBJECT_ID(N'[dbo].[PackageCard]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PackageCard];
GO
IF OBJECT_ID(N'[dbo].[PersonalUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonalUser];
GO
IF OBJECT_ID(N'[dbo].[UserMessage]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserMessage];
GO
IF OBJECT_ID(N'[dbo].[OpenService]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OpenService];
GO
IF OBJECT_ID(N'[dbo].[KeyWords]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KeyWords];
GO
IF OBJECT_ID(N'[dbo].[Tests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tests];
GO
IF OBJECT_ID(N'[dbo].[Order]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Order];
GO
IF OBJECT_ID(N'[dbo].[Feedback]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Feedback];
GO
IF OBJECT_ID(N'[dbo].[GameDemoRecharge]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GameDemoRecharge];
GO
IF OBJECT_ID(N'[dbo].[GameDemoAccounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GameDemoAccounts];
GO
IF OBJECT_ID(N'[dbo].[GameDemoRequires]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GameDemoRequires];
GO
IF OBJECT_ID(N'[dbo].[Package]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Package];
GO
IF OBJECT_ID(N'[dbo].[UserRaiders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRaiders];
GO
IF OBJECT_ID(N'[dbo].[GameDemo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GameDemo];
GO
IF OBJECT_ID(N'[dbo].[CompanyGame]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CompanyGame];
GO
IF OBJECT_ID(N'[dbo].[Recharge]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Recharge];
GO
IF OBJECT_ID(N'[dbo].[Company]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Company];
GO
IF OBJECT_ID(N'[dbo].[RechargedUsed]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RechargedUsed];
GO
IF OBJECT_ID(N'[dbo].[News]', 'U') IS NOT NULL
    DROP TABLE [dbo].[News];
GO
IF OBJECT_ID(N'[dbo].[AuditLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AuditLog];
GO
IF OBJECT_ID(N'[dbo].[VerificationCode]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VerificationCode];
GO
IF OBJECT_ID(N'[dbo].[DemoUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DemoUser];
GO
IF OBJECT_ID(N'[dbo].[PersonalUserSign]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonalUserSign];
GO
IF OBJECT_ID(N'[dbo].[PersonalUserSignDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonalUserSignDetail];
GO
IF OBJECT_ID(N'[dbo].[SeeNews]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SeeNews];
GO
IF OBJECT_ID(N'[dbo].[LeaveMsg]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LeaveMsg];
GO
IF OBJECT_ID(N'[dbo].[ForbiddenList]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ForbiddenList];
GO
IF OBJECT_ID(N'[dbo].[Game]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Game];
GO
IF OBJECT_ID(N'[dbo].[HomePage]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HomePage];
GO
IF OBJECT_ID(N'[dbo].[BlackListIP]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BlackListIP];
GO
IF OBJECT_ID(N'[dbo].[WonderfulTxtImg]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WonderfulTxtImg];
GO
IF OBJECT_ID(N'[dbo].[Seo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Seo];
GO
IF OBJECT_ID(N'[dbo].[BeautifulGirls]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BeautifulGirls];
GO
IF OBJECT_ID(N'[dbo].[Advertisement]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Advertisement];
GO
IF OBJECT_ID(N'[dbo].[Gift]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Gift];
GO
IF OBJECT_ID(N'[dbo].[UserAdress]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAdress];
GO
IF OBJECT_ID(N'[dbo].[UserInfoRoleInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserInfoRoleInfo];
GO
IF OBJECT_ID(N'[dbo].[RoleInfoActionInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleInfoActionInfo];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserInfo'
CREATE TABLE [dbo].[UserInfo] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UName] nvarchar(32)  NOT NULL,
    [Name] nvarchar(32)  NOT NULL,
    [InTime] datetime  NULL,
    [Last_login_Time] datetime  NOT NULL,
    [Login_Num] int  NOT NULL,
    [State] nvarchar(1)  NOT NULL,
    [Pwd] nvarchar(32)  NOT NULL,
    [Tel] nvarchar(11)  NULL,
    [DelFlag] smallint  NOT NULL
);
GO

-- Creating table 'ActionInfo'
CREATE TABLE [dbo].[ActionInfo] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ActionName] nvarchar(32)  NULL,
    [Url] nvarchar(32)  NULL,
    [HttpMethod] nvarchar(32)  NOT NULL,
    [DelFlag] smallint  NULL,
    [ActionType] smallint  NULL,
    [MenuName] nvarchar(100)  NULL,
    [SubTime] datetime  NULL
);
GO

-- Creating table 'RoleInfo'
CREATE TABLE [dbo].[RoleInfo] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RoleName] nvarchar(32)  NULL,
    [DelFlag] smallint  NOT NULL,
    [SubTime] datetime  NULL
);
GO

-- Creating table 'R_UserInfo_ActionInfo'
CREATE TABLE [dbo].[R_UserInfo_ActionInfo] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IsPass] bit  NOT NULL,
    [UserInfoId] int  NOT NULL,
    [ActionInfoId] int  NOT NULL
);
GO

-- Creating table 'PackageCard'
CREATE TABLE [dbo].[PackageCard] (
    [PackageId] int  NOT NULL,
    [UserId] int  NOT NULL,
    [Code] nvarchar(100)  NOT NULL,
    [InTime] datetime  NULL
);
GO

-- Creating table 'PersonalUser'
CREATE TABLE [dbo].[PersonalUser] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UName] varchar(50)  NOT NULL,
    [Password] varchar(50)  NOT NULL,
    [Mobile] varchar(500)  NOT NULL,
    [InTime] datetime  NOT NULL,
    [QQ] varchar(50)  NULL,
    [WeChat] varchar(250)  NULL,
    [Email] varchar(250)  NULL,
    [Birthday] varchar(50)  NULL,
    [Head] varchar(50)  NULL,
    [Contacts] varchar(50)  NULL,
    [ContactNum] varchar(50)  NULL,
    [State] int  NOT NULL,
    [ReferrerId] int  NOT NULL,
    [RecommendNum] int  NOT NULL
);
GO

-- Creating table 'UserMessage'
CREATE TABLE [dbo].[UserMessage] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [Title] nvarchar(500)  NOT NULL,
    [Msg] nvarchar(max)  NOT NULL,
    [State] int  NOT NULL,
    [InTime] datetime  NOT NULL,
    [PayType] nvarchar(1)  NOT NULL,
    [Pay] int  NOT NULL,
    [IsDel] nvarchar(1)  NULL,
    [Memo] nvarchar(250)  NULL,
    [Memo1] nvarchar(250)  NULL,
    [Memo2] nvarchar(250)  NULL,
    [orderNo] nvarchar(150)  NULL
);
GO

-- Creating table 'OpenService'
CREATE TABLE [dbo].[OpenService] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [GameName] nvarchar(250)  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [Url] nvarchar(150)  NOT NULL,
    [PackageId] int  NULL,
    [Rec_Hot] nvarchar(5)  NOT NULL,
    [Rec_Datetime] datetime  NOT NULL,
    [State] nvarchar(5)  NOT NULL,
    [InTime] datetime  NULL,
    [CheckName] nvarchar(50)  NULL,
    [ServerName] nvarchar(50)  NULL
);
GO

-- Creating table 'KeyWords'
CREATE TABLE [dbo].[KeyWords] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(50)  NOT NULL,
    [IsLibrary] nvarchar(1)  NULL,
    [LibraryTime] datetime  NOT NULL
);
GO

-- Creating table 'Tests'
CREATE TABLE [dbo].[Tests] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GameName] nvarchar(150)  NULL,
    [StartTime] datetime  NULL,
    [State] nvarchar(50)  NULL,
    [Url] nvarchar(250)  NULL,
    [Package] nvarchar(250)  NULL,
    [InTime] datetime  NOT NULL
);
GO

-- Creating table 'Order'
CREATE TABLE [dbo].[Order] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [Name] nvarchar(50)  NULL,
    [Price] int  NULL,
    [Num] int  NULL,
    [color] nvarchar(50)  NULL,
    [Address_Name] nvarchar(150)  NULL,
    [Address] nvarchar(500)  NULL,
    [Phone] nvarchar(50)  NULL,
    [state] nvarchar(1)  NOT NULL,
    [Type] nvarchar(150)  NULL,
    [Order_Num] nvarchar(50)  NULL,
    [InTime] datetime  NOT NULL,
    [SendTime] datetime  NULL,
    [ReceiveTime] datetime  NULL,
    [IsDel] int  NOT NULL,
    [Logo] nvarchar(150)  NULL
);
GO

-- Creating table 'Feedback'
CREATE TABLE [dbo].[Feedback] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Msg] nvarchar(max)  NULL,
    [Mobile] nvarchar(500)  NULL,
    [InTime] datetime  NOT NULL
);
GO

-- Creating table 'GameDemoRecharge'
CREATE TABLE [dbo].[GameDemoRecharge] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GameDemoId] int  NOT NULL,
    [AccountId] int  NOT NULL,
    [UserId] int  NOT NULL,
    [Pay] int  NOT NULL,
    [Memo] nvarchar(50)  NULL,
    [Reason] nvarchar(500)  NULL,
    [InTime] datetime  NOT NULL,
    [State] int  NOT NULL
);
GO

-- Creating table 'GameDemoAccounts'
CREATE TABLE [dbo].[GameDemoAccounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GameDemoId] int  NOT NULL,
    [SystemName] nvarchar(250)  NULL,
    [Url] nvarchar(500)  NULL,
    [UName] nvarchar(32)  NULL,
    [Pwd] nvarchar(50)  NULL,
    [City] nvarchar(250)  NULL,
    [CurrentPlayer] nvarchar(50)  NULL,
    [InTime] datetime  NULL
);
GO

-- Creating table 'GameDemoRequires'
CREATE TABLE [dbo].[GameDemoRequires] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GameDemoId] int  NOT NULL,
    [Demand] nvarchar(500)  NULL,
    [AwardCoins] int  NULL,
    [CoinsEquity] nvarchar(500)  NULL
);
GO

-- Creating table 'Package'
CREATE TABLE [dbo].[Package] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [GameName] nvarchar(200)  NOT NULL,
    [ServerName] nvarchar(200)  NOT NULL,
    [Type] nvarchar(10)  NOT NULL,
    [GiftName] nvarchar(200)  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL,
    [Url] nvarchar(200)  NOT NULL,
    [Msg] nvarchar(max)  NOT NULL,
    [Memo] nvarchar(2500)  NOT NULL,
    [State] nvarchar(10)  NOT NULL,
    [Rec] nvarchar(1)  NOT NULL,
    [Rec_Time] datetime  NOT NULL,
    [Rec_Hot] nvarchar(1)  NOT NULL,
    [Rec_Hot_Time] datetime  NOT NULL,
    [Rec_Index] nvarchar(1)  NOT NULL,
    [Rec_Index_Time] datetime  NOT NULL,
    [InTime] datetime  NOT NULL,
    [CheckName] nvarchar(50)  NULL
);
GO

-- Creating table 'UserRaiders'
CREATE TABLE [dbo].[UserRaiders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [GameName] nvarchar(150)  NULL,
    [Title] nvarchar(200)  NOT NULL,
    [EditTitle] nvarchar(200)  NULL,
    [Key] nvarchar(500)  NULL,
    [Msg] nvarchar(max)  NULL,
    [Reason] nvarchar(500)  NULL,
    [InTime] datetime  NOT NULL,
    [State] nvarchar(50)  NOT NULL,
    [Rec] nvarchar(1)  NOT NULL,
    [Rec_Time] datetime  NOT NULL,
    [Rec_Hot] nvarchar(1)  NOT NULL,
    [Rec_Hot_Time] datetime  NULL,
    [CheckName] nvarchar(50)  NULL,
    [Source] nvarchar(50)  NULL,
    [Memo] nvarchar(500)  NULL,
    [Editor] nvarchar(50)  NULL,
    [Views] int  NULL,
    [Tip] int  NULL,
    [Stamp] int  NULL
);
GO

-- Creating table 'GameDemo'
CREATE TABLE [dbo].[GameDemo] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GameName] nvarchar(250)  NULL,
    [Img] nvarchar(50)  NULL,
    [state] int  NOT NULL,
    [Rec] nvarchar(1)  NOT NULL,
    [Rec_Time] datetime  NOT NULL,
    [Rec_Hot] nvarchar(1)  NOT NULL,
    [rec_HotTime] datetime  NOT NULL,
    [InTime] datetime  NOT NULL,
    [CheckName] nvarchar(50)  NULL,
    [Type] nvarchar(1)  NULL
);
GO

-- Creating table 'CompanyGame'
CREATE TABLE [dbo].[CompanyGame] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [GameName] nvarchar(250)  NOT NULL,
    [Type] nvarchar(1)  NOT NULL,
    [Url] nvarchar(500)  NOT NULL,
    [InTime] datetime  NOT NULL
);
GO

-- Creating table 'Recharge'
CREATE TABLE [dbo].[Recharge] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [Money] int  NULL,
    [Num] int  NULL,
    [Used] int  NOT NULL,
    [Remark] nvarchar(500)  NULL,
    [InTime] datetime  NOT NULL,
    [ComboName] nvarchar(100)  NULL
);
GO

-- Creating table 'Company'
CREATE TABLE [dbo].[Company] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UName] nvarchar(32)  NULL,
    [Pwd] nvarchar(32)  NULL,
    [Email] nvarchar(180)  NULL,
    [SystemName] nvarchar(100)  NULL,
    [WebSite] nvarchar(100)  NULL,
    [CompanyName] nvarchar(100)  NULL,
    [Phone] nvarchar(50)  NULL,
    [Address] nvarchar(120)  NULL,
    [Contacts] nvarchar(50)  NULL,
    [Office] nvarchar(50)  NULL,
    [Head] nvarchar(50)  NULL,
    [Logo] nvarchar(100)  NULL,
    [Moblie] nvarchar(50)  NULL,
    [QQ] nvarchar(50)  NULL,
    [Sex] int  NULL,
    [State] int  NOT NULL,
    [CopanyMsg] nvarchar(max)  NULL,
    [InTime] datetime  NOT NULL,
    [Rec_Forum_Index] nvarchar(1)  NOT NULL,
    [Rec_Forum_Index_Time] datetime  NOT NULL,
    [Rec_Index] nvarchar(1)  NOT NULL,
    [Rec_Index_Time] datetime  NOT NULL,
    [DelFlag] smallint  NULL
);
GO

-- Creating table 'RechargedUsed'
CREATE TABLE [dbo].[RechargedUsed] (
    [RechargeId] int  NOT NULL,
    [OpenServiceId] int  NOT NULL,
    [InTime] datetime  NOT NULL
);
GO

-- Creating table 'News'
CREATE TABLE [dbo].[News] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [Title] nvarchar(400)  NULL,
    [EditTitle] nvarchar(400)  NULL,
    [Game] nvarchar(250)  NULL,
    [Kewords] nvarchar(300)  NULL,
    [Type] nvarchar(1)  NOT NULL,
    [CheckName] nvarchar(50)  NULL,
    [Memo] nvarchar(max)  NULL,
    [Msg] nvarchar(max)  NULL,
    [Views] int  NULL,
    [LeaveMsgId] int  NULL,
    [State] nvarchar(50)  NOT NULL,
    [Source] nvarchar(100)  NULL,
    [InTime] datetime  NOT NULL,
    [Rec_Forum_Index] nvarchar(1)  NOT NULL,
    [Rec_Forum_Time] datetime  NOT NULL,
    [Rec_Hot_Index] nvarchar(1)  NOT NULL,
    [Rec_Hot_Time] datetime  NOT NULL,
    [Editor] nvarchar(32)  NULL,
    [AddedBy] nvarchar(32)  NULL,
    [Tip] int  NULL,
    [Stamp] int  NULL,
    [GameTerms] nvarchar(200)  NULL
);
GO

-- Creating table 'AuditLog'
CREATE TABLE [dbo].[AuditLog] (
    [CompanyId] int  NOT NULL,
    [Title] varchar(max)  NOT NULL,
    [Type] int  NOT NULL,
    [TableId] int  NOT NULL,
    [Reason] nvarchar(2500)  NULL,
    [InTime] datetime  NOT NULL
);
GO

-- Creating table 'VerificationCode'
CREATE TABLE [dbo].[VerificationCode] (
    [Mobile] nvarchar(50)  NOT NULL,
    [Code] nvarchar(50)  NOT NULL,
    [InTime] datetime  NOT NULL,
    [Ip] nvarchar(50)  NULL
);
GO

-- Creating table 'DemoUser'
CREATE TABLE [dbo].[DemoUser] (
    [GameDemoId] int  NOT NULL,
    [AccountId] int  NOT NULL,
    [RequiredId] int  NOT NULL,
    [UserId] int  NOT NULL,
    [Img] nvarchar(50)  NULL,
    [State] nvarchar(2)  NOT NULL,
    [Reason] nvarchar(500)  NULL,
    [InTime] datetime  NOT NULL
);
GO

-- Creating table 'PersonalUserSign'
CREATE TABLE [dbo].[PersonalUserSign] (
    [UserId] int IDENTITY(1,1) NOT NULL,
    [LastTime] datetime  NOT NULL,
    [Num] int  NOT NULL
);
GO

-- Creating table 'PersonalUserSignDetail'
CREATE TABLE [dbo].[PersonalUserSignDetail] (
    [UserId] int  NOT NULL,
    [InTime] datetime  NOT NULL
);
GO

-- Creating table 'SeeNews'
CREATE TABLE [dbo].[SeeNews] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(200)  NULL,
    [Image] nvarchar(100)  NULL,
    [Intime] datetime  NOT NULL,
    [Type] nvarchar(5)  NOT NULL,
    [Url] nvarchar(250)  NULL
);
GO

-- Creating table 'LeaveMsg'
CREATE TABLE [dbo].[LeaveMsg] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MsgNum] int  NULL,
    [NewsId] int  NOT NULL,
    [Msg] nvarchar(max)  NULL,
    [InTime] datetime  NOT NULL,
    [PersonalUserId] int  NULL,
    [IP] nvarchar(150)  NULL,
    [City] nvarchar(100)  NULL,
    [ReplyId] int  NULL,
    [UserRaidersId] int  NOT NULL,
    [GirlId] int  NULL,
    [Stamp] int  NULL,
    [Tip] int  NULL
);
GO

-- Creating table 'ForbiddenList'
CREATE TABLE [dbo].[ForbiddenList] (
    [Id] uniqueidentifier  NOT NULL,
    [WordPattern] nvarchar(max)  NULL,
    [IsForbid] bit  NOT NULL,
    [IsMod] bit  NULL,
    [ReplaceWord] nvarchar(1000)  NULL
);
GO

-- Creating table 'Game'
CREATE TABLE [dbo].[Game] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NULL,
    [Alias] nvarchar(150)  NULL,
    [Type] nvarchar(1)  NULL,
    [Theme] nvarchar(2)  NULL,
    [Play] nvarchar(1)  NULL,
    [Company] nvarchar(350)  NULL,
    [Operator] nvarchar(50)  NULL,
    [Msg] nvarchar(max)  NULL,
    [Url] nvarchar(250)  NULL,
    [Rec] nvarchar(1)  NOT NULL,
    [Rec_Time] datetime  NOT NULL,
    [Rec_Hot] nvarchar(1)  NOT NULL,
    [Rec_Hot_Time] datetime  NOT NULL,
    [Rec_Index] nvarchar(1)  NOT NULL,
    [Rec_Index_Time] datetime  NOT NULL,
    [InTime] datetime  NOT NULL,
    [BigImg] nvarchar(150)  NULL,
    [SmallImg] nvarchar(150)  NULL,
    [DescImg] nvarchar(150)  NULL,
    [LoGo] nvarchar(150)  NULL,
    [CutImg] nvarchar(500)  NULL,
    [State] nvarchar(1)  NOT NULL,
    [LikeNum] int  NULL
);
GO

-- Creating table 'HomePage'
CREATE TABLE [dbo].[HomePage] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(500)  NULL,
    [Img] nvarchar(150)  NULL,
    [Url] nvarchar(250)  NULL,
    [InTime] datetime  NOT NULL,
    [Type] nvarchar(50)  NULL,
    [SmallImg] nvarchar(500)  NULL
);
GO

-- Creating table 'BlackListIP'
CREATE TABLE [dbo].[BlackListIP] (
    [Id] uniqueidentifier  NOT NULL,
    [IP] nvarchar(100)  NULL,
    [InTime] datetime  NOT NULL
);
GO

-- Creating table 'WonderfulTxtImg'
CREATE TABLE [dbo].[WonderfulTxtImg] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(500)  NULL,
    [Url] nvarchar(250)  NULL,
    [Image] nvarchar(350)  NULL,
    [InTime] datetime  NOT NULL
);
GO

-- Creating table 'Seo'
CREATE TABLE [dbo].[Seo] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(200)  NULL,
    [Url] nvarchar(250)  NULL,
    [Type] nvarchar(50)  NULL,
    [InTime] datetime  NOT NULL
);
GO

-- Creating table 'BeautifulGirls'
CREATE TABLE [dbo].[BeautifulGirls] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(150)  NOT NULL,
    [Tags] nvarchar(2000)  NOT NULL,
    [Imgs] nvarchar(2000)  NOT NULL,
    [InTime] datetime  NOT NULL,
    [Rec] int  NOT NULL,
    [Rec_Time] datetime  NOT NULL,
    [LeadTxt] nvarchar(500)  NULL,
    [Views] int  NULL,
    [AddedBy] nvarchar(50)  NULL,
    [Tip] int  NULL,
    [Stamp] int  NULL
);
GO

-- Creating table 'Advertisement'
CREATE TABLE [dbo].[Advertisement] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(1)  NULL,
    [State] int  NOT NULL,
    [Title] nvarchar(250)  NULL,
    [Path] nvarchar(1000)  NULL,
    [Url] nvarchar(350)  NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [InTime] datetime  NOT NULL
);
GO

-- Creating table 'Gift'
CREATE TABLE [dbo].[Gift] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(500)  NULL,
    [Logo] nvarchar(500)  NULL,
    [Price] int  NULL,
    [State] nvarchar(1)  NOT NULL,
    [Intime] datetime  NOT NULL,
    [Explain] nvarchar(2000)  NULL,
    [OldPrice] int  NULL,
    [Color] nvarchar(250)  NULL,
    [Stock] int  NULL,
    [Imgs] nvarchar(2000)  NULL,
    [Msgs] nvarchar(max)  NULL
);
GO

-- Creating table 'UserAdress'
CREATE TABLE [dbo].[UserAdress] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Address] nvarchar(500)  NOT NULL,
    [Phone] nvarchar(50)  NOT NULL,
    [UserId] int  NOT NULL,
    [InTime] datetime  NOT NULL,
    [IsDefault] int  NOT NULL
);
GO

-- Creating table 'UserInfoRoleInfo'
CREATE TABLE [dbo].[UserInfoRoleInfo] (
    [UserInfo_Id] int  NOT NULL,
    [RoleInfo_Id] int  NOT NULL
);
GO

-- Creating table 'RoleInfoActionInfo'
CREATE TABLE [dbo].[RoleInfoActionInfo] (
    [RoleInfo_Id] int  NOT NULL,
    [ActionInfo_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UserInfo'
ALTER TABLE [dbo].[UserInfo]
ADD CONSTRAINT [PK_UserInfo]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ActionInfo'
ALTER TABLE [dbo].[ActionInfo]
ADD CONSTRAINT [PK_ActionInfo]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RoleInfo'
ALTER TABLE [dbo].[RoleInfo]
ADD CONSTRAINT [PK_RoleInfo]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'R_UserInfo_ActionInfo'
ALTER TABLE [dbo].[R_UserInfo_ActionInfo]
ADD CONSTRAINT [PK_R_UserInfo_ActionInfo]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [PackageId], [UserId], [Code] in table 'PackageCard'
ALTER TABLE [dbo].[PackageCard]
ADD CONSTRAINT [PK_PackageCard]
    PRIMARY KEY CLUSTERED ([PackageId], [UserId], [Code] ASC);
GO

-- Creating primary key on [Id] in table 'PersonalUser'
ALTER TABLE [dbo].[PersonalUser]
ADD CONSTRAINT [PK_PersonalUser]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserMessage'
ALTER TABLE [dbo].[UserMessage]
ADD CONSTRAINT [PK_UserMessage]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OpenService'
ALTER TABLE [dbo].[OpenService]
ADD CONSTRAINT [PK_OpenService]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'KeyWords'
ALTER TABLE [dbo].[KeyWords]
ADD CONSTRAINT [PK_KeyWords]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tests'
ALTER TABLE [dbo].[Tests]
ADD CONSTRAINT [PK_Tests]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Order'
ALTER TABLE [dbo].[Order]
ADD CONSTRAINT [PK_Order]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Feedback'
ALTER TABLE [dbo].[Feedback]
ADD CONSTRAINT [PK_Feedback]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GameDemoRecharge'
ALTER TABLE [dbo].[GameDemoRecharge]
ADD CONSTRAINT [PK_GameDemoRecharge]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GameDemoAccounts'
ALTER TABLE [dbo].[GameDemoAccounts]
ADD CONSTRAINT [PK_GameDemoAccounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GameDemoRequires'
ALTER TABLE [dbo].[GameDemoRequires]
ADD CONSTRAINT [PK_GameDemoRequires]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Package'
ALTER TABLE [dbo].[Package]
ADD CONSTRAINT [PK_Package]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserRaiders'
ALTER TABLE [dbo].[UserRaiders]
ADD CONSTRAINT [PK_UserRaiders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GameDemo'
ALTER TABLE [dbo].[GameDemo]
ADD CONSTRAINT [PK_GameDemo]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CompanyGame'
ALTER TABLE [dbo].[CompanyGame]
ADD CONSTRAINT [PK_CompanyGame]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Recharge'
ALTER TABLE [dbo].[Recharge]
ADD CONSTRAINT [PK_Recharge]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Company'
ALTER TABLE [dbo].[Company]
ADD CONSTRAINT [PK_Company]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [RechargeId], [OpenServiceId] in table 'RechargedUsed'
ALTER TABLE [dbo].[RechargedUsed]
ADD CONSTRAINT [PK_RechargedUsed]
    PRIMARY KEY CLUSTERED ([RechargeId], [OpenServiceId] ASC);
GO

-- Creating primary key on [Id] in table 'News'
ALTER TABLE [dbo].[News]
ADD CONSTRAINT [PK_News]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [CompanyId], [Title], [Type], [TableId], [InTime] in table 'AuditLog'
ALTER TABLE [dbo].[AuditLog]
ADD CONSTRAINT [PK_AuditLog]
    PRIMARY KEY CLUSTERED ([CompanyId], [Title], [Type], [TableId], [InTime] ASC);
GO

-- Creating primary key on [Mobile] in table 'VerificationCode'
ALTER TABLE [dbo].[VerificationCode]
ADD CONSTRAINT [PK_VerificationCode]
    PRIMARY KEY CLUSTERED ([Mobile] ASC);
GO

-- Creating primary key on [GameDemoId], [AccountId], [RequiredId], [UserId], [InTime] in table 'DemoUser'
ALTER TABLE [dbo].[DemoUser]
ADD CONSTRAINT [PK_DemoUser]
    PRIMARY KEY CLUSTERED ([GameDemoId], [AccountId], [RequiredId], [UserId], [InTime] ASC);
GO

-- Creating primary key on [UserId], [LastTime], [Num] in table 'PersonalUserSign'
ALTER TABLE [dbo].[PersonalUserSign]
ADD CONSTRAINT [PK_PersonalUserSign]
    PRIMARY KEY CLUSTERED ([UserId], [LastTime], [Num] ASC);
GO

-- Creating primary key on [UserId], [InTime] in table 'PersonalUserSignDetail'
ALTER TABLE [dbo].[PersonalUserSignDetail]
ADD CONSTRAINT [PK_PersonalUserSignDetail]
    PRIMARY KEY CLUSTERED ([UserId], [InTime] ASC);
GO

-- Creating primary key on [Id] in table 'SeeNews'
ALTER TABLE [dbo].[SeeNews]
ADD CONSTRAINT [PK_SeeNews]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LeaveMsg'
ALTER TABLE [dbo].[LeaveMsg]
ADD CONSTRAINT [PK_LeaveMsg]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ForbiddenList'
ALTER TABLE [dbo].[ForbiddenList]
ADD CONSTRAINT [PK_ForbiddenList]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Game'
ALTER TABLE [dbo].[Game]
ADD CONSTRAINT [PK_Game]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HomePage'
ALTER TABLE [dbo].[HomePage]
ADD CONSTRAINT [PK_HomePage]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BlackListIP'
ALTER TABLE [dbo].[BlackListIP]
ADD CONSTRAINT [PK_BlackListIP]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'WonderfulTxtImg'
ALTER TABLE [dbo].[WonderfulTxtImg]
ADD CONSTRAINT [PK_WonderfulTxtImg]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Seo'
ALTER TABLE [dbo].[Seo]
ADD CONSTRAINT [PK_Seo]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BeautifulGirls'
ALTER TABLE [dbo].[BeautifulGirls]
ADD CONSTRAINT [PK_BeautifulGirls]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Advertisement'
ALTER TABLE [dbo].[Advertisement]
ADD CONSTRAINT [PK_Advertisement]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Gift'
ALTER TABLE [dbo].[Gift]
ADD CONSTRAINT [PK_Gift]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserAdress'
ALTER TABLE [dbo].[UserAdress]
ADD CONSTRAINT [PK_UserAdress]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UserInfo_Id], [RoleInfo_Id] in table 'UserInfoRoleInfo'
ALTER TABLE [dbo].[UserInfoRoleInfo]
ADD CONSTRAINT [PK_UserInfoRoleInfo]
    PRIMARY KEY CLUSTERED ([UserInfo_Id], [RoleInfo_Id] ASC);
GO

-- Creating primary key on [RoleInfo_Id], [ActionInfo_Id] in table 'RoleInfoActionInfo'
ALTER TABLE [dbo].[RoleInfoActionInfo]
ADD CONSTRAINT [PK_RoleInfoActionInfo]
    PRIMARY KEY CLUSTERED ([RoleInfo_Id], [ActionInfo_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserInfo_Id] in table 'UserInfoRoleInfo'
ALTER TABLE [dbo].[UserInfoRoleInfo]
ADD CONSTRAINT [FK_UserInfoRoleInfo_UserInfo]
    FOREIGN KEY ([UserInfo_Id])
    REFERENCES [dbo].[UserInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RoleInfo_Id] in table 'UserInfoRoleInfo'
ALTER TABLE [dbo].[UserInfoRoleInfo]
ADD CONSTRAINT [FK_UserInfoRoleInfo_RoleInfo]
    FOREIGN KEY ([RoleInfo_Id])
    REFERENCES [dbo].[RoleInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserInfoRoleInfo_RoleInfo'
CREATE INDEX [IX_FK_UserInfoRoleInfo_RoleInfo]
ON [dbo].[UserInfoRoleInfo]
    ([RoleInfo_Id]);
GO

-- Creating foreign key on [UserInfoId] in table 'R_UserInfo_ActionInfo'
ALTER TABLE [dbo].[R_UserInfo_ActionInfo]
ADD CONSTRAINT [FK_UserInfoR_UserInfo_ActionInfo]
    FOREIGN KEY ([UserInfoId])
    REFERENCES [dbo].[UserInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserInfoR_UserInfo_ActionInfo'
CREATE INDEX [IX_FK_UserInfoR_UserInfo_ActionInfo]
ON [dbo].[R_UserInfo_ActionInfo]
    ([UserInfoId]);
GO

-- Creating foreign key on [ActionInfoId] in table 'R_UserInfo_ActionInfo'
ALTER TABLE [dbo].[R_UserInfo_ActionInfo]
ADD CONSTRAINT [FK_ActionInfoR_UserInfo_ActionInfo]
    FOREIGN KEY ([ActionInfoId])
    REFERENCES [dbo].[ActionInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ActionInfoR_UserInfo_ActionInfo'
CREATE INDEX [IX_FK_ActionInfoR_UserInfo_ActionInfo]
ON [dbo].[R_UserInfo_ActionInfo]
    ([ActionInfoId]);
GO

-- Creating foreign key on [RoleInfo_Id] in table 'RoleInfoActionInfo'
ALTER TABLE [dbo].[RoleInfoActionInfo]
ADD CONSTRAINT [FK_RoleInfoActionInfo_RoleInfo]
    FOREIGN KEY ([RoleInfo_Id])
    REFERENCES [dbo].[RoleInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ActionInfo_Id] in table 'RoleInfoActionInfo'
ALTER TABLE [dbo].[RoleInfoActionInfo]
ADD CONSTRAINT [FK_RoleInfoActionInfo_ActionInfo]
    FOREIGN KEY ([ActionInfo_Id])
    REFERENCES [dbo].[ActionInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleInfoActionInfo_ActionInfo'
CREATE INDEX [IX_FK_RoleInfoActionInfo_ActionInfo]
ON [dbo].[RoleInfoActionInfo]
    ([ActionInfo_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------