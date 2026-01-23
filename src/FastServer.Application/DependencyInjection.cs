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
        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // Servicios
        services.AddScoped<ILogServicesHeaderService, LogServicesHeaderService>();
        services.AddScoped<ILogMicroserviceService, LogMicroserviceService>();
        services.AddScoped<ILogServicesContentService, LogServicesContentService>();

        return services;
    }
}
