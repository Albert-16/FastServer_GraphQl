# ============================================================================
# Script de Limpieza de Bases de Datos - FastServer
# ============================================================================
# Elimina las bases de datos FastServerLogsDB y FastServerMicroservicesDB
# para preparar una instalación limpia
# ============================================================================

Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host "  FastServer - Limpieza de Bases de Datos" -ForegroundColor Cyan
Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host ""

# Leer configuración de appsettings.json
$appsettingsPath = "C:\Users\MSI MAG\FastServer\src\FastServer.GraphQL.Api\appsettings.json"
$appsettings = Get-Content $appsettingsPath | ConvertFrom-Json

# Extraer cadenas de conexión
$postgresConn = $appsettings.ConnectionStrings.PostgreSQL
$sqlServerConn = $appsettings.ConnectionStrings.SqlServer

Write-Host "Configuración detectada:" -ForegroundColor Yellow
Write-Host "  PostgreSQL: $postgresConn" -ForegroundColor Gray
Write-Host "  SQL Server: $sqlServerConn" -ForegroundColor Gray
Write-Host ""

# ============================================================================
# Eliminar Base de Datos PostgreSQL
# ============================================================================

Write-Host "[1/2] Eliminando base de datos PostgreSQL: FastServerLogsDB" -ForegroundColor Yellow

try {
    # Extraer parámetros de conexión
    $pgParams = @{}
    $postgresConn -split ';' | ForEach-Object {
        $pair = $_ -split '='
        if ($pair.Length -eq 2) {
            $pgParams[$pair[0].Trim()] = $pair[1].Trim()
        }
    }

    $pgHost = $pgParams['Host']
    $pgPort = $pgParams['Port']
    $pgUser = $pgParams['Username']
    $pgPassword = $pgParams['Password']
    $pgDatabase = $pgParams['Database']

    Write-Host "  Conectando a PostgreSQL ($pgHost:$pgPort)..." -ForegroundColor Gray

    # Comando SQL para eliminar la base de datos
    $dropDbSql = @"
-- Desconectar usuarios activos
SELECT pg_terminate_backend(pg_stat_activity.pid)
FROM pg_stat_activity
WHERE pg_stat_activity.datname = '$pgDatabase'
  AND pid <> pg_backend_pid();

-- Eliminar la base de datos
DROP DATABASE IF EXISTS `"$pgDatabase`";
"@

    # Crear archivo temporal con el SQL
    $tempSqlFile = [System.IO.Path]::GetTempFileName() + ".sql"
    Set-Content -Path $tempSqlFile -Value $dropDbSql -Encoding UTF8

    # Ejecutar con psql (requiere PostgreSQL client instalado)
    $env:PGPASSWORD = $pgPassword
    $psqlResult = & psql -h $pgHost -p $pgPort -U $pgUser -d postgres -f $tempSqlFile 2>&1

    Remove-Item $tempSqlFile -ErrorAction SilentlyContinue

    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ✓ Base de datos PostgreSQL eliminada exitosamente" -ForegroundColor Green
    } else {
        Write-Host "  ⚠ No se pudo eliminar la BD PostgreSQL (puede no existir)" -ForegroundColor Yellow
        Write-Host "    Detalles: $psqlResult" -ForegroundColor Gray
    }

} catch {
    Write-Host "  ⚠ Error al eliminar PostgreSQL: $($_.Exception.Message)" -ForegroundColor Yellow
    Write-Host "    Nota: Si psql no está instalado, elimina la BD manualmente" -ForegroundColor Gray
}

Write-Host ""

# ============================================================================
# Eliminar Base de Datos SQL Server
# ============================================================================

Write-Host "[2/2] Eliminando base de datos SQL Server: FastServerMicroservicesDB" -ForegroundColor Yellow

try {
    # Extraer parámetros de conexión
    $sqlParams = @{}
    $sqlServerConn -split ';' | ForEach-Object {
        $pair = $_ -split '='
        if ($pair.Length -eq 2) {
            $sqlParams[$pair[0].Trim()] = $pair[1].Trim()
        }
    }

    $sqlServer = $sqlParams['Server']
    $sqlDatabase = $sqlParams['Database']
    $sqlUser = if ($sqlParams.ContainsKey('User ID')) { $sqlParams['User ID'] } else { $null }
    $sqlPassword = if ($sqlParams.ContainsKey('Password')) { $sqlParams['Password'] } else { $null }
    $trustedConn = $sqlParams['Trusted_Connection'] -eq 'True'

    Write-Host "  Conectando a SQL Server ($sqlServer)..." -ForegroundColor Gray

    # Comando SQL para eliminar la base de datos
    $dropDbSql = @"
USE master;
GO

-- Poner la BD en modo SINGLE_USER para desconectar usuarios
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'$sqlDatabase')
BEGIN
    ALTER DATABASE [$sqlDatabase] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$sqlDatabase];
    PRINT 'Base de datos eliminada exitosamente';
END
ELSE
BEGIN
    PRINT 'La base de datos no existe';
END
GO
"@

    # Ejecutar con sqlcmd
    $tempSqlFile = [System.IO.Path]::GetTempFileName() + ".sql"
    Set-Content -Path $tempSqlFile -Value $dropDbSql -Encoding UTF8

    if ($trustedConn) {
        # Autenticación Windows
        $sqlcmdResult = & sqlcmd -S $sqlServer -E -i $tempSqlFile 2>&1
    } else {
        # Autenticación SQL
        $sqlcmdResult = & sqlcmd -S $sqlServer -U $sqlUser -P $sqlPassword -i $tempSqlFile 2>&1
    }

    Remove-Item $tempSqlFile -ErrorAction SilentlyContinue

    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ✓ Base de datos SQL Server eliminada exitosamente" -ForegroundColor Green
    } else {
        Write-Host "  ⚠ No se pudo eliminar la BD SQL Server (puede no existir)" -ForegroundColor Yellow
        Write-Host "    Detalles: $sqlcmdResult" -ForegroundColor Gray
    }

} catch {
    Write-Host "  ⚠ Error al eliminar SQL Server: $($_.Exception.Message)" -ForegroundColor Yellow
    Write-Host "    Nota: Si sqlcmd no está instalado, elimina la BD manualmente" -ForegroundColor Gray
}

Write-Host ""
Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host "  Limpieza Completada" -ForegroundColor Cyan
Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Próximos pasos:" -ForegroundColor Yellow
Write-Host "  1. Ejecutar: .\scripts\reset-migrations.ps1" -ForegroundColor White
Write-Host "  2. Ejecutar: .\scripts\install-database.ps1" -ForegroundColor White
Write-Host ""
