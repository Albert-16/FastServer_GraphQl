# Script para generar migraciones limpias de producción
# Ejecutar SOLO cuando se prepare el deployment para el banco

param(
    [switch]$PostgreSQL,
    [switch]$SqlServer,
    [switch]$Both
)

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host " Generador de Migraciones de Producción" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Verificar que estamos en el directorio correcto
if (-not (Test-Path "src/FastServer.Infrastructure/FastServer.Infrastructure.csproj")) {
    Write-Host "ERROR: Ejecutar desde la raíz del proyecto FastServer" -ForegroundColor Red
    exit 1
}

# Crear carpeta para scripts de producción
$productionDir = "scripts/production-migrations"
if (-not (Test-Path $productionDir)) {
    New-Item -ItemType Directory -Path $productionDir | Out-Null
    Write-Host "✓ Carpeta creada: $productionDir" -ForegroundColor Green
}

# Backup de migraciones actuales
$backupDir = "scripts/backup-migrations-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
Write-Host "Creando backup de migraciones actuales..." -ForegroundColor Yellow
New-Item -ItemType Directory -Path $backupDir | Out-Null
Copy-Item -Path "src/FastServer.Infrastructure/Data/Migrations/*" -Destination $backupDir -Recurse
Write-Host "✓ Backup creado en: $backupDir" -ForegroundColor Green
Write-Host ""

# Función para generar migración limpia
function Generate-CleanMigration {
    param(
        [string]$Context,
        [string]$MigrationName,
        [string]$OutputDir
    )

    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host " Generando migración limpia: $Context" -ForegroundColor Cyan
    Write-Host "========================================" -ForegroundColor Cyan

    $fullOutputDir = "Data/Migrations/$OutputDir"

    # Eliminar migraciones existentes del contexto
    Write-Host "1. Eliminando migraciones antiguas de $OutputDir..." -ForegroundColor Yellow
    $migrationsPath = "src/FastServer.Infrastructure/$fullOutputDir"
    if (Test-Path $migrationsPath) {
        Get-ChildItem -Path $migrationsPath -Filter "*.cs" | Where-Object { $_.Name -notlike "*ModelSnapshot.cs" } | Remove-Item -Force
        Write-Host "   ✓ Migraciones antiguas eliminadas" -ForegroundColor Green
    }

    # Generar nueva migración limpia
    Write-Host "2. Generando migración limpia..." -ForegroundColor Yellow
    Push-Location "src/FastServer.Infrastructure"

    try {
        $result = dotnet ef migrations add $MigrationName `
            --context $Context `
            --output-dir $fullOutputDir `
            --project FastServer.Infrastructure.csproj `
            --startup-project ../FastServer.GraphQL.Api/FastServer.GraphQL.Api.csproj 2>&1

        if ($LASTEXITCODE -eq 0) {
            Write-Host "   ✓ Migración generada exitosamente" -ForegroundColor Green
        } else {
            Write-Host "   ✗ Error generando migración" -ForegroundColor Red
            Write-Host $result
            throw "Error en dotnet ef migrations add"
        }
    }
    finally {
        Pop-Location
    }

    # Generar script SQL idempotente
    Write-Host "3. Generando script SQL idempotente..." -ForegroundColor Yellow
    Push-Location "src/FastServer.GraphQL.Api"

    try {
        $sqlFile = "../../$productionDir/$($OutputDir.ToLower())-initial.sql"
        $result = dotnet ef migrations script `
            --context $Context `
            --idempotent `
            --output $sqlFile `
            --project ../FastServer.Infrastructure/FastServer.Infrastructure.csproj 2>&1

        if ($LASTEXITCODE -eq 0) {
            Write-Host "   ✓ Script SQL generado: $sqlFile" -ForegroundColor Green
        } else {
            Write-Host "   ✗ Error generando script SQL" -ForegroundColor Red
            Write-Host $result
            throw "Error en dotnet ef migrations script"
        }
    }
    finally {
        Pop-Location
    }

    Write-Host ""
}

# Generar migraciones según parámetros
if ($PostgreSQL -or $Both) {
    Generate-CleanMigration -Context "PostgreSqlDbContext" -MigrationName "InitialProductionCreate" -OutputDir "PostgreSql"
}

if ($SqlServer -or $Both) {
    Generate-CleanMigration -Context "SqlServerDbContext" -MigrationName "InitialProductionCreate" -OutputDir "SqlServer"
}

# Resumen
Write-Host "========================================" -ForegroundColor Cyan
Write-Host " RESUMEN" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "✓ Migraciones limpias generadas" -ForegroundColor Green
Write-Host "✓ Scripts SQL listos en: $productionDir" -ForegroundColor Green
Write-Host "✓ Backup de migraciones antiguas en: $backupDir" -ForegroundColor Green
Write-Host ""
Write-Host "SIGUIENTE PASO:" -ForegroundColor Yellow
Write-Host "1. Revisar los scripts SQL generados" -ForegroundColor White
Write-Host "2. Probar en ambiente de staging/desarrollo" -ForegroundColor White
Write-Host "3. Ejecutar en el servidor del banco" -ForegroundColor White
Write-Host ""
Write-Host "IMPORTANTE:" -ForegroundColor Red
Write-Host "- NO aplicar estas migraciones en tu PC de desarrollo" -ForegroundColor White
Write-Host "- Estas migraciones son SOLO para instalación limpia en producción" -ForegroundColor White
Write-Host ""

# Mostrar contenido de scripts generados
Write-Host "Archivos generados:" -ForegroundColor Cyan
Get-ChildItem -Path $productionDir -Filter "*.sql" | ForEach-Object {
    $size = [math]::Round($_.Length / 1KB, 2)
    Write-Host "  - $($_.Name) ($size KB)" -ForegroundColor White
}
Write-Host ""
