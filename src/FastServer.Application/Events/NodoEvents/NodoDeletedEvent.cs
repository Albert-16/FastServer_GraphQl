namespace FastServer.Application.Events.NodoEvents;

/// <summary>
/// Evento publicado cuando se elimina un nodo
/// </summary>
public record NodoDeletedEvent(
    Guid NodoId);
