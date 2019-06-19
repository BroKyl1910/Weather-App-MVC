use master;

create database WeatherForecastApp;

go

use WeatherForecastApp;

CREATE TABLE DB_User(
	Username varchar(50) not null primary key,
	Password varchar(100) not null,
	UserType int not null
);

CREATE TABLE DB_Favourite(
	FavouriteID int identity(1,1) not null primary key,
	Username varchar(50),
	CityID int not null,
	Foreign key (Username) REFERENCES [DB_User](Username)
);

CREATE TABLE DB_Forecast(
	ForecastID int identity(1,1) primary key not null,
	CityID int not null,
	ForecastDate Date not null,
	MinTemp int not null,
	MaxTemp int not null,
	WindSpeed int not null,
	Humidity int not null,
	Precipitation int not null
);

INSERT INTO [DB_User] VALUES ('brookies1910','27526cb7fbaee25acdea4e873e1403a9',1);
INSERT INTO [DB_User] VALUES ('cyndi','27526cb7fbaee25acdea4e873e1403a9',0);
INSERT INTO [DB_User] VALUES ('kyle','27526cb7fbaee25acdea4e873e1403a9',1);
INSERT INTO [DB_User] VALUES ('user','27526cb7fbaee25acdea4e873e1403a9',0);
INSERT INTO [DB_User] VALUES ('brookies191','27526cb7fbaee25acdea4e873e1403a9',1);
INSERT INTO [DB_User] VALUES ('jd','2ac9cb7dc02b3c0083eb70898e549b63',1);
INSERT INTO [DB_Favourite] VALUES ('cyndi',1007311);
INSERT INTO [DB_Favourite] VALUES ('kyle',3369157);
INSERT INTO [DB_Favourite] VALUES ('brookies1910',1007311);
INSERT INTO [DB_Favourite] VALUES ('brookies1910',360630);
INSERT INTO [DB_Favourite] VALUES ('brookies1910',1850147);
INSERT INTO [DB_Favourite] VALUES ('brookies1910',3182318);
INSERT INTO [DB_Favourite] VALUES ('jd',3369157);
INSERT INTO [DB_Favourite] VALUES ('jd',1007311);
INSERT INTO [DB_Favourite] VALUES ('jd',1796236);
INSERT INTO [DB_Forecast] VALUES (1007311,'2019/04/05',18,22,4,77,0);
INSERT INTO [DB_Forecast] VALUES (1850147,'2019/04/05',12,61,37,74,17);
INSERT INTO [DB_Forecast] VALUES (519188,'2019/04/05',19,22,5,82,0);
INSERT INTO [DB_Forecast] VALUES (1819729,'2019/04/05',22,26,4,73,0);
INSERT INTO [DB_Forecast] VALUES (1862845,'2019/04/05',8,14,5,54,0);
INSERT INTO [DB_Forecast] VALUES (1862845,'2019/04/06',8,14,5,54,0);
INSERT INTO [DB_Forecast] VALUES (2643743,'2019/04/05',9,11,6,57,0);
INSERT INTO [DB_Forecast] VALUES (1850147,'2019/04/07',8,16,6,47,31);
INSERT INTO [DB_Forecast] VALUES (2130135,'2019/04/07',3,5,1,70,0);
INSERT INTO [DB_Forecast] VALUES (964420,'2019/04/07',23,23,9,69,0);
INSERT INTO [DB_Forecast] VALUES (964420,'2019/04/06',23,23,9,69,0);
INSERT INTO [DB_Forecast] VALUES (1007311,'2019/04/07',19,22,2,78,24);
INSERT INTO [DB_Forecast] VALUES (1007311,'2019/04/08',22,24,2,78,0);
INSERT INTO [DB_Forecast] VALUES (6185811,'2019/04/08',-4,-4,2,76,0);
INSERT INTO [DB_Forecast] VALUES (6185811,'2019/04/09',3,3,3,48,0);
INSERT INTO [DB_Forecast] VALUES (1007311,'2019/04/09',20,23,2,83,0);
INSERT INTO [DB_Forecast] VALUES (1007311,'2019/05/11',22,26,4,64,82);
INSERT INTO [DB_Forecast] VALUES (1007311,'2019/05/12',22,26,8,73,0);
INSERT INTO [DB_Forecast] VALUES (3182318,'2019/05/13',11,16,4,71,2);
INSERT INTO [DB_Forecast] VALUES (1007311,'2019/05/13',24,26,7,64,0);
INSERT INTO [DB_Forecast] VALUES (1796231,'2019/05/13',16,22,6,82,6);

SELECT * FROM [DB_User];
SELECT * FROM [DB_Favourite];
SELECT * FROM [DB_Forecast];