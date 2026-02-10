using FastServer.Application.Events.LogServicesContentEvents;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Publisher de eventos de logs de contenido de servicio para suscripciones GraphQL
/// </summary>
public interface ILogServicesContentEventPublisher
{
    /// <summary>
    /// Publica un evento de creación de log de contenido de servicio
    /// </summary>
    Task PublishLogServicesContentCreatedAsync(LogServicesContentCreatedEvent logEvent);

    /// <summary>
    /// Publica un evento de actualización de log de contenido de servicio
    /// </summary>
    Task PublishLogServicesContentUpdatedAsync(LogServicesContentUpdatedEvent logEvent);

    /// <summary>
    /// Publica un evento de eliminación de log de contenido de servicio
    /// </summary>
    Task PublishLogServicesContentDeletedAsync(LogServicesContentDeletedEvent logEvent);
}
