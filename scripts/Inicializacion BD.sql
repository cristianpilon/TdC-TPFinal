DECLARE @Total as INT;

-- Verifico si existen registros en la tabla de Roles
SELECT @Total = COUNT(*) 
FROM Roles

IF (@Total = 0)
BEGIN
    -- Insertar roles
    INSERT INTO Roles (Nombre) VALUES ('Administrador');
    INSERT INTO Roles (Nombre) VALUES ('Reclutador');
    INSERT INTO Roles (Nombre) VALUES ('Usuario');
END;

-- Verifico si existen registros en la tabla de Usuarios
SELECT @Total = COUNT(*) 
FROM Usuarios

IF (@Total = 0)
BEGIN
	-- Insertar usuarios
	INSERT INTO Usuarios (Nombre, Apellido, Correo, Password, Activo, EnlaceActivacion, EnlaceFechaActivacion, FechaActivacion, IdRol) 
	VALUES ('Cristian', 'Pilon', 'cristian.pilon@uai.edu.ar', 'HiP+DbplZA8yEB+dW3tQDw9yoGPtzZymRY8u94L8BOE=', 1, 'enlace1', GETDATE(), NULL, 1);

	INSERT INTO Usuarios (Nombre, Apellido, Correo, Password, Activo, EnlaceActivacion, EnlaceFechaActivacion, FechaActivacion, IdRol) 
	VALUES ('Nacho', 'Riveira', 'ignacio.riveira@uai.edu.ar', 'HiP+DbplZA8yEB+dW3tQDw9yoGPtzZymRY8u94L8BOE=', 1, 'enlace2', GETDATE(), NULL, 2);

	INSERT INTO Usuarios (Nombre, Apellido, Correo, Password, Activo, EnlaceActivacion, EnlaceFechaActivacion, FechaActivacion, IdRol) 
	VALUES ('Paula', 'Fernandez', 'paula.fernandez@uai.edu.ar', 'HiP+DbplZA8yEB+dW3tQDw9yoGPtzZymRY8u94L8BOE=', 1, 'enlace3', GETDATE(), NULL, 3);
END;

-- Verifico si existen registros en la tabla de Perfiles
SELECT @Total = COUNT(*) 
FROM Perfiles

IF (@Total = 0)
BEGIN
	-- Insertar perfiles
	INSERT INTO Perfiles (Nombre) VALUES ('Desarrollador Junior');
	INSERT INTO Perfiles (Nombre) VALUES ('Desarrollador Semi-Senior');
	INSERT INTO Perfiles (Nombre) VALUES ('Desarrollador Senior');
	INSERT INTO Perfiles (Nombre) VALUES ('QA Junior');
	INSERT INTO Perfiles (Nombre) VALUES ('QA Semi-Senior');
	INSERT INTO Perfiles (Nombre) VALUES ('QA Senior');
	INSERT INTO Perfiles (Nombre) VALUES ('Desarrollador Junior');
	INSERT INTO Perfiles (Nombre) VALUES ('Desarrollador Semi-Senior');
	INSERT INTO Perfiles (Nombre) VALUES ('Desarrollador Senior');
	INSERT INTO Perfiles (Nombre) VALUES ('Analista Funcional Junior');
	INSERT INTO Perfiles (Nombre) VALUES ('Analista Funcional Semi-Senior');
	INSERT INTO Perfiles (Nombre) VALUES ('Analista Funcional Senior');
	INSERT INTO Perfiles (Nombre) VALUES ('DevOps Junior');
	INSERT INTO Perfiles (Nombre) VALUES ('DevOps Semi-Senior');
	INSERT INTO Perfiles (Nombre) VALUES ('DevOps Senior');
END;

-- Verifico si existen registros en la tabla de Etiquetas
SELECT @Total = COUNT(*) 
FROM Etiquetas

IF (@Total = 0)
BEGIN
	-- Insertar etiquetas
	INSERT INTO Etiquetas (Nombre) VALUES ('Node JS');
	INSERT INTO Etiquetas (Nombre) VALUES ('C#');
	INSERT INTO Etiquetas (Nombre) VALUES ('SQL');
	INSERT INTO Etiquetas (Nombre) VALUES ('MongoDB');
	INSERT INTO Etiquetas (Nombre) VALUES ('Automation');
	INSERT INTO Etiquetas (Nombre) VALUES ('Jira');
	INSERT INTO Etiquetas (Nombre) VALUES ('Kanban');
	INSERT INTO Etiquetas (Nombre) VALUES ('Scrum');
	INSERT INTO Etiquetas (Nombre) VALUES ('AWS');
	INSERT INTO Etiquetas (Nombre) VALUES ('Terraform');
	INSERT INTO Etiquetas (Nombre) VALUES ('GitLab Pipelines');
	INSERT INTO Etiquetas (Nombre) VALUES ('Analista Funcional');
	INSERT INTO Etiquetas (Nombre) VALUES ('Diseño sistemas');
	INSERT INTO Etiquetas (Nombre) VALUES ('Atención cliente');
END;

-- Verifico si existen registros en la tabla de Empleos (cargo uno inicial de ejemplo)
SELECT @Total = COUNT(*) 
FROM Empleos

IF (@Total = 0)
BEGIN
	-- Insertar empleos
	INSERT INTO Empleos (Titulo, Descripcion, Ubicacion, Remuneracion, ModalidadTrabajo, FechaPublicacion, HorariosLaborales, TipoTrabajo) 
	VALUES (
	'Implementadores de Software de Gestión Buenos Aires', 
	'<h2>DESCRIPCION DEL EMPLEO</h2><p>Implementadores de Software de Gestión para nuestra sucursal de Buenos Aires</p><p>Para sumarte al equipo estamos buscando personas:</p><ul><li>Con experiencia en implementación y/o seguimiento de proyectos.</li><li>Con conocimientos en lenguajes de programación.</li><li>Software de gestión y/o procesos contables.</li><li>Con experiencia en trabajo en equipo para el cumplimiento de objetivos en tiempo y forma.</li><li>Con disponibilidad para viajar.</li></ul><p>Imaginate:</p><ul><li>Liderando proyectos de implementación de múltiples clientes en simultaneo.</li><li>Relevando necesidades y requenmiontos de nuestros clientes.</li><li>Implementando, capacitando y poniendo en marcha los distintos circuitos.</li><li>Garantizando una atención de excelencia a nuestros clientes.</li></ul><p>Modalidad:</p><ul><li>Full time.</li><li>Sucursal: Buenos aires</li></ul>',
	'Rosario, Santa Fe', 
	100000, 
	'Full-Time', 
	GETDATE(), 
	'Lunes a Viernes de 8 a 16hs', 
	'Híbrido');
	
	-- Insertar perfiles empleos
	INSERT INTO PerfilesEmpleos (IdEmpleo, IdPerfil) 
	VALUES (
	(SELECT Id FROM Empleos WHERE Titulo = 'Implementadores de Software de Gestión Buenos Aires'),
	(SELECT Id FROM Perfiles WHERE Nombre = 'Analista Funcional Junior'));

	-- Insertar etiquetas empleos
	INSERT INTO EtiquetasEmpleos (IdEmpleo, IdEtiqueta) 
	VALUES (
	(SELECT Id FROM Empleos WHERE Titulo = 'Implementadores de Software de Gestión Buenos Aires'),
	(SELECT Id FROM Etiquetas WHERE Nombre = 'Analista Funcional'));

	INSERT INTO EtiquetasEmpleos (IdEmpleo, IdEtiqueta) 
	VALUES (
	(SELECT Id FROM Empleos WHERE Titulo = 'Implementadores de Software de Gestión Buenos Aires'),
	(SELECT Id FROM Etiquetas WHERE Nombre = 'Diseño sistemas'));

	INSERT INTO EtiquetasEmpleos (IdEmpleo, IdEtiqueta) 
	VALUES (
	(SELECT Id FROM Empleos WHERE Titulo = 'Implementadores de Software de Gestión Buenos Aires'),
	(SELECT Id FROM Etiquetas WHERE Nombre = 'Atención cliente'));
END;