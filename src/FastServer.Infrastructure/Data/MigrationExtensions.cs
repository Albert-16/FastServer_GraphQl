using FastServer.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FastServer.Infrastructure.Data;

/// <summary>
/// Extensiones para aplicar migraciones de base de datos
/// </summary>
public static class MigrationExtensions
{
    /// <summary>
    /// Aplica las migraciones pendientes para PostgreSQL
    /// </summary>
    public static async Task MigratePostgreSqlAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetService<ILogger<PostgreSqlLogsDbContext>>();

        try
        {
            var context = scope.ServiceProvider.GetService<PostgreSqlLogsDbContext>();
            if (context != null)
            {
                logger?.LogInformation("Aplicando migraciones de PostgreSQL...");
                await context.Database.MigrateAsync();
                logger?.LogInformation("Migraciones de PostgreSQL aplicadas correctamente");
            }
        }
        catch (Exception ex)
        {
            logger?.LogWarning(ex, "No se pudieron aplicar las migraciones de PostgreSQL. La base de datos podría no estar configurada.");
        }
    }

    /// <summary>
    /// Aplica las migraciones pendientes para PostgreSQL Microservices (BD: FastServer)
    /// </summary>
    public static async Task MigratePostgreSqlMicroservicesAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetService<ILogger<PostgreSqlMicroservicesDbContext>>();

        try
        {
            var context = scope.ServiceProvider.GetService<PostgreSqlMicroservicesDbContext>();
            if (context != null)
            {
                logger?.LogInformation("Aplicando migraciones de PostgreSQL Microservices (FastServer)...");
                await context.Database.MigrateAsync();
                logger?.LogInformation("Migraciones de PostgreSQL Microservices aplicadas correctamente");
            }
        }
        catch (Exception ex)
        {
            logger?.LogWarning(ex, "No se pudieron aplicar las migraciones de PostgreSQL Microservices. La base de datos podría no estar configurada.");
        }
    }

    /// <summary>
    /// Aplica las migraciones pendientes para todas las bases de datos PostgreSQL configuradas
    /// </summary>
    public static async Task MigrateAllDatabasesAsync(this IServiceProvider serviceProvider)
    {
        await serviceProvider.MigratePostgreSqlAsync();
        await serviceProvider.MigratePostgreSqlMicroservicesAsync();
    }

    /// <summary>
    /// Asegura que la base de datos PostgreSQL está creada (útil para desarrollo)
    /// </summary>
    public static async Task EnsurePostgreSqlCreatedAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetService<ILogger<PostgreSqlLogsDbContext>>();

        try
        {
            var context = scope.ServiceProvider.GetService<PostgreSqlLogsDbContext>();
            if (context != null)
            {
                logger?.LogInformation("Verificando base de datos PostgreSQL...");
                await context.Database.EnsureCreatedAsync();
                logger?.LogInformation("Base de datos PostgreSQL verificada");
            }
        }
        catch (Exception ex)
        {
            logger?.LogWarning(ex, "No se pudo verificar la base de datos PostgreSQL.");
        }
    }

    /// <summary>
    /// Asegura que la base de datos PostgreSQL Microservices está creada (útil para desarrollo)
    /// </summary>
    public static async Task EnsurePostgreSqlMicroservicesCreatedAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetService<ILogger<PostgreSqlMicroservicesDbContext>>();

        try
        {
            var context = scope.ServiceProvider.GetService<PostgreSqlMicroservicesDbContext>();
            if (context != null)
            {
                logger?.LogInformation("Verificando base de datos PostgreSQL Microservices (FastServer)...");
                await context.Database.EnsureCreatedAsync();
                logger?.LogInformation("Base de datos PostgreSQL Microservices verificada");
            }
        }
        catch (Exception ex)
        {
            logger?.LogWarning(ex, "No se pudo verificar la base de datos PostgreSQL Microservices.");
        }
    }
}
