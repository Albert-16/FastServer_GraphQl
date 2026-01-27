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
        var logger = scope.ServiceProvider.GetService<ILogger<PostgreSqlDbContext>>();

        try
        {
            var context = scope.ServiceProvider.GetService<PostgreSqlDbContext>();
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
    /// Aplica las migraciones pendientes para SQL Server
    /// </summary>
    public static async Task MigrateSqlServerAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetService<ILogger<SqlServerDbContext>>();

        try
        {
            var context = scope.ServiceProvider.GetService<SqlServerDbContext>();
            if (context != null)
            {
                logger?.LogInformation("Aplicando migraciones de SQL Server...");
                await context.Database.MigrateAsync();
                logger?.LogInformation("Migraciones de SQL Server aplicadas correctamente");
            }
        }
        catch (Exception ex)
        {
            logger?.LogWarning(ex, "No se pudieron aplicar las migraciones de SQL Server. La base de datos podría no estar configurada.");
        }
    }

    /// <summary>
    /// Aplica las migraciones pendientes para todas las bases de datos configuradas
    /// </summary>
    public static async Task MigrateAllDatabasesAsync(this IServiceProvider serviceProvider)
    {
        await serviceProvider.MigratePostgreSqlAsync();
        await serviceProvider.MigrateSqlServerAsync();
    }

    /// <summary>
    /// Asegura que la base de datos PostgreSQL está creada (útil para desarrollo)
    /// </summary>
    public static async Task EnsurePostgreSqlCreatedAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetService<ILogger<PostgreSqlDbContext>>();

        try
        {
            var context = scope.ServiceProvider.GetService<PostgreSqlDbContext>();
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
    /// Asegura que la base de datos SQL Server está creada (útil para desarrollo)
    /// </summary>
    public static async Task EnsureSqlServerCreatedAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetService<ILogger<SqlServerDbContext>>();

        try
        {
            var context = scope.ServiceProvider.GetService<SqlServerDbContext>();
            if (context != null)
            {
                logger?.LogInformation("Verificando base de datos SQL Server...");
                await context.Database.EnsureCreatedAsync();
                logger?.LogInformation("Base de datos SQL Server verificada");
            }
        }
        catch (Exception ex)
        {
            logger?.LogWarning(ex, "No se pudo verificar la base de datos SQL Server.");
        }
    }
}
