ALTER TABLE dbo.Athletes
ADD CONSTRAINT fk_Countries FOREIGN KEY (CountryId) REFERENCES dbo.Countries(Id);


CREATE TABLE AthletesSportsJunction
(
	AthleteId int,
	SportId int,
	CONSTRAINT PK_Athletes_Sports PRIMARY KEY (AthleteId, SportId),
	CONSTRAINT FK_Athletes
		FOREIGN KEY (AthleteId) REFERENCES dbo.Athletes (Id),
	CONSTRAINT FK_Sports
		FOREIGN KEY (SportId) REFERENCES dbo.Sports (Id)
);


INSERT INTO dbo.AthletesSportsJunction
VALUES (1, 1);

SELECT * FROM dbo.AthletesSportsJunction;


/* Karolio variantas */

CREATE TABLE Countries (
id int IDENTITY(1,1) PRIMARY KEY,
CountryName varchar(255),
UNDP varchar(255)
);

CREATE TABLE Sports (
id int IDENTITY(1,1) PRIMARY KEY,
SportName varchar(255),
TeamActivity bit
);

