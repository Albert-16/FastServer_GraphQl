namespace FastServer.Application.Events.MicroservicesRegisterTypeEvents;

/// <summary>
/// Evento publicado cuando se elimina un tipo de registro de microservicio
/// </summary>
public record MicroservicesRegisterTypeDeletedEvent(
    Guid MicroservicesRegisterTypeId);
