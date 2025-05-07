
-- Tabla: Usuarios
CREATE TABLE IF NOT EXISTS Usuarios (
    IDUsuario INTEGER PRIMARY KEY AUTOINCREMENT,
    Usuario TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    esAdmin BOOLEAN DEFAULT 0,
    Activo BOOLEAN DEFAULT 1,
    FechaCreacion DATETIME DEFAULT (datetime('now'))
);

-- Tabla: Productos
CREATE TABLE IF NOT EXISTS Productos (
    IDProducto INTEGER PRIMARY KEY AUTOINCREMENT,
    NombreProducto TEXT NOT NULL,
    Descripcion TEXT,
    PrecioPorUnidad DECIMAL(10,2) NOT NULL,
    Existencias INTEGER NOT NULL DEFAULT 0,
    FechaCreacion DATETIME DEFAULT (datetime('now'))
);

-- Tabla: Recibos
CREATE TABLE IF NOT EXISTS Recibos (
    IDRecibo INTEGER PRIMARY KEY AUTOINCREMENT,
    NombreCliente TEXT NOT NULL,
    CreadoPor INTEGER NOT NULL,
    CreadoEn DATETIME DEFAULT (datetime('now')),
    CantidadTotal DECIMAL(10,2),
    FOREIGN KEY (CreadoPor) REFERENCES Usuarios(IDUsuario)
);

-- Tabla: DetalleRecibos
CREATE TABLE IF NOT EXISTS DetalleRecibos (
    IDDetalleRecibo INTEGER PRIMARY KEY AUTOINCREMENT,
    IDRecibo INTEGER NOT NULL,
    IDProducto INTEGER NOT NULL,
    Cantidad INTEGER NOT NULL,
    PrecioPorUnidad DECIMAL(10,2) NOT NULL,
    Subtotal DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (IDRecibo) REFERENCES Recibos(IDRecibo),
    FOREIGN KEY (IDProducto) REFERENCES Productos(IDProducto)
);

-- Tabla: ElementosExistencias
CREATE TABLE IF NOT EXISTS ElementosExistencias (
    IDElementosExistencias INTEGER PRIMARY KEY AUTOINCREMENT,
    IDProducto INTEGER NOT NULL,
    CantidadAgregada INTEGER NOT NULL,
    AgregadoPor INTEGER NOT NULL,
    AgregadoEn DATETIME DEFAULT (datetime('now')),
    FOREIGN KEY (IDProducto) REFERENCES Productos(IDProducto),
    FOREIGN KEY (AgregadoPor) REFERENCES Usuarios(IDUsuario)
);

-- Tabla: Ajustes
CREATE TABLE IF NOT EXISTS Ajustes (
    LlaveAjuste TEXT PRIMARY KEY,
    ValorAjuste TEXT NOT NULL
);
