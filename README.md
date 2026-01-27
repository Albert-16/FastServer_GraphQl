# FastServer GraphQL API

API GraphQL para el proyecto FastServer que permite consultar y gestionar logs de servicios desde múltiples orígenes de datos (PostgreSQL y SQL Server).

## Arquitectura

El proyecto sigue los principios de **Clean Architecture** con las siguientes capas:

```
FastServer.GraphQL/
├── src/
│   ├── FastServer.Domain/           # Entidades, enums e interfaces
│   ├── FastServer.Application/      # DTOs, servicios y casos de uso
│   ├── FastServer.Infrastructure/   # EF Core, repositorios, contextos DB
│   └── FastServer.GraphQL.Api/      # API GraphQL con Hot Chocolate
├── tests/
│   ├── FastServer.Domain.Tests/
│   ├── FastServer.Application.Tests/
│   └── FastServer.GraphQL.Api.Tests/
└── FastServer.sln
```

## Tecnologías

- **.NET 10.0**
- **Hot Chocolate 15.1.3** - Servidor GraphQL
- **Entity Framework Core 10** - ORM
- **PostgreSQL** y **SQL Server** - Bases de datos soportadas
- **Serilog** - Logging estructurado
- **Health Checks** - Monitoreo de salud de bases de datos

## Modelo de Datos

### Tablas Principales

| Tabla | Descripcion |
|-------|-------------|
| `FastServer_LogServices_Header` | Cabecera de logs con metadatos de cada solicitud |
| `FastServer_LogMicroservice` | Logs detallados de microservicios |
| `FastServer_LogServices_Content` | Contenido de request/response |

### Tablas Historicas

Las mismas tablas con sufijo `_Historico` para datos archivados.

## Configuracion

### Connection Strings

Configura las cadenas de conexion en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=FastServerLogs;Username=postgres;Password=your_password",
    "SqlServer": "Server=localhost;Database=FastServerLogs;User Id=sa;Password=your_password;TrustServerCertificate=True"
  },
  "DefaultDataSource": "PostgreSQL"
}
```

### Origen de Datos Predeterminado

Puedes configurar el origen de datos predeterminado:
- `PostgreSQL` (default)
- `SqlServer`

## Ejecución

### 1. Aplicar Migraciones de Base de Datos

**IMPORTANTE:** Las migraciones ya no se aplican automáticamente al iniciar la API. Debes ejecutarlas manualmente antes de iniciar la aplicación.

```bash
# Aplicar migraciones a todas las bases de datos configuradas
dotnet run --project tools/FastServer.DbMigrator

# Aplicar migraciones solo a PostgreSQL
dotnet run --project tools/FastServer.DbMigrator postgres

# Aplicar migraciones solo a SQL Server
dotnet run --project tools/FastServer.DbMigrator sqlserver
```

### 2. Iniciar la API

```bash
# Restaurar dependencias
dotnet restore

# Compilar
dotnet build

# Ejecutar API
dotnet run --project src/FastServer.GraphQL.Api
```

La API estará disponible en:
- **GraphQL Playground**: `https://localhost:64706/graphql` (HTTPS) o `http://localhost:64707/graphql` (HTTP)
- **Banana Cake Pop (GraphQL IDE)**: `https://localhost:64706/graphql/`
- **Health Check (Liveness)**: `https://localhost:64706/health`
- **Health Check (Readiness)**: `https://localhost:64706/health/ready`

## Health Checks

La API incluye endpoints de health checks para monitorear el estado de la aplicación y las bases de datos:

### Health Check - Liveness Probe
```bash
curl -k https://localhost:64706/health
```

Respuesta cuando está saludable:
```
Healthy
```

### Health Check - Readiness Probe (con detalles)
```bash
curl -k https://localhost:64706/health/ready
```

Respuesta detallada:
```json
{
  "status": "Healthy",
  "checks": [
    {
      "name": "sqlserver-db",
      "status": "Healthy",
      "description": null,
      "duration": 45.2341
    },
    {
      "name": "postgresql-db",
      "status": "Unhealthy",
      "description": "Failed to connect to 127.0.0.1:5432",
      "duration": 4023.1234
    }
  ]
}
```

## Ejemplos de Queries GraphQL

### Obtener todos los logs (paginados)

```graphql
query {
  allLogs(
    pagination: { pageNumber: 1, pageSize: 10 }
    dataSource: SQL_SERVER
  ) {
    totalCount
    pageNumber
    pageSize
    totalPages
    hasNextPage
    hasPreviousPage
    items {
      logId
      microserviceName
      logState
      logDateIn
      logMethodUrl
      httpMethod
      requestDuration
    }
  }
}
```

### Obtener un log por ID

```graphql
query {
  logById(logId: 1, dataSource: SQL_SERVER) {
    logId
    logDateIn
    logDateOut
    logState
    logMethodUrl
    httpMethod
    microserviceName
    requestDuration
    errorCode
    errorDescription
  }
}
```

### Obtener log con detalles completos

```graphql
query {
  logWithDetails(logId: 1, dataSource: SQL_SERVER) {
    logId
    logMethodUrl
    microserviceName
    logState
    logMicroservices {
      logMicroserviceId
      logMicroserviceText
      logDateIn
    }
    logServicesContents {
      logServicesContentId
      logServicesContentText
      contentNo
    }
  }
}
```

### Buscar logs por criterios

```graphql
query {
  searchLogs(
    searchTerm: "error"
    pagination: { pageNumber: 1, pageSize: 10 }
    dataSource: SQL_SERVER
  ) {
    totalCount
    items {
      logId
      microserviceName
      logMethodUrl
      logState
      errorCode
      errorDescription
    }
  }
}
```

### Obtener logs fallidos

```graphql
query {
  failedLogs(
    pagination: { pageNumber: 1, pageSize: 20 }
    dataSource: SQL_SERVER
  ) {
    totalCount
    items {
      logId
      errorCode
      errorDescription
      microserviceName
      logDateIn
    }
  }
}
```

### Consultar orígenes de datos disponibles

```graphql
query {
  availableDataSources
}
```

## Ejemplos de Mutations GraphQL

### Crear un nuevo log

```graphql
mutation {
  createLog(
    input: {
      logDateIn: "2025-01-23T10:00:00Z"
      logDateOut: "2025-01-23T10:00:05Z"
      logState: COMPLETED
      logMethodUrl: "/api/users/123"
      httpMethod: "GET"
      microserviceName: "UserService"
      requestDuration: 5000
    }
    dataSource: SQL_SERVER
  ) {
    logId
    microserviceName
    logMethodUrl
    logState
  }
}
```

### Actualizar un log existente

```graphql
mutation {
  updateLog(
    input: {
      logId: 1
      logState: FAILED
      errorCode: "ERR_TIMEOUT"
      errorDescription: "Connection timeout después de 30 segundos"
    }
    dataSource: SQL_SERVER
  ) {
    logId
    logState
    errorCode
    errorDescription
  }
}
```

### Eliminar un log

```graphql
mutation {
  deleteLog(logId: 1, dataSource: SQL_SERVER)
}
```

## Pruebas con cURL

### Verificar Health Checks

```bash
# Health check básico
curl -k https://localhost:64706/health

# Health check detallado con estado de bases de datos
curl -k https://localhost:64706/health/ready
```

### Ejecutar Queries GraphQL

```bash
# Consultar orígenes de datos disponibles
curl -k -X POST https://localhost:64706/graphql \
  -H "Content-Type: application/json" \
  -d '{"query":"query { availableDataSources }"}'

# Obtener todos los logs con paginación
curl -k -X POST https://localhost:64706/graphql \
  -H "Content-Type: application/json" \
  -d '{"query":"query { allLogs(pagination: {pageNumber: 1, pageSize: 5}, dataSource: SQL_SERVER) { totalCount items { logId microserviceName logState } } }"}'

# Obtener un log por ID
curl -k -X POST https://localhost:64706/graphql \
  -H "Content-Type: application/json" \
  -d '{"query":"query { logById(logId: 1, dataSource: SQL_SERVER) { logId microserviceName logMethodUrl httpMethod logState requestDuration } }"}'
```

## Estados de Log

| Estado | Valor | Descripción |
|--------|-------|-------------|
| PENDING | 0 | Pendiente de procesar |
| IN_PROGRESS | 1 | En proceso |
| COMPLETED | 2 | Completado exitosamente |
| FAILED | 3 | Fallido con error |
| TIMEOUT | 4 | Tiempo de espera agotado |
| CANCELLED | 5 | Cancelado |

## Soporte Multi-Origen de Datos

La API permite consultar y escribir en diferentes orígenes de datos de forma dinámica:

### Orígenes Soportados

1. **PostgreSQL** (`POSTGRESQL`) - Base de datos principal
2. **SQL Server** (`SQL_SERVER`) - Base de datos alternativa

### Configuración del Origen Predeterminado

Configura el origen predeterminado en `appsettings.json`:

```json
{
  "DefaultDataSource": "SqlServer"  // o "PostgreSQL"
}
```

### Especificar Origen en Queries y Mutations

Todas las operaciones GraphQL aceptan el parámetro opcional `dataSource`:

```graphql
# Consultar desde PostgreSQL
query {
  logById(logId: 1, dataSource: POSTGRESQL) {
    logId
    microserviceName
  }
}

# Consultar desde SQL Server
query {
  logById(logId: 1, dataSource: SQL_SERVER) {
    logId
    microserviceName
  }
}

# Crear en SQL Server
mutation {
  createLog(
    input: { ... }
    dataSource: SQL_SERVER
  ) {
    logId
  }
}
```

Si no se especifica `dataSource`, se usa el origen configurado como predeterminado.

## Estructura del Proyecto

```
FastServer/
├── src/
│   ├── FastServer.Domain/              # Capa de Dominio
│   │   ├── Entities/                   # Entidades del dominio
│   │   ├── Enums/                      # Enumeraciones (LogState, DataSourceType)
│   │   ├── Interfaces/                 # Interfaces de repositorios
│   │   └── DataSourceSettings.cs       # Configuración de origen de datos
│   │
│   ├── FastServer.Application/         # Capa de Aplicación
│   │   ├── DTOs/                       # Data Transfer Objects
│   │   ├── Interfaces/                 # Interfaces de servicios
│   │   └── Services/                   # Implementación de servicios
│   │
│   ├── FastServer.Infrastructure/      # Capa de Infraestructura
│   │   ├── Data/
│   │   │   ├── Configurations/         # Configuraciones EF Core
│   │   │   ├── Contexts/               # DbContexts (PostgreSQL, SqlServer)
│   │   │   ├── Migrations/             # Migraciones de BD
│   │   │   ├── Seeders/                # Datos de prueba
│   │   │   ├── DesignTimeDbContextFactory.cs
│   │   │   └── MigrationExtensions.cs  # Extensiones para migraciones
│   │   ├── Repositories/               # Implementación de repositorios
│   │   └── DependencyInjection.cs      # Registro de servicios
│   │
│   └── FastServer.GraphQL.Api/         # API GraphQL
│       ├── GraphQL/
│       │   ├── Mutations/              # Mutations GraphQL
│       │   └── Queries/                # Queries GraphQL
│       ├── Program.cs                  # Punto de entrada
│       └── appsettings.json            # Configuración
│
└── tools/
    └── FastServer.DbMigrator/          # Herramienta de migraciones
        ├── Program.cs                  # Lógica de migración
        ├── appsettings.json            # Configuración base
        ├── appsettings.Development.json # Configuración dev
        └── README.md                   # Documentación
```

## Mejoras Arquitectónicas Implementadas

✅ **Separación de Migraciones**
- Las migraciones ya no se ejecutan automáticamente al iniciar la API
- Herramienta dedicada `FastServer.DbMigrator` para aplicar migraciones
- Soporte para ejecutar migraciones por base de datos específica o todas a la vez
- Manejo robusto de errores cuando una BD no está disponible

✅ **Health Checks**
- Endpoint `/health` para verificación básica de liveness
- Endpoint `/health/ready` con detalles del estado de cada base de datos
- Integración con sistemas de monitoreo y orquestadores (Kubernetes, Docker Swarm)

✅ **Validación de Configuración**
- Validación al inicio de que al menos una cadena de conexión esté configurada
- Mensajes de error claros y descriptivos
- Fail-fast para detectar problemas de configuración tempranamente

✅ **Arquitectura Limpia**
- Separación clara de responsabilidades entre capas
- Inyección de dependencias con ciclos de vida apropiados
- Patrón Factory para manejo de múltiples orígenes de datos

## Gestión de Migraciones

### Crear una Nueva Migración

```bash
# Para PostgreSQL
dotnet ef migrations add MigrationName \
  --project src/FastServer.Infrastructure \
  --startup-project src/FastServer.GraphQL.Api \
  --context PostgreSqlDbContext \
  --output-dir Data/Migrations/PostgreSQL

# Para SQL Server
dotnet ef migrations add MigrationName \
  --project src/FastServer.Infrastructure \
  --startup-project src/FastServer.GraphQL.Api \
  --context SqlServerDbContext \
  --output-dir Data/Migrations/SqlServer
```

### Aplicar Migraciones

```bash
# Usando DbMigrator (recomendado)
dotnet run --project tools/FastServer.DbMigrator

# Usando EF Core CLI (alternativo)
dotnet ef database update --project src/FastServer.Infrastructure --context SqlServerDbContext
```

## Futuras Mejoras

- [ ] Subscriptions GraphQL para eventos en tiempo real
- [ ] Cache distribuido con Redis
- [ ] Rate limiting y throttling
- [ ] Autenticación y autorización (JWT, OAuth2)
- [ ] Logging centralizado (ELK Stack, Azure Application Insights)
- [ ] Métricas y observabilidad (Prometheus, Grafana)
- [ ] Versionado de API GraphQL
- [ ] Documentación interactiva mejorada

## Licencia

Proyecto privado - FastServer
