# 🏦 Instrucciones de Instalación FastServer - Banco

**Versión:** 2.0 - PostgreSQL Exclusivo
**Fecha:** Febrero 2024
**Estado:** ✅ Listo para Producción

---

## 📋 Tabla de Contenidos

1. [Requisitos Previos](#requisitos-previos)
2. [Instalación Paso a Paso](#instalación-paso-a-paso)
3. [Configuración](#configuración)
4. [Verificación de la Instalación](#verificación-de-la-instalación)
5. [Pruebas de Funcionalidad](#pruebas-de-funcionalidad)
6. [Resolución de Problemas](#resolución-de-problemas)
7. [Mantenimiento](#mantenimiento)

---

## 📦 Requisitos Previos

### Software Requerido

| Software | Versión Mínima | Propósito |
|----------|----------------|-----------|
| **.NET SDK** | 10.0 | Runtime de aplicación |
| **PostgreSQL** | 14+ | Base de datos |
| **Git** | 2.30+ | Control de versiones |

### Hardware Recomendado

| Componente | Mínimo | Recomendado |
|-----------|---------|-------------|
| **CPU** | 2 cores | 4+ cores |
| **RAM** | 4 GB | 8+ GB |
| **Disco** | 10 GB | 20+ GB SSD |
| **Red** | 100 Mbps | 1 Gbps |

---

## 🚀 Instalación Paso a Paso

### Paso 1: Instalar .NET 10 SDK

**Windows:**
```powershell
# Descargar desde: https://dotnet.microsoft.com/download/dotnet/10.0
# O usar winget:
winget install Microsoft.DotNet.SDK.10
```

**Linux (Ubuntu/Debian):**
```bash
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install -y dotnet-sdk-10.0
```

**Verificar instalación:**
```bash
dotnet --version
# Debe mostrar: 10.0.x
```

---

### Paso 2: Instalar PostgreSQL

**Windows:**
```powershell
# Descargar desde: https://www.postgresql.org/download/windows/
# Durante instalación:
# - Puerto: 5432
# - Usuario: postgres
# - Contraseña: [CONFIGURAR SEGÚN POLÍTICA DEL BANCO]
```

**Linux (Ubuntu/Debian):**
```bash
sudo apt-get update
sudo apt-get install -y postgresql postgresql-contrib
sudo systemctl start postgresql
sudo systemctl enable postgresql
```

**Verificar instalación:**
```bash
psql --version
# Debe mostrar: psql (PostgreSQL) 14.x o superior
```

---

### Paso 3: Crear Bases de Datos PostgreSQL

**Conectarse a PostgreSQL:**
```bash
# Windows
psql -U postgres

# Linux
sudo -u postgres psql
```

**Ejecutar comandos SQL:**
```sql
-- Crear base de datos para logs
CREATE DATABASE "FastServer_Logs"
    WITH
    ENCODING = 'UTF8'
    LC_COLLATE = 'C'
    LC_CTYPE = 'C'
    TEMPLATE = template0;

-- Crear base de datos para microservicios
CREATE DATABASE "FastServer"
    WITH
    ENCODING = 'UTF8'
    LC_COLLATE = 'C'
    LC_CTYPE = 'C'
    TEMPLATE = template0;

-- Verificar creación
\l

-- Salir
\q
```

**Verificación:**
```bash
psql -U postgres -d FastServer_Logs -c "SELECT version();"
psql -U postgres -d FastServer -c "SELECT version();"
```

---

### Paso 4: Clonar el Repositorio

```bash
# Navegar al directorio de trabajo
cd C:\Aplicaciones  # Windows
cd /opt/aplicaciones  # Linux

# Clonar repositorio
git clone [URL_DEL_REPOSITORIO] FastServer
cd FastServer
```

---

### Paso 5: Configurar Credenciales

**Editar `appsettings.json`:**
```bash
cd src/FastServer.GraphQL.Api
notepad appsettings.json  # Windows
nano appsettings.json     # Linux
```

**Configuración requerida:**
```json
{
  "ConnectionStrings": {
    "PostgreSQLLogs": "Host=localhost;Port=5432;Database=FastServer_Logs;Username=postgres;Password=TU_PASSWORD_AQUI",
    "PostgreSQLMicroservices": "Host=localhost;Port=5432;Database=FastServer;Username=postgres;Password=TU_PASSWORD_AQUI"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    }
  }
}
```

**⚠️ IMPORTANTE:**
- Reemplazar `TU_PASSWORD_AQUI` con la contraseña PostgreSQL del banco
- En producción, usar variables de entorno en lugar de hardcodear passwords

**Configuración con Variables de Entorno (Recomendado):**
```bash
# Windows (PowerShell)
$env:ConnectionStrings__PostgreSQLLogs="Host=localhost;Port=5432;Database=FastServer_Logs;Username=postgres;Password=PASSWORD_SEGURO"
$env:ConnectionStrings__PostgreSQLMicroservices="Host=localhost;Port=5432;Database=FastServer;Username=postgres;Password=PASSWORD_SEGURO"

# Linux (Bash)
export ConnectionStrings__PostgreSQLLogs="Host=localhost;Port=5432;Database=FastServer_Logs;Username=postgres;Password=PASSWORD_SEGURO"
export ConnectionStrings__PostgreSQLMicroservices="Host=localhost;Port=5432;Database=FastServer;Username=postgres;Password=PASSWORD_SEGURO"
```

---

### Paso 6: Aplicar Migraciones de Base de Datos

```bash
cd src/FastServer.Infrastructure

# Aplicar migraciones para BD de Logs (FastServer_Logs)
dotnet ef database update --context PostgreSqlLogsDbContext --startup-project ../FastServer.GraphQL.Api

# Aplicar migraciones para BD de Microservicios (FastServer)
dotnet ef database update --context PostgreSqlMicroservicesDbContext --startup-project ../FastServer.GraphQL.Api
```

**Salida esperada:**
```
Build succeeded.
Applying migration '20260210013959_PostgreSqlInitialCreate'.
Done.
```

**Verificar tablas creadas:**
```sql
-- Conectarse a FastServer_Logs
\c FastServer_Logs
\dt
-- Debe mostrar 6 tablas: LogServicesHeader, LogMicroservice, LogServicesContent + 3 históricos

-- Conectarse a FastServer
\c FastServer
\dt
-- Debe mostrar 8 tablas: EventType, User, ActivityLog, MicroserviceRegister, etc.
```

---

### Paso 7: Compilar el Proyecto

```bash
cd ../../  # Volver al directorio raíz
dotnet build src/FastServer.GraphQL.Api/FastServer.GraphQL.Api.csproj --configuration Release
```

**Salida esperada:**
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

---

### Paso 8: Ejecutar la Aplicación

**Modo Desarrollo (para pruebas):**
```bash
cd src/FastServer.GraphQL.Api
dotnet run
```

**Modo Producción:**
```bash
cd src/FastServer.GraphQL.Api
dotnet run --configuration Release --environment Production
```

**Salida esperada:**
```
[INFO] Configuración de bases de datos:
[INFO]   - BD Logs: FastServer_Logs (PostgreSQL)
[INFO]   - BD Microservices: FastServer (PostgreSQL)
[INFO] FastServer GraphQL API iniciando...
[INFO] Arquitectura: PostgreSQL exclusivo (FastServer_Logs + FastServer)
[INFO] Now listening on: https://localhost:64706
[INFO] Now listening on: http://localhost:64707
[INFO] Application started. Press Ctrl+C to shut down.
```

---

## ⚙️ Configuración

### Configurar como Servicio de Windows

**Crear servicio:**
```powershell
# Publicar aplicación
cd src/FastServer.GraphQL.Api
dotnet publish -c Release -o C:\FastServer\Produccion

# Crear servicio con NSSM o sc.exe
sc.exe create FastServerAPI binPath="C:\FastServer\Produccion\FastServer.GraphQL.Api.exe"
sc.exe start FastServerAPI
```

### Configurar como Servicio systemd (Linux)

**Crear archivo de servicio:**
```bash
sudo nano /etc/systemd/system/fastserver.service
```

**Contenido:**
```ini
[Unit]
Description=FastServer GraphQL API
After=network.target postgresql.service

[Service]
Type=notify
User=fastserver
WorkingDirectory=/opt/FastServer/src/FastServer.GraphQL.Api
ExecStart=/usr/bin/dotnet run --configuration Release
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=fastserver
Environment=ASPNETCORE_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target
```

**Habilitar y arrancar:**
```bash
sudo systemctl daemon-reload
sudo systemctl enable fastserver
sudo systemctl start fastserver
sudo systemctl status fastserver
```

---

## ✅ Verificación de la Instalación

### 1. Verificar que el Servidor Está Corriendo

```bash
# Verificar proceso
ps aux | grep FastServer  # Linux
tasklist | findstr FastServer  # Windows

# Verificar puertos
netstat -an | grep 64706
netstat -an | grep 64707
```

### 2. Verificar GraphQL IDE

Abrir navegador en:
- **HTTPS:** https://localhost:64706/graphql
- **HTTP:** http://localhost:64707/graphql

Debe aparecer **Banana Cake Pop** (GraphQL IDE)

### 3. Verificar Conexión a Bases de Datos

**Query de verificación:**
```graphql
query {
  health
  version
}
```

**Respuesta esperada:**
```json
{
  "data": {
    "health": "OK",
    "version": "2.0.0"
  }
}
```

---

## 🧪 Pruebas de Funcionalidad

### Prueba 1: Crear Log (BD: FastServer_Logs)

```graphql
mutation {
  createLogServicesHeader(input: {
    logDateIn: "2024-02-11T10:00:00Z"
    logDateOut: "2024-02-11T10:00:05Z"
    logState: COMPLETED
    logMethodUrl: "/api/auth/login"
    logMethodName: "UserLogin"
    microserviceName: "AuthService"
    httpMethod: "POST"
    requestDuration: 5000
    transactionId: "TXN-INSTALL-TEST-001"
    userId: "admin"
  }) {
    logId
    logState
    microserviceName
    requestDuration
  }
}
```

**✅ Resultado esperado:** Retorna un `logId` único

---

### Prueba 2: Obtener Logs

```graphql
query {
  allLogs(pagination: { pageNumber: 1, pageSize: 5 }) {
    items {
      logId
      logState
      logMethodUrl
      microserviceName
    }
    totalCount
    pageNumber
  }
}
```

**✅ Resultado esperado:** Lista de logs con paginación

---

### Prueba 3: Crear Microservicio (BD: FastServer)

```graphql
mutation {
  createMicroservice(
    name: "PaymentService"
    active: true
    coreConnection: true
  ) {
    microserviceId
    microserviceName
    microserviceActive
  }
}
```

**✅ Resultado esperado:** Retorna un `microserviceId` único

---

### Prueba 4: Suscripciones en Tiempo Real

**En pestaña 1 del GraphQL IDE - Crear suscripción:**
```graphql
subscription {
  onLogCreated {
    logId
    logState
    microserviceName
    transactionId
  }
}
```

**En pestaña 2 del GraphQL IDE - Crear log:**
```graphql
mutation {
  createLogServicesHeader(input: {
    logDateIn: "2024-02-11T10:00:00Z"
    logDateOut: "2024-02-11T10:00:01Z"
    logState: COMPLETED
    logMethodUrl: "/api/test"
    microserviceName: "TestService"
  }) {
    logId
  }
}
```

**✅ Resultado esperado:** La suscripción en pestaña 1 recibe el evento inmediatamente

---

### Prueba 5: Filtrar Logs

```graphql
query {
  logsByFilter(
    filter: {
      microserviceName: "AuthService"
      state: COMPLETED
      startDate: "2024-02-01T00:00:00Z"
      endDate: "2024-02-28T23:59:59Z"
    }
    pagination: { pageNumber: 1, pageSize: 10 }
  ) {
    items {
      logId
      logState
      microserviceName
      transactionId
    }
    totalCount
  }
}
```

**✅ Resultado esperado:** Logs filtrados correctamente

---

### Prueba 6: Actualizar Log

```graphql
mutation {
  updateLogServicesHeader(input: {
    logId: 1
    logState: FAILED
    errorCode: "AUTH-001"
    errorDescription: "Credenciales inválidas"
  }) {
    logId
    logState
    errorCode
    errorDescription
  }
}
```

**✅ Resultado esperado:** Log actualizado con nuevos valores

---

### Prueba 7: Suscripción a Microservicios

**En pestaña 1 - Suscripción:**
```graphql
subscription {
  onMicroserviceCreated {
    microserviceId
    microserviceName
    microserviceActive
  }
}
```

**En pestaña 2 - Crear:**
```graphql
mutation {
  createMicroservice(
    name: "NotificationService"
    active: true
    coreConnection: false
  ) {
    microserviceId
    microserviceName
  }
}
```

**✅ Resultado esperado:** La suscripción recibe el evento de creación

---

## 🔧 Resolución de Problemas

### Problema: "Connection refused" a PostgreSQL

**Causa:** PostgreSQL no está corriendo o firewall bloqueando

**Solución:**
```bash
# Verificar que PostgreSQL está corriendo
sudo systemctl status postgresql  # Linux
sc query postgresql-x64-14        # Windows

# Iniciar si está detenido
sudo systemctl start postgresql   # Linux
sc start postgresql-x64-14        # Windows

# Verificar puerto
netstat -an | grep 5432
```

### Problema: "Password authentication failed"

**Causa:** Credenciales incorrectas en appsettings.json

**Solución:**
1. Verificar password en `appsettings.json`
2. Resetear password PostgreSQL si es necesario:
```sql
ALTER USER postgres WITH PASSWORD 'nueva_password';
```

### Problema: "Database does not exist"

**Causa:** No se crearon las bases de datos

**Solución:**
```bash
# Crear bases de datos manualmente
psql -U postgres -c "CREATE DATABASE \"FastServer_Logs\";"
psql -U postgres -c "CREATE DATABASE \"FastServer\";"

# Aplicar migraciones
dotnet ef database update --context PostgreSqlLogsDbContext --startup-project ../FastServer.GraphQL.Api
dotnet ef database update --context PostgreSqlMicroservicesDbContext --startup-project ../FastServer.GraphQL.Api
```

### Problema: Error de compilación

**Causa:** Versión incorrecta de .NET o dependencias faltantes

**Solución:**
```bash
# Limpiar y restaurar
dotnet clean
dotnet restore
dotnet build
```

### Problema: Puerto en uso (64706 o 64707)

**Causa:** Otro proceso usando el puerto

**Solución:**
```bash
# Windows - Encontrar proceso
netstat -ano | findstr :64706
taskkill /PID [PID] /F

# Linux - Encontrar y matar proceso
sudo lsof -i :64706
sudo kill -9 [PID]
```

### Problema: Subscripciones no funcionan

**Causa:** WebSockets no configurados correctamente

**Verificar:**
1. Que `app.UseWebSockets()` esté en `Program.cs`
2. Que el navegador soporte WebSockets
3. Que no haya proxy/firewall bloqueando WebSockets

**Probar conexión WebSocket:**
```javascript
// En consola del navegador
const ws = new WebSocket('ws://localhost:64707/graphql');
ws.onopen = () => console.log('✅ WebSocket conectado');
ws.onerror = (e) => console.error('❌ Error:', e);
```

---

## 🛠️ Mantenimiento

### Actualizar a Nueva Versión

```bash
# 1. Detener servicio
sudo systemctl stop fastserver  # Linux
sc stop FastServerAPI           # Windows

# 2. Backup de base de datos
pg_dump -U postgres FastServer_Logs > backup_logs_$(date +%Y%m%d).sql
pg_dump -U postgres FastServer > backup_microservices_$(date +%Y%m%d).sql

# 3. Actualizar código
git pull origin main

# 4. Aplicar nuevas migraciones
cd src/FastServer.Infrastructure
dotnet ef database update --context PostgreSqlLogsDbContext --startup-project ../FastServer.GraphQL.Api
dotnet ef database update --context PostgreSqlMicroservicesDbContext --startup-project ../FastServer.GraphQL.Api

# 5. Compilar
cd ../../
dotnet build --configuration Release

# 6. Reiniciar servicio
sudo systemctl start fastserver  # Linux
sc start FastServerAPI           # Windows
```

### Backup Automático (Linux)

**Crear script de backup:**
```bash
sudo nano /usr/local/bin/backup-fastserver.sh
```

**Contenido:**
```bash
#!/bin/bash
BACKUP_DIR="/backups/fastserver"
DATE=$(date +%Y%m%d_%H%M%S)

mkdir -p $BACKUP_DIR

# Backup de bases de datos
pg_dump -U postgres FastServer_Logs > "$BACKUP_DIR/logs_$DATE.sql"
pg_dump -U postgres FastServer > "$BACKUP_DIR/microservices_$DATE.sql"

# Comprimir
gzip "$BACKUP_DIR/logs_$DATE.sql"
gzip "$BACKUP_DIR/microservices_$DATE.sql"

# Eliminar backups antiguos (más de 30 días)
find $BACKUP_DIR -name "*.sql.gz" -mtime +30 -delete

echo "Backup completado: $DATE"
```

**Hacer ejecutable y programar:**
```bash
sudo chmod +x /usr/local/bin/backup-fastserver.sh

# Agregar a crontab (diario a las 2 AM)
sudo crontab -e
# Agregar línea:
0 2 * * * /usr/local/bin/backup-fastserver.sh
```

### Monitoreo de Logs

**Ver logs en tiempo real:**
```bash
# Linux (systemd)
sudo journalctl -u fastserver -f

# Linux (archivo de log)
tail -f /var/log/fastserver/app.log

# Windows (Event Viewer)
# Abrir Event Viewer → Application Logs → FastServer
```

### Verificar Salud del Sistema

**Crear script de monitoreo:**
```bash
#!/bin/bash
# healthcheck.sh

# Verificar API está respondiendo
RESPONSE=$(curl -s -o /dev/null -w "%{http_code}" http://localhost:64707/graphql?query={health})

if [ "$RESPONSE" == "200" ]; then
    echo "✅ API está funcionando correctamente"
    exit 0
else
    echo "❌ API no está respondiendo (HTTP $RESPONSE)"
    # Reiniciar servicio
    sudo systemctl restart fastserver
    exit 1
fi
```

---

## 📊 Métricas de Rendimiento

### Capacidad Esperada

| Métrica | Valor |
|---------|-------|
| **Requests/segundo** | 500-1000 |
| **Tiempo de respuesta promedio** | <100ms |
| **Subscripciones concurrentes** | 1000+ |
| **Usuarios concurrentes** | 50-100 |
| **Tamaño de BD (inicial)** | ~10 MB |
| **Crecimiento estimado** | ~50 MB/mes |

### Optimizaciones Aplicadas

- ✅ DbContext Pooling (128 conexiones)
- ✅ AsNoTracking() en queries de solo lectura
- ✅ Índices en columnas de búsqueda frecuente
- ✅ Connection pooling de PostgreSQL
- ✅ Compresión de respuestas HTTP

---

## 📞 Soporte

### Contactos

- **Equipo de Desarrollo:** [email-del-equipo]
- **Soporte Técnico:** [email-soporte]
- **Documentación:** Ver archivos README.md en el repositorio

### Información del Sistema

- **Arquitectura:** PostgreSQL Exclusivo
- **Bases de Datos:** FastServer_Logs + FastServer
- **Puerto HTTPS:** 64706
- **Puerto HTTP:** 64707
- **Protocolo GraphQL:** HTTP + WebSockets

---

## ✅ Checklist de Instalación

- [ ] .NET 10 SDK instalado
- [ ] PostgreSQL 14+ instalado y corriendo
- [ ] Bases de datos `FastServer_Logs` y `FastServer` creadas
- [ ] Repositorio clonado
- [ ] `appsettings.json` configurado con credenciales correctas
- [ ] Migraciones aplicadas en ambas BDs
- [ ] Proyecto compilado sin errores
- [ ] Aplicación ejecutándose en puertos 64706/64707
- [ ] GraphQL IDE accesible en navegador
- [ ] Query `health` retorna "OK"
- [ ] Mutation de crear log funciona
- [ ] Mutation de crear microservicio funciona
- [ ] Suscripciones en tiempo real funcionan
- [ ] Servicio configurado (Windows/Linux)
- [ ] Backup automático configurado
- [ ] Monitoreo de logs configurado

---

**🎉 ¡Instalación completada! FastServer está listo para usar en el banco.**

*Última actualización: Febrero 2024*
