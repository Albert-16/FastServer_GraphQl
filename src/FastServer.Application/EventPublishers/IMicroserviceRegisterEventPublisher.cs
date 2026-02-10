using FastServer.Application.Events.MicroserviceRegisterEvents;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Publisher de eventos de registros de microservicios para suscripciones GraphQL
/// </summary>
public interface IMicroserviceRegisterEventPublisher
{
    /// <summary>
    /// Publica un evento de creación de registro de microservicio
    /// </summary>
    Task PublishMicroserviceRegisterCreatedAsync(MicroserviceRegisterCreatedEvent microserviceEvent);

    /// <summary>
    /// Publica un evento de actualización de registro de microservicio
    /// </summary>
    Task PublishMicroserviceRegisterUpdatedAsync(MicroserviceRegisterUpdatedEvent microserviceEvent);

    /// <summary>
    /// Publica un evento de eliminación de registro de microservicio
    /// </summary>
    Task PublishMicroserviceRegisterDeletedAsync(MicroserviceRegisterDeletedEvent microserviceEvent);
}
