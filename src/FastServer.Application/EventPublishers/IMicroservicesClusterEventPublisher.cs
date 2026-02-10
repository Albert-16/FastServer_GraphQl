using FastServer.Application.Events.MicroservicesClusterEvents;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Publisher de eventos de clusters de microservicios para suscripciones GraphQL
/// </summary>
public interface IMicroservicesClusterEventPublisher
{
    /// <summary>
    /// Publica un evento de creación de cluster de microservicios
    /// </summary>
    Task PublishMicroservicesClusterCreatedAsync(MicroservicesClusterCreatedEvent clusterEvent);

    /// <summary>
    /// Publica un evento de actualización de cluster de microservicios
    /// </summary>
    Task PublishMicroservicesClusterUpdatedAsync(MicroservicesClusterUpdatedEvent clusterEvent);

    /// <summary>
    /// Publica un evento de eliminación de cluster de microservicios
    /// </summary>
    Task PublishMicroservicesClusterDeletedAsync(MicroservicesClusterDeletedEvent clusterEvent);
}
