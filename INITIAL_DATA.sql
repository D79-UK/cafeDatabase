IF OBJECT_ID('dbo.admins', 'U') IS NOT NULL 
	DROP TABLE [dbo].[admins];

CREATE TABLE [dbo].[admins] (
    [id]      INT  IDENTITY(1,1) PRIMARY KEY ,
    [name]    NVARCHAR (61) NOT NULL,
    [id_cafe] INT  NOT NULL
);

IF OBJECT_ID('dbo.assortiments_products', 'U') IS NOT NULL 
	DROP TABLE [dbo].[assortiments_products];

IF OBJECT_ID('dbo.stored_products', 'U') IS NOT NULL	
	DROP TABLE [dbo].[stored_products];

IF OBJECT_ID('dbo.supplied_products', 'U') IS NOT NULL 
	DROP TABLE [dbo].[supplied_products];

IF OBJECT_ID('dbo.supplies', 'U') IS NOT NULL
	DROP TABLE [dbo].[supplies];
	
IF OBJECT_ID('dbo.suppliers', 'U') IS NOT NULL
	DROP TABLE [dbo].[suppliers];

IF OBJECT_ID('dbo.available_assortiments', 'U') IS NOT NULL 
	DROP TABLE [dbo].[available_assortiments];

IF OBJECT_ID('dbo.products', 'U') IS NOT NULL 
	DROP TABLE [dbo].[products];

IF OBJECT_ID('dbo.assortiments', 'U') IS NOT NULL 
	DROP TABLE [dbo].[assortiments];

IF OBJECT_ID('dbo.garcons', 'U') IS NOT NULL 
	DROP TABLE [dbo].[garcons];

IF OBJECT_ID('dbo.cafes', 'U') IS NOT NULL 
	DROP TABLE [dbo].[cafes];

CREATE TABLE [dbo].[cafes] (
    [id]      INT  IDENTITY(1,1) PRIMARY KEY ,
    [title]    NVARCHAR (61) NOT NULL,
    [address]    NVARCHAR (61) NOT NULL,
    [lat] FLOAT NULL,
	[lon] FLOAT NULL    
);

CREATE TABLE [dbo].[products]
(
    [id] INT IDENTITY(1,1) PRIMARY KEY, 
    [title]  NVARCHAR (61) NOT NULL,
    [best_before] DATETIME NULL, 
    [id_supplier] INT NULL 
);

CREATE TABLE [dbo].[assortiments] 
(
    [id]    INT  IDENTITY(1,1) PRIMARY KEY ,
    [title] NVARCHAR (127) NULL,
    [price] FLOAT
);

CREATE TABLE [dbo].[assortiments_products] (
    [id]             INT IDENTITY(1,1) PRIMARY KEY,
    [id_assortiment] INT NULL,
    [id_product]     INT NULL,
    [quantity]       INT DEFAULT ((1)) NOT NULL,

CONSTRAINT fk_products
    FOREIGN KEY (id_product)
    REFERENCES products (id)
    ON DELETE CASCADE,
CONSTRAINT fk_assortiments
    FOREIGN KEY (id_assortiment)
    REFERENCES assortiments (id)
    ON DELETE CASCADE
);

CREATE TABLE [dbo].[available_assortiments] (
    [id]             INT IDENTITY(1,1) PRIMARY KEY,
    [id_assortiment] INT NULL,
    [id_cafe]        INT NULL,
    [quantity]       INT DEFAULT ((0)) NOT NULL,

	CONSTRAINT fk_available_assortiments
    FOREIGN KEY (id_assortiment)
    REFERENCES assortiments (id)
    ON DELETE CASCADE,

	CONSTRAINT fk_cafe
    FOREIGN KEY (id_cafe)
    REFERENCES cafes (id)
    ON DELETE CASCADE
);

DROP TABLE [dbo].[discounts];

CREATE TABLE [dbo].[discounts] (
    [id]      INT IDENTITY(1,1) PRIMARY KEY ,
    [rate]    INT NOT NULL,
    [owner_name] NVARCHAR(61) NULL,
    [received_at] DATETIME NULL,
);

CREATE TABLE [dbo].[garcons] (
    [id]      INT  IDENTITY(1,1) PRIMARY KEY ,
    [name]    NVARCHAR (61) NOT NULL,
    [id_cafe] INT  NOT NULL,

	CONSTRAINT fk_garcon_cafe
    FOREIGN KEY (id_cafe)
    REFERENCES cafes (id)
    ON DELETE CASCADE
);

DROP TABLE [dbo].[ordered_assortiments] ;
CREATE TABLE [dbo].[ordered_assortiments] (
    [id]             INT IDENTITY(1,1) PRIMARY KEY,
    [id_assortiment] INT NULL,
    [id_order]        INT NULL,
    [quantity]       INT DEFAULT ((0)) NOT NULL
);

DROP TABLE [dbo].[orders];
CREATE TABLE [dbo].[orders]
(
    [id] INT IDENTITY(1,1) PRIMARY KEY, 
    [amount] FLOAT NULL, 
    [received_at] DATETIME NULL, 
    [serviced_at] DATETIME NULL, 
    [id_garcon] INT NULL, 
    [id_discount] INT NULL	 
);

CREATE TABLE [dbo].[stored_products] (
    [id]             INT IDENTITY(1,1) PRIMARY KEY,
    [id_product] INT NULL,
    [id_cafe]        INT NULL,
    [quantity]       INT DEFAULT ((0)) NOT NULL,

	CONSTRAINT fk_stored_products
    FOREIGN KEY (id_product)
    REFERENCES products (id)
    ON DELETE CASCADE,

	CONSTRAINT fk_stored_cafe
    FOREIGN KEY (id_cafe)
    REFERENCES cafes (id)
    ON DELETE CASCADE
);
CREATE TABLE [dbo].[suppliers] (
    [id]      INT  IDENTITY(1,1) PRIMARY KEY ,
    [title]   NVARCHAR (61) NOT NULL,
    [address] NVARCHAR (61) NOT NULL,
	[lat] FLOAT NULL,
	[lon] FLOAT NULL
);

CREATE TABLE [dbo].[supplies] (
    [id]      INT IDENTITY(1,1) PRIMARY KEY ,    
	[id_supplier] INT NOT NULL,
    [supplied_at] DATETIME NULL,

	CONSTRAINT fk_s_supplier
    FOREIGN KEY (id_supplier)
    REFERENCES suppliers (id)
    ON DELETE CASCADE
);
CREATE TABLE [dbo].[supplied_products] (
    [id]             INT IDENTITY(1,1) PRIMARY KEY,
    [id_product] INT NULL,
	[id_supply] INT NULL,
    [id_cafe]        INT NULL,
    [quantity]       INT DEFAULT ((0)) NOT NULL,

	CONSTRAINT fk_supply
    FOREIGN KEY (id_supply)
    REFERENCES supplies(id)
    ON DELETE CASCADE,

	CONSTRAINT fk_s_product
    FOREIGN KEY (id_product)
    REFERENCES products(id)
    ON DELETE CASCADE
);

INSERT INTO cafes (title,address, lat, lon) VALUES (N'A', N'Хрещатик', 50.448696, 30.522992),
(N'B', N'Парк Тараса Шевченка біля Університету', 50.441901, 30.512896),
(N'C', N'Площа Льва Толстого', 50.439349, 30.516684);

/* https://www.latlong.net/
*/
INSERT INTO garcons (name,id_cafe) VALUES (N'Петро', 1),
(N'Олексій', 1),
(N'Назар', 1);
INSERT INTO garcons (name,id_cafe) VALUES (N'Антон', 2),
(N'Дмитро', 2),
(N'Ганна', 2);
INSERT INTO garcons (name,id_cafe) VALUES (N'Кирил', 3),
(N'Олеся', 3),
(N'Данило', 3);

INSERT INTO assortiments (title, price) VALUES (N'Круассан с лососем (180г) ', 160.0),
(N'Салат Греческий индивидуальный (350/50г)', 125),
(N'Шоколадный Браунис', 75),
(N'Маффин черника/шоколад', 50.0),
(N'Курица гриль двойной (380г)', 188.0),
(N'Моцарелла двойной (300г)', 183.0);

INSERT INTO suppliers (title,address, lat, lon) VALUES (N'Виробник A', N'Адреса', 51.448696, 30.522992),
(N'Виробник  B', N'Адреса', 49.441901, 30.512896),
(N'Виробник  C', N'Адреса', 50.439349, 31.516684);


INSERT INTO available_assortiments (id_assortiment, id_cafe, quantity) VALUES 
(1, 1, 20),
(2, 1, 20),
(3, 1, 20),
(4, 1, 20),
(5, 1, 20),
(6, 1, 20);

SELECT a.title, a.id, a.price FROM available_assortiments aa, assortiments a WHERE aa.id_assortiment = a.id AND aa.id_cafe = (SELECT id_cafe FROM garcons WHERE id = 2);
 