USE MASTER
GO
IF EXISTS (SELECT * FROM sysdatabases WHERE (name = 'Libros')) 
BEGIN
	PRINT 'LA BASE DE DATOS EXISTE SE BORRAR� Y CRER� UNA NUEVA...'
	alter database Libros set single_user with rollback immediate
	DROP DATABASE Libros;
END

	PRINT 'CREANDO OBJETOS DE BASE DE DATOS...'
	CREATE DATABASE  Libros
	USE Libros;
	CREATE TABLE CategoriaLibro(
		CategoriaLibroId int Identity(1,1) not null CONSTRAINT PK_CategoriaLibroID Primary Key,
		Nombre nvarchar(120) not null,
		Activo bit not null Constraint DF_CategoriaLibro_Activo default 1
		)

		INSERT INTO CategoriaLibro (Nombre) Values
		('Cientificos'),
		('Did�cticos'),
		('T�cnicos'),
		('Programaci�n'),
		('Infantiles');
	--GO

	CREATE TABLE Libro(
		LibroId bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_Libros_LibroId PRIMARY KEY,
		Titulo nvarchar(300) NULL,
		Descripcion nvarchar(500) NULL,
		FechaPublicado datetime not null constraint DF_Libro_FechaPublicado default getdate(),
		ImagenUrl nvarchar(200) NULL,
		Precio numeric(7,2) not null constraint DF_Precio default 0,
		Activo bit NOT NULL Constraint DF_Libro_Activo default 1,
		CategoriaLibroId int not null Constraint FK_Libro_CategoraLibroId foreign key(CategoriaLibroId) references CategoriaLibro(CategoriaLibroId) on update cascade on delete cascade
	);
	--GO

	Insert into Libro (Titulo,Descripcion,FechaPublicado,ImagenUrl,Precio,CategoriaLibroId) Values 
		('El Origen de las Especies','Libro de evoluci�n','1850-01-01','',30,1),
		('Breve Historia del Tiempo','Historia del tiempo',getdate(),'',25.50,1),
		('La profundidad del Mar','Inmersiones submarinas','1850-01-01','',60,1),
		('Clean Code','Buenas pr�cticas de programaci�n',getdate(),'',45.80,4),
		('El Principito','Cuentos y fabulas infantiles',getdate(),'',10,5)
	--GO

	--crear copia de tabla libro para un trigger de historio
	SELECT * INTO LibroHistorico FROM Libro WHERE 1 = 0 -- 1 = 0 para crearla sin datos
	--GO

	SET IDENTITY_INSERT LibroHistorico ON --deshabilito el identity en la tabla libro historico
	--GO


	Create table Reviews(
		ReviewId bigint NOT NULL IDENTITY CONSTRAINT PK_Review_Id PRIMARY KEY,
		NombreVotante nvarchar(100) not null,
		NroEstrellas int not null,
		Comentario nvarchar(3000) not null,
		LibroId bigint not null constraint FK_Review_Libro_LibroId references Libro(LibroId) on update cascade on delete cascade
	)
	--GO

	Create Table PrecioOferta(
		PrecioOfertaId bigint not null identity constraint PK_PrecioOfertaId Primary Key,
		NuevoPrecio numeric(7,2) not null,
		TextoPromocional nvarchar(120) not null,
		LibroId bigint not null constraint FK_PrecioOferta_Libro_LibroId references Libro(LibroId) on update cascade on delete cascade 
							 constraint UC_PrecioOferta_LIbroId Unique(LibroId)
		)
	--GO

	Create Table Autor(
		AutorId int not null identity constraint PK_AutorId Primary Key,	
		Nombre nvarchar(120) not null,
		WebUrl nvarchar(120) not null
		)
	--GO

	Create Table AutorLibro(
		AutorLibroId bigint not null Identity constraint PK_AutorLibroId Primary Key,
		AutorId int not null constraint FK_AutorLibro_Autor_AutorId references Autor(AutorId) on update cascade on delete cascade ,	
		LibroId bigint not null constraint FK_AutorLibro_Libro_LibroId references Libro(LibroId) on update cascade on delete cascade , 	
		Orden bigint not null
		)
	--GO
	Alter table AutorLibro add constraint UC_AutorLibroId_LibroId unique (AutorId,LibroId)
	--GO

	insert into PrecioOferta (LibroId,NuevoPrecio,TextoPromocional) values 
		(1,30,'Oferta del mes'),
		(3,35,'Oferta del mes'),
		(4,5,'Oferta del mes'),
		(2,18.23,'Oferta del mes')

	Insert into Autor (Nombre,WebUrl) values
	  ('Autor 01','wwww.webautor.com'),
	  ('Autor 02','wwww.webautor.com'),
	  ('Autor 03','wwww.webautor.com'),
	  ('Autor 04','wwww.webautor.com')
	insert into AutorLibro (AutorId,LibroId,Orden) values
		(1,1,1),
		(2,3,2),
		(3,3,3),
		(4,4,4),
		(4,2,5)

	Insert into Reviews (LibroId,NombreVotante,NroEstrellas,Comentario) values 
		(1,'Nombre votante 01',5,'Comentario Votante 01'),
		(1,'Nombre votante 02',4,'Comentario Votante 02'),
		(2,'Nombre votante 03',2,'Comentario Votante 03'),
		(3,'Nombre votante 04',1,'Comentario Votante 04'),
		(4,'Nombre votante 05',5,'Comentario Votante 05'),
		(4,'Nombre votante 06',3,'Comentario Votante 06'),
		(3,'Nombre votante 07',4,'Comentario Votante 04')

	GO

	

	CREATE OR ALTER PROCEDURE LibrosPublicadosRango 
		@rangoInicio int,
		@rangoFin int
	AS 
	BEGIN
		Select a.Nombre NombreAutor, l.Titulo TituloLibro, l.FechaPublicado FechaPublicacion,
			CASE 
				When Year(l.FechaPublicado) < 1900 Then 'Antiguo'
				When Year(l.FechaPublicado) >= 1900 Then 'Moderno'
			END AS TipoLibro 
			from AutorLibro al
			Join Autor a on al.AutorId = a.AutorId
			Join Libro l on al.LibroId = l.LibroId
			Where Year(l.FechaPublicado) >= @rangoInicio AND Year(l.FechaPublicado) <= @rangoFin
		
	END
	GO

	CREATE TABLE LibroCambioTitulo
	(
	  LibroCambioTituloId bigint IDENTITY not null  CONSTRAINT PK_LibroCambioTituloId PRIMARY KEY,
	  LibroId bigint not null constraint FK_LibroCambioTitulo_Libro_LibroId references Libro(LibroId) on update cascade on delete cascade, 
	  TituloAnterior nvarchar(120) not null,
	  TituloActualizado nvarchar(120) not null,
	  FechaActualizado datetime not null constraint DF_LibroCambioTitulo_FechaActualizado default getdate()
	 )
	GO 

	CREATE PROCEDURE CambioPrecioCategoria
		@categoriaId int,
		@porcentajeDesc int,
		@texto nvarchar(120)
	AS 
	BEGIN
		IF (@porcentajeDesc < 0)
			BEGIN
				Print 'El procentaje no puede ser menor que 100'
			END 
		ELSE 
			BEGIN
				DELETE FROM PrecioOferta where LibroId IN (SELECT l.LibroId from Libro l Where l.CategoriaLibroId = @categoriaId)			
				INSERT INTO PrecioOferta (LibroId,NuevoPrecio,TextoPromocional) 
					SELECT l.LibroId,(l.Precio -(l.Precio * @porcentajeDesc)/100) PrecioOferta,@Texto from Libro l
				WHERE l.CategoriaLibroId = @categoriaId	
			
			END
	END

	GO

	CREATE TRIGGER tr_LibrosCambioTitulo_afterUpdate
	ON Libro
	AFTER UPDATE
	AS 
	BEGIN
		IF UPDATE(titulo)
		BEGIN
			INSERT INTO	LibroCambioTitulo(LibroId,TituloAnterior,TituloActualizado,FechaActualizado)
			SELECT i.LibroId,d.Titulo, i.Titulo,GETDATE() from	inserted i
			JOIN deleted d on i.LibroId = d.LibroId
		END
	END
	GO

	CREATE TRIGGER tr_Libros_afterDelete
	ON Libro
	AFTER DELETE
	AS 	
	BEGIN
		INSERT INTO	LibroHistorico(LibroId,Titulo,Descripcion,FechaPublicado,Precio,ImagenUrl,CategoriaLibroId,Activo)
		SELECT LibroId,Titulo,Descripcion,FechaPublicado,Precio,ImagenUrl,CategoriaLibroId,Activo from	deleted
	END

	GO	

	Insert into Libro (Titulo,Descripcion,FechaPublicado,ImagenUrl,Precio,CategoriaLibroId,Activo) Values 
		('Libro prueba trigger','Libro prueba trigger','1850-01-01','',30,1,1)
	GO
	Delete  from Libro where Titulo = 'Libro prueba trigger'
	GO

	UPDATE Libro set Titulo = 'El Origen de las Especies Vol 1' where LibroId = 1
	GO
	select * from LibroCambioTitulo
	GO
	EXECUTE LibrosPublicadosRango 1800,2023
	GO
	exec CambioPrecioCategoria 1,20,"Descuento para categor�a 1 20%"
	GO
	Select * from PrecioOferta
	GO
	Select C.Nombre Categoria, COUNT(*) CantLibros from Libro L
		JOIN CategoriaLibro C on L.CategoriaLibroId = C.CategoriaLibroId
		GROUP BY C.Nombre
		HAVING COUNT(*) > 2
	GO
	print 'Base Creada'
