# Arquitectura de FastServer GraphQL API

## Visión General

FastServer es una API GraphQL construida con .NET 10 y Hot Chocolate 15 que permite gestionar logs de servicios con soporte para múltiples bases de datos (PostgreSQL y SQL Server).

## Principios Arquitectónicos

El proyecto sigue **Clean Architecture** con separación clara de responsabilidades:

```
┌─────────────────────────────────────────┐
│           API GraphQL Layer              │
│    (FastServer.GraphQL.Api)              │
│  - GraphQL Queries & Mutations           │
│  - Health Checks                         │
│  - Dependency Injection Setup            │
└────────────┬────────────────────────────┘
             │
┌────────────▼────────────────────────────┐
│        Application Layer                 │
│    (FastServer.Application)              │
│  - Services (Lógica de Negocio)          │
│  - DTOs (Data Transfer Objects)          │
│  - AutoMapper Profiles                   │
└────────────┬────────────────────────────┘
             │
┌────────────▼────────────────────────────┐
│      Infrastructure Layer                │
│   (FastServer.Infrastructure)            │
│  - Entity Framework DbContexts           │
│  - Repositorios                          │
│  - Unit of Work                          │
│  - Data Source Factory                   │
└────────────┬────────────────────────────┘
             │
┌────────────▼────────────────────────────┐
│         Domain Layer                     │
│      (FastServer.Domain)                 │
│  - Entidades                             │
│  - Interfaces                            │
│  - Enums                                 │
│  - Domain Settings                       │
└──────────────────────────────────────────┘
```

## Componentes Clave

### 1. DataSourceSettings (Domain Layer)

**Ubicación:** `src/FastServer.Domain/DataSourceSettings.cs`

**Propósito:** Configuración inmutable del origen de datos predeterminado.

**Ciclo de vida:** Singleton

**Por qué es importante:**
- Se configura una sola vez al iniciar la aplicación
- Permite que todos los servicios accedan al mismo origen de datos predeterminado
- Se inyecta en los servicios de aplicación para determinar qué BD usar cuando no se especifica

```csharp
// Se registra en Program.cs
builder.Services.ConfigureDefaultDataSource(dataSourceType);

// Se inyecta en servicios
public LogServicesHeaderService(
    IDataSourceFactory dataSourceFactory,
    IMapper mapper,
    DataSourceSettings dataSourceSettings) // ← Inyección aquí
{
    _defaultDataSource = dataSourceSettings.DefaultDataSource;
}
```

### 2. DataSourceFactory (Infrastructure Layer)

**Ubicación:** `src/FastServer.Infrastructure/Repositories/DataSourceFactory.cs`

**Propósito:** Patrón Factory para crear UnitOfWork específicos por base de datos.

**Ciclo de vida:** Scoped (uno por request HTTP)

**Flujo de trabajo:**
1. Servicio solicita UnitOfWork para un DataSourceType
2. Factory valida que ese origen esté disponible
3. Resuelve el DbContext apropiado desde DI
4. Crea y retorna UnitOfWork con ese contexto

```csharp
// Ejemplo de uso en un servicio
using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
var entity = await uow.LogServicesHeaders.GetByIdAsync(id, cancellationToken);
```

### 3. Dependency Injection Setup (Infrastructure Layer)

**Ubicación:** `src/FastServer.Infrastructure/DependencyInjection.cs`

**Responsabilidades:**
- Configurar DbContexts con políticas de resiliencia
- Registrar DataSourceFactory con lista de orígenes disponibles
- Configurar repositorios genéricos

**Características importantes:**
- Solo registra DbContexts que tienen cadena de conexión configurada
- Configura reintentos automáticos (3 intentos) para fallos transitorios
- Timeout de 30 segundos para comandos SQL

### 4. Application Services

**Ubicación:** `src/FastServer.Application/Services/`

**Servicios principales:**
- `LogServicesHeaderService` - Gestión de logs de cabecera
- `LogMicroserviceService` - Gestión de logs de microservicios
- `LogServicesContentService` - Gestión de contenido de logs

**Patrón común:**
```csharp
public async Task<LogServicesHeaderDto?> GetByIdAsync(
    long id,
    DataSourceType? dataSource = null,
    CancellationToken cancellationToken = default)
{
    // 1. Crear UnitOfWork con origen de datos especificado o predeterminado
    using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);

    // 2. Ejecutar operación a través del repositorio
    var entity = await uow.LogServicesHeaders.GetByIdAsync(id, cancellationToken);

    // 3. Mapear entidad a DTO y retornar
    return entity == null ? null : _mapper.Map<LogServicesHeaderDto>(entity);
}
```

## Flujo de una Request GraphQL

```
1. Cliente envía GraphQL Query
   ↓
2. Hot Chocolate enruta a resolver apropiado
   ↓
3. Resolver inyecta servicio de aplicación
   ↓
4. Servicio solicita UnitOfWork a DataSourceFactory
   ↓
5. Factory crea DbContext apropiado (PostgreSQL o SQL Server)
   ↓
6. UnitOfWork proporciona acceso a repositorios
   ↓
7. Repositorio ejecuta query en la BD
   ↓
8. Entidad se mapea a DTO con AutoMapper
   ↓
9. DTO se retorna como respuesta GraphQL
```

## Configuración de Orígenes de Datos

### appsettings.json

```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=FastServerLogs;...",
    "SqlServer": "Server=localhost;Database=FastServerLogs;..."
  },
  "DefaultDataSource": "SqlServer"
}
```

### Validación al Inicio (Program.cs)

El sistema valida al inicio:
1. ✓ Al menos una cadena de conexión está configurada
2. ✓ `DefaultDataSource` es un valor válido (PostgreSQL o SqlServer)
3. ✓ El origen predeterminado tiene su cadena de conexión configurada

Si alguna validación falla, la aplicación no inicia (fail-fast).

## Ciclos de Vida en DI

| Servicio | Ciclo de Vida | Justificación |
|----------|---------------|---------------|
| `DataSourceSettings` | Singleton | Inmutable, igual para toda la app |
| `DataSourceFactory` | Scoped | Uno por request HTTP |
| `DbContext` | Scoped | Uno por request HTTP |
| `Servicios de Aplicación` | Scoped | Uno por request HTTP |
| `Repositorios` | Scoped | Uno por request HTTP |

## Health Checks

### Liveness Probe: `/health`
- Verifica que la aplicación esté viva
- Kubernetes lo usa para reiniciar pods

### Readiness Probe: `/health/ready`
- Verifica que la aplicación esté lista (BD disponibles)
- Retorna JSON con estado de cada dependencia
- Kubernetes lo usa para enviar tráfico al pod

```json
{
  "status": "Healthy",
  "checks": [
    {
      "name": "sqlserver-db",
      "status": "Healthy",
      "duration": 45.23
    }
  ]
}
```

## Mejores Prácticas Implementadas

### 1. Separación de Migraciones
- Las migraciones NO se ejecutan automáticamente
- Se usa herramienta dedicada: `tools/FastServer.DbMigrator`
- Previene problemas en producción

### 2. Políticas de Resiliencia
- Reintentos automáticos para fallos transitorios (3 intentos)
- Timeouts configurados (30 segundos)

### 3. Validación Temprana
- Configuración se valida al inicio (fail-fast)
- Previene errores en tiempo de ejecución

### 4. Inyección de Dependencias
- Todos los servicios usan DI
- Facilita testing con mocks
- Ciclos de vida apropiados

### 5. Mapeo con AutoMapper
- Separación entre entidades y DTOs
- Previene exposición de lógica interna

## Extensibilidad

### Agregar Nueva Base de Datos

1. Crear nuevo DbContext que herede de `BaseDbContext`
2. Agregar enum value a `DataSourceType`
3. Actualizar `DataSourceFactory.CreateUnitOfWork`
4. Registrar DbContext en `DependencyInjection.AddInfrastructureServices`

### Agregar Nueva Query GraphQL

1. Crear método en clase de Query correspondiente
2. Inyectar servicio necesario con `[Service]`
3. Retornar DTO apropiado
4. Hot Chocolate lo detecta automáticamente

## Comentarios en el Código

El código incluye comentarios detallados en:
- ✅ `DataSourceSettings.cs` - Explica propósito y uso
- ✅ `DataSourceFactory.cs` - Explica patrón Factory y flujo
- ✅ `LogServicesHeaderService.cs` - Explica servicio y operaciones
- ✅ `DependencyInjection.cs` - Explica registro de servicios
- ✅ `Program.cs` - Explica configuración completa de la API

Los comentarios siguen el principio: **"Explica el POR QUÉ, no el QUÉ"**

## Recursos Adicionales

- [README.md](./README.md) - Guía de usuario y ejemplos de queries
- [Hot Chocolate Documentation](https://chillicream.com/docs/hotchocolate)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
