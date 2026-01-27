using FastServer.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace FastServer.DbMigrator;

/// <summary>
/// Herramienta para aplicar migraciones de base de datos
/// </summary>
class Program
{
    static async Task<int> Main(string[] args)
    {
        // Configurar Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            Log.Information("=== FastServer Database Migrator ===");
            Log.Information("Iniciando aplicación de migraciones...");

            var host = CreateHostBuilder(args).Build();

            var config = host.Services.GetRequiredService<IConfiguration>();

            // Determinar qué base de datos migrar
            var database = args.Length > 0 ? args[0].ToLower() : "all";

            switch (database)
            {
                case "postgres":
                case "postgresql":
                    await MigratePostgreSqlAsync(host.Services, config);
                    break;

                case "sqlserver":
                case "mssql":
                    await MigrateSqlServerAsync(host.Services, config);
                    break;

                case "all":
                default:
                    Log.Information("Aplicando migraciones a todas las bases de datos configuradas...");

                    var pgSuccess = await MigratePostgreSqlAsync(host.Services, config);
                    var sqlSuccess = await MigrateSqlServerAsync(host.Services, config);

                    // Si ambas fallaron, retornar error
                    if (!pgSuccess && !sqlSuccess)
                    {
                        Log.Fatal("Todas las migraciones fallaron");
                        return 1;
                    }
                    break;
            }

            Log.Information("✓ Migraciones aplicadas exitosamente");
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "✗ Error al aplicar migraciones");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureAppConfiguration((context, config) =>
            {
                // Obtener el directorio base de la aplicación
                var basePath = AppContext.BaseDirectory;

                config.SetBasePath(basePath)
                      .AddJsonFile("appsettings.json", optional: false)
                      .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
                      .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                var config = context.Configuration;

                // Configurar PostgreSQL si existe
                var postgresConnection = config.GetConnectionString("PostgreSQL");
                if (!string.IsNullOrEmpty(postgresConnection))
                {
                    services.AddDbContext<PostgreSqlDbContext>(options =>
                        options.UseNpgsql(postgresConnection));
                }

                // Configurar SQL Server si existe
                var sqlServerConnection = config.GetConnectionString("SqlServer");
                if (!string.IsNullOrEmpty(sqlServerConnection))
                {
                    services.AddDbContext<SqlServerDbContext>(options =>
                        options.UseSqlServer(sqlServerConnection));
                }
            });

    static async Task<bool> MigratePostgreSqlAsync(IServiceProvider services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("PostgreSQL");

        if (string.IsNullOrEmpty(connectionString))
        {
            Log.Warning("PostgreSQL: No hay cadena de conexión configurada, omitiendo...");
            return false;
        }

        Log.Information("PostgreSQL: Aplicando migraciones...");

        try
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PostgreSqlDbContext>();

            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            var pendingCount = pendingMigrations.Count();

            if (pendingCount == 0)
            {
                Log.Information("PostgreSQL: No hay migraciones pendientes");
                return true;
            }

            Log.Information($"PostgreSQL: Aplicando {pendingCount} migración(es) pendiente(s)...");
            await context.Database.MigrateAsync();
            Log.Information("PostgreSQL: ✓ Migraciones aplicadas correctamente");
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "PostgreSQL: ✗ Error al aplicar migraciones");
            return false;
        }
    }

    static async Task<bool> MigrateSqlServerAsync(IServiceProvider services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("SqlServer");

        if (string.IsNullOrEmpty(connectionString))
        {
            Log.Warning("SQL Server: No hay cadena de conexión configurada, omitiendo...");
            return false;
        }

        Log.Information("SQL Server: Aplicando migraciones...");

        try
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SqlServerDbContext>();

            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            var pendingCount = pendingMigrations.Count();

            if (pendingCount == 0)
            {
                Log.Information("SQL Server: No hay migraciones pendientes");
                return true;
            }

            Log.Information($"SQL Server: Aplicando {pendingCount} migración(es) pendiente(s)...");
            await context.Database.MigrateAsync();
            Log.Information("SQL Server: ✓ Migraciones aplicadas correctamente");
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "SQL Server: ✗ Error al aplicar migraciones");
            return false;
        }
    }
}
