-- DROP SCHEMA dbo;

CREATE SCHEMA dbo;
-- ManejoPresupuesto.dbo.TipoOperaciones definition

-- Drop table

-- DROP TABLE ManejoPresupuesto.dbo.TipoOperaciones;

CREATE TABLE ManejoPresupuesto.dbo.TipoOperaciones (
	Id int IDENTITY(1,1) NOT NULL,
	Descripcion nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	CONSTRAINT Transaccion_Id_PK PRIMARY KEY (Id)
);


-- ManejoPresupuesto.dbo.Usuarios definition

-- Drop table

-- DROP TABLE ManejoPresupuesto.dbo.Usuarios;

CREATE TABLE ManejoPresupuesto.dbo.Usuarios (
	Id int IDENTITY(1,1) NOT NULL,
	Email nvarchar(256) COLLATE Modern_Spanish_CI_AS NOT NULL,
	EmailNormalizado nvarchar(256) COLLATE Modern_Spanish_CI_AS NOT NULL,
	PasswordHash nvarchar(MAX) COLLATE Modern_Spanish_CI_AS NOT NULL,
	CONSTRAINT Usuarios_PK PRIMARY KEY (Id)
);


-- ManejoPresupuesto.dbo.Categorias definition

-- Drop table

-- DROP TABLE ManejoPresupuesto.dbo.Categorias;

CREATE TABLE ManejoPresupuesto.dbo.Categorias (
	Id int IDENTITY(1,1) NOT NULL,
	Nombre nvarchar(50) COLLATE Modern_Spanish_CI_AS NOT NULL,
	TipoOperacionId int NOT NULL,
	UsuarioId int NOT NULL,
	CONSTRAINT Categorias_PK PRIMARY KEY (Id),
	CONSTRAINT Categorias_TipoOperaciones_FK FOREIGN KEY (TipoOperacionId) REFERENCES ManejoPresupuesto.dbo.TipoOperaciones(Id),
	CONSTRAINT Categorias_Usuarios_FK FOREIGN KEY (UsuarioId) REFERENCES ManejoPresupuesto.dbo.Usuarios(Id)
);


-- ManejoPresupuesto.dbo.TiposCuentas definition

-- Drop table

-- DROP TABLE ManejoPresupuesto.dbo.TiposCuentas;

CREATE TABLE ManejoPresupuesto.dbo.TiposCuentas (
	Nombre nvarchar(50) COLLATE Modern_Spanish_CI_AS NOT NULL,
	UsuarioId int NOT NULL,
	Orden int NOT NULL,
	Id int IDENTITY(1,1) NOT NULL,
	CONSTRAINT TiposCuentas_PK PRIMARY KEY (Id),
	CONSTRAINT TiposCuentas_FK FOREIGN KEY (UsuarioId) REFERENCES ManejoPresupuesto.dbo.Usuarios(Id)
);


-- ManejoPresupuesto.dbo.Cuentas definition

-- Drop table

-- DROP TABLE ManejoPresupuesto.dbo.Cuentas;

CREATE TABLE ManejoPresupuesto.dbo.Cuentas (
	Nombre nvarchar(50) COLLATE Modern_Spanish_CI_AS NOT NULL,
	TipoCuentaId int NOT NULL,
	Balance decimal(18,2) NOT NULL,
	Descripcion nvarchar(1000) COLLATE Modern_Spanish_CI_AS NULL,
	Id int IDENTITY(1,1) NOT NULL,
	CONSTRAINT Cuentas_PK PRIMARY KEY (Id),
	CONSTRAINT Cuentas_FK FOREIGN KEY (TipoCuentaId) REFERENCES ManejoPresupuesto.dbo.TiposCuentas(Id)
);


-- ManejoPresupuesto.dbo.Transacciones definition

-- Drop table

-- DROP TABLE ManejoPresupuesto.dbo.Transacciones;

CREATE TABLE ManejoPresupuesto.dbo.Transacciones (
	Id int IDENTITY(1,1) NOT NULL,
	UsuarioId int NOT NULL,
	FechaTransaccion datetime NOT NULL,
	Monto decimal(18,2) NOT NULL,
	Nota nvarchar(1000) COLLATE Modern_Spanish_CI_AS NULL,
	CuentaId int NOT NULL,
	CategoriaId int NOT NULL,
	CONSTRAINT Id_PK PRIMARY KEY (Id),
	CONSTRAINT Transacciones_Categorias_FK FOREIGN KEY (CategoriaId) REFERENCES ManejoPresupuesto.dbo.Categorias(Id),
	CONSTRAINT Transacciones_Cuentas_FK FOREIGN KEY (CuentaId) REFERENCES ManejoPresupuesto.dbo.Cuentas(Id),
	CONSTRAINT Transacciones_Usuarios_FK FOREIGN KEY (UsuarioId) REFERENCES ManejoPresupuesto.dbo.Usuarios(Id)
);
