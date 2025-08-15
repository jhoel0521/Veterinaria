-- Crear la base de datos
USE master;
GO

-- Eliminar la base de datos si existe (opcional - descomenta si necesitas recrear)
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'SistemaVeterinario')
BEGIN
    ALTER DATABASE SistemaVeterinario SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SistemaVeterinario;
END
GO

-- Crear nueva base de datos
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SistemaVeterinario')
BEGIN
    CREATE DATABASE SistemaVeterinario;
END
GO

USE SistemaVeterinario;
GO

-- PERSONAS
CREATE TABLE persona (
    id INT PRIMARY KEY IDENTITY(1,1),
    tipo NVARCHAR(255) NOT NULL CHECK (tipo IN ('Fisica', 'Juridica')),
    email NVARCHAR(255),
    direccion NVARCHAR(255),
    telefono NVARCHAR(255),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE persona_fisica (
    id INT PRIMARY KEY,
    ci VARCHAR(15),
    nombre VARCHAR(255),
    apellido VARCHAR(255),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (id) REFERENCES persona(id) ON DELETE CASCADE
);
GO

CREATE TABLE persona_juridica (
    id INT PRIMARY KEY,
    razon_social VARCHAR(255),
    nit VARCHAR(20),
    encargado_nombre VARCHAR(255),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (id) REFERENCES persona(id) ON DELETE CASCADE
);
GO

-- ANIMALES
CREATE TABLE animal (
    id INT PRIMARY KEY IDENTITY(1,1),
    tipo NVARCHAR(255),
    nombre NVARCHAR(255),
    fecha_nacimiento DATE,
    persona_id INT,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (persona_id) REFERENCES persona(id)
);
GO

-- HISTÓRICO CLÍNICO
CREATE TABLE historico (
    id INT PRIMARY KEY IDENTITY(1,1),
    notas_importantes TEXT,
    animal_id INT,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (animal_id) REFERENCES animal(id)
);
GO

CREATE TABLE detalle_historico (
    id INT PRIMARY KEY IDENTITY(1,1),
    tipo NVARCHAR(255) NOT NULL CHECK (tipo IN ('Diagnóstico', 'Tratamiento', 'Control', 'Vacunación', 'Cirugía')),
    historico_id INT,
    referencia VARCHAR(15),
    observaciones TEXT,
    tratamiento TEXT,
    fecha_evento DATE DEFAULT GETDATE(),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (historico_id) REFERENCES historico(id)
);
GO

-- PERSONAL
CREATE TABLE personal (
    id INT PRIMARY KEY IDENTITY(1,1),
    fecha_contratacion DATE,
    nombre VARCHAR(50),
    apellido VARCHAR(50),
    email VARCHAR(100),
    usuario VARCHAR(50),
    contrasena VARCHAR(255),
    telefono VARCHAR(20),
    direccion VARCHAR(120),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE personal_rol (
    id INT PRIMARY KEY IDENTITY(1,1),
    personal_id INT,
    rol NVARCHAR(255) NOT NULL CHECK (rol IN ('Veterinario', 'Auxiliar', 'Recepcionista', 'Administrador')),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (personal_id) REFERENCES personal(id) ON DELETE CASCADE
);
GO

-- PRODUCTOS
CREATE TABLE categoria (
    id INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(255),
    requiere_diagnostico BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE producto (
    id INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(255),
    precio DECIMAL(10,2),
    requiere_diagnostico BIT DEFAULT 1,
    categoria_id INT,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (categoria_id) REFERENCES categoria(id)
);
GO

-- FACTURACIÓN
CREATE TABLE factura (
    id INT PRIMARY KEY IDENTITY(1,1),
    ref_factura NVARCHAR(255) UNIQUE,
    fecha DATE DEFAULT GETDATE(),
    persona_id INT,
    total DECIMAL(10,2) DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (persona_id) REFERENCES persona(id)
);
GO

CREATE TABLE detalle_factura (
    id INT PRIMARY KEY IDENTITY(1,1),
    factura_id INT,
    producto_id INT,
    cantidad INT,
    precio_unitario DECIMAL(10,2),
    subtotal DECIMAL(10,2) DEFAULT 0,
    diagnostico_verificado BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (factura_id) REFERENCES factura(id) ON DELETE CASCADE,
    FOREIGN KEY (producto_id) REFERENCES producto(id)
);
GO

-- DIAGNÓSTICO
CREATE TABLE diagnostico (
    id INT PRIMARY KEY IDENTITY(1,1),
    descripcion TEXT,
    factura_id INT,
    precio DECIMAL(10,2) DEFAULT 0,
    personal_id INT,
    ref_detalle_historico_id INT UNIQUE,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (factura_id) REFERENCES factura(id),
    FOREIGN KEY (personal_id) REFERENCES personal(id),
    FOREIGN KEY (ref_detalle_historico_id) REFERENCES detalle_historico(id)
);
GO

-- Crear índices para mejorar rendimiento
CREATE INDEX idx_animal_persona ON animal(persona_id);
CREATE INDEX idx_detalle_historico_historico ON detalle_historico(historico_id);
CREATE INDEX idx_diagnostico_detalle ON diagnostico(ref_detalle_historico_id);
CREATE INDEX idx_producto_categoria ON producto(categoria_id);
CREATE INDEX idx_factura_persona ON factura(persona_id);
CREATE INDEX idx_detalle_factura_factura ON detalle_factura(factura_id);
GO

-- Crear triggers
CREATE TRIGGER trg_calcular_subtotal
ON detalle_factura
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE df
    SET df.subtotal = i.cantidad * i.precio_unitario
    FROM detalle_factura df
    INNER JOIN inserted i ON df.id = i.id
    WHERE df.id = i.id;
END
GO

CREATE TRIGGER trg_actualizar_total_factura
ON detalle_factura
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @factura_id INT;
    
    -- Obtener factura_id de los registros afectados
    IF EXISTS (SELECT * FROM inserted)
        SELECT @factura_id = factura_id FROM inserted;
    ELSE
        SELECT @factura_id = factura_id FROM deleted;
    
    -- Actualizar total de la factura
    UPDATE f
    SET f.total = COALESCE((
        SELECT SUM(df.subtotal)
        FROM detalle_factura df
        WHERE df.factura_id = @factura_id
    ), 0)
    FROM factura f
    WHERE f.id = @factura_id;
END
GO

CREATE TRIGGER trg_validar_producto_critico
ON detalle_factura
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @requiere_diagnostico BIT;
    DECLARE @diagnostico_verificado BIT;
    DECLARE @producto_id INT;
    
    SELECT 
        @requiere_diagnostico = p.requiere_diagnostico,
        @diagnostico_verificado = i.diagnostico_verificado,
        @producto_id = i.producto_id
    FROM inserted i
    INNER JOIN producto p ON p.id = i.producto_id;
    
    IF @requiere_diagnostico = 1 AND @diagnostico_verificado = 0
    BEGIN
        RAISERROR('Productos que requieren diagnóstico deben estar verificados.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- ==========================================
-- DATOS DE PRUEBA - PERSONAL (USUARIOS DEL SISTEMA)
-- ==========================================

-- Insertar personal del sistema
IF NOT EXISTS (SELECT * FROM personal WHERE usuario = 'admin')
BEGIN
    INSERT INTO personal (fecha_contratacion, nombre, apellido, email, usuario, contrasena, telefono, direccion)
    VALUES 
        ('2024-01-15', 'Admin', 'Sistema', 'admin@zoofipets.com', 'admin', '123456', '555-0001', 'Oficina Principal'),
        ('2024-02-01', 'Juan', 'Pérez', 'juan@email.com', 'jperez', '123456', '555-0002', 'Calle 123'),
        ('2024-02-15', 'María', 'González', 'maria@email.com', 'mgonzalez', '123456', '555-0003', 'Avenida 456'),
        ('2024-03-01', 'Carlos', 'Veterinario', 'carlos@zoofipets.com', 'cveterinario', '123456', '555-0004', 'Clínica Principal');
    
    PRINT 'Personal de prueba insertado exitosamente.';
END
GO

-- Asignar roles al personal
IF NOT EXISTS (SELECT * FROM personal_rol pr INNER JOIN personal p ON pr.personal_id = p.id WHERE p.usuario = 'admin')
BEGIN
    INSERT INTO personal_rol (personal_id, rol)
    SELECT p.id, 'Administrador'
    FROM personal p WHERE p.usuario = 'admin';
    
    INSERT INTO personal_rol (personal_id, rol)
    SELECT p.id, 'Recepcionista'
    FROM personal p WHERE p.usuario = 'jperez';
    
    INSERT INTO personal_rol (personal_id, rol)
    SELECT p.id, 'Auxiliar'
    FROM personal p WHERE p.usuario = 'mgonzalez';
    
    INSERT INTO personal_rol (personal_id, rol)
    SELECT p.id, 'Veterinario'
    FROM personal p WHERE p.usuario = 'cveterinario';
    
    PRINT 'Roles de personal asignados exitosamente.';
END
GO

-- ==========================================
-- DATOS DE PRUEBA - PERSONAS FÍSICAS (CLIENTES)
-- ==========================================

IF NOT EXISTS (SELECT * FROM persona p INNER JOIN persona_fisica pf ON p.id = pf.id WHERE pf.ci = '12345678')
BEGIN
    -- Insertar personas base
    INSERT INTO persona (tipo, email, direccion, telefono)
    VALUES 
        ('Fisica', 'ana@email.com', 'Calle 10 #123', '555-1001'),
        ('Fisica', 'carlos@email.com', 'Avenida 20 #456', '555-1002'),
        ('Fisica', 'sofia@email.com', 'Carrera 30 #789', '555-1003'),
        ('Fisica', 'pedro@email.com', 'Calle 40 #321', '555-1004'),
        ('Fisica', 'laura@email.com', 'Avenida 50 #654', '555-1005');
    
    -- Obtener los IDs insertados e insertar los datos específicos de personas físicas
    DECLARE @persona_id1 INT, @persona_id2 INT, @persona_id3 INT, @persona_id4 INT, @persona_id5 INT;
    
    SELECT @persona_id1 = id FROM persona WHERE email = 'ana@email.com';
    SELECT @persona_id2 = id FROM persona WHERE email = 'carlos@email.com';
    SELECT @persona_id3 = id FROM persona WHERE email = 'sofia@email.com';
    SELECT @persona_id4 = id FROM persona WHERE email = 'pedro@email.com';
    SELECT @persona_id5 = id FROM persona WHERE email = 'laura@email.com';
    
    INSERT INTO persona_fisica (id, ci, nombre, apellido)
    VALUES 
        (@persona_id1, '12345678', 'Ana', 'Martínez'),
        (@persona_id2, '23456789', 'Carlos', 'López'),
        (@persona_id3, '34567890', 'Sofia', 'Rodríguez'),
        (@persona_id4, '45678901', 'Pedro', 'Jiménez'),
        (@persona_id5, '56789012', 'Laura', 'Fernández');
    
    PRINT 'Personas físicas (clientes) de prueba insertadas exitosamente.';
END
GO

-- ==========================================
-- DATOS DE PRUEBA - ANIMALES (MASCOTAS)
-- ==========================================

IF NOT EXISTS (SELECT * FROM animal a INNER JOIN persona p ON a.persona_id = p.id INNER JOIN persona_fisica pf ON p.id = pf.id WHERE a.nombre = 'Max' AND pf.ci = '12345678')
BEGIN
    DECLARE @cliente_id1 INT, @cliente_id2 INT, @cliente_id3 INT, @cliente_id4 INT, @cliente_id5 INT;
    
    SELECT @cliente_id1 = p.id FROM persona p INNER JOIN persona_fisica pf ON p.id = pf.id WHERE pf.ci = '12345678';
    SELECT @cliente_id2 = p.id FROM persona p INNER JOIN persona_fisica pf ON p.id = pf.id WHERE pf.ci = '23456789';
    SELECT @cliente_id3 = p.id FROM persona p INNER JOIN persona_fisica pf ON p.id = pf.id WHERE pf.ci = '34567890';
    SELECT @cliente_id4 = p.id FROM persona p INNER JOIN persona_fisica pf ON p.id = pf.id WHERE pf.ci = '45678901';
    SELECT @cliente_id5 = p.id FROM persona p INNER JOIN persona_fisica pf ON p.id = pf.id WHERE pf.ci = '56789012';
    
    INSERT INTO animal (tipo, nombre, fecha_nacimiento, persona_id)
    VALUES 
        ('Perro - Labrador', 'Max', '2021-03-15', @cliente_id1),
        ('Gato - Persa', 'Luna', '2022-06-20', @cliente_id1),
        ('Perro - Pastor Alemán', 'Rocky', '2019-11-10', @cliente_id2),
        ('Gato - Siamés', 'Mimi', '2023-01-25', @cliente_id3),
        ('Perro - Golden Retriever', 'Buddy', '2020-08-05', @cliente_id4),
        ('Gato - Común', 'Whiskers', '2021-12-12', @cliente_id5);
    
    PRINT 'Animales (mascotas) de prueba insertados exitosamente.';
END
GO

-- ==========================================
-- DATOS DE PRUEBA - CATEGORÍAS Y PRODUCTOS
-- ==========================================

IF NOT EXISTS (SELECT * FROM categoria WHERE nombre = 'Alimentos')
BEGIN
    INSERT INTO categoria (nombre, requiere_diagnostico)
    VALUES 
        ('Alimentos', 0),
        ('Juguetes', 0),
        ('Medicamentos', 1),
        ('Accesorios', 0);
    
    PRINT 'Categorías de productos insertadas exitosamente.';
END
GO

IF NOT EXISTS (SELECT * FROM producto WHERE nombre = 'Alimento para Perros Premium')
BEGIN
    DECLARE @cat_alimentos INT, @cat_juguetes INT, @cat_medicamentos INT, @cat_accesorios INT;
    
    SELECT @cat_alimentos = id FROM categoria WHERE nombre = 'Alimentos';
    SELECT @cat_juguetes = id FROM categoria WHERE nombre = 'Juguetes';
    SELECT @cat_medicamentos = id FROM categoria WHERE nombre = 'Medicamentos';
    SELECT @cat_accesorios = id FROM categoria WHERE nombre = 'Accesorios';
    
    INSERT INTO producto (nombre, precio, requiere_diagnostico, categoria_id)
    VALUES 
        ('Alimento para Perros Premium', 25.50, 0, @cat_alimentos),
        ('Alimento para Gatos', 22.00, 0, @cat_alimentos),
        ('Alimento para Cachorros', 28.00, 0, @cat_alimentos),
        ('Pelota de Goma', 8.50, 0, @cat_juguetes),
        ('Cuerda para Jugar', 12.75, 0, @cat_juguetes),
        ('Ratón de Juguete', 6.25, 0, @cat_juguetes),
        ('Vitaminas para Mascotas', 15.75, 1, @cat_medicamentos),
        ('Antiparasitario', 18.50, 1, @cat_medicamentos),
        ('Collar para Perro', 14.00, 0, @cat_accesorios),
        ('Correa Retráctil', 22.50, 0, @cat_accesorios);
    
    PRINT 'Productos de prueba insertados exitosamente.';
END
GO

-- ==========================================
-- PROCEDIMIENTOS ALMACENADOS
-- ==========================================

-- Crear procedimientos adicionales útiles
CREATE PROCEDURE sp_registrar_persona_fisica
    @tipo NVARCHAR(255),
    @email NVARCHAR(255),
    @direccion NVARCHAR(255),
    @telefono NVARCHAR(255),
    @ci VARCHAR(15),
    @nombre VARCHAR(255),
    @apellido VARCHAR(255)
AS
BEGIN
    BEGIN TRANSACTION;
    
    INSERT INTO persona (tipo, email, direccion, telefono)
    VALUES (@tipo, @email, @direccion, @telefono);
    
    DECLARE @persona_id INT = SCOPE_IDENTITY();
    
    INSERT INTO persona_fisica (id, ci, nombre, apellido)
    VALUES (@persona_id, @ci, @nombre, @apellido);
    
    COMMIT TRANSACTION;
    RETURN @persona_id;
END
GO

CREATE PROCEDURE sp_crear_diagnostico
    @descripcion TEXT,
    @precio DECIMAL(10,2),
    @personal_id INT,
    @ref_detalle_historico_id INT,
    @persona_id INT
AS
BEGIN
    BEGIN TRANSACTION;
    
    -- Crear factura para el diagnóstico
    DECLARE @ref_factura NVARCHAR(255) = 'FAC-' + CONVERT(VARCHAR(20), GETDATE(), 112) + '-' + RIGHT('0000' + CAST(ABS(CHECKSUM(NEWID())) % 10000 AS VARCHAR), 4);
    
    INSERT INTO factura (ref_factura, persona_id)
    VALUES (@ref_factura, @persona_id);
    
    DECLARE @factura_id INT = SCOPE_IDENTITY();
    
    -- Insertar diagnóstico
    INSERT INTO diagnostico (descripcion, factura_id, precio, personal_id, ref_detalle_historico_id)
    VALUES (@descripcion, @factura_id, @precio, @personal_id, @ref_detalle_historico_id);
    
    -- Actualizar total de la factura
    UPDATE factura SET total = @precio WHERE id = @factura_id;
    
    COMMIT TRANSACTION;
    RETURN @factura_id;
END
GO

-- Mensaje de éxito
PRINT 'Base de datos, objetos y datos de prueba creados exitosamente!';