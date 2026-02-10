using FastServer.Application.Events.LogMicroserviceEvents;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Publisher de eventos de logs de microservicios para suscripciones GraphQL
/// </summary>
public interface ILogMicroserviceEventPublisher
{
    /// <summary>
    /// Publica un evento de creación de log de microservicio
    /// </summary>
    Task PublishLogMicroserviceCreatedAsync(LogMicroserviceCreatedEvent logEvent);

    /// <summary>
    /// Publica un evento de actualización de log de microservicio
    /// </summary>
    Task PublishLogMicroserviceUpdatedAsync(LogMicroserviceUpdatedEvent logEvent);

    /// <summary>
    /// Publica un evento de eliminación de log de microservicio
    /// </summary>
    Task PublishLogMicroserviceDeletedAsync(LogMicroserviceDeletedEvent logEvent);
}
