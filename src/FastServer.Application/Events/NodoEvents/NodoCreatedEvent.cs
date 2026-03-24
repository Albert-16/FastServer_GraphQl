namespace FastServer.Application.Events.NodoEvents;

/// <summary>
/// Evento publicado cuando se crea un nuevo nodo
/// </summary>
public record NodoCreatedEvent(
    Guid NodoId,
    Guid MicroserviceMethodId,
    Guid MicroservicesClusterId,
    DateTime? CreateAt);
