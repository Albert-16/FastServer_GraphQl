namespace FastServer.Application.Events.MicroservicesRegisterTypeEvents;

/// <summary>
/// Evento publicado cuando se crea un nuevo tipo de registro de microservicio
/// </summary>
public record MicroservicesRegisterTypeCreatedEvent(
    Guid MicroservicesRegisterTypeId,
    string? MicroservicesRegisterTypeName,
    string? MicroservicesRegisterTypeDescription,
    DateTime? CreateAt);
