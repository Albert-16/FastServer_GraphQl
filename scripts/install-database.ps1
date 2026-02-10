# ============================================================================
# Script de Instalación de Base de Datos - FastServer
# ============================================================================
# Crea las bases de datos y aplica todas las migraciones
# USO: Ejecutar en el servidor del banco después de configurar appsettings.json
# ============================================================================

param(
    [switch]$SkipBackup,
    [switch]$Force
)

Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host "  FastServer - Instalación de Base de Datos" -ForegroundColor Cyan
Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host ""

$projectPath = Split-Path -Parent $PSScriptRoot
$infraPath = "$projectPath\src\FastServer.Infrastructure"
$apiPath = "$projectPath\src\FastServer.GraphQL.Api"
$appsettingsPath = "$apiPath\appsettings.json"

# ============================================================================
# Validaciones Previas
# ============================================================================

Write-Host "[Validación] Verificando prerequisitos..." -ForegroundColor Yellow

# Verificar que appsettings.json existe
if (-not (Test-Path $appsettingsPath)) {
    Write-Host "  ✗ Error: No se encontró appsettings.json en $apiPath" -ForegroundColor Red
    exit 1
}

# Verificar que dotnet ef está instalado
$efVersion = dotnet ef --version 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "  ✗ Error: dotnet ef no está instalado" -ForegroundColor Red
    Write-Host "  Instalar con: dotnet tool install --global dotnet-ef" -ForegroundColor Yellow
    exit 1
}

Write-Host "  ✓ dotnet ef versión: $efVersion" -ForegroundColor Green

# Leer configuración
$appsettings = Get-Content $appsettingsPath | ConvertFrom-Json
$postgresConn = $appsettings.ConnectionStrings.PostgreSQL
$sqlServerConn = $appsettings.ConnectionStrings.SqlServer

Write-Host "  ✓ Configuración cargada desde appsettings.json" -ForegroundColor Green
Write-Host ""

# ============================================================================
# Mostrar Configuración
# ============================================================================

Write-Host "Configuración de conexiones:" -ForegroundColor Cyan
Write-Host "  PostgreSQL: " -NoNewline -ForegroundColor White
Write-Host $postgresConn -ForegroundColor Gray
Write-Host "  SQL Server: " -NoNewline -ForegroundColor White
Write-Host $sqlServerConn -ForegroundColor Gray
Write-Host ""

# ============================================================================
# Confirmación del Usuario
# ============================================================================

if (-not $Force) {
    Write-Host "⚠️  ADVERTENCIA" -ForegroundColor Yellow
    Write-Host "Esta operación creará las bases de datos y ejecutará migraciones." -ForegroundColor Yellow
    Write-Host ""
    $confirm = Read-Host "¿Desea continuar? (S/N)"

    if ($confirm -ne 'S' -and $confirm -ne 's' -and $confirm -ne 'Y' -and $confirm -ne 'y') {
        Write-Host "Instalación cancelada por el usuario" -ForegroundColor Yellow
        exit 0
    }
    Write-Host ""
}

# ============================================================================
# Paso 1: Aplicar Migraciones a PostgreSQL
# ============================================================================

Write-Host "[1/2] Aplicando migraciones a PostgreSQL..." -ForegroundColor Yellow
Write-Host "  Base de datos: FastServerLogsDB" -ForegroundColor Gray
Write-Host ""

Push-Location $infraPath

try {
    $result = dotnet ef database update `
        --context PostgreSqlDbContext `
        --project $infraPath `
        --startup-project $apiPath `
        --verbose `
        2>&1

    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ✓ Migraciones de PostgreSQL aplicadas exitosamente" -ForegroundColor Green
    } else {
        Write-Host "  ✗ Error al aplicar migraciones de PostgreSQL" -ForegroundColor Red
        Write-Host ""
        Write-Host "Detalles del error:" -ForegroundColor Yellow
        Write-Host $result -ForegroundColor Gray
        Pop-Location
        exit 1
    }
} catch {
    Write-Host "  ✗ Excepción: $($_.Exception.Message)" -ForegroundColor Red
    Pop-Location
    exit 1
}

Write-Host ""

# ============================================================================
# Paso 2: Aplicar Migraciones a SQL Server
# ============================================================================

Write-Host "[2/2] Aplicando migraciones a SQL Server..." -ForegroundColor Yellow
Write-Host "  Base de datos: FastServerMicroservicesDB" -ForegroundColor Gray
Write-Host ""

try {
    $result = dotnet ef database update `
        --context SqlServerDbContext `
        --project $infraPath `
        --startup-project $apiPath `
        --verbose `
        2>&1

    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ✓ Migraciones de SQL Server aplicadas exitosamente" -ForegroundColor Green
    } else {
        Write-Host "  ✗ Error al aplicar migraciones de SQL Server" -ForegroundColor Red
        Write-Host ""
        Write-Host "Detalles del error:" -ForegroundColor Yellow
        Write-Host $result -ForegroundColor Gray
        Pop-Location
        exit 1
    }
} catch {
    Write-Host "  ✗ Excepción: $($_.Exception.Message)" -ForegroundColor Red
    Pop-Location
    exit 1
}

Pop-Location

Write-Host ""

# ============================================================================
# Verificación Post-Instalación
# ============================================================================

Write-Host "[Verificación] Comprobando instalación..." -ForegroundColor Yellow

# Compilar el proyecto
Push-Location $apiPath

$buildResult = dotnet build --configuration Release 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "  ✓ Proyecto compilado correctamente" -ForegroundColor Green
} else {
    Write-Host "  ⚠ Error al compilar el proyecto" -ForegroundColor Yellow
}

Pop-Location

Write-Host ""

# ============================================================================
# Resumen Final
# ============================================================================

Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host "  ✓ Instalación Completada Exitosamente" -ForegroundColor Green
Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Bases de datos creadas:" -ForegroundColor Cyan
Write-Host "  ✓ PostgreSQL: FastServerLogsDB" -ForegroundColor White
Write-Host "  ✓ SQL Server: FastServerMicroservicesDB" -ForegroundColor White
Write-Host ""

Write-Host "Tablas creadas en PostgreSQL:" -ForegroundColor Cyan
Write-Host "  - LogServicesHeaders" -ForegroundColor White
Write-Host "  - LogMicroservices" -ForegroundColor White
Write-Host "  - LogServicesContents" -ForegroundColor White
Write-Host ""

Write-Host "Tablas creadas en SQL Server:" -ForegroundColor Cyan
Write-Host "  - MicroserviceRegisters" -ForegroundColor White
Write-Host "  - MicroservicesClusters" -ForegroundColor White
Write-Host "  - Users" -ForegroundColor White
Write-Host "  - ActivityLogs" -ForegroundColor White
Write-Host "  - EventTypes" -ForegroundColor White
Write-Host "  - CoreConnectorCredentials" -ForegroundColor White
Write-Host "  - MicroserviceCoreConnectors" -ForegroundColor White
Write-Host ""

Write-Host "Próximos pasos:" -ForegroundColor Yellow
Write-Host "  1. Iniciar la API: cd src\FastServer.GraphQL.Api && dotnet run" -ForegroundColor White
Write-Host "  2. Acceder a GraphQL: http://localhost:64707/graphql" -ForegroundColor White
Write-Host "  3. Verificar endpoints de salud: http://localhost:64707/health" -ForegroundColor White
Write-Host ""

Write-Host "Nota para el servidor del banco:" -ForegroundColor Yellow
Write-Host "  - Configurar firewall para permitir puertos 64706-64707" -ForegroundColor Gray
Write-Host "  - Configurar HTTPS con certificado del banco" -ForegroundColor Gray
Write-Host "  - Configurar cadenas de conexión en appsettings.Production.json" -ForegroundColor Gray
Write-Host ""
