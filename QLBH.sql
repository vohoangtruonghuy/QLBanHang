CREATE DATABASE [QuanLyBanHang];

GO

USE [QuanLyBanHang];

GO

CREATE TABLE Categories (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name nvarchar(50) NOT NULL
);

GO

CREATE TABLE Decentralization (
    PermissionId int PRIMARY KEY,
    NamePermission nvarchar(10)
);

GO

CREATE TABLE Users (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Username nvarchar(50) NOT NULL,
    Password nvarchar(255) NOT NULL,
    FullName nvarchar(50) NOT NULL,
    Address nvarchar(max),
    Email nvarchar(50) NOT NULL,
    DayOfBirth datetime NOT NULL,
    PermissionId int NOT NULL DEFAULT 0,
    FOREIGN KEY (PermissionId) REFERENCES Decentralization(PermissionId)
);

GO

CREATE TABLE Supplier (
    Id int IDENTITY(1,1) PRIMARY KEY,
    SupplierName nvarchar(max) NOT NULL
);

GO

CREATE TABLE Products (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name nvarchar(40) NOT NULL,
    TinyDes nvarchar(80) NOT NULL,
    FullDes nvarchar(max) NOT NULL,
    Price decimal NOT NULL,
    Image varbinary(MAX),
    CatID int NOT NULL,
    SupID int NOT NULL,
    Quantity int NOT NULL DEFAULT 0,
    FOREIGN KEY (CatID) REFERENCES Categories(Id),
    FOREIGN KEY (SupID) REFERENCES Supplier(Id)
);

GO

CREATE TABLE Orders (
    Id int IDENTITY(1,1) PRIMARY KEY,
    OrderDate datetime NOT NULL,
    Status int NOT NULL,
    Pay int NOT NULL,
    Address nvarchar(max),
    UserID int NOT NULL,
    Total decimal NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(Id)
);

GO

CREATE TABLE OrderDetails (
    ID int IDENTITY(1,1) PRIMARY KEY,
    OrderID int NOT NULL,
    ProID int NOT NULL,
    Quantity int NOT NULL,
    Price decimal NOT NULL,
    Amount decimal NOT NULL,
    PaymentMethods int NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES Orders(Id),
    FOREIGN KEY (ProID) REFERENCES Products(Id)
);

GO

CREATE TABLE Comments (
    Id int IDENTITY(1,1) PRIMARY KEY,
    UserId int NOT NULL,
    ProductId int NOT NULL,
    CommentText nvarchar(max) NOT NULL,
    CommentDate datetime NOT NULL,
    LastEditDate datetime NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

GO

INSERT INTO Users (Username, Password, FullName, Address, Email, DayOfBirth, PermissionId) 
VALUES (N'HuyMem', N'TruongHuy0403', N'Võ Hoàng Trường Huy', N'12 Lê Nin', N'votruonghuy0403@gmail.com', '2014-02-26 00:00:00.000', 0);

INSERT INTO Users (Username, Password, FullName, Address, Email, DayOfBirth, PermissionId) 
VALUES (N'HuyAdmin', N'TruongHuy2004', N'Võ Hoàng Trường Huy', N'12 Lê Nin', N'votruonghuy2004@gmail.com', '2014-03-19 00:00:00.000', 1);

INSERT INTO Decentralization (PermissionId, NamePermission) VALUES (0,'Member');
INSERT INTO Decentralization (PermissionId, NamePermission) VALUES (1,'Admin');