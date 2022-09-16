USE ECommerceSellingClothes
GO
CREATE TABLE Account (
	Id int IDENTITY,
	ShopId int,
	UserName nvarchar(255),
	Password nvarchar(255),
	Name nvarchar(255),
	Address nvarchar(255),
	PhoneNumber nvarchar(10),
	DateCreated datetime,
	UserGroupId int,
	CONSTRAINT PK_Account PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
GO
CREATE TABLE Ordered(
	Id int IDENTITY,
	AccountId int,
	StatusId int,
	PaymentId int,
	DateCreate datetime,
	PhoneNumber nvarchar(10),
	Address nvarchar(255),
	CONSTRAINT PK_Ordered PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
GO
CREATE TABLE Payment (
	Id int PRIMARY KEY, 
	Type nvarchar(255),
);

CREATE TABLE Shop (
	Id int IDENTITY,
	Name nvarchar(255), 
	Address nvarchar(255),
	PhomeNumber nvarchar(10),
	DateCreated datetime,
	CONSTRAINT PK_Shop PRIMARY KEY CLUSTERED (Id),
);

CREATE TABLE Category (
	Id int IDENTITY,
	ParentId int,
	ShopId int,
	Name nvarchar(255),
	Description nvarchar(255),
	Gender char(1),
	CONSTRAINT PK_Category PRIMARY KEY CLUSTERED (Id),
)
ON [PRIMARY]
GO

CREATE TABLE OrderDetail(
	Id int IDENTITY,
	OrderId int,
	ItemId int,
	Quantity int,
	CONSTRAINT PK_OrderDetail PRIMARY KEY CLUSTERED (Id),
)
ON [PRIMARY]
GO
CREATE TABLE Item(
	Id int IDENTITY,
	CategoryId int,
	ShopId int,
	Name nvarchar(255),
	Price int,
	DateCreated datetime,
	Description nvarchar(255),
	Size nvarchar(5),
	Quantity int, 
	CONSTRAINT PK_Item PRIMARY KEY CLUSTERED (Id),
)
ON [PRIMARY]
GO

CREATE TABLE Image(
	Id int IDENTITY,
	ItemId int,
	ShopId int,
	Path nvarchar(255),
	CONSTRAINT PK_Image PRIMARY KEY CLUSTERED (Id),
);

CREATE TABLE UserGroup(
	Id int IDENTITY,
	Name nvarchar(255),
	CONSTRAINT PK_UserGroup PRIMARY KEY CLUSTERED (Id),
)
ON [PRIMARY]
GO

CREATE TABLE Role (
	Id int IDENTITY,
	Name nvarchar(255),
	CONSTRAINT PK_Role PRIMARY KEY CLUSTERED (Id),
)
ON [PRIMARY]
GO

CREATE TABLE Cendential (
	RoleId int,
	UserGroupId int,
);

CREATE TABLE Status (
	Id int PRIMARY KEY,
	Name nvarchar(255),
);

ALTER TABLE Ordered
ADD CONSTRAINT FK_Ordered_Payment FOREIGN KEY (PaymentId)
REFERENCES Payment(Id);

ALTER TABLE Account
ADD CONSTRAINT FK_Account_UserGroup FOREIGN KEY (UserGroupId)
REFERENCES UserGroup(Id);

ALTER TABLE Account
ADD CONSTRAINT FK_Account_Shop FOREIGN KEY (ShopId)
REFERENCES Shop(Id);

ALTER TABLE Payment
ADD CONSTRAINT FK_Payment_Shop FOREIGN KEY (Id)
REFERENCES Shop(Id);

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
  ADD FOREIGN KEY (CategoryId) REFERENCES Category(Id)
GO

ALTER TABLE Category WITH NOCHECK
  ADD FOREIGN KEY (ShopId) REFERENCES Shop(Id)
GO

ALTER TABLE Image WITH NOCHECK
  ADD FOREIGN KEY (ItemId) REFERENCES Item(Id)
GO

ALTER TABLE Image WITH NOCHECK
  ADD FOREIGN KEY (ShopId) REFERENCES Shop(Id)
GO