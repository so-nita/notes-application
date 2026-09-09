
IF DB_ID('NoteAppDb') IS null
BEGIN
    CREATE DATABASE NoteAppDb;
END
GO

USE NoteAppDb;
GO

IF OBJECT_ID('dbo.Notes', 'U') IS not null drop table dbo.Notes;
IF OBJECT_ID('dbo.Users', 'U') IS not null drop table dbo.Users;
GO

CREATE TABLE Users (
                       Id          nvarchar(450)   not null primary key,
                       Username    nvarchar(100)   not null unique,
                       Password    nvarchar(255)   not null,
                       UserType    nvarchar(50)    not null,
                       CreatedBy   nvarchar(450)   null,
                       IsDeleted   BIT             not null default 0,
                       CreatedAt   datetime2       not null default SYSDATETIME(),
                       UpdatedAt   datetime2       null
);
GO

CREATE TABLE Notes (
                       Id          nvarchar(450)   not null primary key,
                       Title       nvarchar(200)   not null,
                       Content     nvarchar(MAX)   null,
                       IsDeleted   BIT             not null default 0,
                       CreatedBy   nvarchar(450)   null,
                       CreatedAt   datetime2       not null default SYSDATETIME(),
                       UpdatedAt   datetime2       null,
                       CONSTRAINT FK_Notes_Users_CreatedBy foreign key (CreatedBy) references Users(Id)
);
GO

CREATE INDEX IX_Notes_CreatedBy ON Notes(CreatedBy);
GO