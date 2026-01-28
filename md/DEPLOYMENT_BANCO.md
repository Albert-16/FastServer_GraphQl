# Guía de Instalación Limpia - Servidores del Banco

## ⚠️ IMPORTANTE: Esta guía es para la PRIMERA instalación en los servidores del banco

---

## Estrategia de Migraciones

### Para Desarrollo (Tu PC)
- **Mantener migraciones actuales**: Ya están aplicadas y funcionando
- **NO ejecutar estos pasos** en tu PC de desarrollo

### Para Producción (Banco)
- **Usar migraciones limpias**: Estado final sin historial de cambios
- **Seguir esta guía paso a paso**

---

## Pre-requisitos en el Servidor del Banco

1. ✅ .NET 10.0 SDK instalado
2. ✅ PostgreSQL instalado y corriendo
3. ✅ SQL Server instalado y corriendo (opcional, si se usará)
4. ✅ Conexión a las bases de datos verificada
5. ✅ Usuario con permisos para crear bases de datos

---

## Paso 1: Clonar el Repositorio

```bash
cd /ruta/donde/quieres/instalar
git clone https://github.com/tu-usuario/FastServer.git
cd FastServer
```

---

## Paso 2: Configurar Cadenas de Conexión

### 2.1. Editar `src/FastServer.GraphQL.Api/appsettings.json`

```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=SERVIDOR_BANCO_PG;Port=5432;Database=FastServerLogs;Username=USUARIO_BANCO;Password=PASSWORD_BANCO;SslMode=Require",
    "SqlServer": "Server=SERVIDOR_BANCO_SQL;Database=FastServerLogs;User Id=USUARIO_BANCO;Password=PASSWORD_BANCO;TrustServerCertificate=True;Encrypt=True"
  },
  "DefaultDataSource": "PostgreSQL"
}
```

**IMPORTANTE**:
- Reemplazar `SERVIDOR_BANCO_PG` con el host de PostgreSQL del banco
- Reemplazar `SERVIDOR_BANCO_SQL` con el host de SQL Server del banco
- Usar credenciales reales del banco
- Considerar usar `appsettings.Production.json` para estas configuraciones

### 2.2. Crear `appsettings.Production.json` (RECOMENDADO)

```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=prod-postgres.banco.interno;Port=5432;Database=FastServerLogs;Username=fastserver_app;Password=***;SslMode=Require",
    "SqlServer": "Server=prod-sqlserver.banco.interno;Database=FastServerLogs;User Id=fastserver_app;Password=***;TrustServerCertificate=True;Encrypt=True"
  },
  "DefaultDataSource": "PostgreSQL",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "HotChocolate": "Warning"
    }
  }
}
```

⚠️ **NUNCA commitear passwords reales a Git**

---

## Paso 3: Crear Migraciones Limpias para Producción

### 3.1. Crear carpeta temporal para migraciones de producción

```bash
mkdir -p scripts/production-migrations
```

### 3.2. Generar migración inicial limpia para PostgreSQL

```bash
cd src/FastServer.Infrastructure

# Eliminar snapshot temporal para forzar migración limpia
rm Data/Migrations/PostgreSql/PostgreSqlDbContextModelSnapshot.cs

# Crear migración inicial limpia
dotnet ef migrations add InitialProductionCreate \
  --context PostgreSqlDbContext \
  --output-dir Data/Migrations/PostgreSql \
  --project ../FastServer.Infrastructure.csproj \
  --startup-project ../FastServer.GraphQL.Api/FastServer.GraphQL.Api.csproj
```

### 3.3. Generar migración inicial limpia para SQL Server (si se usa)

```bash
# Si el banco usará SQL Server para microservicios
dotnet ef migrations add InitialProductionCreate \
  --context SqlServerDbContext \
  --output-dir Data/Migrations/SqlServer \
  --project ../FastServer.Infrastructure.csproj \
  --startup-project ../FastServer.GraphQL.Api/FastServer.GraphQL.Api.csproj
```

---

## Paso 4: Aplicar Migraciones en Producción

### Opción A: Usando dotnet ef (Durante la instalación)

```bash
cd src/FastServer.GraphQL.Api

# Aplicar migración PostgreSQL
dotnet ef database update --context PostgreSqlDbContext

# Aplicar migración SQL Server (si se usa)
dotnet ef database update --context SqlServerDbContext
```

### Opción B: Usando Scripts SQL (RECOMENDADO para Producción)

#### 4.1. Generar scripts SQL (en tu PC de desarrollo)

```bash
cd src/FastServer.GraphQL.Api

# Script para PostgreSQL
dotnet ef migrations script \
  --context PostgreSqlDbContext \
  --idempotent \
  --output ../../scripts/production-migrations/postgresql-initial.sql

# Script para SQL Server
dotnet ef migrations script \
  --context SqlServerDbContext \
  --idempotent \
  --output ../../scripts/production-migrations/sqlserver-initial.sql
```

#### 4.2. Revisar scripts generados

**IMPORTANTE**: Antes de ejecutar en producción, revisar los scripts:
- ✅ No contienen DROP de datos existentes
- ✅ Crean todas las tablas necesarias
- ✅ Crean todos los índices
- ✅ NO crean foreign keys en PostgreSQL (correcto)
- ✅ SÍ crean foreign keys en SQL Server (correcto para microservicios)

#### 4.3. Ejecutar scripts en el servidor del banco

**PostgreSQL:**
```bash
psql -h SERVIDOR_BANCO_PG -U USUARIO_BANCO -d FastServerLogs -f scripts/production-migrations/postgresql-initial.sql
```

**SQL Server:**
```bash
sqlcmd -S SERVIDOR_BANCO_SQL -U USUARIO_BANCO -P PASSWORD_BANCO -d FastServerLogs -i scripts/production-migrations/sqlserver-initial.sql
```

---

## Paso 5: Compilar y Publicar la Aplicación

### 5.1. Compilar en modo Release

```bash
cd src/FastServer.GraphQL.Api
dotnet build --configuration Release
```

### 5.2. Publicar la aplicación

```bash
dotnet publish --configuration Release --output ../../publish
```

### 5.3. Copiar archivos al servidor

```bash
# Copiar carpeta publish al servidor del banco
scp -r publish/* usuario@servidor-banco:/opt/fastserver/
```

---

## Paso 6: Configurar Servicio Systemd (Linux)

### 6.1. Crear archivo de servicio

```bash
sudo nano /etc/systemd/system/fastserver.service
```

**Contenido:**
```ini
[Unit]
Description=FastServer GraphQL API
After=network.target

[Service]
Type=notify
WorkingDirectory=/opt/fastserver
ExecStart=/usr/bin/dotnet /opt/fastserver/FastServer.GraphQL.Api.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=fastserver
User=fastserver
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

### 6.2. Habilitar e iniciar el servicio

```bash
sudo systemctl enable fastserver
sudo systemctl start fastserver
sudo systemctl status fastserver
```

---

## Paso 7: Verificar Instalación

### 7.1. Verificar que el servicio está corriendo

```bash
curl http://localhost:64707/graphql
```

**Respuesta esperada**: Error GraphQL (normal, necesita query válido)

### 7.2. Ejecutar query de prueba

```bash
curl -X POST http://localhost:64707/graphql \
  -H "Content-Type: application/json" \
  -d '{"query": "{ health }"}'
```

**Respuesta esperada**: `{"data":{"health":"Healthy"}}`

### 7.3. Verificar que las tablas se crearon

**PostgreSQL:**
```sql
SELECT table_name
FROM information_schema.tables
WHERE table_schema = 'public'
  AND table_name LIKE 'FastServer_%';
```

**Tablas esperadas:**
- FastServer_LogServices_Header
- FastServer_LogServices_Header_Historico
- FastServer_LogMicroservice
- FastServer_LogMicroservice_Historico
- FastServer_LogServices_Content
- FastServer_LogServices_Content_Historico

### 7.4. Verificar que NO hay foreign keys en PostgreSQL

```sql
SELECT
    tc.table_name,
    kcu.column_name,
    ccu.table_name AS foreign_table_name,
    ccu.column_name AS foreign_column_name
FROM information_schema.table_constraints AS tc
JOIN information_schema.key_column_usage AS kcu
    ON tc.constraint_name = kcu.constraint_name
JOIN information_schema.constraint_column_usage AS ccu
    ON ccu.constraint_name = tc.constraint_name
WHERE tc.constraint_type = 'FOREIGN KEY'
  AND tc.table_name LIKE 'FastServer_%';
```

**Resultado esperado**: 0 filas (sin foreign keys)

---

## Paso 8: Configurar Nginx como Reverse Proxy (Opcional)

### 8.1. Instalar Nginx

```bash
sudo apt install nginx
```

### 8.2. Configurar sitio

```bash
sudo nano /etc/nginx/sites-available/fastserver
```

**Contenido:**
```nginx
server {
    listen 80;
    server_name api.fastserver.banco.interno;

    location / {
        proxy_pass http://localhost:64707;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

### 8.3. Habilitar sitio

```bash
sudo ln -s /etc/nginx/sites-available/fastserver /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl reload nginx
```

---

## Paso 9: Configurar SSL/TLS (IMPORTANTE para Producción)

### 9.1. Obtener certificado del banco

Coordinar con el equipo de seguridad del banco para:
- Certificado SSL emitido por la CA interna
- O usar Let's Encrypt si es permitido

### 9.2. Configurar HTTPS en Nginx

```nginx
server {
    listen 443 ssl http2;
    server_name api.fastserver.banco.interno;

    ssl_certificate /etc/ssl/certs/fastserver.crt;
    ssl_certificate_key /etc/ssl/private/fastserver.key;
    ssl_protocols TLSv1.2 TLSv1.3;
    ssl_ciphers HIGH:!aNULL:!MD5;

    location / {
        proxy_pass http://localhost:64707;
        # ... resto de configuración proxy
    }
}

server {
    listen 80;
    server_name api.fastserver.banco.interno;
    return 301 https://$server_name$request_uri;
}
```

---

## Paso 10: Monitoreo y Logs

### 10.1. Ver logs de la aplicación

```bash
# Logs del servicio
sudo journalctl -u fastserver -f

# Logs de Nginx
sudo tail -f /var/log/nginx/access.log
sudo tail -f /var/log/nginx/error.log
```

### 10.2. Configurar log rotation

```bash
sudo nano /etc/logrotate.d/fastserver
```

**Contenido:**
```
/var/log/fastserver/*.log {
    daily
    rotate 30
    compress
    delaycompress
    notifempty
    create 0640 fastserver fastserver
    sharedscripts
    postrotate
        systemctl reload fastserver > /dev/null 2>&1 || true
    endscript
}
```

---

## Troubleshooting

### Problema: No se puede conectar a PostgreSQL

**Verificar:**
```bash
psql -h SERVIDOR_BANCO_PG -U USUARIO_BANCO -d postgres -c "SELECT version();"
```

**Soluciones:**
- Verificar firewall permite conexiones al puerto 5432
- Verificar `pg_hba.conf` permite conexiones desde el servidor de la API
- Verificar credenciales correctas

### Problema: Error "Cannot create a DbSet"

**Causa**: Configuración de DbContext incorrecta

**Solución**: Verificar que `appsettings.Production.json` tiene las cadenas de conexión correctas

### Problema: Tablas no se crean

**Verificar permisos:**
```sql
-- PostgreSQL
SELECT * FROM information_schema.role_table_grants
WHERE grantee = 'USUARIO_BANCO';
```

**Solución**: Otorgar permisos CREATE TABLE al usuario

---

## Checklist Final de Instalación

- [ ] Repositorio clonado en el servidor
- [ ] Cadenas de conexión configuradas (Production)
- [ ] Bases de datos creadas (PostgreSQL y/o SQL Server)
- [ ] Migraciones aplicadas exitosamente
- [ ] Aplicación compilada y publicada
- [ ] Servicio systemd configurado y corriendo
- [ ] Nginx configurado como reverse proxy
- [ ] SSL/TLS configurado
- [ ] Queries de prueba funcionando
- [ ] Logs configurados
- [ ] Monitoreo configurado
- [ ] Documentación entregada al equipo del banco

---

## Contacto y Soporte

Para soporte durante la instalación:
- **Desarrollador**: [Tu nombre/contacto]
- **Documentación técnica**: Ver `/docs` en el repositorio
- **Issues**: GitHub Issues del proyecto

---

## Notas Importantes

1. **Seguridad**:
   - NUNCA commitear passwords reales
   - Usar variables de entorno o Azure Key Vault en producción
   - Habilitar autenticación/autorización según políticas del banco

2. **Backup**:
   - Configurar backups automáticos de PostgreSQL/SQL Server
   - Probar restauración de backups periódicamente

3. **Actualizaciones**:
   - Mantener .NET actualizado
   - Revisar security advisories
   - Documentar cambios en CHANGELOG.md

4. **Compliance**:
   - Asegurar cumplimiento con políticas del banco
   - Auditoría de logs habilitada
   - Encriptación en tránsito y en reposo
