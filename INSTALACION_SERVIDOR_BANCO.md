# 🏦 Guía de Instalación - FastServer API (Servidor del Banco)

**Fecha**: 2026-02-09
**Versión**: 1.0
**Entorno**: Servidor del Banco (Producción)

---

## 📋 Prerequisitos en el Servidor del Banco

### Software Requerido

| Software | Versión Mínima | Propósito |
|----------|----------------|-----------|
| **.NET 10 SDK** | 10.0.102 | Runtime de la aplicación |
| **PostgreSQL** | 14+ | Base de datos de logs |
| **SQL Server** | 2019+ | Base de datos de microservicios |
| **dotnet-ef** | 10.x | Herramienta de migraciones |

### Verificar Instalaciones

```powershell
# Verificar .NET
dotnet --version
# Debe mostrar: 10.0.102 o superior

# Verificar dotnet-ef
dotnet ef --version
# Si no está instalado:
dotnet tool install --global dotnet-ef

# Verificar PostgreSQL (desde psql)
psql --version

# Verificar SQL Server (desde sqlcmd)
sqlcmd -?
```

---

## 🚀 Proceso de Instalación (Paso a Paso)

### Paso 1: Copiar Archivos al Servidor

1. **Transferir el proyecto** al servidor del banco:
   ```
   C:\FastServer\
   ```

2. **Verificar estructura** de carpetas:
   ```
   C:\FastServer\
   ├── src\
   │   ├── FastServer.Domain\
   │   ├── FastServer.Application\
   │   ├── FastServer.Infrastructure\
   │   └── FastServer.GraphQL.Api\
   ├── scripts\
   │   ├── cleanup-databases.ps1
   │   ├── reset-migrations.ps1
   │   └── install-database.ps1
   └── INSTALACION_SERVIDOR_BANCO.md
   ```

---

### Paso 2: Configurar Cadenas de Conexión

1. **Abrir** `C:\FastServer\src\FastServer.GraphQL.Api\appsettings.Production.json`

2. **Configurar PostgreSQL**:
   ```json
   {
     "ConnectionStrings": {
       "PostgreSQL": "Host=servidor-postgres-banco;Port=5432;Database=FastServerLogsDB;Username=fastserver_user;Password=PASSWORD_SEGURO_AQUI;"
     }
   }
   ```

3. **Configurar SQL Server**:
   ```json
   {
     "ConnectionStrings": {
       "SqlServer": "Server=servidor-sqlserver-banco;Database=FastServerMicroservicesDB;User ID=fastserver_user;Password=PASSWORD_SEGURO_AQUI;TrustServerCertificate=True;Encrypt=True;"
     }
   }
   ```

4. **Configurar origen predeterminado**:
   ```json
   {
     "DefaultDataSource": "PostgreSQL"
   }
   ```

**IMPORTANTE**: Reemplazar `PASSWORD_SEGURO_AQUI` con contraseñas reales proporcionadas por el administrador del banco.

---

### Paso 3: Crear Migraciones Iniciales (Solo Primera Vez)

**Ejecutar desde:** `C:\FastServer\scripts\`

```powershell
# Asegurarse de tener permisos de ejecución
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass

# Crear migraciones iniciales limpias
.\reset-migrations.ps1
```

**Resultado esperado:**
```
=====================================================
  FastServer - Reset de Migraciones
=====================================================

[1/3] Eliminando migraciones existentes...
  ✓ Migraciones de PostgreSQL eliminadas
  ✓ Migraciones de SQL Server eliminadas

[2/3] Creando migración inicial para PostgreSQL...
  ✓ Migración inicial de PostgreSQL creada

[3/3] Creando migración inicial para SQL Server...
  ✓ Migración inicial de SQL Server creada

=====================================================
  Migraciones Reseteadas Exitosamente
=====================================================
```

---

### Paso 4: Instalar Bases de Datos

**Ejecutar desde:** `C:\FastServer\scripts\`

```powershell
# Instalar bases de datos y aplicar migraciones
.\install-database.ps1
```

El script preguntará confirmación:
```
⚠️  ADVERTENCIA
Esta operación creará las bases de datos y ejecutará migraciones.

¿Desea continuar? (S/N):
```

Escribir **S** y presionar Enter.

**Resultado esperado:**
```
=====================================================
  FastServer - Instalación de Base de Datos
=====================================================

[Validación] Verificando prerequisitos...
  ✓ dotnet ef versión: 10.0.0

Configuración de conexiones:
  PostgreSQL: Host=servidor-postgres-banco;...
  SQL Server: Server=servidor-sqlserver-banco;...

[1/2] Aplicando migraciones a PostgreSQL...
  ✓ Migraciones de PostgreSQL aplicadas exitosamente

[2/2] Aplicando migraciones a SQL Server...
  ✓ Migraciones de SQL Server aplicadas exitosamente

[Verificación] Comprobando instalación...
  ✓ Proyecto compilado correctamente

=====================================================
  ✓ Instalación Completada Exitosamente
=====================================================

Bases de datos creadas:
  ✓ PostgreSQL: FastServerLogsDB
  ✓ SQL Server: FastServerMicroservicesDB
```

---

### Paso 5: Verificar Instalación

#### 5.1 Verificar Bases de Datos Creadas

**PostgreSQL:**
```sql
-- Conectar a PostgreSQL
psql -h servidor-postgres-banco -U fastserver_user -d FastServerLogsDB

-- Listar tablas
\dt

-- Debe mostrar:
-- LogServicesHeaders
-- LogMicroservices
-- LogServicesContents
-- __EFMigrationsHistory
```

**SQL Server:**
```sql
-- Conectar a SQL Server
sqlcmd -S servidor-sqlserver-banco -U fastserver_user -P PASSWORD -d FastServerMicroservicesDB

-- Listar tablas
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';
GO

-- Debe mostrar:
-- MicroserviceRegisters
-- MicroservicesClusters
-- Users
-- ActivityLogs
-- EventTypes
-- CoreConnectorCredentials
-- MicroserviceCoreConnectors
-- __EFMigrationsHistory
```

#### 5.2 Iniciar la API (Modo Prueba)

```powershell
cd C:\FastServer\src\FastServer.GraphQL.Api

# Iniciar en modo desarrollo
dotnet run --environment Production
```

**Verificar logs de inicio:**
```
[INFO] FastServer GraphQL API iniciando...
[INFO] Origen de datos predeterminado: PostgreSQL
[INFO] Now listening on: https://localhost:64706
[INFO] Now listening on: http://localhost:64707
[INFO] Application started. Press Ctrl+C to shut down.
```

#### 5.3 Probar Endpoints

**Health Check:**
```powershell
curl http://localhost:64707/health
```

**Respuesta esperada:**
```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.0123456"
}
```

**GraphQL:**
```powershell
# Abrir en navegador
start http://localhost:64707/graphql
```

Ejecutar query de prueba:
```graphql
query {
  __schema {
    queryType {
      name
    }
  }
}
```

---

## 🔧 Configuración Adicional para Producción

### 1. Configurar como Servicio de Windows

Crear archivo `install-service.ps1`:

```powershell
# Instalar como servicio de Windows
New-Service -Name "FastServerAPI" `
    -BinaryPathName "C:\FastServer\src\FastServer.GraphQL.Api\bin\Release\net10.0\FastServer.GraphQL.Api.exe" `
    -DisplayName "FastServer GraphQL API" `
    -Description "API GraphQL para monitoreo de microservicios" `
    -StartupType Automatic

# Iniciar servicio
Start-Service -Name "FastServerAPI"

# Verificar estado
Get-Service -Name "FastServerAPI"
```

### 2. Configurar Firewall

```powershell
# Permitir puerto HTTP (64707)
New-NetFirewallRule -DisplayName "FastServer API HTTP" `
    -Direction Inbound `
    -LocalPort 64707 `
    -Protocol TCP `
    -Action Allow

# Permitir puerto HTTPS (64706)
New-NetFirewallRule -DisplayName "FastServer API HTTPS" `
    -Direction Inbound `
    -LocalPort 64706 `
    -Protocol TCP `
    -Action Allow
```

### 3. Configurar HTTPS con Certificado del Banco

1. **Obtener certificado** del banco (.pfx)

2. **Configurar en appsettings.Production.json**:
   ```json
   {
     "Kestrel": {
       "Endpoints": {
         "Https": {
           "Url": "https://+:64706",
           "Certificate": {
             "Path": "C:\\Certificados\\banco-cert.pfx",
             "Password": "PASSWORD_DEL_CERTIFICADO"
           }
         },
         "Http": {
           "Url": "http://+:64707"
         }
       }
     }
   }
   ```

---

## 📊 Monitoreo y Logs

### Ubicación de Logs

Los logs de la aplicación se guardan en:
```
C:\FastServer\src\FastServer.GraphQL.Api\logs\
├── fastserver-20260209.log
├── fastserver-20260210.log
└── ...
```

### Verificar Logs en Tiempo Real

```powershell
Get-Content C:\FastServer\src\FastServer.GraphQL.Api\logs\fastserver-*.log -Tail 50 -Wait
```

### Niveles de Log

| Nivel | Descripción |
|-------|-------------|
| **INF** | Información general |
| **WRN** | Advertencias (no críticas) |
| **ERR** | Errores (requieren atención) |
| **FTL** | Errores fatales (aplicación cae) |

---

## 🚨 Troubleshooting

### Problema: "No se puede conectar a PostgreSQL"

**Solución:**
1. Verificar que PostgreSQL esté corriendo:
   ```powershell
   Get-Service -Name postgresql*
   ```

2. Verificar firewall de PostgreSQL (puerto 5432)

3. Verificar que el usuario `fastserver_user` existe y tiene permisos:
   ```sql
   CREATE USER fastserver_user WITH PASSWORD 'PASSWORD';
   GRANT ALL PRIVILEGES ON DATABASE FastServerLogsDB TO fastserver_user;
   ```

### Problema: "No se puede conectar a SQL Server"

**Solución:**
1. Verificar que SQL Server esté corriendo:
   ```powershell
   Get-Service -Name MSSQLSERVER
   ```

2. Verificar que SQL Server acepta autenticación mixta

3. Verificar que el usuario SQL existe:
   ```sql
   CREATE LOGIN fastserver_user WITH PASSWORD = 'PASSWORD';
   USE FastServerMicroservicesDB;
   CREATE USER fastserver_user FOR LOGIN fastserver_user;
   ALTER ROLE db_owner ADD MEMBER fastserver_user;
   ```

### Problema: "Error al aplicar migraciones"

**Solución:**
1. Verificar que las cadenas de conexión son correctas

2. Ejecutar migraciones manualmente con verbose:
   ```powershell
   cd C:\FastServer\src\FastServer.Infrastructure

   dotnet ef database update `
       --context PostgreSqlDbContext `
       --startup-project ..\FastServer.GraphQL.Api `
       --verbose
   ```

3. Revisar logs de error en la salida del comando

---

## 📞 Soporte

### Contactos

| Rol | Contacto |
|-----|----------|
| **Desarrollador** | [Tu nombre] |
| **DBA PostgreSQL** | [Nombre DBA] |
| **DBA SQL Server** | [Nombre DBA] |
| **Soporte Infraestructura** | [Contacto infraestructura] |

### Documentación Adicional

- **Arquitectura**: `docs/ARQUITECTURA.md`
- **API GraphQL**: http://localhost:64707/graphql (después de iniciar)
- **Suscripciones**: `VALIDACION_SUSCRIPCIONES.md`

---

## ✅ Checklist de Instalación

- [ ] Software prerequisito instalado (.NET 10, PostgreSQL, SQL Server)
- [ ] Archivos copiados al servidor (`C:\FastServer\`)
- [ ] Cadenas de conexión configuradas en `appsettings.Production.json`
- [ ] Migraciones creadas (`.\reset-migrations.ps1`)
- [ ] Bases de datos instaladas (`.\install-database.ps1`)
- [ ] Verificación de tablas en PostgreSQL
- [ ] Verificación de tablas en SQL Server
- [ ] API inicia correctamente (`dotnet run`)
- [ ] Health check responde correctamente
- [ ] GraphQL responde correctamente
- [ ] Firewall configurado (puertos 64706-64707)
- [ ] Certificado HTTPS instalado (opcional pero recomendado)
- [ ] Servicio de Windows configurado
- [ ] Servicio de Windows iniciando automáticamente
- [ ] Logs funcionando correctamente
- [ ] FastServer UI conecta correctamente a la API

---

## 🎯 Resumen de Comandos

```powershell
# 1. Navegar al directorio de scripts
cd C:\FastServer\scripts

# 2. Habilitar ejecución de scripts
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass

# 3. Crear migraciones (solo primera vez)
.\reset-migrations.ps1

# 4. Instalar bases de datos
.\install-database.ps1

# 5. Iniciar API
cd ..\src\FastServer.GraphQL.Api
dotnet run --environment Production

# 6. Verificar salud
curl http://localhost:64707/health

# 7. Abrir GraphQL
start http://localhost:64707/graphql
```

---

**Documento generado**: 2026-02-09
**Última actualización**: 2026-02-09
**Versión**: 1.0 - Instalación Inicial
