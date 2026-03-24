namespace FastServer.Application.Events.NodoEvents;

/// <summary>
/// Evento publicado cuando se actualiza un nodo
/// </summary>
public record NodoUpdatedEvent(
    Guid NodoId,
    Guid MicroserviceMethodId,
    Guid MicroservicesClusterId,
    DateTime? ModifyAt);
