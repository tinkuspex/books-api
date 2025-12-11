

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'LibraryDb')
BEGIN
    CREATE DATABASE LibraryDb;
END
GO

USE LibraryDb;
GO


IF OBJECT_ID('dbo.Books', 'U') IS NOT NULL
    DROP TABLE dbo.Books;
GO

CREATE TABLE dbo.Books (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(150) NOT NULL,
    PublicationYear INT NOT NULL,
    Publisher NVARCHAR(150) NULL,
    PageCount INT NOT NULL,
    Category NVARCHAR(100) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO


CREATE INDEX IX_Books_Title ON dbo.Books (Title);

-- Búsquedas por autor
CREATE INDEX IX_Books_Author ON dbo.Books (Author);

-- Búsqueda por categoría
CREATE INDEX IX_Books_Category ON dbo.Books (Category);
GO


INSERT INTO dbo.Books (Title, Author, PublicationYear, Publisher, PageCount, Category)
VALUES
('Clean Code', 'Robert C. Martin', 2008, 'Prentice Hall', 464, 'Software Development'),
('The Pragmatic Programmer', 'Andrew Hunt, David Thomas', 1999, 'Addison-Wesley', 352, 'Software Development'),
('Design Patterns', 'Erich Gamma et al.', 1994, 'Addison-Wesley', 395, 'Software Architecture'),
('Refactoring', 'Martin Fowler', 1999, 'Addison-Wesley', 448, 'Software Development');
GO

CREATE OR ALTER PROCEDURE dbo.SpBooks_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Id,
        Title,
        Author,
        PublicationYear,
        Publisher,
        PageCount,
        Category,
        CreatedAt
    FROM dbo.Books;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SpBooks_GetById
(
    @Id INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Id,
        Title,
        Author,
        PublicationYear,
        Publisher,
        PageCount,
        Category,
        CreatedAt
    FROM dbo.Books
    WHERE Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SpBooks_Create
(
    @Title NVARCHAR(200),
    @Author NVARCHAR(150),
    @PublicationYear INT,
    @Publisher NVARCHAR(150),
    @PageCount INT,
    @Category NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Books 
    (
        Title, Author, PublicationYear, Publisher, PageCount, Category
    )
    VALUES 
    (
        @Title, @Author, @PublicationYear, @Publisher, @PageCount, @Category
    );

    SELECT SCOPE_IDENTITY() AS NewId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SpBooks_Update
(
    @Id INT,
    @Title NVARCHAR(200),
    @Author NVARCHAR(150),
    @PublicationYear INT,
    @Publisher NVARCHAR(150),
    @PageCount INT,
    @Category NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Books
    SET 
        Title = @Title,
        Author = @Author,
        PublicationYear = @PublicationYear,
        Publisher = @Publisher,
        PageCount = @PageCount,
        Category = @Category
    WHERE Id = @Id;

    SELECT @@ROWCOUNT AS AffectedRows;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SpBooks_Delete
(
    @Id INT
)
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.Books WHERE Id = @Id;

    SELECT @@ROWCOUNT AS AffectedRows;
END;
GO
