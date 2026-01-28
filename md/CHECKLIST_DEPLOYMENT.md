# ✅ Checklist de Deployment al Banco

Usa esta lista para preparar y ejecutar el deployment.

---

## 📦 Fase 1: Preparación (Antes de ir al banco)

### En tu PC de Desarrollo

- [ ] **Código completo y funcionando**
  - [ ] Todas las features implementadas
  - [ ] Tests pasando
  - [ ] Sin errores de compilación

- [ ] **Generar migraciones limpias de producción**
  ```powershell
  # PowerShell
  .\scripts\generate-production-migrations.ps1 -Both
  ```
  - [ ] Migración PostgreSQL generada
  - [ ] Migración SQL Server generada (si se usa)
  - [ ] Scripts SQL generados en `scripts/production-migrations/`

- [ ] **Revisar scripts SQL**
  - [ ] Abrir `postgresql-initial.sql`
  - [ ] Verificar que crea todas las tablas
  - [ ] Verificar que NO crea FKs en PostgreSQL
  - [ ] Verificar índices están presentes
  - [ ] Script usa IF NOT EXISTS (idempotente)

- [ ] **Preparar configuración**
  - [ ] Crear plantilla `appsettings.Production.json` (sin passwords)
  - [ ] Documentar variables de entorno necesarias
  - [ ] Listar puertos usados (64706, 64707)

- [ ] **Documentación**
  - [ ] `DEPLOYMENT_BANCO.md` actualizado
  - [ ] `ESTRATEGIA_MIGRACIONES.md` revisado
  - [ ] `README.md` con instrucciones básicas

- [ ] **Git**
  - [ ] Commit de todos los cambios
  ```bash
  git add .
  git commit -m "feat: Preparación para deployment en banco - migraciones limpias"
  ```
  - [ ] Crear tag de versión
  ```bash
  git tag -a v1.0.0-banco -m "Release para instalación inicial en banco"
  ```
  - [ ] Push al repositorio
  ```bash
  git push origin master
  git push origin v1.0.0-banco
  ```

---

## 📋 Fase 2: Información para el Banco

### Datos a Solicitar al Equipo del Banco

- [ ] **PostgreSQL**
  - [ ] Host/IP: `_________________`
  - [ ] Puerto: `_________________` (default: 5432)
  - [ ] Database name: `_________________` (sugerido: FastServerLogs)
  - [ ] Username: `_________________`
  - [ ] Password: `_________________`
  - [ ] ¿Requiere SSL?: `□ Sí  □ No`

- [ ] **SQL Server** (opcional)
  - [ ] Host/IP: `_________________`
  - [ ] Puerto: `_________________` (default: 1433)
  - [ ] Database name: `_________________` (sugerido: FastServerLogs)
  - [ ] Username: `_________________`
  - [ ] Password: `_________________`
  - [ ] ¿Integrated Security?: `□ Sí  □ No`

- [ ] **Servidor de Aplicación**
  - [ ] IP/Hostname: `_________________`
  - [ ] OS: `□ Linux  □ Windows  □ Otro: _______`
  - [ ] .NET 10 instalado: `□ Sí  □ No`
  - [ ] Acceso: `□ SSH  □ RDP  □ Otro: _______`
  - [ ] Usuario con permisos: `_________________`

- [ ] **Red**
  - [ ] ¿Firewall entre app y DBs?: `□ Sí  □ No`
  - [ ] Puertos a abrir: `_________________`
  - [ ] ¿Reverse proxy (Nginx/IIS)?: `□ Sí  □ No`
  - [ ] ¿SSL/TLS requerido?: `□ Sí  □ No`

- [ ] **Seguridad**
  - [ ] ¿Autenticación requerida?: `□ Sí  □ No`
  - [ ] Método: `□ AD/LDAP  □ OAuth  □ Otro: _______`
  - [ ] ¿Logs centralizados?: `□ Sí  □ No`
  - [ ] ¿Monitoreo requerido?: `□ Sí  □ No`

---

## 🚀 Fase 3: Instalación en el Banco

### Día de la Instalación

**Hora inicio**: `_________________`

### 3.1. Acceso al Servidor

- [ ] Conectado al servidor
  ```bash
  ssh usuario@servidor-banco
  ```
- [ ] Verificar .NET instalado
  ```bash
  dotnet --version
  # Debe mostrar: 10.0.x
  ```

### 3.2. Clonar Repositorio

- [ ] Crear directorio
  ```bash
  sudo mkdir -p /opt/fastserver
  sudo chown $USER:$USER /opt/fastserver
  ```
- [ ] Clonar
  ```bash
  cd /opt
  git clone https://github.com/tu-usuario/FastServer.git fastserver
  cd fastserver
  git checkout v1.0.0-banco
  ```

### 3.3. Configurar Cadenas de Conexión

- [ ] Crear `appsettings.Production.json`
  ```bash
  nano src/FastServer.GraphQL.Api/appsettings.Production.json
  ```
- [ ] Pegar configuración con credenciales reales
- [ ] Verificar permisos del archivo (no legible por otros)
  ```bash
  chmod 600 src/FastServer.GraphQL.Api/appsettings.Production.json
  ```

### 3.4. Verificar Conectividad a Bases de Datos

- [ ] Test PostgreSQL
  ```bash
  psql -h HOST -U USER -d postgres -c "SELECT version();"
  ```
- [ ] Test SQL Server (si se usa)
  ```bash
  sqlcmd -S HOST -U USER -P PASSWORD -Q "SELECT @@VERSION"
  ```

### 3.5. Crear Bases de Datos

- [ ] Crear DB en PostgreSQL
  ```sql
  CREATE DATABASE "FastServerLogs"
    WITH ENCODING = 'UTF8'
    LC_COLLATE = 'en_US.UTF-8'
    LC_CTYPE = 'en_US.UTF-8'
    TEMPLATE = template0;
  ```
- [ ] Otorgar permisos al usuario
  ```sql
  GRANT ALL PRIVILEGES ON DATABASE "FastServerLogs" TO usuario_app;
  ```

### 3.6. Aplicar Migraciones

**Opción A: Scripts SQL**

- [ ] Aplicar migración PostgreSQL
  ```bash
  psql -h HOST -U USER -d FastServerLogs \
    -f scripts/production-migrations/postgresql-initial.sql
  ```
- [ ] Verificar resultado sin errores
- [ ] Contar tablas creadas
  ```sql
  SELECT count(*) FROM information_schema.tables
  WHERE table_schema = 'public' AND table_name LIKE 'FastServer_%';
  -- Debe retornar: 6 tablas
  ```

**Opción B: dotnet ef**

- [ ] Aplicar con EF Core
  ```bash
  cd src/FastServer.GraphQL.Api
  dotnet ef database update --context PostgreSqlDbContext
  ```

### 3.7. Compilar Aplicación

- [ ] Compilar en modo Release
  ```bash
  cd /opt/fastserver
  dotnet build src/FastServer.GraphQL.Api/FastServer.GraphQL.Api.csproj \
    --configuration Release
  ```
- [ ] Verificar sin errores de compilación

### 3.8. Publicar Aplicación

- [ ] Publicar
  ```bash
  dotnet publish src/FastServer.GraphQL.Api/FastServer.GraphQL.Api.csproj \
    --configuration Release \
    --output /opt/fastserver/publish
  ```
- [ ] Verificar archivos generados
  ```bash
  ls -lh /opt/fastserver/publish/
  ```

### 3.9. Configurar como Servicio (Linux)

- [ ] Crear usuario del servicio
  ```bash
  sudo useradd -r -s /bin/false fastserver
  ```
- [ ] Crear archivo de servicio
  ```bash
  sudo nano /etc/systemd/system/fastserver.service
  ```
- [ ] Pegar configuración del servicio (ver DEPLOYMENT_BANCO.md)
- [ ] Recargar systemd
  ```bash
  sudo systemctl daemon-reload
  ```
- [ ] Habilitar servicio
  ```bash
  sudo systemctl enable fastserver
  ```
- [ ] Iniciar servicio
  ```bash
  sudo systemctl start fastserver
  ```
- [ ] Verificar estado
  ```bash
  sudo systemctl status fastserver
  # Debe mostrar: active (running)
  ```

### 3.10. Verificar API Funcionando

- [ ] Test básico
  ```bash
  curl http://localhost:64707/graphql
  ```
  - [ ] Responde (aunque sea error GraphQL)

- [ ] Test health endpoint
  ```bash
  curl -X POST http://localhost:64707/graphql \
    -H "Content-Type: application/json" \
    -d '{"query": "{ health }"}'
  ```
  - [ ] Responde: `{"data":{"health":"Healthy"}}`

- [ ] Test query real
  ```bash
  curl -X POST http://localhost:64707/graphql \
    -H "Content-Type: application/json" \
    -d '{"query": "{ allLogs(pagination: {pageNumber: 1, pageSize: 5}) { totalCount } }"}'
  ```
  - [ ] Responde correctamente

### 3.11. Configurar Reverse Proxy (si aplica)

- [ ] Instalar Nginx (si aplica)
  ```bash
  sudo apt install nginx
  ```
- [ ] Configurar sitio (ver DEPLOYMENT_BANCO.md)
- [ ] Habilitar sitio
- [ ] Recargar Nginx
  ```bash
  sudo nginx -t
  sudo systemctl reload nginx
  ```

### 3.12. Configurar SSL/TLS (si aplica)

- [ ] Obtener certificado del banco
- [ ] Instalar certificado
- [ ] Configurar HTTPS en Nginx/IIS
- [ ] Verificar conexión HTTPS
  ```bash
  curl https://api.fastserver.banco.interno/graphql
  ```

---

## ✅ Fase 4: Validación Final

### Tests de Funcionalidad

- [ ] **Query básico funciona**
  - [ ] `allLogs` retorna datos
  - [ ] Paginación funciona
  - [ ] Filtros funcionan

- [ ] **Queries separadas funcionan**
  - [ ] `logMicroservicesByLogId` funciona
  - [ ] `logContentsByLogId` funciona

- [ ] **Verificación de arquitectura**
  - [ ] SQL generado NO tiene JOINs
  - [ ] No hay errores de FK en logs

- [ ] **Performance básica**
  - [ ] Queries responden en < 3 segundos
  - [ ] No hay errores en logs

### Tests de Base de Datos

- [ ] **Verificar estructura PostgreSQL**
  ```sql
  -- Listar tablas
  SELECT table_name FROM information_schema.tables
  WHERE table_schema = 'public' AND table_name LIKE 'FastServer_%';

  -- Verificar sin FKs
  SELECT constraint_name FROM information_schema.table_constraints
  WHERE constraint_type = 'FOREIGN KEY'
    AND table_name LIKE 'FastServer_%';
  -- Debe retornar: 0 filas

  -- Verificar índices
  SELECT tablename, indexname FROM pg_indexes
  WHERE tablename LIKE 'FastServer_%';
  ```

- [ ] **Insertar registro de prueba**
  ```sql
  INSERT INTO "FastServer_LogServices_Header"
  (fastserver_log_date_in, fastserver_log_date_out,
   fastserver_log_state, fastserver_log_method_url)
  VALUES (NOW(), NOW(), 'Completed', '/test');
  ```

- [ ] **Query registro de prueba vía API**
  ```bash
  curl -X POST http://localhost:64707/graphql \
    -H "Content-Type: application/json" \
    -d '{"query": "{ allLogs(pagination: {pageNumber: 1, pageSize: 1}) { items { logMethodUrl } } }"}'
  ```

### Verificación de Logs

- [ ] Ver logs del servicio
  ```bash
  sudo journalctl -u fastserver -n 50 --no-pager
  ```
- [ ] No hay errores críticos
- [ ] No hay warnings de FK
- [ ] Queries se ejecutan correctamente

---

## 📝 Fase 5: Documentación Post-Instalación

### Información a Documentar

- [ ] **Conexión**
  - URL de la API: `_________________`
  - Puerto: `_________________`
  - Protocolo: `□ HTTP  □ HTTPS`

- [ ] **Credenciales** (guardar de forma segura)
  - Usuario DB PostgreSQL: `_________________`
  - Usuario DB SQL Server: `_________________`
  - Usuario servicio Linux: `_________________`

- [ ] **Ubicaciones**
  - Directorio aplicación: `_________________`
  - Archivo de servicio: `_________________`
  - Logs: `_________________`
  - Configuración: `_________________`

- [ ] **Comandos útiles**
  ```bash
  # Ver estado
  sudo systemctl status fastserver

  # Ver logs en tiempo real
  sudo journalctl -u fastserver -f

  # Reiniciar servicio
  sudo systemctl restart fastserver

  # Detener servicio
  sudo systemctl stop fastserver
  ```

### Entrega al Cliente

- [ ] Documento con URLs de acceso
- [ ] Credenciales en sobre sellado/gestor seguro
- [ ] Manual de operación básico
- [ ] Contactos de soporte
- [ ] Procedimiento de backup
- [ ] Procedimiento de actualización futura

---

## 🎉 Instalación Completada

**Hora fin**: `_________________`
**Duración total**: `_________________`

### Firma de Aprobación

- [ ] **Instalador**
  - Nombre: `_________________`
  - Firma: `_________________`
  - Fecha: `_________________`

- [ ] **Cliente (Banco)**
  - Nombre: `_________________`
  - Cargo: `_________________`
  - Firma: `_________________`
  - Fecha: `_________________`

---

## 📞 Contactos de Soporte

**Desarrollador**: `_________________`
**Email**: `_________________`
**Teléfono**: `_________________`
**Horario**: `_________________`

---

## 🔄 Próximos Pasos

- [ ] Capacitación al equipo del banco
- [ ] Configurar monitoreo proactivo
- [ ] Establecer ventanas de mantenimiento
- [ ] Planificar backups regulares
- [ ] Coordinar proceso de actualizaciones futuras
