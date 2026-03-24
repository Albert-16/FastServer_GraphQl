using FastServer.Application.Events.NodoEvents;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Publisher de eventos de nodos para suscripciones GraphQL
/// </summary>
public interface INodoEventPublisher
{
    /// <summary>
    /// Publica un evento de creacion de nodo
    /// </summary>
    Task PublishNodoCreatedAsync(NodoCreatedEvent evt, CancellationToken ct = default);

    /// <summary>
    /// Publica un evento de actualizacion de nodo
    /// </summary>
    Task PublishNodoUpdatedAsync(NodoUpdatedEvent evt, CancellationToken ct = default);

    /// <summary>
    /// Publica un evento de eliminacion de nodo
    /// </summary>
    Task PublishNodoDeletedAsync(NodoDeletedEvent evt, CancellationToken ct = default);
}
