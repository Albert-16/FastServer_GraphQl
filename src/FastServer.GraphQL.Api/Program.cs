using FastServer.Application;
using FastServer.GraphQL.Api.GraphQL;
using FastServer.GraphQL.Api.GraphQL.Mutations;
using FastServer.GraphQL.Api.GraphQL.Queries;
using FastServer.GraphQL.Api.GraphQL.Subscriptions;
using FastServer.GraphQL.Api.GraphQL.Types;
using FastServer.Infrastructure;
using FastServer.Infrastructure.Data;
using Serilog;

/*
 * FastServer GraphQL API
 *
 * API GraphQL para gestionar logs de servicios y microservicios usando PostgreSQL exclusivamente.
 * Dos bases de datos PostgreSQL separadas:
 *   - FastServer_Logs: Logging de servicios (6 tablas)
 *   - FastServer: Gestión de microservicios (8 tablas)
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
// VALIDACIÓN DE CONFIGURACIÓN DE BASES DE DATOS POSTGRESQL
// ========================================

// Obtener cadenas de conexión desde appsettings.json
var postgresLogsConn = builder.Configuration.GetConnectionString("PostgreSQLLogs");
var postgresMicroservicesConn = builder.Configuration.GetConnectionString("PostgreSQLMicroservices");

// Validar que ambas bases de datos PostgreSQL estén configuradas
// FastServer_Logs (logs) y FastServer (microservicios)
if (string.IsNullOrEmpty(postgresLogsConn))
{
    throw new InvalidOperationException(
        "La cadena de conexión 'PostgreSQLLogs' debe estar configurada en appsettings.json");
}

if (string.IsNullOrEmpty(postgresMicroservicesConn))
{
    throw new InvalidOperationException(
        "La cadena de conexión 'PostgreSQLMicroservices' debe estar configurada en appsettings.json");
}

Log.Information("Configuración de bases de datos:");
Log.Information("  - BD Logs: FastServer_Logs (PostgreSQL)");
Log.Information("  - BD Microservices: FastServer (PostgreSQL)");

// ========================================
// CONFIGURACIÓN DE GRAPHQL CON HOT CHOCOLATE
// ========================================

// Configurar servidor GraphQL con todas las queries, mutations, subscriptions y tipos
builder.Services
    .AddGraphQLServer()
    // Tipos raíz de GraphQL
    .AddQueryType<Query>()                       // Query raíz (punto de entrada para consultas)
    .AddMutationType<Mutation>()                 // Mutation raíz (punto de entrada para modificaciones)
    .AddSubscriptionType<Subscription>()         // Subscription raíz (punto de entrada para suscripciones en tiempo real)

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

    // Extensiones de subscriptions (se unen al tipo Subscription raíz)
    .AddTypeExtension<LogServicesSubscription>()          // Suscripciones para LogServicesHeader
    .AddTypeExtension<LogMicroserviceSubscription>()      // Suscripciones para LogMicroservice
    .AddTypeExtension<LogServicesContentSubscription>()   // Suscripciones para LogServicesContent
    .AddTypeExtension<MicroserviceRegisterSubscription>() // Suscripciones para MicroserviceRegister
    .AddTypeExtension<MicroservicesClusterSubscription>() // Suscripciones para MicroservicesCluster
    .AddTypeExtension<UserSubscription>()                 // Suscripciones para User
    .AddTypeExtension<ActivityLogSubscription>()          // Suscripciones para ActivityLog
    .AddTypeExtension<CoreConnectorCredentialSubscription>() // Suscripciones para CoreConnectorCredential

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
    .AddType<BulkCreateLogServicesHeaderInputType>()
    .AddType<BulkCreateLogMicroserviceInputType>()
    .AddType<BulkInsertErrorType>()
    .AddType<BulkInsertLogServicesHeaderResultType>()
    .AddType<BulkInsertLogMicroserviceResultType>()

    // Características adicionales de Hot Chocolate
    .AddFiltering()                              // Habilita filtrado en queries
    .AddSorting()                                // Habilita ordenamiento en queries
    .AddProjections()                            // Habilita proyecciones para optimizar queries

    // Suscripciones en tiempo real (in-memory)
    .AddInMemorySubscriptions()                  // Sistema de pub/sub en memoria para suscripciones GraphQL

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

// Habilitar WebSockets para suscripciones GraphQL (debe ir antes de UseRouting)
app.UseWebSockets();

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
Log.Information("Arquitectura: PostgreSQL exclusivo (FastServer_Logs + FastServer)");

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
