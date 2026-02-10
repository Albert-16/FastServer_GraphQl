using FastServer.Application.Events.LogEvents;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Publisher de eventos de logs para suscripciones GraphQL
/// </summary>
public interface ILogEventPublisher
{
    /// <summary>
    /// Publica un evento de creación de log
    /// </summary>
    Task PublishLogCreatedAsync(LogCreatedEvent logEvent);

    /// <summary>
    /// Publica un evento de actualización de log
    /// </summary>
    Task PublishLogUpdatedAsync(LogUpdatedEvent logEvent);

    /// <summary>
    /// Publica un evento de eliminación de log
    /// </summary>
    Task PublishLogDeletedAsync(LogDeletedEvent logEvent);
}
