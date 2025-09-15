# Sistema Veterinario

Un sistema completo de gestión para clínicas veterinarias desarrollado en C# con Windows Forms. Permite administrar mascotas, dueños, productos, citas, historial clínico y generar reportes.

## 📋 Descripción del Proyecto

Este sistema está diseñado para facilitar la gestión integral de una clínica veterinaria, proporcionando herramientas para:

- **Gestión de Mascotas**: Registro completo de información de mascotas incluyendo especie, raza, fecha de nacimiento, peso, color, género, esterilización y microchip.
- **Gestión de Dueños**: Soporte para personas físicas y jurídicas con información completa de contacto.
- **Historial Clínico**: Registro detallado del historial médico de cada mascota incluyendo notas generales, alergias y condiciones médicas.
- **Inventario de Productos**: Control de stock, precios y productos que requieren receta médica.
- **Sistema de Ventas**: Gestión de ventas y facturación.
- **Citas y Consultas**: Programación y seguimiento de citas veterinarias.
- **Reportes**: Generación de reportes detallados y facturas.

## 🏗️ Arquitectura

El proyecto sigue una arquitectura en capas bien definida:

### Capa de Datos (CapaDatos)
Contiene las clases de acceso a datos que interactúan directamente con la base de datos SQL Server:
- `DMascotas.cs`: Gestión de operaciones CRUD para mascotas
- `DPersona.cs`: Manejo de personas físicas y jurídicas
- `DProductos.cs`: Control de inventario y productos
- `DHistorial.cs`: Acceso al historial clínico
- `DUsuario.cs`: Gestión de usuarios del sistema
- `DbConnection.cs`: Conexión singleton a la base de datos

### Capa de Negocio (CapaNegocio)
Implementa la lógica de negocio y validaciones:
- `NMascotas.cs`: Lógica de negocio para mascotas
- `NPersona.cs`: Validaciones y reglas para personas
- `NProductos.cs`: Gestión de inventario y precios
- `NUsuario.cs`: Autenticación y permisos

### Capa de Presentación (SistemVeterinario)
Interfaz de usuario desarrollada con Windows Forms:
- `Dashboard.cs`: Panel principal de navegación
- `Login.cs`: Formulario de autenticación
- `ClientesModule.cs`: Gestión de dueños/clientes
- `MascotasModule.cs`: Administración de mascotas
- `ConsultaModule.cs`: Gestión de consultas veterinarias
- `HistorialModule.cs`: Visualización del historial clínico
- `FacturaReportForm.cs`: Generación de facturas

## 🛠️ Tecnologías Utilizadas

- **Lenguaje**: C#
- **Framework**: .NET Framework
- **Base de Datos**: SQL Server con procedimientos almacenados
- **Interfaz**: Windows Forms
- **Iconos**: FontAwesome.Sharp
- **ORM**: ADO.NET con Microsoft.Data.SqlClient

## 📁 Estructura del Proyecto

```
SistemaVeterinario/
├── CapaDatos/                 # Capa de acceso a datos
│   ├── DMascotas.cs          # CRUD mascotas
│   ├── DPersona.cs           # CRUD personas
│   ├── DProductos.cs         # CRUD productos
│   ├── DHistorial.cs         # Historial clínico
│   ├── DUsuario.cs           # Usuarios del sistema
│   ├── DbConnection.cs       # Conexión BD
│   └── Sql/                  # Scripts SQL
│       ├── migrations/       # Migraciones BD
│       └── seeders/          # Datos iniciales
├── CapaNegocio/              # Lógica de negocio
│   ├── NMascotas.cs
│   ├── NPersona.cs
│   ├── NProductos.cs
│   └── NUsuario.cs
├── SistemVeterinario/        # Interfaz de usuario
│   ├── Dashboard.cs          # Panel principal
│   ├── Login.cs              # Autenticación
│   ├── Program.cs            # Punto de entrada
│   └── Forms/                # Módulos de la aplicación
│       ├── ClientesModule.cs
│       ├── MascotasModule.cs
│       ├── ConsultaModule.cs
│       └── HistorialModule.cs
├── DatabaseMigrator/         # Herramienta de migraciones
└── SistemVeterinario.csproj  # Archivo de proyecto principal
```

## 🚀 Instalación y Configuración

### Prerrequisitos

- **.NET Framework** 4.7.2 o superior
- **SQL Server** 2016 o superior
- **Visual Studio** 2019 o superior (para desarrollo)

### Configuración de la Base de Datos

1. **Ejecutar las migraciones**:
   ```bash
   cd DatabaseMigrator
   dotnet run
   ```

2. **Configurar la cadena de conexión**:
   - Editar el archivo `CapaDatos/DbConnection.cs`
   - Actualizar la cadena de conexión con tus credenciales de SQL Server

### Compilación y Ejecución

1. **Abrir el proyecto**:
   ```bash
   # Abrir en Visual Studio
   start SistemVeterinario.sln
   ```

2. **Restaurar paquetes NuGet**:
   ```bash
   # En Visual Studio: Tools > NuGet Package Manager > Restore NuGet Packages
   ```

3. **Compilar y ejecutar**:
   - Presionar F5 en Visual Studio
   - O ejecutar desde el explorador de archivos: `bin/Debug/SistemVeterinario.exe`

## 📊 Funcionalidades Principales

### Gestión de Mascotas
- ✅ Registro completo de mascotas
- ✅ Búsqueda por nombre, especie o propietario
- ✅ Actualización de información
- ✅ Eliminación lógica (desactivación)

### Gestión de Dueños
- ✅ Personas físicas y jurídicas
- ✅ Información de contacto completa
- ✅ Validaciones de datos
- ✅ Búsqueda avanzada

### Historial Clínico
- ✅ Registro de consultas veterinarias
- ✅ Notas médicas detalladas
- ✅ Seguimiento de alergias y condiciones
- ✅ Historial cronológico

### Inventario y Ventas
- ✅ Control de stock de productos
- ✅ Productos con/sin receta
- ✅ Gestión de precios
- ✅ Sistema de facturación

### Sistema de Usuarios
- ✅ Autenticación segura
- ✅ Control de permisos
- ✅ Gestión de roles

## 🔧 Configuración Avanzada

### Base de Datos
El sistema utiliza procedimientos almacenados para todas las operaciones CRUD. Los scripts se encuentran en:
- `CapaDatos/Sql/migrations/`: Estructura de la base de datos
- `CapaDatos/Sql/seeders/`: Datos iniciales

### Conexión a Base de Datos
```csharp
// En DbConnection.cs
private static readonly string connectionString =
    "Server=localhost;Database=Sistema_Veterinario;Trusted_Connection=True;";
```

## 📈 Reportes y Estadísticas

- **Facturas**: Generación automática de facturas por servicio
- **Reportes de Mascotas**: Listados y estadísticas
- **Historial de Consultas**: Seguimiento detallado
- **Inventario**: Reportes de stock y productos

## 🐛 Solución de Problemas

### Problemas Comunes

1. **Error de conexión a BD**:
   - Verificar que SQL Server esté ejecutándose
   - Comprobar la cadena de conexión
   - Ejecutar migraciones si es necesario

2. **Error al compilar**:
   - Restaurar paquetes NuGet
   - Verificar versión de .NET Framework
   - Limpiar y reconstruir la solución

3. **Problemas con permisos**:
   - Verificar permisos de usuario en SQL Server
   - Comprobar configuración de autenticación

## 🤝 Contribución

1. Fork el proyecto
2. Crear una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request

## 📝 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para más detalles.

## 📞 Contacto

- **Autor**: Jhoel0521
- **Repositorio**: [GitHub](https://github.com/jhoel0521/Veterinaria)

## 📸 Capturas de Pantalla

### Pantalla de Login
<img width="912" height="699" alt="Pantalla de Login" src="https://github.com/user-attachments/assets/5cd84372-53b0-4a6d-b794-b33747f0c4d3" />

### Panel Principal (Dashboard)
<img width="1059" height="790" alt="Panel Principal" src="https://github.com/user-attachments/assets/f3626405-2fb7-4d10-ba84-b4b24dd9d3d3" />

### Reporte de Sistema
<img width="1580" height="1001" alt="Reporte del Sistema" src="https://github.com/user-attachments/assets/12939a17-6562-44f0-ac2f-71dfaa7cd565" />

### Factura Generada
<img width="288" height="796" alt="Factura" src="https://github.com/user-attachments/assets/0b0cd434-e185-482f-aec2-e0746685a0e8" />

---

*Sistema desarrollado para optimizar la gestión de clínicas veterinarias con una interfaz intuitiva y funcionalidad completa.*
