# Optimización de Rendimiento para Producción

## 🚨 Problemas Críticos Detectados

### Problema 1: GetAllAsync carga TODOS los registros en memoria
**Ubicación:** `LogServicesHeaderService.GetAllAsync` (líneas 95-99)

```csharp
// ❌ PROBLEMA: Carga TODOS los registros antes de paginar
var totalCount = await uow.LogServicesHeaders.CountAsync(cancellationToken);
var entities = await uow.LogServicesHeaders.GetAllAsync(cancellationToken);  // ← Trae TODOS los registros

var pagedEntities = entities
    .Skip(pagination.Skip)   // ← Paginación en memoria (LENTO)
    .Take(pagination.PageSize)
    .ToList();
```

**Impacto:**
- Con 10,000 registros: ~2-3 segundos
- Con 100,000 registros: ~10-20 segundos
- Con 1,000,000 registros: Out of Memory Exception

**Solución requerida:**
```csharp
// ✅ SOLUCIÓN: Paginar en la base de datos
var entities = await uow.LogServicesHeaders
    .Query()
    .OrderByDescending(x => x.LogDateIn)
    .Skip(pagination.Skip)
    .Take(pagination.PageSize)
    .ToListAsync(cancellationToken);

var totalCount = await uow.LogServicesHeaders.CountAsync(cancellationToken);
```

### Problema 2: Falta de índices en base de datos
**Consultas sin índices = escaneo completo de tabla**

#### Índices para PostgreSQL (Base de datos actual/principal):
```sql
-- Para ordenamiento y paginación
CREATE INDEX ix_logservices_logdatein ON "FastServer_LogServices_Header"(fastserver_log_date_in DESC);

-- Para filtros comunes
CREATE INDEX ix_logservices_state ON "FastServer_LogServices_Header"(fastserver_log_state);
CREATE INDEX ix_logservices_microservice ON "FastServer_LogServices_Header"(fastserver_microservice_name);
CREATE INDEX ix_logservices_userid ON "FastServer_LogServices_Header"(fastserver_user_id);

-- Índice compuesto para consultas complejas
CREATE INDEX ix_logservices_datestate ON "FastServer_LogServices_Header"(fastserver_log_date_in DESC, fastserver_log_state);

-- Optimización específica PostgreSQL: índice parcial para logs activos
CREATE INDEX ix_logservices_active_logs ON "FastServer_LogServices_Header"(fastserver_log_date_in DESC)
WHERE fastserver_log_state = 2; -- Solo logs completados
```

#### Índices para SQL Server (Base de datos secundaria):
```sql
-- Para ordenamiento y paginación
CREATE INDEX IX_LogServices_LogDateIn ON FastServer_LogServices_Header(fastserver_log_date_in DESC);

-- Para filtros comunes
CREATE INDEX IX_LogServices_State ON FastServer_LogServices_Header(fastserver_log_state);
CREATE INDEX IX_LogServices_Microservice ON FastServer_LogServices_Header(fastserver_microservice_name);
CREATE INDEX IX_LogServices_UserId ON FastServer_LogServices_Header(fastserver_user_id);

-- Índice compuesto para consultas complejas
CREATE INDEX IX_LogServices_DateState ON FastServer_LogServices_Header(fastserver_log_date_in DESC, fastserver_log_state)
INCLUDE (fastserver_microservice_name, fastserver_user_id); -- Covering index
```

### Problema 3: No hay caché implementado
**Cada query va a la base de datos**, incluso para datos que cambian poco.

## 📊 Respuesta a tus Preguntas

### ¿Cumple con el propósito de respuestas rápidas?

**Estado actual: NO ❌**
- Query simple: 2.8 segundos (INACEPTABLE para producción)
- Meta para producción: < 200ms para queries simples

**Después de optimizaciones: SÍ ✅**
- Con índices + paginación correcta: < 100ms
- Con caché: < 10ms para datos frecuentes

### ¿El cambio de origen de datos está automatizado?

**SÍ ✅ - Perfectamente diseñado**

```json
// Cambio de PostgreSQL a SQL Server (o viceversa)
// Solo cambiar UNA línea en appsettings.json:
{
  "DefaultDataSource": "SqlServer"  // ← Cambiar aquí
}
```

**Ventajas de la arquitectura actual:**
1. ✅ Cero cambios de código
2. ✅ Solo reiniciar la aplicación
3. ✅ Funciona con una o ambas bases de datos
4. ✅ Queries específicas pueden usar origen diferente al predeterminado

**Si el día de mañana unifican en un solo origen:**
1. Eliminar cadena de conexión no usada de appsettings.json
2. Cambiar `DefaultDataSource` al único origen
3. Listo - cero cambios de código

## 🚀 Plan de Optimización Recomendado

### Fase 1: Correcciones Críticas (Urgente)

#### 1.1 Arreglar paginación en GetAllAsync
```csharp
public async Task<PaginatedResultDto<LogServicesHeaderDto>> GetAllAsync(
    PaginationParamsDto pagination,
    DataSourceType? dataSource = null,
    CancellationToken cancellationToken = default)
{
    using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);

    // Ejecutar en paralelo: query paginada + count total
    var queryTask = uow.LogServicesHeaders
        .Query()
        .OrderByDescending(x => x.LogDateIn)
        .Skip(pagination.Skip)
        .Take(pagination.PageSize)
        .ToListAsync(cancellationToken);

    var countTask = uow.LogServicesHeaders.CountAsync(cancellationToken);

    await Task.WhenAll(queryTask, countTask);

    return new PaginatedResultDto<LogServicesHeaderDto>
    {
        Items = _mapper.Map<IEnumerable<LogServicesHeaderDto>>(queryTask.Result),
        TotalCount = countTask.Result,
        PageNumber = pagination.PageNumber,
        PageSize = pagination.PageSize
    };
}
```

#### 1.2 Agregar índices a la base de datos
Ver scripts SQL arriba.

**Impacto esperado:** 2.8s → 100-200ms (mejora de 10-20x)

### Fase 2: Caché (Importante)

#### 2.1 Implementar caché distribuido
```bash
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis
```

```csharp
// En Program.cs
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "FastServer_";
});
```

#### 2.2 Agregar caché a servicios
```csharp
public class LogServicesHeaderService
{
    private readonly IDistributedCache _cache;

    public async Task<LogServicesHeaderDto?> GetByIdAsync(long id, ...)
    {
        // Intentar obtener del caché
        var cacheKey = $"log:{id}:{dataSource}";
        var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (cached != null)
            return JsonSerializer.Deserialize<LogServicesHeaderDto>(cached);

        // Si no está en caché, consultar BD
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entity = await uow.LogServicesHeaders.GetByIdAsync(id, cancellationToken);

        if (entity != null)
        {
            var dto = _mapper.Map<LogServicesHeaderDto>(entity);

            // Guardar en caché (5 minutos)
            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(dto),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) },
                cancellationToken);

            return dto;
        }

        return null;
    }
}
```

**Impacto esperado:** Queries repetidas: 100ms → 5-10ms

### Fase 3: Proyecciones y Select Específicos

En lugar de cargar entidades completas, seleccionar solo campos necesarios:

```csharp
// ❌ Carga todos los campos
var entities = await uow.LogServicesHeaders.GetAllAsync();

// ✅ Solo carga campos necesarios
var entities = await uow.LogServicesHeaders
    .Query()
    .Select(x => new LogServicesHeaderDto
    {
        LogId = x.LogId,
        LogDateIn = x.LogDateIn,
        MicroserviceName = x.MicroserviceName,
        // Solo campos necesarios
    })
    .ToListAsync();
```

**Impacto:** Reduce transferencia de red y uso de memoria en 50-70%

### Fase 4: Connection Pooling y Configuración EF

#### Optimizaciones para PostgreSQL (Base de datos actual):
```csharp
// En DependencyInjection.cs
services.AddDbContext<PostgreSqlDbContext>(options =>
    options.UseNpgsql(postgresConnection, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(3);
        npgsqlOptions.CommandTimeout(30);

        // NUEVAS OPTIMIZACIONES POSTGRESQL:
        npgsqlOptions.MaxBatchSize(100);              // Batching para inserts/updates
        npgsqlOptions.UseQuerySplittingBehavior(     // Split queries complejas
            QuerySplittingBehavior.SplitQuery);
    })
    .EnableSensitiveDataLogging(false)           // Deshabilitar en producción
    .EnableDetailedErrors(false)                  // Deshabilitar en producción
    .UseQueryTrackingBehavior(                    // No tracking para queries read-only
        QueryTrackingBehavior.NoTracking));
```

**Optimizaciones adicionales PostgreSQL en appsettings.json:**
```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=FastServerLogs;Username=postgres;Password=***;Pooling=true;Minimum Pool Size=10;Maximum Pool Size=100;Connection Idle Lifetime=300;Connection Pruning Interval=10"
  }
}
```

**Beneficios del connection pooling en PostgreSQL:**
- `Pooling=true` - Habilita reutilización de conexiones
- `Minimum Pool Size=10` - Mantiene 10 conexiones calientes
- `Maximum Pool Size=100` - Máximo 100 conexiones simultáneas
- `Connection Idle Lifetime=300` - Cierra conexiones inactivas después de 5 minutos
- `Connection Pruning Interval=10` - Limpia conexiones cada 10 segundos

#### Optimizaciones para SQL Server (Base de datos secundaria):
```csharp
services.AddDbContext<SqlServerDbContext>(options =>
    options.UseSqlServer(sqlServerConnection, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(3);
        sqlOptions.CommandTimeout(30);

        // NUEVAS OPTIMIZACIONES SQL SERVER:
        sqlOptions.MaxBatchSize(100);              // Batching para inserts/updates
        sqlOptions.UseQuerySplittingBehavior(     // Split queries complejas
            QuerySplittingBehavior.SplitQuery);
    })
    .EnableSensitiveDataLogging(false)           // Deshabilitar en producción
    .EnableDetailedErrors(false)                  // Deshabilitar en producción
    .UseQueryTrackingBehavior(                    // No tracking para queries read-only
        QueryTrackingBehavior.NoTracking));
```

### Fase 5: Compresión de Respuestas

```csharp
// En Program.cs
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

// En pipeline
app.UseResponseCompression();
```

**Impacto:** Reduce tamaño de respuesta en 60-80%

## 📈 Métricas de Rendimiento Esperadas

| Escenario | Sin Optimización | Con Optimización |
|-----------|------------------|------------------|
| Query simple por ID | 50-100ms | 5-10ms (con caché) |
| Lista paginada (10 items) | 2,800ms | 80-150ms |
| Lista paginada (con caché) | 2,800ms | 10-20ms |
| Query compleja con filtros | 5,000ms+ | 200-400ms |
| Throughput (req/seg) | ~10 | ~500-1000 |

## 🔧 Implementación por Prioridad

### Alta Prioridad (Implementar YA)
1. ✅ Arreglar paginación en GetAllAsync
2. ✅ Agregar índices en base de datos
3. ✅ Configurar QueryTrackingBehavior.NoTracking

### Media Prioridad (Próximas 2 semanas)
4. ⚠️ Implementar caché distribuido (Redis)
5. ⚠️ Proyecciones específicas en queries
6. ⚠️ Compresión de respuestas

### Baja Prioridad (Mejora continua)
7. 📊 Monitoreo y métricas (Application Insights)
8. 📊 Query optimization con EF Core logging
9. 📊 Load testing con k6 o JMeter

## ✅ Ventajas de la Arquitectura Actual

**Tu diseño multi-origen ya está perfectamente preparado para:**

1. **Migración sin downtime:**
   ```
   PostgreSQL (origen actual)
   ↓
   Ambos en paralelo (migración gradual)
   ↓
   SQL Server (origen nuevo)
   ```

2. **Replicación Read/Write:**
   - Writes → SQL Server
   - Reads → PostgreSQL replica
   - Configurar por query con parámetro `dataSource`

3. **Sharding geográfico:**
   - US → SQL Server
   - EU → PostgreSQL
   - GraphQL decide dinámicamente

4. **Disaster Recovery:**
   - Si PostgreSQL falla → cambiar a SQL Server en segundos
   - Solo cambiar `DefaultDataSource` y reiniciar

## 🎯 Conclusión

**¿Cumple el propósito?**
- Arquitectura: ✅ Excelente, preparada para escalar
- Rendimiento actual: ❌ Necesita optimizaciones críticas
- Rendimiento potencial: ✅ Con optimizaciones será muy rápido (< 200ms)

**¿Cambio de origen automatizado?**
- ✅ SÍ, perfectamente automatizado
- ✅ Un solo cambio en configuración
- ✅ Cero cambios de código
- ✅ Diseño futuro-proof para cualquier escenario

**Próximo paso recomendado:**
Implementar las optimizaciones de Fase 1 (críticas) ANTES de llevar a producción.

## 🐘 Optimizaciones Específicas para PostgreSQL

### 1. Configuración de PostgreSQL Server

**postgresql.conf - Ajustes recomendados para producción:**

```ini
# MEMORIA
shared_buffers = 4GB                    # 25% de RAM disponible
effective_cache_size = 12GB             # 75% de RAM disponible
work_mem = 50MB                         # Para ordenamientos y hash joins
maintenance_work_mem = 1GB              # Para VACUUM, CREATE INDEX

# PLANIFICADOR DE QUERIES
random_page_cost = 1.1                  # Para SSDs (default es 4.0)
effective_io_concurrency = 200          # Para SSDs (default es 1)
default_statistics_target = 100         # Mejora estadísticas para planner

# ESCRITURA (WAL)
wal_buffers = 16MB
min_wal_size = 1GB
max_wal_size = 4GB
checkpoint_completion_target = 0.9

# PARALELISMO
max_worker_processes = 8
max_parallel_workers_per_gather = 4
max_parallel_workers = 8
max_parallel_maintenance_workers = 4

# LOGGING (para identificar queries lentas)
log_min_duration_statement = 1000       # Log queries > 1 segundo
log_line_prefix = '%t [%p]: [%l-1] user=%u,db=%d,app=%a,client=%h '
log_checkpoints = on
log_connections = on
log_disconnections = on
log_lock_waits = on
```

### 2. Monitoreo de Queries Lentas

**Habilitar pg_stat_statements (extensión de PostgreSQL):**

```sql
-- Ejecutar como superusuario
CREATE EXTENSION IF NOT EXISTS pg_stat_statements;

-- Ver queries más lentas
SELECT
    query,
    calls,
    total_exec_time,
    mean_exec_time,
    max_exec_time,
    rows
FROM pg_stat_statements
ORDER BY mean_exec_time DESC
LIMIT 20;

-- Reset estadísticas
SELECT pg_stat_statements_reset();
```

### 3. VACUUM y ANALYZE Automático

PostgreSQL acumula "dead tuples" que ralentizan queries. Configurar autovacuum agresivo:

```ini
# postgresql.conf
autovacuum = on
autovacuum_max_workers = 4
autovacuum_naptime = 15s               # Revisar cada 15 segundos
autovacuum_vacuum_threshold = 25       # Menos tuplas muertas para activar
autovacuum_analyze_threshold = 10
```

**Ejecutar VACUUM ANALYZE manualmente después de cargas masivas:**

```sql
-- Después de insertar muchos registros
VACUUM ANALYZE "FastServer_LogServices_Header";
```

### 4. Particionamiento de Tablas (Para logs históricos)

Si tienes millones de registros, particiona por fecha:

```sql
-- Crear tabla particionada
CREATE TABLE "FastServer_LogServices_Header_Partitioned" (
    LIKE "FastServer_LogServices_Header" INCLUDING ALL
) PARTITION BY RANGE (fastserver_log_date_in);

-- Crear particiones por mes
CREATE TABLE logs_2025_01 PARTITION OF "FastServer_LogServices_Header_Partitioned"
    FOR VALUES FROM ('2025-01-01') TO ('2025-02-01');

CREATE TABLE logs_2025_02 PARTITION OF "FastServer_LogServices_Header_Partitioned"
    FOR VALUES FROM ('2025-02-01') TO ('2025-03-01');

-- Crear partición para futuros meses automáticamente
CREATE OR REPLACE FUNCTION create_partition_if_not_exists()
RETURNS trigger AS $$
BEGIN
    -- Lógica para crear particiones automáticamente
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;
```

**Beneficios del particionamiento:**
- Queries solo escanean particiones relevantes
- VACUUM más rápido (solo en particiones afectadas)
- Fácil archivar logs antiguos (DROP partition)
- Queries por fecha 10-100x más rápidas

### 5. Índices BRIN para Datos Temporales

Para logs con muchos registros ordenados por fecha, usar BRIN (Block Range Index):

```sql
-- BRIN es 10-100x más pequeño que B-tree para datos ordenados
CREATE INDEX ix_logservices_brin_date
ON "FastServer_LogServices_Header"
USING BRIN (fastserver_log_date_in);

-- Combinar con B-tree para otros campos
CREATE INDEX ix_logservices_state_btree
ON "FastServer_LogServices_Header"
USING BTREE (fastserver_log_state);
```

**Cuándo usar BRIN:**
- ✅ Columnas con datos ordenados naturalmente (timestamps, IDs autoincrementales)
- ✅ Tablas grandes (> 1 millón de registros)
- ✅ Queries con range scans (WHERE date >= X AND date <= Y)
- ❌ No usar para datos desordenados

### 6. Configuración de Connection Pooling con PgBouncer

Para alta concurrencia, usar PgBouncer como proxy:

```ini
# pgbouncer.ini
[databases]
fastserverlogs = host=localhost port=5432 dbname=FastServerLogs

[pgbouncer]
listen_port = 6432
listen_addr = *
auth_type = md5
auth_file = /etc/pgbouncer/userlist.txt
pool_mode = transaction                # Más eficiente
max_client_conn = 1000                 # Conexiones de clientes
default_pool_size = 25                 # Conexiones reales a PostgreSQL
reserve_pool_size = 5
reserve_pool_timeout = 3
```

**Cambiar connection string para usar PgBouncer:**
```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=6432;Database=FastServerLogs;..."
  }
}
```

### 7. Explain Analyze para Debugging

**Antes de crear índices, analiza queries:**

```sql
EXPLAIN (ANALYZE, BUFFERS, VERBOSE)
SELECT * FROM "FastServer_LogServices_Header"
WHERE fastserver_log_date_in >= '2025-01-01'
ORDER BY fastserver_log_date_in DESC
LIMIT 10;
```

**Qué buscar en el output:**
- ❌ `Seq Scan` = Escaneo completo de tabla (MALO)
- ✅ `Index Scan` = Usa índice (BUENO)
- ❌ `Buffers: shared hit=1000 read=5000` = Lee mucho desde disco (necesita índices)
- ✅ `Buffers: shared hit=50 read=0` = Todo en memoria (BUENO)

### 8. Compresión de Columnas (PostgreSQL 14+)

Para columnas con texto largo (como logs JSON):

```sql
ALTER TABLE "FastServer_LogServices_Header"
ALTER COLUMN fastserver_error_description SET COMPRESSION lz4;

-- O al crear la tabla
CREATE TABLE logs (
    id BIGINT,
    message TEXT COMPRESSION lz4
);
```

### 9. Scripts de Mantenimiento

**Script para ejecutar diariamente:**

```sql
-- 1. Recopilar estadísticas
ANALYZE "FastServer_LogServices_Header";

-- 2. Limpiar tuplas muertas
VACUUM "FastServer_LogServices_Header";

-- 3. Ver bloat (espacio desperdiciado)
SELECT
    schemaname,
    tablename,
    pg_size_pretty(pg_total_relation_size(schemaname||'.'||tablename)) AS size,
    n_dead_tup
FROM pg_stat_user_tables
WHERE n_dead_tup > 1000
ORDER BY n_dead_tup DESC;

-- 4. Ver índices no usados (considerar eliminar)
SELECT
    schemaname,
    tablename,
    indexname,
    idx_scan,
    pg_size_pretty(pg_relation_size(indexrelid)) AS index_size
FROM pg_stat_user_indexes
WHERE idx_scan = 0
    AND indexrelname NOT LIKE '%pkey%'
ORDER BY pg_relation_size(indexrelid) DESC;
```

## 📊 Métricas Específicas PostgreSQL

### Verificar Salud de la Base de Datos:

```sql
-- 1. Cache hit ratio (debe ser > 99%)
SELECT
    sum(heap_blks_read) as heap_read,
    sum(heap_blks_hit) as heap_hit,
    sum(heap_blks_hit) / (sum(heap_blks_hit) + sum(heap_blks_read)) * 100 AS cache_hit_ratio
FROM pg_statio_user_tables;

-- 2. Tamaño de la base de datos
SELECT
    pg_size_pretty(pg_database_size('FastServerLogs')) AS db_size;

-- 3. Tablas más grandes
SELECT
    tablename,
    pg_size_pretty(pg_total_relation_size(schemaname||'.'||tablename)) AS size
FROM pg_tables
WHERE schemaname NOT IN ('pg_catalog', 'information_schema')
ORDER BY pg_total_relation_size(schemaname||'.'||tablename) DESC
LIMIT 10;

-- 4. Queries activas
SELECT
    pid,
    now() - query_start AS duration,
    state,
    query
FROM pg_stat_activity
WHERE state != 'idle'
ORDER BY duration DESC;
```

## 🎯 Resumen de Optimizaciones PostgreSQL

### Prioridad ALTA (Implementar primero):
1. ✅ Crear índices básicos (ix_logservices_logdatein, etc.)
2. ✅ Configurar connection pooling en connection string
3. ✅ Habilitar pg_stat_statements para monitoreo
4. ✅ Ajustar shared_buffers y effective_cache_size

### Prioridad MEDIA (Próximas semanas):
5. ⚠️ Configurar autovacuum agresivo
6. ⚠️ Implementar BRIN index para timestamps
7. ⚠️ Configurar log_min_duration_statement para queries lentas

### Prioridad BAJA (Cuando haya > 10M registros):
8. 📊 Implementar particionamiento por fecha
9. 📊 Configurar PgBouncer para alta concurrencia
10. 📊 Compresión de columnas de texto largo
