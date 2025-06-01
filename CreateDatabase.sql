CREATE DATABASE AIMS;
GO
USE AIMS;
GO

-- Table: [User]
CREATE TABLE [User] (
	user_id 				INT 			NOT NULL IDENTITY(1,1),
	username				VARCHAR(50)		NOT NULL,
	password				VARCHAR(50)		NOT NULL,
	email					VARCHAR(50)		NOT NULL,
	isAdmin					BIT				NOT NULL,
	PRIMARY KEY (user_id)
);

-- Table: Media
CREATE TABLE Media (
	media_id				INT				NOT NULL IDENTITY(1,1),
	title					VARCHAR(50)		NOT NULL,
	price					INT				NOT NULL,
	totalQuantity			INT				NOT NULL,
	weight					FLOAT			NOT NULL,
	rushOrderSupported		BIT,
	imageUrl				VARCHAR(200),

	barcode					VARCHAR(50)		NOT NULL,
	description				VARCHAR(255)	NOT NULL,
	productDimension		VARCHAR(50),
	importDate				DATE,

	PRIMARY KEY (media_id)
);

-- Table: Book
CREATE TABLE Book (
	media_id				INT				NOT NULL,
	authors					VARCHAR(50) 	NOT NULL,
	coverType				VARCHAR(50) 	NOT NULL,
	publisher 				VARCHAR(50) 	NOT NULL,
	publicationDate			DATE 			NOT NULL,

	pages					INT,
	language				VARCHAR(50),
	genre					VARCHAR(50),
	PRIMARY KEY (media_id),
	FOREIGN KEY (media_id) REFERENCES Media (media_id) ON DELETE CASCADE
);

-- Table: CD_and_LP
CREATE TABLE CD_and_LP (
	media_id				INT				NOT NULL,
	isCD					BIT				NOT NULL,
	artists					VARCHAR(50) 	NOT NULL,
	recordLabel 			VARCHAR(50) 	NOT NULL,
	trackList				VARCHAR(200)	NOT NULL,
	genre					VARCHAR(50)		NOT NULL,

	releaseDate				DATE,
	PRIMARY KEY (media_id),
	FOREIGN KEY (media_id) REFERENCES Media (media_id) ON DELETE CASCADE
);

-- Table: DVD
CREATE TABLE DVD (
	media_id				INT				NOT NULL,
	dvdType					VARCHAR(50)		NOT NULL,
	director				VARCHAR(50) 	NOT NULL,
	runtime		 			INT		 		NOT NULL,
	studio					VARCHAR(50)		NOT NULL,
	language				VARCHAR(50) 	NOT NULL,
	subtitles				VARCHAR(50) 	NOT NULL,

	releasedDate			DATE,
	genre					VARCHAR(50),
	PRIMARY KEY (media_id),
	FOREIGN KEY (media_id) REFERENCES Media (media_id) ON DELETE CASCADE
);

-- Table: DeliveryInfo
CREATE TABLE DeliveryInfo (
	delivery_id				INT 			NOT NULL IDENTITY(1,1),
	name 					VARCHAR(50)		NOT NULL,
	phone					VARCHAR(15)		NOT NULL,
	email					VARCHAR(50)		NOT NULL,
	province				VARCHAR(50)		NOT NULL,
	address					VARCHAR(200)	NOT NULL,
	message					VARCHAR(200),
	PRIMARY KEY (delivery_id)
);

-- Table: OrderInfo
CREATE TABLE OrderInfo(
	order_id 				INT 			NOT NULL IDENTITY(1,1),
	shippingFees			INT				NOT NULL,
	subtotal				INT				NOT NULL,
	status					VARCHAR(20)		NOT NULL,	-- 1 pending, 2 success?

	delivery_id				INT				NOT NULL,
	PRIMARY KEY (order_id),
	FOREIGN KEY (delivery_id) REFERENCES DeliveryInfo(delivery_id) ON DELETE CASCADE
);

-- Table: Order_Media
CREATE TABLE Order_Media (
	media_id				INT				NOT NULL,
	order_id				INT				NOT NULL,
	quantity				INT				NOT NULL,
	orderType				INT				NOT NULL,
	PRIMARY KEY (media_id, order_id),
	FOREIGN KEY (media_id) REFERENCES Media(media_id) ON DELETE CASCADE,
	FOREIGN KEY (order_id) REFERENCES OrderInfo(order_id) ON DELETE CASCADE
);

-- Table: RushOrderInfo
CREATE TABLE RushOrderInfo (
	rush_id 				INT 			NOT NULL IDENTITY(1,1),
	deliveryTime			DATETIME		NOT NULL,
	instruction				VARCHAR(200)	NOT NULL,
	order_id				INT				NOT NULL,
	PRIMARY KEY (rush_id),
	FOREIGN KEY (order_id) REFERENCES OrderInfo(order_id) ON DELETE CASCADE
);

-- Table: PaymentTransaction
CREATE TABLE PaymentTransaction(
	transaction_id 			INT 			NOT NULL IDENTITY(1,1),
	paymentTime				DATETIME		NOT NULL,
	paymentAmount			INT				NOT NULL,
	content					VARCHAR(500)	NOT NULL,

	bankTransactionId		VARCHAR(50)		NOT NULL,
	cardType				VARCHAR(50)		NOT NULL,
	PRIMARY KEY (transaction_id)
);

-- Table: Invoice
CREATE TABLE Invoice(
	invoice_id 				INT 			NOT NULL IDENTITY(1,1),
	totalAmount				INT				NOT NULL,
	transaction_id			INT 			NOT NULL,
	order_id				INT				NOT NULL,
	PRIMARY KEY (invoice_id),
	FOREIGN KEY (transaction_id) REFERENCES PaymentTransaction(transaction_id),
	FOREIGN KEY (order_id) REFERENCES OrderInfo(order_id) ON DELETE CASCADE
);

-- Indexes
CREATE INDEX delivery_id_index ON OrderInfo (delivery_id);
CREATE INDEX invoice_transaction_id_index ON Invoice (transaction_id);
CREATE INDEX invoice_order_id_index ON Invoice (order_id);
CREATE INDEX rush_order_id_index ON RushOrderInfo (order_id);
