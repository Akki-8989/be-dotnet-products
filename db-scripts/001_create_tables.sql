-- Products Service Tables
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products')
BEGIN
    CREATE TABLE Products (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        Price DECIMAL(18,2) NOT NULL,
        Category NVARCHAR(100),
        CreatedAt DATETIME2 DEFAULT GETDATE()
    );

    INSERT INTO Products (Name, Price, Category) VALUES
    ('Laptop', 79999.00, 'Electronics'),
    ('Headphones', 2999.00, 'Electronics'),
    ('Keyboard', 1499.00, 'Accessories'),
    ('Mouse', 899.00, 'Accessories');
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Categories')
BEGIN
    CREATE TABLE Categories (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL
    );

    INSERT INTO Categories (Name) VALUES ('Electronics'), ('Accessories');
END
GO
