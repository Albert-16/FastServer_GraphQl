namespace FastServer.Application.Events.MicroservicesRegisterTypeEvents;

/// <summary>
/// Evento publicado cuando se actualiza un tipo de registro de microservicio
/// </summary>
public record MicroservicesRegisterTypeUpdatedEvent(
    Guid MicroservicesRegisterTypeId,
    string? MicroservicesRegisterTypeName,
    string? MicroservicesRegisterTypeDescription,
    DateTime? ModifyAt);
