# Migración de SQL Server a PostgreSQL

## Contexto

Actualmente tienes:
- **SQL Server**: Base de datos con datos actuales de logs
- **PostgreSQL**: Base de datos vacía (destino final)

Necesitas migrar todos los datos de SQL Server a PostgreSQL para que PostgreSQL sea la base de datos principal.

## Opción 1: Usar pgloader (Recomendado - Automático)

### ¿Qué es pgloader?
Herramienta que migra automáticamente esquema + datos de SQL Server a PostgreSQL.

### Instalación

**Windows:**
```powershell
# Descargar desde: https://github.com/dimitri/pgloader/releases
# O usar WSL (Windows Subsystem for Linux)
wsl --install
```

**Linux/WSL:**
```bash
sudo apt-get update
sudo apt-get install pgloader
```

### Script de Migración

Crear archivo `migrate.load`:

```lisp
LOAD DATABASE
    FROM mssql://username:password@localhost:1433/FastServerLogs
    INTO postgresql://postgres:password@localhost:5432/FastServerLogs

WITH include drop, create tables, create indexes, reset sequences

SET work_mem TO '256MB',
    maintenance_work_mem TO '512MB'

CAST type datetime to timestamptz
     drop default drop not null using zero-dates-to-null,
     type decimal when (= precision 18) and (= scale 0) to bigint,
     type nvarchar to varchar drop typemod,
     type nchar to char drop typemod

BEFORE LOAD DO
    $$ DROP SCHEMA IF EXISTS public CASCADE; $$,
    $$ CREATE SCHEMA public; $$;
```

### Ejecutar Migración

```bash
pgloader migrate.load
```

**Ventajas:**
- ✅ Migra esquema + datos automáticamente
- ✅ Convierte tipos de datos automáticamente
- ✅ Crea índices automáticamente
- ✅ Muy rápido (usa COPY en PostgreSQL)

**Desventajas:**
- ❌ Requiere instalación de herramienta adicional
- ❌ Puede necesitar ajustes en tipos de datos

---

## Opción 2: Script Manual con .NET (Control Total)

### Paso 1: Crear herramienta de migración

```bash
cd tools
dotnet new console -n FastServer.DataMigrator
cd FastServer.DataMigrator
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add reference ../../src/FastServer.Domain/FastServer.Domain.csproj
dotnet add reference ../../src/FastServer.Infrastructure/FastServer.Infrastructure.csproj
```

### Paso 2: Implementar migrador

**Program.cs:**

```csharp
using FastServer.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var sqlServerConn = config.GetConnectionString("SqlServer");
var postgresConn = config.GetConnectionString("PostgreSQL");

// Crear contextos
var sqlServerOptions = new DbContextOptionsBuilder<SqlServerDbContext>()
    .UseSqlServer(sqlServerConn)
    .Options;

var postgresOptions = new DbContextOptionsBuilder<PostgreSqlDbContext>()
    .UseNpgsql(postgresConn)
    .Options;

using var sourceDb = new SqlServerDbContext(sqlServerOptions);
using var targetDb = new PostgreSqlDbContext(postgresOptions);

Console.WriteLine("Iniciando migración de SQL Server a PostgreSQL...");

// Migrar LogServicesHeader
Console.WriteLine("Migrando LogServicesHeader...");
var batchSize = 1000;
var totalRecords = await sourceDb.LogServicesHeaders.CountAsync();
var processed = 0;

while (processed < totalRecords)
{
    var batch = await sourceDb.LogServicesHeaders
        .OrderBy(x => x.LogId)
        .Skip(processed)
        .Take(batchSize)
        .AsNoTracking()
        .ToListAsync();

    if (!batch.Any()) break;

    await targetDb.LogServicesHeaders.AddRangeAsync(batch);
    await targetDb.SaveChangesAsync();

    processed += batch.Count;
    Console.WriteLine($"Migrados {processed}/{totalRecords} registros");

    // Detach entities para liberar memoria
    foreach (var entity in batch)
    {
        targetDb.Entry(entity).State = EntityState.Detached;
    }
}

Console.WriteLine("Migración completada exitosamente!");
```

**appsettings.json:**

```json
{
  "ConnectionStrings": {
    "SqlServer": "Server=localhost;Database=FastServerLogs;User Id=sa;Password=***;TrustServerCertificate=True",
    "PostgreSQL": "Host=localhost;Port=5432;Database=FastServerLogs;Username=postgres;Password=***"
  }
}
```

### Paso 3: Ejecutar migración

```bash
dotnet run --project tools/FastServer.DataMigrator
```

**Ventajas:**
- ✅ Control total del proceso
- ✅ Usa tus entidades existentes
- ✅ Fácil de debuggear
- ✅ Puedes agregar transformaciones personalizadas

**Desventajas:**
- ❌ Más lento que pgloader
- ❌ Requiere más código manual

---

## Opción 3: Export/Import con CSV (Simple)

### Paso 1: Exportar desde SQL Server

```sql
-- En SQL Server Management Studio
USE FastServerLogs;
GO

-- Exportar LogServicesHeader
EXEC sp_configure 'show advanced options', 1;
RECONFIGURE;
EXEC sp_configure 'xp_cmdshell', 1;
RECONFIGURE;

EXEC xp_cmdshell 'bcp "SELECT * FROM FastServerLogs.dbo.FastServer_LogServices_Header" queryout "C:\temp\logs_header.csv" -c -t"," -S localhost -T';
```

### Paso 2: Importar a PostgreSQL

```bash
# Copiar CSV al servidor PostgreSQL
psql -U postgres -d FastServerLogs -c "\COPY \"FastServer_LogServices_Header\" FROM 'C:\temp\logs_header.csv' WITH (FORMAT csv, DELIMITER ',', HEADER true)"
```

**Ventajas:**
- ✅ No requiere herramientas adicionales
- ✅ Rápido para tablas grandes
- ✅ Fácil de auditar (archivos CSV intermedios)

**Desventajas:**
- ❌ Manual y propenso a errores
- ❌ Problemas con caracteres especiales
- ❌ No migra esquema automáticamente

---

## Opción 4: Azure Data Studio (GUI - Más Fácil)

### Paso 1: Instalar Azure Data Studio

Descargar desde: https://aka.ms/azuredatastudio

### Paso 2: Instalar extensión PostgreSQL

1. Abrir Azure Data Studio
2. Extensions → Buscar "PostgreSQL"
3. Instalar extensión

### Paso 3: Usar SQL Server Import Extension

1. Conectar a SQL Server
2. Click derecho en base de datos → Tasks → Export Data
3. Seleccionar PostgreSQL como destino
4. Mapear tablas
5. Ejecutar

**Ventajas:**
- ✅ Interfaz gráfica fácil de usar
- ✅ No requiere programación
- ✅ Vista previa de mapeo de datos

**Desventajas:**
- ❌ Menos control que opciones programáticas
- ❌ Puede tener problemas con tipos de datos complejos

---

## Recomendación por Escenario

### Si tienes < 100,000 registros:
**Opción 2 (Script .NET manual)** - Máximo control, usa tus entidades

### Si tienes > 100,000 registros:
**Opción 1 (pgloader)** - Más rápido, automático

### Si prefieres GUI:
**Opción 4 (Azure Data Studio)** - Interfaz visual

### Si quieres simplicidad:
**Opción 3 (CSV)** - Simple pero manual

---

## Post-Migración: Verificación

### 1. Verificar conteo de registros

```sql
-- SQL Server
SELECT COUNT(*) FROM FastServer_LogServices_Header;

-- PostgreSQL
SELECT COUNT(*) FROM "FastServer_LogServices_Header";
```

### 2. Verificar secuencias (IDs autoincrementales)

```sql
-- PostgreSQL: Resetear secuencia después de migración
SELECT setval(
    pg_get_serial_sequence('"FastServer_LogServices_Header"', 'fastserver_log_id'),
    (SELECT MAX(fastserver_log_id) FROM "FastServer_LogServices_Header")
);
```

### 3. Crear índices (si no se migraron)

```sql
-- Ejecutar scripts de PERFORMANCE_OPTIMIZATION.md
CREATE INDEX ix_logservices_logdatein ON "FastServer_LogServices_Header"(fastserver_log_date_in DESC);
CREATE INDEX ix_logservices_state ON "FastServer_LogServices_Header"(fastserver_log_state);
-- ... etc
```

### 4. Ejecutar VACUUM ANALYZE

```sql
VACUUM ANALYZE "FastServer_LogServices_Header";
```

---

## Configuración Final en appsettings.json

Después de la migración:

```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=FastServerLogs;Username=postgres;Password=***;Pooling=true;Minimum Pool Size=10;Maximum Pool Size=100",
    "SqlServer": "Server=localhost;Database=FastServerLogsNew;User Id=sa;Password=***;TrustServerCertificate=True"
  },
  "DefaultDataSource": "PostgreSQL"
}
```

Nota: Ahora PostgreSQL es el default, y SQL Server apunta a la nueva base de datos que estás diseñando.
