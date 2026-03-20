using FastServer.Application.Events.FastServerClusterEvents;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Publisher de eventos de clusters de FastServer para suscripciones GraphQL
/// </summary>
public interface IFastServerClusterEventPublisher
{
    /// <summary>
    /// Publica un evento de creación de cluster de FastServer
    /// </summary>
    Task PublishFastServerClusterCreatedAsync(FastServerClusterCreatedEvent clusterEvent);

    /// <summary>
    /// Publica un evento de actualización de cluster de FastServer
    /// </summary>
    Task PublishFastServerClusterUpdatedAsync(FastServerClusterUpdatedEvent clusterEvent);

    /// <summary>
    /// Publica un evento de eliminación de cluster de FastServer
    /// </summary>
    Task PublishFastServerClusterDeletedAsync(FastServerClusterDeletedEvent clusterEvent);
}
