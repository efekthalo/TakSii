
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/17/2017 14:26:59
-- Generated from EDMX file: C:\Users\adamg\Source\Repos\SiiTaxi\SiiTaxi\Models\SiiTaxiEFModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-SiiTaxi-20170129084253];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Approver_ToApprovers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Taxi] DROP CONSTRAINT [FK_Approver_ToApprovers];
GO
IF OBJECT_ID(N'[dbo].[FK_Approvers_ToTaxi]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Approvers] DROP CONSTRAINT [FK_Approvers_ToTaxi];
GO
IF OBJECT_ID(N'[dbo].[FK_Owner_ToPeople]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Taxi] DROP CONSTRAINT [FK_Owner_ToPeople];
GO
IF OBJECT_ID(N'[dbo].[FK_People_ToPeople]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaxiPeople] DROP CONSTRAINT [FK_People_ToPeople];
GO
IF OBJECT_ID(N'[dbo].[FK_Taxi_ToTaxi]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaxiPeople] DROP CONSTRAINT [FK_Taxi_ToTaxi];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Approvers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Approvers];
GO
IF OBJECT_ID(N'[dbo].[People]', 'U') IS NOT NULL
    DROP TABLE [dbo].[People];
GO
IF OBJECT_ID(N'[dbo].[Taxi]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Taxi];
GO
IF OBJECT_ID(N'[dbo].[TaxiPeople]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaxiPeople];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Approvers'
CREATE TABLE [dbo].[Approvers] (
    [PeopleId] int  NOT NULL,
    [IsApprover] bit  NOT NULL
);
GO

-- Creating table 'People'
CREATE TABLE [dbo].[People] (
    [PeopleId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NOT NULL,
    [AltEmail] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL
);
GO

-- Creating table 'Taxi'
CREATE TABLE [dbo].[Taxi] (
    [TaxiId] int IDENTITY(1,1) NOT NULL,
    [From] nvarchar(max)  NOT NULL,
    [To] nvarchar(max)  NOT NULL,
    [Owner] int  NOT NULL,
    [Time] datetime  NOT NULL,
    [Confirm] nvarchar(max)  NULL,
    [IsConfirmed] bit  NOT NULL,
    [IsOrdered] bit  NOT NULL,
    [Approver] int  NOT NULL,
    [IsBigTaxi] bit  NOT NULL,
    [Order] bit  NOT NULL
);
GO

-- Creating table 'TaxiPeople'
CREATE TABLE [dbo].[TaxiPeople] (
    [TaxiId] int  NULL,
    [PeopleId] int  NULL,
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [PeopleId] in table 'Approvers'
ALTER TABLE [dbo].[Approvers]
ADD CONSTRAINT [PK_Approvers]
    PRIMARY KEY CLUSTERED ([PeopleId] ASC);
GO

-- Creating primary key on [PeopleId] in table 'People'
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [PK_People]
    PRIMARY KEY CLUSTERED ([PeopleId] ASC);
GO

-- Creating primary key on [TaxiId] in table 'Taxi'
ALTER TABLE [dbo].[Taxi]
ADD CONSTRAINT [PK_Taxi]
    PRIMARY KEY CLUSTERED ([TaxiId] ASC);
GO

-- Creating primary key on [Id] in table 'TaxiPeople'
ALTER TABLE [dbo].[TaxiPeople]
ADD CONSTRAINT [PK_TaxiPeople]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Approver] in table 'Taxi'
ALTER TABLE [dbo].[Taxi]
ADD CONSTRAINT [FK_Approver_ToApprovers]
    FOREIGN KEY ([Approver])
    REFERENCES [dbo].[Approvers]
        ([PeopleId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Approver_ToApprovers'
CREATE INDEX [IX_FK_Approver_ToApprovers]
ON [dbo].[Taxi]
    ([Approver]);
GO

-- Creating foreign key on [PeopleId] in table 'Approvers'
ALTER TABLE [dbo].[Approvers]
ADD CONSTRAINT [FK_Approvers_ToTaxi]
    FOREIGN KEY ([PeopleId])
    REFERENCES [dbo].[People]
        ([PeopleId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Owner] in table 'Taxi'
ALTER TABLE [dbo].[Taxi]
ADD CONSTRAINT [FK_Owner_ToPeople]
    FOREIGN KEY ([Owner])
    REFERENCES [dbo].[People]
        ([PeopleId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Owner_ToPeople'
CREATE INDEX [IX_FK_Owner_ToPeople]
ON [dbo].[Taxi]
    ([Owner]);
GO

-- Creating foreign key on [PeopleId] in table 'TaxiPeople'
ALTER TABLE [dbo].[TaxiPeople]
ADD CONSTRAINT [FK_People_ToPeople]
    FOREIGN KEY ([PeopleId])
    REFERENCES [dbo].[People]
        ([PeopleId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_People_ToPeople'
CREATE INDEX [IX_FK_People_ToPeople]
ON [dbo].[TaxiPeople]
    ([PeopleId]);
GO

-- Creating foreign key on [TaxiId] in table 'TaxiPeople'
ALTER TABLE [dbo].[TaxiPeople]
ADD CONSTRAINT [FK_Taxi_ToTaxi]
    FOREIGN KEY ([TaxiId])
    REFERENCES [dbo].[Taxi]
        ([TaxiId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Taxi_ToTaxi'
CREATE INDEX [IX_FK_Taxi_ToTaxi]
ON [dbo].[TaxiPeople]
    ([TaxiId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------