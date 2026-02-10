using FastServer.Application.Events.MicroserviceRegisterEvents;
using HotChocolate.Subscriptions;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Implementación del publisher de eventos de registros de microservicios usando HotChocolate
/// </summary>
public class MicroserviceRegisterEventPublisher : IMicroserviceRegisterEventPublisher
{
    private readonly ITopicEventSender _eventSender;

    public MicroserviceRegisterEventPublisher(ITopicEventSender eventSender)
    {
        _eventSender = eventSender;
    }

    public async Task PublishMicroserviceRegisterCreatedAsync(MicroserviceRegisterCreatedEvent microserviceEvent)
    {
        await _eventSender.SendAsync("MicroserviceRegisterCreated", microserviceEvent);
    }

    public async Task PublishMicroserviceRegisterUpdatedAsync(MicroserviceRegisterUpdatedEvent microserviceEvent)
    {
        await _eventSender.SendAsync("MicroserviceRegisterUpdated", microserviceEvent);
    }

    public async Task PublishMicroserviceRegisterDeletedAsync(MicroserviceRegisterDeletedEvent microserviceEvent)
    {
        await _eventSender.SendAsync("MicroserviceRegisterDeleted", microserviceEvent);
    }
}
