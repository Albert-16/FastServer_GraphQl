# ============================================================================
# Script de Reset de Migraciones - FastServer
# ============================================================================
# Elimina todas las migraciones existentes y crea una migración inicial limpia
# ============================================================================

Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host "  FastServer - Reset de Migraciones" -ForegroundColor Cyan
Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host ""

$projectPath = "C:\Users\MSI MAG\FastServer"
$infraPath = "$projectPath\src\FastServer.Infrastructure"
$apiPath = "$projectPath\src\FastServer.GraphQL.Api"

# ============================================================================
# Paso 1: Eliminar migraciones existentes
# ============================================================================

Write-Host "[1/3] Eliminando migraciones existentes..." -ForegroundColor Yellow

# PostgreSQL
$pgMigrationsPath = "$infraPath\Data\Migrations\PostgreSql"
if (Test-Path $pgMigrationsPath) {
    Write-Host "  Eliminando migraciones de PostgreSQL..." -ForegroundColor Gray
    Get-ChildItem -Path $pgMigrationsPath -Filter "*.cs" | Remove-Item -Force
    Write-Host "  ✓ Migraciones de PostgreSQL eliminadas" -ForegroundColor Green
}

# SQL Server
$sqlMigrationsPath = "$infraPath\Data\Migrations\SqlServer"
if (Test-Path $sqlMigrationsPath) {
    Write-Host "  Eliminando migraciones de SQL Server..." -ForegroundColor Gray
    Get-ChildItem -Path $sqlMigrationsPath -Filter "*.cs" | Remove-Item -Force
    Write-Host "  ✓ Migraciones de SQL Server eliminadas" -ForegroundColor Green
}

Write-Host ""

# ============================================================================
# Paso 2: Crear nueva migración inicial para PostgreSQL
# ============================================================================

Write-Host "[2/3] Creando migración inicial para PostgreSQL..." -ForegroundColor Yellow

Push-Location $infraPath

try {
    $result = dotnet ef migrations add PostgreSqlInitialCreate `
        --context PostgreSqlDbContext `
        --output-dir Data\Migrations\PostgreSql `
        --project $infraPath `
        --startup-project $apiPath `
        2>&1

    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ✓ Migración inicial de PostgreSQL creada" -ForegroundColor Green
    } else {
        Write-Host "  ✗ Error al crear migración de PostgreSQL" -ForegroundColor Red
        Write-Host "    $result" -ForegroundColor Gray
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
# Paso 3: Crear nueva migración inicial para SQL Server
# ============================================================================

Write-Host "[3/3] Creando migración inicial para SQL Server..." -ForegroundColor Yellow

try {
    $result = dotnet ef migrations add SqlServerInitialCreate `
        --context SqlServerDbContext `
        --output-dir Data\Migrations\SqlServer `
        --project $infraPath `
        --startup-project $apiPath `
        2>&1

    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ✓ Migración inicial de SQL Server creada" -ForegroundColor Green
    } else {
        Write-Host "  ✗ Error al crear migración de SQL Server" -ForegroundColor Red
        Write-Host "    $result" -ForegroundColor Gray
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
Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host "  Migraciones Reseteadas Exitosamente" -ForegroundColor Cyan
Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host ""

# ============================================================================
# Verificar archivos creados
# ============================================================================

Write-Host "Archivos de migración creados:" -ForegroundColor Yellow
Write-Host ""

Write-Host "PostgreSQL:" -ForegroundColor Cyan
Get-ChildItem -Path $pgMigrationsPath -Filter "*.cs" | ForEach-Object {
    Write-Host "  - $($_.Name)" -ForegroundColor White
}

Write-Host ""
Write-Host "SQL Server:" -ForegroundColor Cyan
Get-ChildItem -Path $sqlMigrationsPath -Filter "*.cs" | ForEach-Object {
    Write-Host "  - $($_.Name)" -ForegroundColor White
}

Write-Host ""
Write-Host "Próximo paso:" -ForegroundColor Yellow
Write-Host "  Ejecutar: .\scripts\install-database.ps1" -ForegroundColor White
Write-Host ""
