# PostgreSQL Performance: Monitoreo y Optimización

## 📋 Tabla de Contenidos

1. [Introducción](#introducción)
2. [Habilitación de Extensiones Clave](#habilitación-de-extensiones-clave)
3. [Rastreo de Queries Lentas](#rastreo-de-queries-lentas)
4. [Análisis de Índices](#análisis-de-índices)
5. [Métricas de Performance](#métricas-de-performance)
6. [Vacuum y Mantenimiento](#vacuum-y-mantenimiento)
7. [Connection Pooling](#connection-pooling)
8. [Configuración Óptima](#configuración-óptima)
9. [Alertas y Monitoreo Proactivo](#alertas-y-monitoreo-proactivo)
10. [Scripts de Utilidad](#scripts-de-utilidad)

---

## Introducción

PostgreSQL proporciona herramientas poderosas para monitorear y optimizar el rendimiento de tus bases de datos. Este documento cubre las mejores prácticas específicas para **FastServer** con dos bases de datos PostgreSQL:

- **FastServer_Logs** - Base de datos de logs (alta escritura)
- **FastServer** - Base de datos de microservicios (lecturas y escrituras balanceadas)

**Audiencia:** DBAs, DevOps, Desarrolladores Senior

**Tiempo de lectura:** 30 minutos

---

## Habilitación de Extensiones Clave

### 1. pg_stat_statements (Esencial)

Esta extensión **rastrea todas las queries ejecutadas** y sus estadísticas de rendimiento.

#### Instalación

```sql
-- Conectar como superusuario
psql -U postgres -d FastServer_Logs

-- Crear extensión
CREATE EXTENSION IF NOT EXISTS pg_stat_statements;

-- Verificar instalación
SELECT * FROM pg_extension WHERE extname = 'pg_stat_statements';
```

#### Configuración en postgresql.conf

```ini
# Ubicación: /var/lib/postgresql/data/postgresql.conf (Linux)
# o C:\Program Files\PostgreSQL\14\data\postgresql.conf (Windows)

# Agregar al final del archivo
shared_preload_libraries = 'pg_stat_statements'

# Configuración de pg_stat_statements
pg_stat_statements.max = 10000                 # Número máximo de queries rastreadas
pg_stat_statements.track = all                 # all | top | none
pg_stat_statements.track_utility = on          # Rastrear DDL (CREATE, DROP, etc.)
pg_stat_statements.save = on                   # Persistir estadísticas al reiniciar
```

#### Reiniciar PostgreSQL

```bash
# Linux
sudo systemctl restart postgresql

# Windows (PowerShell como Admin)
Restart-Service postgresql-x64-14

# Verificar que está funcionando
psql -U postgres -c "SELECT count(*) FROM pg_stat_statements;"
```

---

### 2. pg_stat_activity

Extensión **nativa** (no requiere instalación) que muestra queries en ejecución en tiempo real.

```sql
-- Ver queries activas en este momento
SELECT
    pid,
    usename,
    application_name,
    client_addr,
    state,
    query,
    query_start,
    state_change
FROM pg_stat_activity
WHERE state != 'idle'
ORDER BY query_start;
```

---

### 3. auto_explain (Opcional pero Recomendado)

Genera automáticamente planes de ejecución para queries lentas y los guarda en los logs.

#### Configuración en postgresql.conf

```ini
# Agregar a postgresql.conf
shared_preload_libraries = 'pg_stat_statements,auto_explain'  # Agregar auto_explain

# Configuración de auto_explain
auto_explain.log_min_duration = 1000           # Loguear queries > 1 segundo
auto_explain.log_analyze = on                  # Incluir estadísticas de ejecución
auto_explain.log_buffers = on                  # Incluir uso de buffers
auto_explain.log_timing = on                   # Incluir tiempos de cada nodo
auto_explain.log_triggers = on                 # Incluir triggers
auto_explain.log_verbose = on                  # Modo verbose
auto_explain.log_format = json                 # json | text | xml | yaml
```

**Reiniciar PostgreSQL después de cambios.**

---

## Rastreo de Queries Lentas

### Query 1: Top 10 Queries Más Lentas

```sql
-- Ver las 10 queries más lentas en promedio
SELECT
    query,
    calls,                                     -- Número de ejecuciones
    total_exec_time,                           -- Tiempo total (ms)
    mean_exec_time,                            -- Tiempo promedio (ms)
    max_exec_time,                             -- Tiempo máximo (ms)
    min_exec_time,                             -- Tiempo mínimo (ms)
    stddev_exec_time,                          -- Desviación estándar
    rows,                                      -- Filas retornadas
    100.0 * shared_blks_hit /
        NULLIF(shared_blks_hit + shared_blks_read, 0) AS cache_hit_ratio
FROM pg_stat_statements
ORDER BY mean_exec_time DESC
LIMIT 10;
```

**Interpretación:**
- `mean_exec_time > 100ms` → Query lenta, necesita optimización
- `cache_hit_ratio < 95%` → Problema de índices o memoria

---

### Query 2: Queries Más Frecuentes

```sql
-- Queries que se ejecutan más seguido
SELECT
    LEFT(query, 100) AS query_preview,         -- Primeros 100 caracteres
    calls,
    total_exec_time,
    mean_exec_time,
    calls * mean_exec_time AS impact_score     -- Impacto total
FROM pg_stat_statements
ORDER BY calls DESC
LIMIT 10;
```

**Acción:**
- Alta frecuencia + alta latencia = **Prioridad máxima de optimización**
- Considera agregar índices o cache

---

### Query 3: Queries con Mayor Impacto

```sql
-- Queries que consumen más recursos (frecuencia × latencia)
SELECT
    LEFT(query, 100) AS query_preview,
    calls,
    total_exec_time,
    mean_exec_time,
    (calls * mean_exec_time) AS total_impact,  -- Métrica de impacto
    100.0 * total_exec_time / SUM(total_exec_time) OVER() AS percentage_of_total
FROM pg_stat_statements
ORDER BY total_impact DESC
LIMIT 10;
```

---

### Query 4: Queries con Writes Pesados

```sql
-- Queries que escriben/modifican muchos datos
SELECT
    LEFT(query, 100) AS query_preview,
    calls,
    total_exec_time,
    (shared_blks_written + local_blks_written) AS total_blocks_written,
    (shared_blks_written + local_blks_written) / calls AS avg_blocks_per_call
FROM pg_stat_statements
WHERE (shared_blks_written + local_blks_written) > 0
ORDER BY total_blocks_written DESC
LIMIT 10;
```

---

### Resetear Estadísticas

```sql
-- Resetear todas las estadísticas (útil después de optimizaciones)
SELECT pg_stat_statements_reset();

-- Verificar que se reseteó
SELECT count(*) FROM pg_stat_statements;
```

**⚠️ Precaución:** Solo resetear en desarrollo o después de recopilar métricas.

---

## Análisis de Índices

### Query 5: Índices No Utilizados

```sql
-- Índices que NUNCA han sido usados
SELECT
    schemaname,
    tablename,
    indexname,
    idx_scan,                                  -- Número de scans (0 = nunca usado)
    idx_tup_read,                              -- Tuplas leídas
    idx_tup_fetch,                             -- Tuplas obtenidas
    pg_size_pretty(pg_relation_size(indexrelid)) AS index_size
FROM pg_stat_user_indexes
WHERE idx_scan = 0                             -- Índice nunca usado
  AND indexrelname NOT LIKE 'pg_toast%'        -- Excluir índices internos
ORDER BY pg_relation_size(indexrelid) DESC;
```

**Acción:**
- Índices grandes (>10 MB) sin uso → **Candidatos para eliminación**
- Consultar con el equipo antes de eliminar

```sql
-- Eliminar índice no utilizado (CUIDADO)
DROP INDEX IF EXISTS nombre_del_indice;
```

---

### Query 6: Tablas sin Índices

```sql
-- Tablas que NO tienen índices (excepto PK)
SELECT
    schemaname,
    tablename,
    pg_size_pretty(pg_total_relation_size(schemaname||'.'||tablename)) AS total_size
FROM pg_tables
WHERE schemaname NOT IN ('pg_catalog', 'information_schema')
  AND tablename NOT IN (
      SELECT DISTINCT tablename
      FROM pg_indexes
      WHERE schemaname NOT IN ('pg_catalog', 'information_schema')
  )
ORDER BY pg_total_relation_size(schemaname||'.'||tablename) DESC;
```

---

### Query 7: Índices Duplicados

```sql
-- Detectar índices duplicados o redundantes
SELECT
    pg_size_pretty(SUM(pg_relation_size(idx))::BIGINT) AS size,
    (array_agg(idx))[1] AS idx1,
    (array_agg(idx))[2] AS idx2,
    (array_agg(idx))[3] AS idx3,
    (array_agg(idx))[4] AS idx4
FROM (
    SELECT
        indexrelid::regclass AS idx,
        (indrelid::text || E'\n' || indclass::text || E'\n' ||
         indkey::text || E'\n' || COALESCE(indexprs::text, '') || E'\n' ||
         COALESCE(indpred::text, '')) AS key
    FROM pg_index
) sub
GROUP BY key
HAVING COUNT(*) > 1
ORDER BY SUM(pg_relation_size(idx)) DESC;
```

**Acción:**
- Eliminar índices duplicados para ahorrar espacio y mejorar velocidad de INSERT/UPDATE

---

### Query 8: Eficiencia de Índices (Cache Hit Ratio)

```sql
-- Verificar qué tan efectivos son los índices
SELECT
    schemaname,
    tablename,
    indexname,
    idx_scan,
    idx_tup_read,
    idx_tup_fetch,
    CASE
        WHEN idx_tup_read > 0
        THEN ROUND(100.0 * idx_tup_fetch / idx_tup_read, 2)
        ELSE 0
    END AS hit_rate_percentage
FROM pg_stat_user_indexes
WHERE idx_scan > 0
ORDER BY hit_rate_percentage ASC
LIMIT 20;
```

**Interpretación:**
- `hit_rate < 90%` → Índice poco eficiente, revisar diseño

---

### Query 9: Tamaño de Índices vs Tablas

```sql
-- Comparar tamaño de índices vs tamaño de tabla
SELECT
    t.schemaname,
    t.tablename,
    pg_size_pretty(pg_total_relation_size(t.schemaname||'.'||t.tablename)) AS total_size,
    pg_size_pretty(pg_relation_size(t.schemaname||'.'||t.tablename)) AS table_size,
    pg_size_pretty(pg_total_relation_size(t.schemaname||'.'||t.tablename) -
                   pg_relation_size(t.schemaname||'.'||t.tablename)) AS indexes_size,
    ROUND(100.0 * (pg_total_relation_size(t.schemaname||'.'||t.tablename) -
                   pg_relation_size(t.schemaname||'.'||t.tablename)) /
          NULLIF(pg_total_relation_size(t.schemaname||'.'||t.tablename), 0), 2) AS index_ratio
FROM pg_tables t
WHERE t.schemaname NOT IN ('pg_catalog', 'information_schema')
ORDER BY pg_total_relation_size(t.schemaname||'.'||t.tablename) DESC
LIMIT 20;
```

**Interpretación:**
- `index_ratio > 100%` → Índices ocupan más que la tabla (revisar si es necesario)

---

## Métricas de Performance

### Query 10: Cache Hit Ratio Global

```sql
-- Porcentaje de datos servidos desde memoria (objetivo: >95%)
SELECT
    'Cache Hit Ratio' AS metric,
    ROUND(
        100.0 * sum(heap_blks_hit) / NULLIF(sum(heap_blks_hit + heap_blks_read), 0),
        2
    ) AS percentage,
    CASE
        WHEN ROUND(100.0 * sum(heap_blks_hit) / NULLIF(sum(heap_blks_hit + heap_blks_read), 0), 2) > 95
        THEN '✅ Excelente'
        WHEN ROUND(100.0 * sum(heap_blks_hit) / NULLIF(sum(heap_blks_hit + heap_blks_read), 0), 2) > 90
        THEN '⚠️ Bueno'
        ELSE '❌ Pobre - Aumentar shared_buffers'
    END AS status
FROM pg_statio_user_tables;
```

---

### Query 11: Tamaño de Bases de Datos

```sql
-- Tamaño de cada base de datos
SELECT
    datname AS database_name,
    pg_size_pretty(pg_database_size(datname)) AS size,
    pg_database_size(datname) AS size_bytes
FROM pg_database
ORDER BY pg_database_size(datname) DESC;
```

---

### Query 12: Tamaño de Tablas

```sql
-- Top 20 tablas más grandes
SELECT
    schemaname,
    tablename,
    pg_size_pretty(pg_total_relation_size(schemaname||'.'||tablename)) AS total_size,
    pg_size_pretty(pg_relation_size(schemaname||'.'||tablename)) AS table_size,
    pg_size_pretty(pg_total_relation_size(schemaname||'.'||tablename) -
                   pg_relation_size(schemaname||'.'||tablename)) AS indexes_size
FROM pg_tables
WHERE schemaname NOT IN ('pg_catalog', 'information_schema')
ORDER BY pg_total_relation_size(schemaname||'.'||tablename) DESC
LIMIT 20;
```

---

### Query 13: Tablas con Más Escrituras

```sql
-- Tablas con más INSERTs/UPDATEs/DELETEs
SELECT
    schemaname,
    relname AS tablename,
    n_tup_ins AS inserts,
    n_tup_upd AS updates,
    n_tup_del AS deletes,
    (n_tup_ins + n_tup_upd + n_tup_del) AS total_writes,
    n_live_tup AS live_rows,
    n_dead_tup AS dead_rows,
    ROUND(100.0 * n_dead_tup / NULLIF(n_live_tup, 0), 2) AS dead_ratio
FROM pg_stat_user_tables
ORDER BY total_writes DESC
LIMIT 20;
```

**Interpretación:**
- `dead_ratio > 20%` → Necesita VACUUM urgente

---

### Query 14: Bloat (Fragmentación)

```sql
-- Detectar tablas e índices con fragmentación (bloat)
SELECT
    schemaname,
    tablename,
    pg_size_pretty(pg_total_relation_size(schemaname||'.'||tablename)) AS size,
    n_dead_tup AS dead_tuples,
    n_live_tup AS live_tuples,
    ROUND(100.0 * n_dead_tup / NULLIF(n_live_tup + n_dead_tup, 0), 2) AS bloat_ratio
FROM pg_stat_user_tables
WHERE n_dead_tup > 1000  -- Solo tablas con bloat significativo
ORDER BY bloat_ratio DESC
LIMIT 20;
```

**Acción:**
- `bloat_ratio > 20%` → Ejecutar `VACUUM FULL` (requiere lock exclusivo) o `VACUUM`

---

## Vacuum y Mantenimiento

### ¿Qué es VACUUM?

VACUUM recupera espacio ocupado por filas "muertas" (eliminadas o actualizadas). PostgreSQL requiere mantenimiento regular.

### Tipos de VACUUM

| Comando | Descripción | Lock | Cuándo Usar |
|---------|-------------|------|-------------|
| `VACUUM` | Recupera espacio sin lock exclusivo | Lecturas OK | Diario/Semanal |
| `VACUUM FULL` | Reescribe tabla completa | **Lock exclusivo** | Mensual/Trimestral |
| `VACUUM ANALYZE` | VACUUM + actualiza estadísticas | Lecturas OK | **Recomendado** |
| `ANALYZE` | Solo actualiza estadísticas | Sin lock | Después de cambios grandes |

---

### Query 15: Estado de Autovacuum

```sql
-- Ver cuándo fue el último autovacuum/analyze por tabla
SELECT
    schemaname,
    relname AS tablename,
    last_vacuum,
    last_autovacuum,
    last_analyze,
    last_autoanalyze,
    n_dead_tup AS dead_tuples,
    n_mod_since_analyze AS changes_since_analyze
FROM pg_stat_user_tables
ORDER BY n_dead_tup DESC
LIMIT 20;
```

**Interpretación:**
- `last_autovacuum = NULL` y `n_dead_tup > 1000` → Autovacuum no está funcionando correctamente
- `n_mod_since_analyze > 10000` → Necesita ANALYZE

---

### Ejecutar VACUUM Manualmente

```sql
-- VACUUM simple (no bloquea lecturas)
VACUUM log_services_headers;

-- VACUUM ANALYZE (recomendado)
VACUUM ANALYZE log_services_headers;

-- VACUUM FULL (requiere mantenimiento programado)
VACUUM FULL log_services_headers;

-- VACUUM en toda la base de datos
VACUUM ANALYZE;
```

---

### Configuración de Autovacuum en postgresql.conf

```ini
# Habilitar autovacuum (debe estar ON)
autovacuum = on

# Configuración agresiva para tablas con alta escritura (FastServer_Logs)
autovacuum_naptime = 1min                      # Frecuencia de chequeo (default: 1min)
autovacuum_vacuum_threshold = 50               # Mínimo de dead tuples para trigger
autovacuum_vacuum_scale_factor = 0.1           # 10% de la tabla (default: 0.2)
autovacuum_analyze_threshold = 50
autovacuum_analyze_scale_factor = 0.05         # 5% de la tabla

# Workers (procesos paralelos)
autovacuum_max_workers = 3                     # Aumentar si hay muchas tablas

# Configuración para tablas grandes
autovacuum_vacuum_cost_delay = 10ms            # Pausa entre operaciones (menos = más rápido)
autovacuum_vacuum_cost_limit = 200             # Budget de operaciones
```

**Reiniciar PostgreSQL después de cambios.**

---

## Connection Pooling

### PgBouncer (Recomendado para Producción)

PgBouncer es un **connection pooler** que reduce la sobrecarga de crear/destruir conexiones.

#### Instalación

```bash
# Ubuntu/Debian
sudo apt-get install pgbouncer

# Windows - Descargar de https://www.pgbouncer.org/
```

#### Configuración (/etc/pgbouncer/pgbouncer.ini)

```ini
[databases]
FastServer_Logs = host=localhost port=5432 dbname=FastServer_Logs
FastServer = host=localhost port=5432 dbname=FastServer

[pgbouncer]
listen_addr = *
listen_port = 6432
auth_type = md5
auth_file = /etc/pgbouncer/userlist.txt
pool_mode = transaction                        # session | transaction | statement
max_client_conn = 500                          # Máximo de clientes
default_pool_size = 25                         # Conexiones por database
reserve_pool_size = 5                          # Conexiones de reserva
reserve_pool_timeout = 3
server_lifetime = 3600                         # Reciclar conexión después de 1h
server_idle_timeout = 600                      # Cerrar conexión idle después de 10min
```

#### Archivo de usuarios (/etc/pgbouncer/userlist.txt)

```
"postgres" "md5hash_de_password"
"fastserver_user" "md5hash_de_password"
```

**Generar hash MD5:**
```bash
echo -n "passwordpostgres" | md5sum
```

#### Actualizar Connection String en FastServer

```json
{
  "ConnectionStrings": {
    "PostgreSQLLogs": "Host=localhost;Port=6432;Database=FastServer_Logs;Username=postgres;Password=Souma;Pooling=false",
    "PostgreSQLMicroservices": "Host=localhost;Port=6432;Database=FastServer;Username=postgres;Password=Souma;Pooling=false"
  }
}
```

**Nota:** Desactivar pooling de Npgsql (`Pooling=false`) cuando se usa PgBouncer.

---

### Query 16: Monitorear Conexiones

```sql
-- Ver conexiones activas por base de datos
SELECT
    datname AS database,
    count(*) AS connections,
    max(max_conn) AS max_connections,
    ROUND(100.0 * count(*) / max(max_conn), 2) AS usage_percentage
FROM (
    SELECT datname FROM pg_stat_activity
    CROSS JOIN (SELECT setting::int AS max_conn FROM pg_settings WHERE name = 'max_connections') AS mc
) AS stats
GROUP BY datname
ORDER BY connections DESC;
```

---

### Query 17: Conexiones Bloqueadas

```sql
-- Ver conexiones bloqueadas (locks)
SELECT
    blocked_locks.pid AS blocked_pid,
    blocked_activity.usename AS blocked_user,
    blocking_locks.pid AS blocking_pid,
    blocking_activity.usename AS blocking_user,
    blocked_activity.query AS blocked_statement,
    blocking_activity.query AS blocking_statement
FROM pg_catalog.pg_locks blocked_locks
JOIN pg_catalog.pg_stat_activity blocked_activity ON blocked_activity.pid = blocked_locks.pid
JOIN pg_catalog.pg_locks blocking_locks ON blocking_locks.locktype = blocked_locks.locktype
    AND blocking_locks.database IS NOT DISTINCT FROM blocked_locks.database
    AND blocking_locks.relation IS NOT DISTINCT FROM blocked_locks.relation
    AND blocking_locks.page IS NOT DISTINCT FROM blocked_locks.page
    AND blocking_locks.tuple IS NOT DISTINCT FROM blocked_locks.tuple
    AND blocking_locks.virtualxid IS NOT DISTINCT FROM blocked_locks.virtualxid
    AND blocking_locks.transactionid IS NOT DISTINCT FROM blocked_locks.transactionid
    AND blocking_locks.classid IS NOT DISTINCT FROM blocked_locks.classid
    AND blocking_locks.objid IS NOT DISTINCT FROM blocked_locks.objid
    AND blocking_locks.objsubid IS NOT DISTINCT FROM blocked_locks.objsubid
    AND blocking_locks.pid != blocked_locks.pid
JOIN pg_catalog.pg_stat_activity blocking_activity ON blocking_activity.pid = blocking_locks.pid
WHERE NOT blocked_locks.granted;
```

---

## Configuración Óptima

### postgresql.conf Recomendado para FastServer

```ini
# =====================================
# CONFIGURACIÓN PARA FASTSERVER
# Base: PostgreSQL 14+
# Hardware: 8 GB RAM, 4 CPU cores
# =====================================

# --- CONEXIONES ---
max_connections = 200                          # Máximo de conexiones simultáneas

# --- MEMORIA ---
shared_buffers = 2GB                           # 25% de RAM total (8GB * 0.25)
effective_cache_size = 6GB                     # 75% de RAM total
maintenance_work_mem = 512MB                   # Memoria para VACUUM, CREATE INDEX
work_mem = 16MB                                # Memoria por operación de sort/hash
                                               # (shared_buffers / max_connections)

# --- WAL (Write-Ahead Logging) ---
wal_buffers = 16MB                             # Buffer de WAL
checkpoint_timeout = 10min                     # Frecuencia de checkpoints
max_wal_size = 2GB                             # Tamaño máximo de WAL antes de checkpoint
min_wal_size = 1GB                             # Tamaño mínimo de WAL
checkpoint_completion_target = 0.9             # Distribuir checkpoint en 90% del intervalo

# --- QUERY PLANNER ---
random_page_cost = 1.1                         # Para SSD (default: 4.0 para HDD)
effective_io_concurrency = 200                 # Para SSD (default: 1 para HDD)
default_statistics_target = 100                # Estadísticas para el planner (default: 100)

# --- LOGGING ---
log_destination = 'stderr'
logging_collector = on
log_directory = 'log'
log_filename = 'postgresql-%Y-%m-%d_%H%M%S.log'
log_rotation_age = 1d
log_rotation_size = 100MB
log_line_prefix = '%t [%p]: [%l-1] user=%u,db=%d,app=%a,client=%h '
log_min_duration_statement = 1000              # Loguear queries > 1 segundo
log_checkpoints = on
log_connections = on
log_disconnections = on
log_lock_waits = on

# --- AUTOVACUUM (CRÍTICO) ---
autovacuum = on
autovacuum_max_workers = 3
autovacuum_naptime = 1min
autovacuum_vacuum_threshold = 50
autovacuum_vacuum_scale_factor = 0.1
autovacuum_analyze_threshold = 50
autovacuum_analyze_scale_factor = 0.05

# --- PERFORMANCE EXTENSIONS ---
shared_preload_libraries = 'pg_stat_statements'
pg_stat_statements.max = 10000
pg_stat_statements.track = all
```

**⚠️ Reiniciar PostgreSQL después de modificar postgresql.conf**

---

## Alertas y Monitoreo Proactivo

### Script de Monitoreo Diario

Guardar como `monitor_postgres.sh`:

```bash
#!/bin/bash
# Script de monitoreo diario para FastServer PostgreSQL

PGUSER="postgres"
PGDB="FastServer_Logs"

echo "=== PostgreSQL Health Check - $(date) ==="

# 1. Cache Hit Ratio
echo -e "\n1. Cache Hit Ratio (objetivo: >95%):"
psql -U $PGUSER -d $PGDB -t -c "
SELECT ROUND(100.0 * sum(heap_blks_hit) / NULLIF(sum(heap_blks_hit + heap_blks_read), 0), 2) AS cache_hit_ratio
FROM pg_statio_user_tables;
"

# 2. Conexiones activas
echo -e "\n2. Conexiones activas:"
psql -U $PGUSER -d $PGDB -t -c "
SELECT count(*) FROM pg_stat_activity WHERE state != 'idle';
"

# 3. Tablas que necesitan VACUUM
echo -e "\n3. Tablas con bloat > 20%:"
psql -U $PGUSER -d $PGDB -c "
SELECT tablename, n_dead_tup, n_live_tup,
       ROUND(100.0 * n_dead_tup / NULLIF(n_live_tup, 0), 2) AS bloat_ratio
FROM pg_stat_user_tables
WHERE n_dead_tup > 1000 AND ROUND(100.0 * n_dead_tup / NULLIF(n_live_tup, 0), 2) > 20
ORDER BY bloat_ratio DESC;
"

# 4. Queries lentas (top 5)
echo -e "\n4. Top 5 queries lentas:"
psql -U $PGUSER -d $PGDB -c "
SELECT LEFT(query, 80) AS query, calls, ROUND(mean_exec_time, 2) AS avg_ms
FROM pg_stat_statements
ORDER BY mean_exec_time DESC
LIMIT 5;
"

echo -e "\n=== Fin del reporte ==="
```

**Ejecutar diariamente con cron:**
```bash
chmod +x monitor_postgres.sh

# Agregar a crontab (ejecutar cada día a las 8 AM)
crontab -e
# Agregar línea:
0 8 * * * /path/to/monitor_postgres.sh >> /var/log/postgres_monitor.log 2>&1
```

---

## Scripts de Utilidad

### Script 1: Reindexar Tablas con Bloat

```sql
-- Reindexar tablas con fragmentación
-- PRECAUCIÓN: REINDEX bloquea escrituras
DO $$
DECLARE
    r RECORD;
BEGIN
    FOR r IN
        SELECT schemaname, tablename
        FROM pg_stat_user_tables
        WHERE ROUND(100.0 * n_dead_tup / NULLIF(n_live_tup, 0), 2) > 20
    LOOP
        RAISE NOTICE 'Reindexing table: %.%', r.schemaname, r.tablename;
        EXECUTE 'REINDEX TABLE ' || quote_ident(r.schemaname) || '.' || quote_ident(r.tablename);
    END LOOP;
END $$;
```

---

### Script 2: Backup Automático

```bash
#!/bin/bash
# backup_postgres.sh

TIMESTAMP=$(date +%Y%m%d_%H%M%S)
BACKUP_DIR="/backups/postgres"
PGUSER="postgres"

# Crear directorio si no existe
mkdir -p $BACKUP_DIR

# Backup de FastServer_Logs (retención 30 días)
pg_dump -U $PGUSER -F custom -f $BACKUP_DIR/fastserver_logs_$TIMESTAMP.backup FastServer_Logs

# Backup de FastServer (retención indefinida)
pg_dump -U $PGUSER -F custom -f $BACKUP_DIR/fastserver_$TIMESTAMP.backup FastServer

# Limpiar backups antiguos de logs (>30 días)
find $BACKUP_DIR/fastserver_logs_* -mtime +30 -delete

echo "Backup completado: $TIMESTAMP"
```

**Ejecutar diariamente con cron:**
```bash
0 2 * * * /path/to/backup_postgres.sh >> /var/log/postgres_backup.log 2>&1
```

---

### Script 3: Verificar Índices Faltantes

```sql
-- Sugerir índices para queries frecuentes
SELECT
    schemaname,
    tablename,
    seq_scan,                                  -- Número de scans secuenciales (lentos)
    seq_tup_read,                              -- Tuplas leídas en scans secuenciales
    idx_scan,                                  -- Número de scans con índice (rápidos)
    seq_tup_read / NULLIF(seq_scan, 0) AS avg_seq_tup_per_scan,
    'CREATE INDEX ON ' || schemaname || '.' || tablename || ' (...);' AS suggestion
FROM pg_stat_user_tables
WHERE seq_scan > 1000                          -- Tabla con muchos scans secuenciales
  AND seq_tup_read / NULLIF(seq_scan, 0) > 10000  -- Scans que leen muchas filas
  AND idx_scan < seq_scan                      -- Más scans secuenciales que con índice
ORDER BY seq_tup_read DESC
LIMIT 10;
```

---

## Dashboard Recomendado (Grafana + Prometheus)

Para monitoreo en tiempo real, instalar:

1. **Prometheus** - Recolector de métricas
2. **postgres_exporter** - Exportador de métricas de PostgreSQL
3. **Grafana** - Visualización de métricas

### Instalación Rápida (Docker)

```yaml
# docker-compose.yml
version: '3'
services:
  prometheus:
    image: prom/prometheus
    ports:
      - "9090:9090"
    volumes:
      - ./prometheus.yml:/etc/prometheus/prometheus.yml

  postgres-exporter:
    image: prometheuscommunity/postgres-exporter
    environment:
      DATA_SOURCE_NAME: "postgresql://postgres:Souma@host.docker.internal:5432/FastServer_Logs?sslmode=disable"
    ports:
      - "9187:9187"

  grafana:
    image: grafana/grafana
    ports:
      - "3000:3000"
    environment:
      GF_SECURITY_ADMIN_PASSWORD: admin
```

```bash
docker-compose up -d
```

**Acceder a Grafana:** http://localhost:3000 (admin/admin)

**Importar Dashboard:** https://grafana.com/grafana/dashboards/9628 (PostgreSQL Database)

---

## Checklist de Optimización

### ✅ Configuración Inicial
- [ ] Instalar extensión `pg_stat_statements`
- [ ] Configurar `auto_explain` para queries > 1s
- [ ] Ajustar `shared_buffers` a 25% de RAM
- [ ] Configurar `autovacuum` agresivo para tablas de logs
- [ ] Configurar logging de queries lentas

### ✅ Monitoreo Semanal
- [ ] Revisar Top 10 queries lentas
- [ ] Verificar cache hit ratio (>95%)
- [ ] Identificar índices no utilizados
- [ ] Verificar tablas con bloat >20%
- [ ] Revisar conexiones bloqueadas

### ✅ Mantenimiento Mensual
- [ ] VACUUM ANALYZE en todas las tablas
- [ ] REINDEX tablas con fragmentación >30%
- [ ] Revisar y eliminar índices duplicados
- [ ] Actualizar estadísticas del planner
- [ ] Backup completo y prueba de restore

### ✅ Auditoría Trimestral
- [ ] Revisar configuración de postgresql.conf
- [ ] Análisis de tendencias de crecimiento
- [ ] Planificación de capacidad (disco, RAM, CPU)
- [ ] Revisar políticas de retención de datos
- [ ] Actualizar PostgreSQL a última versión estable

---

## Recursos Adicionales

- **Documentación Oficial:** https://www.postgresql.org/docs/14/index.html
- **PgBouncer:** https://www.pgbouncer.org/
- **pg_stat_statements:** https://www.postgresql.org/docs/14/pgstatstatements.html
- **Postgres Wiki:** https://wiki.postgresql.org/wiki/Main_Page
- **Explain Visualizer:** https://explain.dalibo.com/

---

## Conclusión

Este documento proporciona las herramientas necesarias para:

✅ **Monitorear** performance de PostgreSQL en tiempo real
✅ **Identificar** queries lentas y cuellos de botella
✅ **Optimizar** índices y configuración
✅ **Mantener** bases de datos saludables con VACUUM
✅ **Escalar** con confidence usando métricas objetivas

**Para FastServer en el banco:**
- Ejecuta el script de monitoreo diario
- Revisa métricas semanalmente
- Aplica optimizaciones según hallazgos
- Documenta cambios y resultados

**Contacto:** Equipo de DevOps/DBA
**Última actualización:** Febrero 2026
