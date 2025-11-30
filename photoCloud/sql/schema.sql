-- Create Database
IF NOT EXISTS(SELECT name FROM sys.databases WHERE name = 'photoCloud')
BEGIN
    CREATE DATABASE photoCloud;
END
GO

USE photoCloud;
GO


---------------------------
-- USERS TABLE
---------------------------
CREATE TABLE Users(
    userId INT IDENTITY(1,1) PRIMARY KEY,
    userName VARCHAR(50) NOT NULL UNIQUE,
    passwordHash VARCHAR(MAX) NOT NULL,
    Email VARCHAR(50) NOT NULL UNIQUE
);
GO


---------------------------
-- INSERT USER SP
---------------------------
CREATE OR ALTER PROCEDURE insertIntoUsers
    @userName VARCHAR(50),
    @passwordHash VARCHAR(MAX),
    @Email VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS(SELECT 1 FROM Users WHERE userName = @userName)
    BEGIN
        SELECT -1 AS Status;
        RETURN;
    END

    IF EXISTS(SELECT 1 FROM Users WHERE Email = @Email)
    BEGIN
        SELECT -2 AS Status;
        RETURN;
    END

    INSERT INTO Users (userName, passwordHash, Email)
    VALUES (@userName, @passwordHash, @Email);

    SELECT 1 AS Status;
END
GO


---------------------------
-- LOGIN SP (username OR email)
---------------------------
CREATE OR ALTER PROCEDURE login
    @loginInput VARCHAR(50),
    @passwordHash VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @userId INT;

    SELECT @userId = userId
    FROM Users
    WHERE (userName = @loginInput OR Email = @loginInput)
      AND passwordHash = @passwordHash;

    IF @userId IS NULL
    BEGIN
        SELECT -1 AS Status;
        RETURN;
    END

    SELECT @userId AS Status;
END
GO


---------------------------
-- FILES TABLE
---------------------------
CREATE TABLE files (
    mediaId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    userId INT NOT NULL,
    filePath VARCHAR(MAX) NOT NULL,
    contentType VARCHAR(50) NOT NULL,
    size BIGINT NOT NULL,
    uploadedAt DATETIME2 NOT NULL 
        CONSTRAINT DF_MediaFiles_UploadedAt DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_MediaFiles_Users FOREIGN KEY (userId)
        REFERENCES Users(userId)
);
GO


---------------------------
-- INSERT MEDIA SP
---------------------------
CREATE OR ALTER PROCEDURE insertMedia
    @userId INT,
    @filePath VARCHAR(MAX),
    @contentType VARCHAR(50),
    @size BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO files (userId, filePath, contentType, size)
    VALUES (@userId, @filePath, @contentType, @size);

    SELECT 1 AS Status;
END
GO
