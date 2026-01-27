using FastServer.Domain;
using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;
using FastServer.Infrastructure.Data.Contexts;
using FastServer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FastServer.Infrastructure;

/// <summary>
/// Configuración de inyección de dependencias para la capa de Infrastructure.
/// Registra DbContexts, repositorios y la fábrica de orígenes de datos.
/// </summary>
/// <remarks>
/// Esta clase es responsable de:
/// 1. Configurar Entity Framework Core para cada base de datos disponible
/// 2. Registrar la fábrica que permite crear UnitOfWork dinámicamente
/// 3. Configurar políticas de resiliencia (reintentos, timeouts)
///
/// Ciclos de vida de los servicios:
/// - DbContexts: Scoped (uno por request HTTP)
/// - DataSourceFactory: Scoped (uno por request HTTP)
/// - Repositorios: Scoped (uno por request HTTP)
/// - DataSourceSettings: Singleton (uno para toda la aplicación)
/// </remarks>
public static class DependencyInjection
{
    /// <summary>
    /// Registra todos los servicios de infraestructura en el contenedor de DI.
    /// </summary>
    /// <param name="services">Colección de servicios donde registrar las dependencias</param>
    /// <param name="configuration">Configuración de la aplicación (appsettings.json)</param>
    /// <returns>La misma colección de servicios para permitir encadenamiento</returns>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Lista para rastrear qué orígenes de datos están disponibles
        var availableDataSources = new List<DataSourceType>();

        // ========================================
        // CONFIGURACIÓN DE POSTGRESQL
        // ========================================
        // Solo se configura si hay cadena de conexión en appsettings.json
        var postgresConnection = configuration.GetConnectionString("PostgreSQL");
        if (!string.IsNullOrEmpty(postgresConnection))
        {
            services.AddDbContext<PostgreSqlDbContext>(options =>
                options.UseNpgsql(postgresConnection, npgsqlOptions =>
                {
                    // Reintentar hasta 3 veces en caso de fallo transitorio
                    npgsqlOptions.EnableRetryOnFailure(3);

                    // Timeout de comandos SQL en 30 segundos
                    npgsqlOptions.CommandTimeout(30);
                }));

            // Marcar PostgreSQL como disponible
            availableDataSources.Add(DataSourceType.PostgreSQL);
        }

        // ========================================
        // CONFIGURACIÓN DE SQL SERVER
        // ========================================
        // Solo se configura si hay cadena de conexión en appsettings.json
        var sqlServerConnection = configuration.GetConnectionString("SqlServer");
        if (!string.IsNullOrEmpty(sqlServerConnection))
        {
            services.AddDbContext<SqlServerDbContext>(options =>
                options.UseSqlServer(sqlServerConnection, sqlOptions =>
                {
                    // Reintentar hasta 3 veces en caso de fallo transitorio
                    sqlOptions.EnableRetryOnFailure(3);

                    // Timeout de comandos SQL en 30 segundos
                    sqlOptions.CommandTimeout(30);
                }));

            // Marcar SQL Server como disponible
            availableDataSources.Add(DataSourceType.SqlServer);
        }

        // ========================================
        // REGISTRO DE DATA SOURCE FACTORY
        // ========================================
        // La fábrica necesita saber qué orígenes de datos están disponibles
        // para poder validar y crear UnitOfWork apropiados
        var availableDataSourcesList = availableDataSources.ToList();
        services.AddScoped<IDataSourceFactory>(sp =>
            new DataSourceFactory(sp, availableDataSourcesList));

        // ========================================
        // REGISTRO DE REPOSITORIOS GENÉRICOS
        // ========================================
        // Permite que IRepository<TEntity> se resuelva automáticamente
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }

    /// <summary>
    /// Registra el origen de datos predeterminado que se usará cuando
    /// no se especifique explícitamente en las queries.
    /// </summary>
    /// <param name="services">Colección de servicios donde registrar la configuración</param>
    /// <param name="defaultDataSource">El origen de datos predeterminado (PostgreSQL o SqlServer)</param>
    /// <returns>La misma colección de servicios para permitir encadenamiento</returns>
    /// <remarks>
    /// DataSourceSettings se registra como Singleton porque:
    /// 1. Es inmutable (no cambia durante la ejecución)
    /// 2. Debe ser el mismo para toda la aplicación
    /// 3. Es thread-safe (solo lectura)
    ///
    /// Los servicios de aplicación inyectan este Singleton para conocer
    /// qué base de datos usar por defecto.
    /// </remarks>
    public static IServiceCollection ConfigureDefaultDataSource(
        this IServiceCollection services,
        DataSourceType defaultDataSource)
    {
        // Registrar como Singleton: una única instancia para toda la app
        services.AddSingleton(new DataSourceSettings(defaultDataSource));
        return services;
    }
}
