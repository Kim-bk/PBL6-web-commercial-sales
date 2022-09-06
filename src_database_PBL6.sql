Create database PBL6
go
USE PBL6
GO
CREATE TABLE Account (
	Id int NOT NULL IDENTITY,
	ShopId int NOT NULL,
	Username varchar(255) NOT NULL,
	Password varchar(255) NOT NULL,
	Name varchar(255) NOT NULL,
	Address varchar(255) NOT NULL,
	PhoneNumber varchar(10) NOT NULL,
	DateCreated datetime NOT NULL,
	UserGroupId int NOT NULL,
	CONSTRAINT PK_Account PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
GO
CREATE TABLE Ordered(
	Id int NOT NULL IDENTITY,
	AccountId int NOT NULL,
	StatusId int NOT NULL,
	PaymentId int NOT NULL,
	DateCreate datetime NOT NULL,
	PhoneNumber varchar(10),
	Address varchar(255),
	CONSTRAINT PK_Ordered PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
GO
CREATE TABLE Payment (
	Id int NOT NULL PRIMARY KEY, 
	Type varchar(255),
);

CREATE TABLE Shop (
	ShopId int NOT NULL IDENTITY,
	Name varchar(255) NOT NULL, 
	Address varchar(255) NOT NULL,
	PhomeNumber varchar(10) NOT NULL,
	DateCreated datetime NOT NULL,
	CONSTRAINT PK_Shop PRIMARY KEY CLUSTERED (ShopId),
);

CREATE TABLE Category (
	CategoryId int NOT NULL IDENTITY,
	ParentId int NOT NULL,
	ShopId int NOT NULL,
	Name varchar(255),
	Decription varchar(255),
	CONSTRAINT PK_Category PRIMARY KEY CLUSTERED (CategoryId),
)
ON [PRIMARY]
GO

CREATE TABLE OrderDetail(
	Id int NOT NULL IDENTITY,
	OrderId int NOT NULL,
	ItemId int NOT NULL,
	Quantity int,
	CONSTRAINT PK_OrderDetail PRIMARY KEY CLUSTERED (Id),
)
ON [PRIMARY]
GO
CREATE TABLE Item(
	Id int NOT NULL IDENTITY,
	CategoryId int NOT NULL,
	ShopId int NOT NULL,
	Name varchar(255) NOT NULL,
	Price int NOT NULL,
	DateCreated datetime,
	Decription varchar(255) NOT NULL,
	Size varchar(5) NOT NULL,
	Quantity int NOT NULL, 
	CONSTRAINT PK_Item PRIMARY KEY CLUSTERED (Id),
)
ON [PRIMARY]
GO

CREATE TABLE Image(
	Id int NOT NULL IDENTITY,
	ItemId int NOT NULL,
	ShopId int NOT NULL,
	Path varchar(255),
	CONSTRAINT PK_Image PRIMARY KEY CLUSTERED (Id),
);

CREATE TABLE UserGroup(
	Id int NOT NULL IDENTITY,
	Name varchar(255) NOT NULL,
	CONSTRAINT PK_UserGroup PRIMARY KEY CLUSTERED (Id),
)
ON [PRIMARY]
GO

CREATE TABLE Role (
	Id int NOT NULL IDENTITY,
	Name varchar(255),
	CONSTRAINT PK_Role PRIMARY KEY CLUSTERED (Id),
)
ON [PRIMARY]
GO

CREATE TABLE Cendential (
	RoleId int NOT NULL,
	UserGroupId int NOT NULL,
);

CREATE TABLE Status (
	Id int NOT NULL PRIMARY KEY,
	Name varchar(255),
);

ALTER TABLE Ordered
ADD CONSTRAINT FK_Ordered_Payment FOREIGN KEY (PaymentId)
REFERENCES Payment(Id);

ALTER TABLE Account
ADD CONSTRAINT FK_Account_UserGroup FOREIGN KEY (UserGroupId)
REFERENCES UserGroup(Id);

ALTER TABLE Account
ADD CONSTRAINT FK_Account_Shop FOREIGN KEY (ShopId)
REFERENCES Shop(ShopId);

ALTER TABLE Payment
ADD CONSTRAINT FK_Payment_Shop FOREIGN KEY (Id)
REFERENCES Shop(ShopId);

ALTER TABLE Ordered
ADD CONSTRAINT FK_Ordered_Status FOREIGN KEY (StatusId)
REFERENCES Status(Id);

ALTER TABLE OrderDetail
ADD CONSTRAINT FK_OrderDetail_Item FOREIGN KEY (ItemId)
REFERENCES Item(Id);

ALTER TABLE OrderDetail WITH NOCHECK
  ADD FOREIGN KEY (OrderId) REFERENCES Ordered(Id)
GO

ALTER TABLE Ordered WITH NOCHECK
  ADD FOREIGN KEY (AccountId) REFERENCES Account(Id)
GO

ALTER TABLE Cendential WITH NOCHECK
  ADD FOREIGN KEY (RoleId) REFERENCES Role(Id)
GO

ALTER TABLE Cendential WITH NOCHECK
  ADD FOREIGN KEY (UserGroupId) REFERENCES UserGroup(Id)
GO

ALTER TABLE Item WITH NOCHECK
  ADD FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId)
GO

ALTER TABLE Category WITH NOCHECK
  ADD FOREIGN KEY (CategoryId) REFERENCES Shop(ShopId)
GO

ALTER TABLE Image WITH NOCHECK
  ADD FOREIGN KEY (ItemId) REFERENCES Item(Id)
GO

ALTER TABLE Image WITH NOCHECK
  ADD FOREIGN KEY (ShopId) REFERENCES Shop(ShopId)
GO