CREATE DATABASE RadioheadSales;
GO

USE RadioheadSales;
GO

CREATE TABLE Albums (
    Id INT PRIMARY KEY IDENTITY(1,1),
    AlbumName NVARCHAR(100) NOT NULL,
    Stock INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL
);
GO

INSERT INTO Albums (AlbumName, Stock, Price) VALUES ('OK Computer', 100, 9.99);
INSERT INTO Albums (AlbumName, Stock, Price) VALUES ('Kid A', 100, 12.99);
INSERT INTO Albums (AlbumName, Stock, Price) VALUES ('In Rainbows', 100, 14.99);
GO

CREATE TABLE AlbumSales (
    Id INT PRIMARY KEY IDENTITY(1,1),
    AlbumName NVARCHAR(100) NOT NULL,
    Sales INT NOT NULL,
    SaleDate DATETIME NOT NULL DEFAULT GETDATE()
);
GO

INSERT INTO AlbumSales (AlbumName, Sales) VALUES ('OK Computer', 5000);
INSERT INTO AlbumSales (AlbumName, Sales) VALUES ('Kid A', 3000);
INSERT INTO AlbumSales (AlbumName, Sales) VALUES ('In Rainbows', 7000);
GO