using FastServer.Application.Events.ActivityLogEvents;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Publisher de eventos de logs de actividad para suscripciones GraphQL
/// </summary>
public interface IActivityLogEventPublisher
{
    /// <summary>
    /// Publica un evento de creación de log de actividad
    /// </summary>
    Task PublishActivityLogCreatedAsync(ActivityLogCreatedEvent activityLogEvent);

    /// <summary>
    /// Publica un evento de actualización de log de actividad
    /// </summary>
    Task PublishActivityLogUpdatedAsync(ActivityLogUpdatedEvent activityLogEvent);

    /// <summary>
    /// Publica un evento de eliminación de log de actividad
    /// </summary>
    Task PublishActivityLogDeletedAsync(ActivityLogDeletedEvent activityLogEvent);
}
