using FastServer.Application.Interfaces;
using FastServer.Domain.Interfaces;
using FastServer.Infrastructure.Data.Contexts;
using FastServer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FastServer.Infrastructure;

/// <summary>
/// Configuración de inyección de dependencias para la capa de Infrastructure.
/// Registra dos DbContexts PostgreSQL: uno para logs (FastServer_Logs) y otro para microservicios (FastServer).
/// </summary>
/// <remarks>
/// Esta clase es responsable de:
/// 1. Configurar Entity Framework Core con DbContextPool para ambas bases de datos PostgreSQL
/// 2. Configurar políticas de resiliencia (reintentos, timeouts)
/// 3. Registrar repositorios genéricos
///
/// Arquitectura:
/// - PostgreSqlLogsDbContext → FastServer_Logs (6 tablas de logging)
/// - PostgreSqlMicroservicesDbContext → FastServer (8 tablas de gestión de microservicios)
///
/// Ciclos de vida:
/// - DbContexts: Pooled (reutilización eficiente de instancias)
/// - Repositorios: Scoped (uno por request HTTP)
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
        // ========================================
        // CONFIGURACIÓN DE POSTGRESQL LOGS (FastServer_Logs)
        // ========================================
        var postgresLogsConnection = configuration.GetConnectionString("PostgreSQLLogs");
        if (!string.IsNullOrEmpty(postgresLogsConnection))
        {
            services.AddDbContextPool<PostgreSqlLogsDbContext>(options =>
                options.UseNpgsql(postgresLogsConnection, npgsqlOptions =>
                {
                    // Reintentar hasta 3 veces en caso de fallo transitorio
                    npgsqlOptions.EnableRetryOnFailure(3);

                    // Timeout de comandos SQL en 30 segundos
                    npgsqlOptions.CommandTimeout(30);
                }),
                poolSize: 128); // Pool optimizado para performance

            // Registrar interfaz para inyección en servicios de Application
            services.AddScoped<ILogsDbContext>(sp => sp.GetRequiredService<PostgreSqlLogsDbContext>());
        }

        // ========================================
        // CONFIGURACIÓN DE POSTGRESQL MICROSERVICES (FastServer)
        // ========================================
        var postgresMicroservicesConnection = configuration.GetConnectionString("PostgreSQLMicroservices");
        if (!string.IsNullOrEmpty(postgresMicroservicesConnection))
        {
            services.AddDbContextPool<PostgreSqlMicroservicesDbContext>(options =>
                options.UseNpgsql(postgresMicroservicesConnection, npgsqlOptions =>
                {
                    // Reintentar hasta 3 veces en caso de fallo transitorio
                    npgsqlOptions.EnableRetryOnFailure(3);

                    // Timeout de comandos SQL en 30 segundos
                    npgsqlOptions.CommandTimeout(30);
                }),
                poolSize: 128); // Pool optimizado para performance

            // Registrar interfaz para inyección en servicios de Application
            services.AddScoped<IMicroservicesDbContext>(sp => sp.GetRequiredService<PostgreSqlMicroservicesDbContext>());
        }

        // ========================================
        // REGISTRO DE REPOSITORIOS GENÉRICOS
        // ========================================
        // Permite que IRepository<TEntity> se resuelva automáticamente
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }

}
