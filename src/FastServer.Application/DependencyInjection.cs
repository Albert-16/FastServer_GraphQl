using FastServer.Application.EventPublishers;
using FastServer.Application.Interfaces;
using FastServer.Application.Mappings;
using FastServer.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FastServer.Application;

/// <summary>
/// Extensión para registrar los servicios de la capa de Application
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // AutoMapper — escanea todos los Profile en este assembly
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));

        // Servicios de logs
        services.AddScoped<ILogServicesHeaderService, LogServicesHeaderService>();
        services.AddScoped<ILogMicroserviceService, LogMicroserviceService>();
        services.AddScoped<ILogServicesContentService, LogServicesContentService>();

        // Event Publishers para suscripciones
        services.AddScoped<ILogEventPublisher, LogEventPublisher>();
        services.AddScoped<ILogMicroserviceEventPublisher, LogMicroserviceEventPublisher>();
        services.AddScoped<ILogServicesContentEventPublisher, LogServicesContentEventPublisher>();
        services.AddScoped<IMicroserviceRegisterEventPublisher, MicroserviceRegisterEventPublisher>();
        services.AddScoped<IMicroservicesClusterEventPublisher, MicroservicesClusterEventPublisher>();
        services.AddScoped<IUserEventPublisher, UserEventPublisher>();
        services.AddScoped<IActivityLogEventPublisher, ActivityLogEventPublisher>();
        services.AddScoped<ICoreConnectorCredentialEventPublisher, CoreConnectorCredentialEventPublisher>();
        services.AddScoped<IMicroserviceMethodEventPublisher, MicroserviceMethodEventPublisher>();

        return services;
    }
}
