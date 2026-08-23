/* ============================================================
   PROYECTO FINAL - BIKESTORE
   Base de datos: BikeStoreDB
   Motor: SQL Server
   ============================================================ */

IF DB_ID('BikeStoreDB') IS NULL
BEGIN
    CREATE DATABASE BikeStoreDB;
END;
GO

USE BikeStoreDB;
GO

IF OBJECT_ID('DetalleVenta', 'U') IS NOT NULL DROP TABLE DetalleVenta;
IF OBJECT_ID('Venta', 'U') IS NOT NULL DROP TABLE Venta;
IF OBJECT_ID('Bicicleta', 'U') IS NOT NULL DROP TABLE Bicicleta;
IF OBJECT_ID('Cliente', 'U') IS NOT NULL DROP TABLE Cliente;
IF OBJECT_ID('Categoria', 'U') IS NOT NULL DROP TABLE Categoria;
GO

CREATE TABLE Categoria (
    IdCategoria INT IDENTITY(1,1) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(250) NULL,
    Activo BIT NOT NULL DEFAULT 1,
    CONSTRAINT PK_Categoria PRIMARY KEY (IdCategoria),
    CONSTRAINT UQ_Categoria_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE Bicicleta (
    IdBicicleta INT IDENTITY(1,1) NOT NULL,
    IdCategoria INT NOT NULL,
    Marca VARCHAR(100) NOT NULL,
    Modelo VARCHAR(100) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL,
    Estado VARCHAR(20) NOT NULL DEFAULT 'DISPONIBLE',
    CONSTRAINT PK_Bicicleta PRIMARY KEY (IdBicicleta),
    CONSTRAINT FK_Bicicleta_Categoria FOREIGN KEY (IdCategoria) REFERENCES Categoria(IdCategoria),
    CONSTRAINT CK_Bicicleta_Precio CHECK (Precio >= 0),
    CONSTRAINT CK_Bicicleta_Stock CHECK (Stock >= 0),
    CONSTRAINT CK_Bicicleta_Estado CHECK (Estado IN ('DISPONIBLE', 'AGOTADO', 'INACTIVO'))
);
GO

CREATE TABLE Cliente (
    IdCliente INT IDENTITY(1,1) NOT NULL,
    Cedula VARCHAR(10) NOT NULL,
    Nombres VARCHAR(100) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20) NULL,
    Correo VARCHAR(150) NULL,
    CONSTRAINT PK_Cliente PRIMARY KEY (IdCliente),
    CONSTRAINT UQ_Cliente_Cedula UNIQUE (Cedula)
);
GO

CREATE TABLE Venta (
    IdVenta INT IDENTITY(1,1) NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    IdCliente INT NOT NULL,
    Subtotal DECIMAL(10,2) NOT NULL DEFAULT 0,
    Iva DECIMAL(10,2) NOT NULL DEFAULT 0,
    Total DECIMAL(10,2) NOT NULL DEFAULT 0,
    Estado VARCHAR(20) NOT NULL DEFAULT 'REGISTRADA',
    CONSTRAINT PK_Venta PRIMARY KEY (IdVenta),
    CONSTRAINT FK_Venta_Cliente FOREIGN KEY (IdCliente) REFERENCES Cliente(IdCliente),
    CONSTRAINT CK_Venta_Subtotal CHECK (Subtotal >= 0),
    CONSTRAINT CK_Venta_Iva CHECK (Iva >= 0),
    CONSTRAINT CK_Venta_Total CHECK (Total >= 0),
    CONSTRAINT CK_Venta_Estado CHECK (Estado IN ('REGISTRADA', 'ANULADA'))
);
GO

CREATE TABLE DetalleVenta (
    IdDetalle INT IDENTITY(1,1) NOT NULL,
    IdVenta INT NOT NULL,
    IdBicicleta INT NOT NULL,
    Cantidad INT NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Subtotal DECIMAL(10,2) NOT NULL,
    CONSTRAINT PK_DetalleVenta PRIMARY KEY (IdDetalle),
    CONSTRAINT FK_DetalleVenta_Venta FOREIGN KEY (IdVenta) REFERENCES Venta(IdVenta),
    CONSTRAINT FK_DetalleVenta_Bicicleta FOREIGN KEY (IdBicicleta) REFERENCES Bicicleta(IdBicicleta),
    CONSTRAINT CK_DetalleVenta_Cantidad CHECK (Cantidad > 0),
    CONSTRAINT CK_DetalleVenta_Precio CHECK (Precio >= 0),
    CONSTRAINT CK_DetalleVenta_Subtotal CHECK (Subtotal >= 0)
);
GO

INSERT INTO Categoria (Nombre, Descripcion, Activo)
VALUES
('Montaña', 'Bicicletas diseñadas para terrenos irregulares y caminos de montaña.', 1),
('Ruta', 'Bicicletas diseñadas para velocidad en carretera.', 1),
('BMX', 'Bicicletas para acrobacias, saltos y uso deportivo urbano.', 1),
('Eléctricas', 'Bicicletas con asistencia eléctrica al pedaleo.', 1),
('Infantiles', 'Bicicletas diseñadas para niños y niñas.', 1);
GO

INSERT INTO Bicicleta (IdCategoria, Marca, Modelo, Precio, Stock, Estado)
VALUES
(1, 'Trek', 'Marlin 5', 850.00, 10, 'DISPONIBLE'),
(1, 'Giant', 'Talon 3', 780.00, 5, 'DISPONIBLE'),
(2, 'Specialized', 'Allez', 1200.00, 4, 'DISPONIBLE'),
(2, 'Cannondale', 'Synapse', 1450.00, 2, 'DISPONIBLE'),
(3, 'GT', 'Performer BMX', 450.00, 8, 'DISPONIBLE'),
(4, 'Trek', 'Verve+ 2', 2400.00, 3, 'DISPONIBLE'),
(5, 'GW', 'Kids Bike 20', 220.00, 12, 'DISPONIBLE');
GO

INSERT INTO Cliente (Cedula, Nombres, Apellidos, Telefono, Correo)
VALUES
('1002003001', 'Carlos Andrés', 'Pérez López', '0991112222', 'carlos.perez@email.com'),
('1002003002', 'María Fernanda', 'Gómez Ruiz', '0983334444', 'maria.gomez@email.com'),
('1002003003', 'Luis Alberto', 'Torres Mina', '0975556666', 'luis.torres@email.com');
GO

INSERT INTO Venta (Fecha, IdCliente, Subtotal, Iva, Total, Estado)
VALUES
(GETDATE(), 1, 850.00, 127.50, 977.50, 'REGISTRADA'),
(GETDATE(), 2, 900.00, 135.00, 1035.00, 'REGISTRADA');
GO

INSERT INTO DetalleVenta (IdVenta, IdBicicleta, Cantidad, Precio, Subtotal)
VALUES
(1, 1, 1, 850.00, 850.00),
(2, 5, 2, 450.00, 900.00);
GO

UPDATE Bicicleta SET Stock = Stock - 1 WHERE IdBicicleta = 1;
UPDATE Bicicleta SET Stock = Stock - 2 WHERE IdBicicleta = 5;
GO

SELECT * FROM Categoria;
SELECT * FROM Bicicleta;
SELECT * FROM Cliente;
SELECT * FROM Venta;
SELECT * FROM DetalleVenta;
GO
