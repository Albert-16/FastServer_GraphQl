-- Script de creación de índices para PostgreSQL
-- FastServer Logs Database

\timing on

-- ==========================================
-- Índices para FastServer_LogServices_Header
-- ==========================================

-- Para ordenamiento y paginación
CREATE INDEX IF NOT EXISTS ix_logservices_logdatein
ON "FastServer_LogServices_Header"(fastserver_log_date_in DESC);

-- Para filtros comunes
CREATE INDEX IF NOT EXISTS ix_logservices_state
ON "FastServer_LogServices_Header"(fastserver_log_state);

CREATE INDEX IF NOT EXISTS ix_logservices_microservice
ON "FastServer_LogServices_Header"(fastserver_microservice_name);

CREATE INDEX IF NOT EXISTS ix_logservices_userid
ON "FastServer_LogServices_Header"(fastserver_user_id);

-- Índice compuesto para consultas complejas
CREATE INDEX IF NOT EXISTS ix_logservices_datestate
ON "FastServer_LogServices_Header"(fastserver_log_date_in DESC, fastserver_log_state);

-- Optimización específica PostgreSQL: índice parcial para logs completados
CREATE INDEX IF NOT EXISTS ix_logservices_active_logs
ON "FastServer_LogServices_Header"(fastserver_log_date_in DESC)
WHERE fastserver_log_state = 2; -- Solo logs completados

-- ==========================================
-- VACUUM ANALYZE para optimizar estadísticas
-- ==========================================

VACUUM ANALYZE "FastServer_LogServices_Header";

-- ==========================================
-- Verificar índices creados
-- ==========================================

SELECT
    schemaname,
    tablename,
    indexname,
    pg_size_pretty(pg_relation_size(indexrelid)) AS index_size
FROM pg_stat_user_indexes
WHERE tablename = 'FastServer_LogServices_Header'
ORDER BY indexname;

\echo 'Índices creados exitosamente'
