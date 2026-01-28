#!/bin/bash
# Script para generar migraciones limpias de producción
# Ejecutar SOLO cuando se prepare el deployment para el banco

set -e

# Colores
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

echo -e "${CYAN}========================================${NC}"
echo -e "${CYAN} Generador de Migraciones de Producción${NC}"
echo -e "${CYAN}========================================${NC}"
echo ""

# Verificar que estamos en el directorio correcto
if [ ! -f "src/FastServer.Infrastructure/FastServer.Infrastructure.csproj" ]; then
    echo -e "${RED}ERROR: Ejecutar desde la raíz del proyecto FastServer${NC}"
    exit 1
fi

# Parsear argumentos
GENERATE_POSTGRESQL=false
GENERATE_SQLSERVER=false

while [[ $# -gt 0 ]]; do
    case $1 in
        --postgresql)
            GENERATE_POSTGRESQL=true
            shift
            ;;
        --sqlserver)
            GENERATE_SQLSERVER=true
            shift
            ;;
        --both)
            GENERATE_POSTGRESQL=true
            GENERATE_SQLSERVER=true
            shift
            ;;
        *)
            echo -e "${RED}Argumento desconocido: $1${NC}"
            echo "Uso: $0 [--postgresql] [--sqlserver] [--both]"
            exit 1
            ;;
    esac
done

# Si no se especificó nada, generar ambos
if [ "$GENERATE_POSTGRESQL" = false ] && [ "$GENERATE_SQLSERVER" = false ]; then
    echo "Generando migraciones para ambas bases de datos..."
    GENERATE_POSTGRESQL=true
    GENERATE_SQLSERVER=true
fi

# Crear carpeta para scripts de producción
PRODUCTION_DIR="scripts/production-migrations"
if [ ! -d "$PRODUCTION_DIR" ]; then
    mkdir -p "$PRODUCTION_DIR"
    echo -e "${GREEN}✓ Carpeta creada: $PRODUCTION_DIR${NC}"
fi

# Backup de migraciones actuales
BACKUP_DIR="scripts/backup-migrations-$(date +%Y%m%d-%H%M%S)"
echo -e "${YELLOW}Creando backup de migraciones actuales...${NC}"
mkdir -p "$BACKUP_DIR"
cp -r src/FastServer.Infrastructure/Data/Migrations/* "$BACKUP_DIR/" 2>/dev/null || true
echo -e "${GREEN}✓ Backup creado en: $BACKUP_DIR${NC}"
echo ""

# Función para generar migración limpia
generate_clean_migration() {
    local CONTEXT=$1
    local MIGRATION_NAME=$2
    local OUTPUT_DIR=$3

    echo -e "${CYAN}========================================${NC}"
    echo -e "${CYAN} Generando migración limpia: $CONTEXT${NC}"
    echo -e "${CYAN}========================================${NC}"

    local FULL_OUTPUT_DIR="Data/Migrations/$OUTPUT_DIR"
    local MIGRATIONS_PATH="src/FastServer.Infrastructure/$FULL_OUTPUT_DIR"

    # Eliminar migraciones existentes del contexto
    echo -e "${YELLOW}1. Eliminando migraciones antiguas de $OUTPUT_DIR...${NC}"
    if [ -d "$MIGRATIONS_PATH" ]; then
        find "$MIGRATIONS_PATH" -type f -name "*.cs" ! -name "*ModelSnapshot.cs" -delete
        echo -e "   ${GREEN}✓ Migraciones antiguas eliminadas${NC}"
    fi

    # Generar nueva migración limpia
    echo -e "${YELLOW}2. Generando migración limpia...${NC}"
    cd src/FastServer.Infrastructure

    if dotnet ef migrations add "$MIGRATION_NAME" \
        --context "$CONTEXT" \
        --output-dir "$FULL_OUTPUT_DIR" \
        --project FastServer.Infrastructure.csproj \
        --startup-project ../FastServer.GraphQL.Api/FastServer.GraphQL.Api.csproj > /dev/null 2>&1; then
        echo -e "   ${GREEN}✓ Migración generada exitosamente${NC}"
    else
        echo -e "   ${RED}✗ Error generando migración${NC}"
        cd ../..
        exit 1
    fi

    cd ../..

    # Generar script SQL idempotente
    echo -e "${YELLOW}3. Generando script SQL idempotente...${NC}"
    cd src/FastServer.GraphQL.Api

    local SQL_FILE="../../$PRODUCTION_DIR/$(echo $OUTPUT_DIR | tr '[:upper:]' '[:lower:]')-initial.sql"

    if dotnet ef migrations script \
        --context "$CONTEXT" \
        --idempotent \
        --output "$SQL_FILE" \
        --project ../FastServer.Infrastructure/FastServer.Infrastructure.csproj > /dev/null 2>&1; then
        echo -e "   ${GREEN}✓ Script SQL generado: $SQL_FILE${NC}"
    else
        echo -e "   ${RED}✗ Error generando script SQL${NC}"
        cd ../..
        exit 1
    fi

    cd ../..
    echo ""
}

# Generar migraciones según parámetros
if [ "$GENERATE_POSTGRESQL" = true ]; then
    generate_clean_migration "PostgreSqlDbContext" "InitialProductionCreate" "PostgreSql"
fi

if [ "$GENERATE_SQLSERVER" = true ]; then
    generate_clean_migration "SqlServerDbContext" "InitialProductionCreate" "SqlServer"
fi

# Resumen
echo -e "${CYAN}========================================${NC}"
echo -e "${CYAN} RESUMEN${NC}"
echo -e "${CYAN}========================================${NC}"
echo ""
echo -e "${GREEN}✓ Migraciones limpias generadas${NC}"
echo -e "${GREEN}✓ Scripts SQL listos en: $PRODUCTION_DIR${NC}"
echo -e "${GREEN}✓ Backup de migraciones antiguas en: $BACKUP_DIR${NC}"
echo ""
echo -e "${YELLOW}SIGUIENTE PASO:${NC}"
echo -e "1. Revisar los scripts SQL generados"
echo -e "2. Probar en ambiente de staging/desarrollo"
echo -e "3. Ejecutar en el servidor del banco"
echo ""
echo -e "${RED}IMPORTANTE:${NC}"
echo -e "- NO aplicar estas migraciones en tu PC de desarrollo"
echo -e "- Estas migraciones son SOLO para instalación limpia en producción"
echo ""

# Mostrar contenido de scripts generados
echo -e "${CYAN}Archivos generados:${NC}"
for file in "$PRODUCTION_DIR"/*.sql; do
    if [ -f "$file" ]; then
        size=$(du -h "$file" | cut -f1)
        echo -e "  - $(basename $file) ($size)"
    fi
done
echo ""
