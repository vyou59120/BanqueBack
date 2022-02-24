CREATE TABLE "User"
(
UserId        integer NOT NULL GENERATED ALWAYS AS IDENTITY (
start 1
),
Nom           varchar(50) NOT NULL,
Prenom        varchar(50) NOT NULL,
Adresse       varchar(50) NOT NULL,
CP            varchar(50) NOT NULL,
Ville         varchar(50) NOT NULL,
Email         varchar(50) NOT NULL,
MotDePasse    varchar(50) NOT NULL,
Role          varchar(50) NOT NULL,
DateNaissance timestamp with time zone NOT NULL,
CONSTRAINT PK_Customer PRIMARY KEY ( UserId ),
CONSTRAINT AK1_Customer_CustomerName UNIQUE ( Nom )
);


CREATE TABLE "Agence"
(
AgenceId  integer NOT NULL GENERATED ALWAYS AS IDENTITY (
start 1
),
NomAgence varchar(40) NOT NULL,
Phone     varchar(20) NULL,
Ville     varchar(50) NOT NULL,
CONSTRAINT PK_Supplier PRIMARY KEY ( AgenceId ),
CONSTRAINT AK1_Supplier_CompanyName UNIQUE ( NomAgence )
);


CREATE TABLE "Account"
(
AccountId    integer NOT NULL GENERATED ALWAYS AS IDENTITY (
start 1
),
NumAccount   varchar(50) NULL,
AgenceId     integer NOT NULL,
UserId       integer NOT NULL,
DateCreation timestamp with time zone NOT NULL,
Solde        decimal(12,2) NOT NULL,
DateCloture  timestamp with time zone NOT NULL,
CONSTRAINT PK_Order PRIMARY KEY ( AccountId ),
CONSTRAINT AK1_Order_OrderNumber UNIQUE ( NumAccount ),
CONSTRAINT FK_142 FOREIGN KEY ( AgenceId ) REFERENCES "Agence" ( AgenceId ),
CONSTRAINT FK_Order_CustomerId_Customer FOREIGN KEY ( UserId ) REFERENCES "User" ( UserId )
);

CREATE INDEX FK_144 ON "Account"
(
AgenceId
);

CREATE INDEX FK_Order_CustomerId_Customer ON "Account"
(
UserId
);


CREATE TABLE "Transaction"
(
TransactionId    integer NOT NULL GENERATED ALWAYS AS IDENTITY (
start 1
),
AccountId   integer NOT NULL,
Date        timestamp with time zone NOT NULL,
Montant     decimal(18,2) NOT NULL,
Operation   varchar(50) NOT NULL,
description varchar(50) NOT NULL,
CONSTRAINT PK_OrderItem PRIMARY KEY ( TransactionId ),
CONSTRAINT FK_OrderItem_OrderId_Order FOREIGN KEY ( AccountId ) REFERENCES "Account" ( AccountId )
);

CREATE INDEX FK_OrderItem_OrderId_Order ON "Transaction"
(
AccountId
);



