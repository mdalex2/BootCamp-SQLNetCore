USE MASTER;
GO
IF EXISTS (SELECT * FROM sysdatabases WHERE (name = 'Ejercicio01')) 
BEGIN
	DROP DATABASE Ejercicio01
END
Go
PRINT 'LA BASE DE DATOS NO EXISTE...'
PRINT 'CREANDO OBJETOS DE BASE DE DATOS...'
CREATE DATABASE  Ejercicio01;
GO
USE Ejercicio01
GO
CREATE TABLE [dbo].[CategoriasPlato](
	[CategoriaPlatoId] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_CategoriaPlatoId] PRIMARY KEY,
	[Categoria] [varchar](50) NULL,
	[Icono] [varchar](50) NULL,
	[Activo] [bit] NOT NULL,
)
GO
Insert into CategoriasPlato (Categoria,Icono,Activo) values 
		('Pescados','icono1',1),
		('Mariscos','icono2',1),
		('Carnes','icono3',1),
		('Pollos','icono3',1)
Go
CREATE TABLE [dbo].[Platos](
	[PlatoId] [bigint] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_PlatoId] PRIMARY KEY,
	[Nombre] [varchar](100) NULL,
	[NombreCorto] [varchar](50) NULL,
	[DescripcionPlato] [varchar](500) NULL,
	[Foto] varchar(100) null,
	[Activo] [bit] NOT NULL,
	CategoriaPlatoId int not null,
	Constraint [FK_Plato_CategoriaPlato_Id] foreign key(CategoriaPlatoId) references CategoriasPlato(CategoriaPlatoId) on update cascade on delete cascade
)
GO
insert into Platos (Nombre,NombreCorto,DescripcionPlato,Activo,CategoriaPlatoId) values
	('Pollo al guiso', 'Pollo guisado','',1,4),
	('Ojo de Bife asado', 'Bife asado','',1,3)
Print 'Base datos creada'

