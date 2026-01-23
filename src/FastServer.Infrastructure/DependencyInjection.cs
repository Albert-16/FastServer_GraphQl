using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;
using FastServer.Infrastructure.Data.Contexts;
using FastServer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FastServer.Infrastructure;

/// <summary>
/// Extensión para registrar los servicios de la capa de Infrastructure
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var availableDataSources = new List<DataSourceType>();

        // Configurar PostgreSQL si existe la cadena de conexión
        var postgresConnection = configuration.GetConnectionString("PostgreSQL");
        if (!string.IsNullOrEmpty(postgresConnection))
        {
            services.AddDbContext<PostgreSqlDbContext>(options =>
                options.UseNpgsql(postgresConnection, npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(3);
                    npgsqlOptions.CommandTimeout(30);
                }));

            availableDataSources.Add(DataSourceType.PostgreSQL);
        }

        // Configurar SQL Server si existe la cadena de conexión
        var sqlServerConnection = configuration.GetConnectionString("SqlServer");
        if (!string.IsNullOrEmpty(sqlServerConnection))
        {
            services.AddDbContext<SqlServerDbContext>(options =>
                options.UseSqlServer(sqlServerConnection, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(3);
                    sqlOptions.CommandTimeout(30);
                }));

            availableDataSources.Add(DataSourceType.SqlServer);
        }

        // Registrar la fábrica de orígenes de datos
        services.AddSingleton<IDataSourceFactory>(sp =>
            new DataSourceFactory(sp, availableDataSources));

        // Registrar repositorios genéricos
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }

    /// <summary>
    /// Configura el origen de datos predeterminado
    /// </summary>
    public static IServiceCollection ConfigureDefaultDataSource(
        this IServiceCollection services,
        DataSourceType defaultDataSource)
    {
        services.AddSingleton(defaultDataSource);
        return services;
    }
}
