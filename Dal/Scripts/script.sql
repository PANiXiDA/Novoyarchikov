create database NetCoreTemplateDb
alter database NetCoreTemplateDb set recovery simple
go

use NetCoreTemplateDb
go

-- Компания
CREATE TABLE Companies (
  [CompanyID] integer IDENTITY(1,1) NOT NULL constraint PK_companies PRIMARY KEY,
  [CompanyName]  nvarchar(255) NULL,
  [Owner] nvarchar(255) NULL
)
GO

-- Автобусы
CREATE TABLE Buses (
  [BusID] integer IDENTITY(1,1) NOT NULL constraint PK_buses PRIMARY KEY,
  [BusNumber] integer NULL,
  [CompanyID] integer NOT NULL constraint companies_buses FOREIGN KEY REFERENCES Companies ([CompanyID]),
  [NumberOfSeats] integer NULL,
  [Model] nvarchar(255) NULL,
  [ReleaseDate] date NULL
)
GO

-- Остановки
CREATE TABLE Stops (
  [StopID] integer IDENTITY(1,1) NOT NULL constraint PK_stops PRIMARY KEY,
  [StopName] nvarchar(255) NULL
)
GO

-- Маршрут
CREATE TABLE Routes (
  [RouteID] integer IDENTITY(1,1) NOT NULL constraint PK_routes PRIMARY KEY,
  [CompanyID] integer NOT NULL constraint companies_routes FOREIGN KEY REFERENCES Companies ([CompanyID]),
  [RouteName] nvarchar(255) NULL,
  [RouteDuration] integer NULL
)
GO

-- Расписание
CREATE TABLE Schedule (
  [ScheduleID] integer IDENTITY(1,1) NOT NULL constraint PK_schedule PRIMARY KEY,
  [RouteID] integer NOT NULL constraint routes_schedule FOREIGN KEY REFERENCES Routes ([RouteID]),
  [StopID] integer NOT NULL constraint stops_schedule FOREIGN KEY REFERENCES Stops ([StopID]),
  [DepartureTime] time NULL,
  [ArrivalTime] time NULL
)
GO

-- Водитель
CREATE TABLE Driver (
  [DriverID] integer IDENTITY(1,1) NOT NULL constraint PK_driver PRIMARY KEY,
  [CompanyID] integer NOT NULL constraint companies_driver FOREIGN KEY REFERENCES Companies ([CompanyID]),
  [DriverName] nvarchar(255) NULL,
  [Experience] integer NULL,
  [NumberOfAccidents] integer NULL
)
GO

-- Маршрутный лист
CREATE TABLE RouteList (
  [SheetID] integer IDENTITY(1,1) NOT NULL constraint PK_routeList PRIMARY KEY,
  [BusID] integer NOT NULL constraint buses_route FOREIGN KEY REFERENCES Buses ([BusID]),
  [DriverID] integer NOT NULL constraint drivers_routeList FOREIGN KEY REFERENCES Driver ([DriverID]),
  [RouteID] integer NOT NULL constraint routes_routeList FOREIGN KEY REFERENCES Routes ([RouteID]),
  [DataRoute] datetime NULL
)
GO

-- Ремонт
CREATE TABLE RepairOrder (
  [OrderID] integer IDENTITY(1,1) NOT NULL constraint PK_repairOrder PRIMARY KEY,
  [BusID] integer NOT NULL constraint buses_repairOrder FOREIGN KEY REFERENCES Buses ([BusID]),
  [RepairPrice] integer NULL,
  [RepairDate] date NULL
)
GO

-- Пользователь
CREATE TABLE Users
(
	Id INT IDENTITY (1,1) PRIMARY KEY,
    IsBlocked BIT NOT NULL,
    Login NVARCHAR(50) NOT NULL,
    Password NVARCHAR(250),
    RegistrationDate DATETIME,
    Role INT NOT NULL
)

-- Процедуры

------------------------------------Не принимающая, выводящая таблицу, не возвращающая значение
CREATE PROCEDURE ShowCompanyBuses
    @CompanyID int
AS
BEGIN
    SELECT 
		Companies.CompanyName AS CompanyName,
		Companies.Owner AS CompanyOwner,
		Buses.BusNumber AS BusNumber,
		Buses.Model AS BusModel,
		Buses.NumberOfSeats AS BusNumberOfSeats,
		Buses.ReleaseDate AS BusReleaseDate,
		Driver.DriverName AS DriverName,
		Driver.Experience AS DriverExperience,
		Driver.NumberOfAccidents AS DriverNumberOfAccidents,
		RepairOrder.RepairDate AS RepairDate
    FROM 
		Companies
	INNER JOIN 
		Buses ON Companies.CompanyID = Buses.CompanyID
	INNER JOIN 
		Driver ON Companies.CompanyID = Driver.CompanyID
	INNER JOIN 
		RepairOrder ON Buses.BusID = RepairOrder.BusID
    WHERE 
		Companies.CompanyID = @CompanyID
END

DROP PROCEDURE ShowCompanyBuses

EXEC ShowCompanyBuses @CompanyID = 1

-----------------------------------Принимающая, выводящая таблицу, возвращающая значение
CREATE PROCEDURE SelectBusForRepair
   @busModel NVARCHAR(255),
   @repairDate DATE,
   @repairCount INT OUTPUT
AS
BEGIN
   DECLARE @totalRepairPrice INT;

   SELECT @totalRepairPrice = SUM(RepairPrice)
   FROM RepairOrder
   WHERE BusID IN (SELECT BusID FROM Buses WHERE Model = @busModel)
   AND RepairDate = @repairDate;

   SELECT *
   FROM Buses
   WHERE Model = @busModel
   AND NOT EXISTS (
       SELECT 1
       FROM RepairOrder
       WHERE RepairOrder.BusID = Buses.BusID
       AND RepairDate = @repairDate
   );

   SET @repairCount = @@ROWCOUNT;
END;

DROP PROCEDURE SelectBusForRepair

DECLARE @repairCount INT;
EXEC SelectBusForRepair @busModel = 'Ford', @repairDate = '2024-04-29', @repairCount = @repairCount OUTPUT;
PRINT 'Number of suitable buses for repair: ' + CONVERT(NVARCHAR, @repairCount);

-----------------------------------Принимающая, возвращающая значение
CREATE PROCEDURE CalculateRepairExpensesForPeriod
  @start_date DATE, 
  @end_date DATE
AS
BEGIN
  DECLARE @TotalRepairCost INT;
  SELECT @TotalRepairCost = SUM(RepairPrice)
  FROM Buses
  INNER JOIN RepairOrder ON Buses.BusID = RepairOrder.BusID
  WHERE RepairOrder.RepairDate BETWEEN @start_date AND @end_date;
  RETURN @TotalRepairCost;
END;

DROP PROCEDURE CalculateRepairExpensesForPeriod

DECLARE @TotalRepairCost INT;
EXEC @TotalRepairCost = CalculateRepairExpensesForPeriod '2023-01-01', '2023-12-31';
PRINT 'Общие расходы на ремонт за период: '  + CONVERT(NVARCHAR, @TotalRepairCost);

------------------------------------Обновляющая
CREATE PROCEDURE CalculateRouteListTotalCost
  @sheetId INT
AS
BEGIN
  DECLARE @totalCost DECIMAL(10, 2)	
  DECLARE @repairPrice DECIMAL(10, 2)
  SELECT @repairPrice = RepairPrice
  FROM RepairOrder
  WHERE BusID = (SELECT BusID FROM RouteList WHERE SheetID = @sheetId)
  SELECT @totalCost = @repairPrice
  UPDATE RepairOrder
  SET  repairPrice = @totalCost + (SELECT RouteDuration FROM Routes WHERE RouteID = (SELECT RouteID FROM RouteList WHERE SheetID = @sheetId))
  WHERE BusID = (SELECT BusID FROM RouteList WHERE SheetID = @sheetId)
END

DROP PROCEDURE CalculateRouteListTotalCost

EXEC CalculateRouteListTotalCost @sheetId = 2
SELECT * FROM RouteList;
SELECT * FROM RepairOrder;

---------------------------Курсор
CREATE PROCEDURE GetAllCompanies
AS
BEGIN
   SET NOCOUNT ON;

   DECLARE @CompanyName NVARCHAR(255);
   DECLARE @Owner NVARCHAR(255);
   DECLARE @BusNumber INT;
   DECLARE @NumberOfSeats INT;
   DECLARE @Model NVARCHAR(255);
   DECLARE @ReleaseDate DATE;

   DECLARE CompaniesCursor CURSOR FOR 
   SELECT CompanyName, Owner
   FROM Companies;

   OPEN CompaniesCursor;

   FETCH NEXT FROM CompaniesCursor INTO @CompanyName, @Owner;

   WHILE @@FETCH_STATUS = 0
   BEGIN
       PRINT 'Company Name: ' + @CompanyName;
       PRINT 'Owner: ' + @Owner;

       DECLARE @CompanyID INT;
       SET @CompanyID = (SELECT TOP 1 CompanyID FROM Companies WHERE CompanyName = @CompanyName); 

       DECLARE BusesCursor CURSOR FOR 
       SELECT BusNumber, NumberOfSeats, Model, ReleaseDate
       FROM Buses
       WHERE CompanyID = @CompanyID;

       OPEN BusesCursor;

       FETCH NEXT FROM BusesCursor INTO @BusNumber, @NumberOfSeats, @Model, @ReleaseDate;

       WHILE @@FETCH_STATUS = 0
       BEGIN
           PRINT 'Bus Number: ' + CAST(@BusNumber AS VARCHAR(10));
           PRINT 'Number Of Seats: ' + CAST(@NumberOfSeats AS VARCHAR(10));
           PRINT 'Model: ' + @Model;
           PRINT 'Release Date: ' + CAST(@ReleaseDate AS VARCHAR(10));

           FETCH NEXT FROM BusesCursor INTO @BusNumber, @NumberOfSeats, @Model, @ReleaseDate;
       END;

       CLOSE BusesCursor;
       DEALLOCATE BusesCursor;

       FETCH NEXT FROM CompaniesCursor INTO @CompanyName, @Owner;
   END;

   CLOSE CompaniesCursor;
   DEALLOCATE CompaniesCursor;
END;

EXEC GetAllCompanies;

DROP PROCEDURE GetAllCompanies

-- Представления

--  ID, дата ремонта, модель автобуса у Ford
CREATE VIEW RepairBuses  
AS  
SELECT Buses.BusID, RepairOrder.RepairDate, Buses.Model  
FROM Buses 
INNER JOIN  RepairOrder ON Buses.BusID = RepairOrder.BusID  
WHERE (dbo.Buses.Model = N'Ford')  

-- Имя водителя и номер автобуса в каждом маршрутном листе
CREATE VIEW RouteView  
AS  
SELECT TOP (100) PERCENT Routes.RouteName, Driver.DriverName, Buses.BusNumber  
FROM RouteList INNER JOIN  
Buses ON RouteList.BusID = Buses.BusID INNER JOIN  
Driver ON RouteList.DriverID = Driver.DriverID INNER JOIN  
Routes ON RouteList.RouteID = Routes.RouteID  
ORDER BY Routes.RouteName DESC  

--  В расписании название остановки, время отправки и прибытия
CREATE VIEW ScheduleView  
AS  
SELECT Stops.StopName, Schedule.DepartureTime, Schedule.ArrivalTime  
FROM Schedule LEFT JOIN  
Stops ON Schedule.StopID = Stops.StopID  

-- Проверка представлений
SELECT * FROM RepairBuses;
SELECT * FROM RouteView;
SELECT * FROM ScheduleView;

-- Просмотр кода
EXEC sp_helptext 'RepairBuses';
EXEC sp_helptext 'RouteView';
EXEC sp_helptext 'ScheduleView';

-- Шифрование представлений

DROP VIEW View_Index;

CREATE VIEW ScheduleView  
WITH ENCRYPTION AS  
SELECT Stops.StopName, Schedule.DepartureTime, Schedule.ArrivalTime  
FROM Schedule LEFT JOIN  
Stops ON Schedule.StopID = Stops.StopID  

-- Обновляемое представление (Обновляем название компании у автобусов в зависимости от кол-ва мест в нем)

CREATE VIEW BusesUpdate  
AS  
SELECT Buses.NumberOfSeats, Companies.CompanyName, Buses.Model, Buses.ReleaseDate  
FROM Buses INNER JOIN  
Companies ON Buses.CompanyID = Companies.CompanyID  
WHERE (Buses.NumberOfSeats >= 30)  
WITH CHECK OPTION;  

EXEC sp_helptext 'BusesUpdate';

--ТЕСТ
UPDATE BusesUpdate
SET CompanyName = 'Bad Company'
WHERE NumberOfSeats = 20;

UPDATE BusesUpdate
SET CompanyName = 'Good Company'
WHERE NumberOfSeats = 30;

SELECT * FROM BusesUpdate;

--Параметры
SET NUMERIC_ROUNDABORT OFF;
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON;
GO

--Индексированное представление
CREATE VIEW View_Index
WITH SCHEMABINDING
AS
SELECT RepairOrder.RepairPrice, Buses.BusNumber, Buses.NumberOfSeats, Buses.Model, Buses.ReleaseDate, RepairOrder.RepairDate
FROM dbo.Buses INNER JOIN
dbo.RepairOrder ON Buses.BusID = RepairOrder.BusID
WHERE (Buses.Model = N'Ford')

exec sp_helptext View_Index

CREATE unique CLUSTERED INDEX idx_View_Index
ON View_Index (BusNumber)

SELECT BusNumber FROM View_Index WITH (NOEXPAND)

SELECT BusNumber FROM Buses