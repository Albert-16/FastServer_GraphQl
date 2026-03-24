using FastServer.Application.Events.MicroservicesRegisterTypeEvents;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Publisher de eventos de tipos de registro de microservicios para suscripciones GraphQL
/// </summary>
public interface IMicroservicesRegisterTypeEventPublisher
{
    /// <summary>
    /// Publica un evento de creacion de tipo de registro de microservicio
    /// </summary>
    Task PublishMicroservicesRegisterTypeCreatedAsync(MicroservicesRegisterTypeCreatedEvent evt, CancellationToken ct = default);

    /// <summary>
    /// Publica un evento de actualizacion de tipo de registro de microservicio
    /// </summary>
    Task PublishMicroservicesRegisterTypeUpdatedAsync(MicroservicesRegisterTypeUpdatedEvent evt, CancellationToken ct = default);

    /// <summary>
    /// Publica un evento de eliminacion de tipo de registro de microservicio
    /// </summary>
    Task PublishMicroservicesRegisterTypeDeletedAsync(MicroservicesRegisterTypeDeletedEvent evt, CancellationToken ct = default);
}
