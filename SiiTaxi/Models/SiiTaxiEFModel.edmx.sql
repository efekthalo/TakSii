
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/18/2017 19:46:01
-- Generated from EDMX file: C:\Users\adamg\documents\visual studio 2015\Projects\SiiTaxi\SiiTaxi\Models\SiiTaxiEFModel.edmx
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
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetRoles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUsers];
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

IF OBJECT_ID(N'[dbo].[C__MigrationHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[C__MigrationHistory];
GO
IF OBJECT_ID(N'[dbo].[Approvers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Approvers];
GO
IF OBJECT_ID(N'[dbo].[AspNetRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserClaims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserClaims];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserLogins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserLogins];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers];
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
    [Approver] int  NOT NULL
);
GO

-- Creating table 'TaxiPeople'
CREATE TABLE [dbo].[TaxiPeople] (
    [TaxiId] int  NULL,
    [PeopleId] int  NULL,
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'C__MigrationHistory'
-- CREATE TABLE [dbo].[C__MigrationHistory] (
--     [MigrationId] nvarchar(150)  NOT NULL,
--     [ContextKey] nvarchar(300)  NOT NULL,
--     [Model] varbinary(max)  NOT NULL,
--     [ProductVersion] nvarchar(32)  NOT NULL
-- );
-- GO

-- Creating table 'AspNetRoles'
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'AspNetUserClaims'
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(128)  NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetUserLogins'
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] nvarchar(128)  NOT NULL,
    [ProviderKey] nvarchar(128)  NOT NULL,
    [UserId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUsers'
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(128)  NOT NULL,
    [Email] nvarchar(256)  NULL,
    [EmailConfirmed] bit  NOT NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberConfirmed] bit  NOT NULL,
    [TwoFactorEnabled] bit  NOT NULL,
    [LockoutEndDateUtc] datetime  NULL,
    [LockoutEnabled] bit  NOT NULL,
    [AccessFailedCount] int  NOT NULL,
    [UserName] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'AspNetUserRoles'
CREATE TABLE [dbo].[AspNetUserRoles] (
    [RoleId] nvarchar(128)  NOT NULL,
    [UserId] nvarchar(128)  NOT NULL
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

-- Creating primary key on [MigrationId], [ContextKey] in table 'C__MigrationHistory'
-- ALTER TABLE [dbo].[C__MigrationHistory]
-- ADD CONSTRAINT [PK_C__MigrationHistory]
--    PRIMARY KEY CLUSTERED ([MigrationId], [ContextKey] ASC);
-- GO

-- Creating primary key on [Id] in table 'AspNetRoles'
ALTER TABLE [dbo].[AspNetRoles]
ADD CONSTRAINT [PK_AspNetRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [PK_AspNetUserClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [LoginProvider], [ProviderKey], [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [PK_AspNetUserLogins]
    PRIMARY KEY CLUSTERED ([LoginProvider], [ProviderKey], [UserId] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [PK_AspNetUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [AspNetRoles_Id], [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [PK_AspNetUserRoles]
    PRIMARY KEY CLUSTERED ([RoleId], [UserId] ASC);
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

-- Creating foreign key on [UserId] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserClaims]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserLogins]
    ([UserId]);
GO

-- Creating foreign key on [AspNetRoles_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserRoles_AspNetUsers'
CREATE INDEX [IX_FK_AspNetUserRoles_AspNetUsers]
ON [dbo].[AspNetUserRoles]
    ([UserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------