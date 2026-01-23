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

## Tecnologias

- **.NET 8.0**
- **Hot Chocolate 14** - Servidor GraphQL
- **Entity Framework Core 8** - ORM
- **PostgreSQL** y **SQL Server** - Bases de datos soportadas
- **AutoMapper** - Mapeo de objetos
- **Serilog** - Logging
- **xUnit + FluentAssertions + Moq** - Testing

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

## Ejecucion

```bash
# Restaurar dependencias
dotnet restore

# Compilar
dotnet build

# Ejecutar API
dotnet run --project src/FastServer.GraphQL.Api

# Ejecutar tests
dotnet test
```

La API estara disponible en:
- GraphQL Playground: `http://localhost:5000/graphql`
- Health Check: `http://localhost:5000/health`

## Ejemplos de Queries GraphQL

### Obtener un log por ID

```graphql
query {
  getLogById(logId: 1) {
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

### Obtener log con detalles

```graphql
query {
  getLogWithDetails(logId: 1) {
    logId
    logMethodUrl
    logMicroservices {
      logMicroserviceText
    }
    logServicesContents {
      logServicesContentText
      contentNo
    }
  }
}
```

### Filtrar logs

```graphql
query {
  getLogsByFilter(
    filter: {
      startDate: "2024-01-01T00:00:00Z"
      endDate: "2024-12-31T23:59:59Z"
      state: FAILED
      microserviceName: "UserService"
    }
    pagination: { pageNumber: 1, pageSize: 20 }
  ) {
    items {
      logId
      logDateIn
      logState
      errorCode
    }
    totalCount
    totalPages
    hasNextPage
  }
}
```

### Obtener logs fallidos

```graphql
query {
  getFailedLogs(fromDate: "2024-01-01T00:00:00Z") {
    logId
    errorCode
    errorDescription
    microserviceName
  }
}
```

### Consultar desde SQL Server

```graphql
query {
  getLogById(logId: 1, dataSource: SQL_SERVER) {
    logId
    logMethodUrl
  }
}
```

## Ejemplos de Mutations GraphQL

### Crear un log

```graphql
mutation {
  createLogServicesHeader(
    input: {
      logDateIn: "2024-01-15T10:00:00Z"
      logDateOut: "2024-01-15T10:00:05Z"
      logState: COMPLETED
      logMethodUrl: "/api/users/123"
      httpMethod: "GET"
      microserviceName: "UserService"
      requestDuration: 5000
    }
  ) {
    logId
    logMethodUrl
  }
}
```

### Actualizar un log

```graphql
mutation {
  updateLogServicesHeader(
    input: {
      logId: 1
      logState: FAILED
      errorCode: "ERR001"
      errorDescription: "Connection timeout"
    }
  ) {
    logId
    logState
    errorCode
  }
}
```

### Eliminar un log

```graphql
mutation {
  deleteLogServicesHeader(logId: 1)
}
```

## Estados de Log

| Estado | Valor | Descripcion |
|--------|-------|-------------|
| PENDING | 0 | Pendiente de procesar |
| IN_PROGRESS | 1 | En proceso |
| COMPLETED | 2 | Completado exitosamente |
| FAILED | 3 | Fallido con error |
| TIMEOUT | 4 | Tiempo de espera agotado |
| CANCELLED | 5 | Cancelado |

## Soporte Multi-Origen de Datos

La API permite consultar y escribir en diferentes origenes de datos:

1. **PostgreSQL** - Origen predeterminado
2. **SQL Server** - Origen alternativo

Para especificar el origen en cada operacion, usa el parametro `dataSource`:

```graphql
query {
  getLogById(logId: 1, dataSource: POSTGRESQL) { ... }
}

query {
  getLogById(logId: 1, dataSource: SQL_SERVER) { ... }
}
```

### Consultar origenes disponibles

```graphql
query {
  getAvailableDataSources
}
```

## Estructura de Carpetas

```
src/
├── FastServer.Domain/
│   ├── Entities/           # Entidades del dominio
│   ├── Enums/              # Enumeraciones
│   └── Interfaces/         # Contratos/Interfaces
│
├── FastServer.Application/
│   ├── DTOs/               # Data Transfer Objects
│   ├── Interfaces/         # Interfaces de servicios
│   ├── Mappings/           # Perfiles de AutoMapper
│   └── Services/           # Implementacion de servicios
│
├── FastServer.Infrastructure/
│   ├── Data/
│   │   ├── Configurations/ # Configuraciones EF Core
│   │   └── Contexts/       # DbContexts
│   └── Repositories/       # Implementacion de repositorios
│
└── FastServer.GraphQL.Api/
    └── GraphQL/
        ├── Types/          # Tipos GraphQL
        ├── Queries/        # Queries GraphQL
        └── Mutations/      # Mutations GraphQL
```

## Testing

```bash
# Ejecutar todos los tests
dotnet test

# Ejecutar tests con cobertura
dotnet test --collect:"XPlat Code Coverage"

# Ejecutar tests de un proyecto especifico
dotnet test tests/FastServer.Domain.Tests
```

## Futuras Mejoras

- [ ] Subscriptions GraphQL para eventos en tiempo real
- [ ] Unificacion de esquemas entre PostgreSQL y SQL Server
- [ ] Migracion automatica de datos entre origenes
- [ ] Cache con Redis
- [ ] Rate limiting
- [ ] Autenticacion y autorizacion

## Licencia

Proyecto privado - FastServer
