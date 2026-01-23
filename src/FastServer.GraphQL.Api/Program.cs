using FastServer.Application;
using FastServer.Domain.Enums;
using FastServer.GraphQL.Api.GraphQL;
using FastServer.GraphQL.Api.GraphQL.Mutations;
using FastServer.GraphQL.Api.GraphQL.Queries;
using FastServer.GraphQL.Api.GraphQL.Types;
using FastServer.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Agregar servicios de las capas
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Configurar el origen de datos predeterminado
var defaultDataSource = builder.Configuration.GetValue<string>("DefaultDataSource") ?? "PostgreSQL";
var dataSourceType = Enum.Parse<DataSourceType>(defaultDataSource, true);
builder.Services.ConfigureDefaultDataSource(dataSourceType);

// Configurar GraphQL con Hot Chocolate
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddTypeExtension<LogServicesQuery>()
    .AddTypeExtension<LogMicroserviceQuery>()
    .AddTypeExtension<LogServicesContentQuery>()
    .AddTypeExtension<LogServicesMutation>()
    .AddTypeExtension<LogMicroserviceMutation>()
    .AddTypeExtension<LogServicesContentMutation>()
    .AddType<LogServicesHeaderType>()
    .AddType<LogMicroserviceType>()
    .AddType<LogServicesContentType>()
    .AddType<PaginatedLogServicesHeaderType>()
    .AddType<CreateLogServicesHeaderInputType>()
    .AddType<UpdateLogServicesHeaderInputType>()
    .AddType<CreateLogMicroserviceInputType>()
    .AddType<CreateLogServicesContentInputType>()
    .AddType<LogFilterInputType>()
    .AddType<PaginationInputType>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = builder.Environment.IsDevelopment())
    .AddInstrumentation(o =>
    {
        o.RenameRootActivity = true;
        o.IncludeDocument = true;
    });

// Agregar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Agregar Health Checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configurar pipeline
app.UseSerilogRequestLogging();

app.UseCors("AllowAll");

app.UseRouting();

// Endpoint de health check
app.MapHealthChecks("/health");

// Endpoint de GraphQL
app.MapGraphQL();

// Página principal redirige a GraphQL
app.MapGet("/", () => Results.Redirect("/graphql"));

Log.Information("FastServer GraphQL API iniciando...");
Log.Information("Origen de datos predeterminado: {DataSource}", defaultDataSource);

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló al iniciar");
}
finally
{
    Log.CloseAndFlush();
}
