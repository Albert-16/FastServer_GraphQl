# FastServer Database Migrator

Herramienta de consola para aplicar migraciones de base de datos de forma segura y controlada.

## 🚀 Uso

### Aplicar migraciones a todas las bases de datos configuradas
```bash
dotnet run --project tools/FastServer.DbMigrator
```

### Aplicar migraciones solo a PostgreSQL
```bash
dotnet run --project tools/FastServer.DbMigrator postgres
```

### Aplicar migraciones solo a SQL Server
```bash
dotnet run --project tools/FastServer.DbMigrator sqlserver
```

## ⚙️ Configuración

La herramienta utiliza el archivo `appsettings.json` para obtener las cadenas de conexión:

```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=FastServerLogs;Username=postgres;Password=yourpassword",
    "SqlServer": "Server=localhost;Database=FastServerLogs;Integrated Security=True;TrustServerCertificate=True"
  }
}
```

## 📝 Notas

- La herramienta solo aplica las migraciones pendientes
- Si no hay migraciones pendientes, lo indica y termina exitosamente
- Si una base de datos no está configurada o no está disponible, muestra un error pero continúa con las demás
- Retorna código 0 si al menos una BD migró exitosamente, 1 solo si todas fallaron
- Los errores de conexión no detienen la ejecución al usar "all"

## 🔄 Integración con CI/CD

### Azure Pipelines
```yaml
- task: DotNetCoreCLI@2
  displayName: 'Apply Database Migrations'
  inputs:
    command: 'run'
    projects: 'tools/FastServer.DbMigrator/FastServer.DbMigrator.csproj'
    arguments: 'sqlserver'
```

### GitHub Actions
```yaml
- name: Apply Database Migrations
  run: dotnet run --project tools/FastServer.DbMigrator sqlserver
```

### Docker
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet build tools/FastServer.DbMigrator

FROM mcr.microsoft.com/dotnet/runtime:10.0
WORKDIR /app
COPY --from=build /src/tools/FastServer.DbMigrator/bin/Release/net10.0 .
ENTRYPOINT ["dotnet", "FastServer.DbMigrator.dll"]
```
