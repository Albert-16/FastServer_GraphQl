# Script PowerShell para ejecutar el script SQL de índices

$env:PGPASSWORD = "Souma"
& "C:\Program Files\PostgreSQL\17\bin\psql.exe" -U postgres -d FastServerLogs_Dev -f "create-postgresql-indexes.sql"

if ($LASTEXITCODE -eq 0) {
    Write-Host "Índices creados exitosamente" -ForegroundColor Green
} else {
    Write-Host "Error creando índices" -ForegroundColor Red
}
