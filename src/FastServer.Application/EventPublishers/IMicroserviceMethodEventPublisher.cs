using FastServer.Application.Events.MicroserviceMethodEvents;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Publisher de eventos de métodos de microservicios para suscripciones GraphQL
/// </summary>
public interface IMicroserviceMethodEventPublisher
{
    Task PublishMicroserviceMethodCreatedAsync(MicroserviceMethodCreatedEvent methodEvent);
    Task PublishMicroserviceMethodUpdatedAsync(MicroserviceMethodUpdatedEvent methodEvent);
    Task PublishMicroserviceMethodDeletedAsync(MicroserviceMethodDeletedEvent methodEvent);
}
