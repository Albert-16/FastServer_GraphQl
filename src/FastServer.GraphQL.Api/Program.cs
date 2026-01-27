using FastServer.Application;
using FastServer.Domain.Enums;
using FastServer.GraphQL.Api.GraphQL;
using FastServer.GraphQL.Api.GraphQL.Mutations;
using FastServer.GraphQL.Api.GraphQL.Queries;
using FastServer.GraphQL.Api.GraphQL.Types;
using FastServer.Infrastructure;
using FastServer.Infrastructure.Data;
using Serilog;

/*
 * FastServer GraphQL API
 *
 * API GraphQL para gestionar logs de servicios con soporte multi-base de datos.
 * Soporta PostgreSQL y SQL Server, permitiendo consultas dinámicas a cualquiera
 * de las bases de datos configuradas.
 *
 * Arquitectura: Clean Architecture con capas Domain, Application, Infrastructure y API
 *
 * Principales características:
 * - GraphQL con Hot Chocolate 15
 * - Multi-base de datos (PostgreSQL y SQL Server)
 * - Health Checks para monitoreo
 * - Logging estructurado con Serilog
 * - Paginación y filtrado avanzado
 */

var builder = WebApplication.CreateBuilder(args);

// ========================================
// CONFIGURACIÓN DE LOGGING
// ========================================

// Configurar Serilog para logging estructurado
// Lee la configuración desde appsettings.json (niveles, sinks, etc.)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// ========================================
// REGISTRO DE SERVICIOS POR CAPA
// ========================================

// Registrar servicios de la capa de aplicación (servicios de negocio, DTOs, AutoMapper)
builder.Services.AddApplicationServices();

// Registrar servicios de infraestructura (DbContexts, Repositorios, UnitOfWork)
builder.Services.AddInfrastructureServices(builder.Configuration);

// Registrar servicios de microservicios
builder.Services.AddScoped<FastServer.Application.Services.Microservices.EventTypeService>();
builder.Services.AddScoped<FastServer.Application.Services.Microservices.UserService>();
builder.Services.AddScoped<FastServer.Application.Services.Microservices.ActivityLogService>();
builder.Services.AddScoped<FastServer.Application.Services.Microservices.MicroserviceRegisterService>();
builder.Services.AddScoped<FastServer.Application.Services.Microservices.MicroservicesClusterService>();
builder.Services.AddScoped<FastServer.Application.Services.Microservices.CoreConnectorCredentialService>();
builder.Services.AddScoped<FastServer.Application.Services.Microservices.MicroserviceCoreConnectorService>();

// ========================================
// VALIDACIÓN DE CONFIGURACIÓN DE BASES DE DATOS
// ========================================

// Obtener cadenas de conexión desde appsettings.json
var postgresConn = builder.Configuration.GetConnectionString("PostgreSQL");
var sqlServerConn = builder.Configuration.GetConnectionString("SqlServer");

// Validar que al menos una base de datos esté configurada
// Esto garantiza que la aplicación tenga al menos un origen de datos disponible
if (string.IsNullOrEmpty(postgresConn) && string.IsNullOrEmpty(sqlServerConn))
{
    throw new InvalidOperationException(
        "Al menos una cadena de conexión (PostgreSQL o SqlServer) debe estar configurada");
}

// ========================================
// CONFIGURACIÓN DEL ORIGEN DE DATOS PREDETERMINADO
// ========================================

// Leer el origen de datos predeterminado desde appsettings.json
// Si no está configurado, se usa PostgreSQL por defecto
var defaultDataSource = builder.Configuration.GetValue<string>("DefaultDataSource") ?? "PostgreSQL";

// Parsear el string a enum DataSourceType
if (!Enum.TryParse<DataSourceType>(defaultDataSource, true, out var dataSourceType))
{
    throw new InvalidOperationException(
        $"DefaultDataSource '{defaultDataSource}' no es válido. Valores aceptados: PostgreSQL, SqlServer");
}

// Validar que el origen predeterminado tenga cadena de conexión configurada
// Esto previene errores en tiempo de ejecución cuando se intente usar el origen predeterminado
if (dataSourceType == DataSourceType.PostgreSQL && string.IsNullOrEmpty(postgresConn))
{
    throw new InvalidOperationException(
        "DefaultDataSource está configurado como PostgreSQL pero no hay cadena de conexión PostgreSQL");
}

if (dataSourceType == DataSourceType.SqlServer && string.IsNullOrEmpty(sqlServerConn))
{
    throw new InvalidOperationException(
        "DefaultDataSource está configurado como SqlServer pero no hay cadena de conexión SqlServer");
}

// Registrar DataSourceSettings como Singleton en DI
// Esto permite que todos los servicios accedan al origen de datos predeterminado
builder.Services.ConfigureDefaultDataSource(dataSourceType);

// ========================================
// CONFIGURACIÓN DE GRAPHQL CON HOT CHOCOLATE
// ========================================

// Configurar servidor GraphQL con todas las queries, mutations y tipos
builder.Services
    .AddGraphQLServer()
    // Tipos raíz de GraphQL
    .AddQueryType<Query>()                       // Query raíz (punto de entrada para consultas)
    .AddMutationType<Mutation>()                 // Mutation raíz (punto de entrada para modificaciones)

    // Extensiones de queries (se unen al tipo Query raíz)
    .AddTypeExtension<LogServicesQuery>()        // Queries para LogServicesHeader
    .AddTypeExtension<LogMicroserviceQuery>()    // Queries para LogMicroservice
    .AddTypeExtension<LogServicesContentQuery>() // Queries para LogServicesContent
    .AddTypeExtension<MicroservicesQuery>()      // Queries para Microservicios (NUEVO)

    // Extensiones de mutations (se unen al tipo Mutation raíz)
    .AddTypeExtension<LogServicesMutation>()     // Mutations para LogServicesHeader
    .AddTypeExtension<LogMicroserviceMutation>() // Mutations para LogMicroservice
    .AddTypeExtension<LogServicesContentMutation>() // Mutations para LogServicesContent
    .AddTypeExtension<MicroservicesMutation>()   // Mutations para Microservicios (NUEVO)

    // Tipos de objetos GraphQL (representan entidades del dominio)
    .AddType<LogServicesHeaderType>()
    .AddType<LogMicroserviceType>()
    .AddType<LogServicesContentType>()
    .AddType<PaginatedLogServicesHeaderType>()

    // Tipos de microservicios (NUEVO)
    .AddType<FastServer.GraphQL.Api.GraphQL.Types.Microservices.EventTypeType>()
    .AddType<FastServer.GraphQL.Api.GraphQL.Types.Microservices.UserType>()
    .AddType<FastServer.GraphQL.Api.GraphQL.Types.Microservices.ActivityLogType>()
    .AddType<FastServer.GraphQL.Api.GraphQL.Types.Microservices.MicroserviceRegisterType>()
    .AddType<FastServer.GraphQL.Api.GraphQL.Types.Microservices.MicroservicesClusterType>()
    .AddType<FastServer.GraphQL.Api.GraphQL.Types.Microservices.CoreConnectorCredentialType>()
    .AddType<FastServer.GraphQL.Api.GraphQL.Types.Microservices.MicroserviceCoreConnectorType>()

    // Tipos de input (para recibir datos en mutations)
    .AddType<CreateLogServicesHeaderInputType>()
    .AddType<UpdateLogServicesHeaderInputType>()
    .AddType<CreateLogMicroserviceInputType>()
    .AddType<CreateLogServicesContentInputType>()
    .AddType<LogFilterInputType>()
    .AddType<PaginationInputType>()

    // Características adicionales de Hot Chocolate
    .AddFiltering()                              // Habilita filtrado en queries
    .AddSorting()                                // Habilita ordenamiento en queries
    .AddProjections()                            // Habilita proyecciones para optimizar queries

    // Opciones de configuración
    .ModifyRequestOptions(opt =>
    {
        // Incluir detalles de excepciones solo en desarrollo (seguridad)
        opt.IncludeExceptionDetails = builder.Environment.IsDevelopment();
    })
    .AddInstrumentation(o =>
    {
        // Configuración de telemetría/observabilidad
        o.RenameRootActivity = true;
        o.IncludeDocument = true;
    });

// ========================================
// CONFIGURACIÓN DE CORS
// ========================================

// Configurar CORS para permitir solicitudes desde cualquier origen
// NOTA: En producción, se debe restringir a orígenes específicos por seguridad
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // Permite cualquier dominio
              .AllowAnyMethod()    // Permite cualquier método HTTP (GET, POST, etc.)
              .AllowAnyHeader();   // Permite cualquier header
    });
});

// ========================================
// CONFIGURACIÓN DE HEALTH CHECKS
// ========================================

// Los health checks permiten monitorear el estado de la aplicación y sus dependencias
// Se usan en Kubernetes, Docker Swarm y sistemas de monitoreo para verificar disponibilidad
var healthChecksBuilder = builder.Services.AddHealthChecks();

// Agregar health check para PostgreSQL si está configurado
// Verifica que la base de datos PostgreSQL esté disponible y responda
var postgresConnection = builder.Configuration.GetConnectionString("PostgreSQL");
if (!string.IsNullOrEmpty(postgresConnection))
{
    healthChecksBuilder.AddNpgSql(
        postgresConnection,
        name: "postgresql-db",
        tags: new[] { "db", "postgresql", "ready" }); // Tag 'ready' para readiness probe
}

// Agregar health check para SQL Server si está configurado
// Verifica que la base de datos SQL Server esté disponible y responda
var sqlServerConnection = builder.Configuration.GetConnectionString("SqlServer");
if (!string.IsNullOrEmpty(sqlServerConnection))
{
    healthChecksBuilder.AddSqlServer(
        sqlServerConnection,
        name: "sqlserver-db",
        tags: new[] { "db", "sqlserver", "ready" }); // Tag 'ready' para readiness probe
}

// ========================================
// CONSTRUCCIÓN DE LA APLICACIÓN
// ========================================

var app = builder.Build();

// ========================================
// IMPORTANTE: MIGRACIONES DE BASE DE DATOS
// ========================================
// Las migraciones NO se aplican automáticamente al iniciar la API.
// Esto previene problemas en entornos de producción y permite control manual.
//
// Para aplicar migraciones:
// 1. Usando DbMigrator (recomendado):
//    dotnet run --project tools/FastServer.DbMigrator
//
// 2. Usando EF Core CLI:
//    dotnet ef database update --project src/FastServer.Infrastructure --context SqlServerDbContext
//
// 3. En pipelines de CI/CD:
//    Ejecutar DbMigrator como paso previo al despliegue

// ========================================
// CONFIGURACIÓN DEL PIPELINE DE MIDDLEWARE
// ========================================

// Logging de requests HTTP con Serilog (registra cada solicitud)
app.UseSerilogRequestLogging();

// Habilitar CORS (debe ir antes de UseRouting)
app.UseCors("AllowAll");

// Habilitar routing
app.UseRouting();

// ========================================
// ENDPOINTS DE HEALTH CHECK
// ========================================

// Liveness Probe: /health
// Verifica que la aplicación esté viva y pueda procesar solicitudes
// Retorna: "Healthy" o "Unhealthy"
// Uso: Kubernetes lo usa para reiniciar pods que no respondan
app.MapHealthChecks("/health");

// Readiness Probe: /health/ready
// Verifica que la aplicación esté lista para recibir tráfico
// Incluye checks de bases de datos y dependencias externas
// Retorna: JSON con estado detallado de cada dependencia
// Uso: Kubernetes lo usa para saber cuándo enviar tráfico al pod
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    // Solo incluir checks con tag 'ready' (bases de datos)
    Predicate = check => check.Tags.Contains("ready"),

    // Escribir respuesta detallada en JSON
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds
            })
        });
        await context.Response.WriteAsync(result);
    }
});

// ========================================
// ENDPOINTS DE GRAPHQL
// ========================================

// Endpoint principal de GraphQL: /graphql
// Aquí se procesan todas las queries y mutations
// También sirve Banana Cake Pop (GraphQL IDE) en desarrollo
app.MapGraphQL();

// Redirigir página principal a GraphQL IDE
app.MapGet("/", () => Results.Redirect("/graphql"));

// ========================================
// INICIO DE LA APLICACIÓN
// ========================================

Log.Information("FastServer GraphQL API iniciando...");
Log.Information("Origen de datos predeterminado: {DataSource}", defaultDataSource);

try
{
    // Iniciar el servidor y bloquear hasta que se detenga
    app.Run();
}
catch (Exception ex)
{
    // Capturar errores fatales durante el inicio
    Log.Fatal(ex, "La aplicación falló al iniciar");
}
finally
{
    // Asegurar que todos los logs se escriban antes de cerrar
    Log.CloseAndFlush();
}
