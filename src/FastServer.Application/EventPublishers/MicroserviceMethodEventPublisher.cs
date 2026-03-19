using FastServer.Application.Events.MicroserviceMethodEvents;
using HotChocolate.Subscriptions;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Implementación del publisher de eventos de métodos de microservicios usando HotChocolate
/// </summary>
public class MicroserviceMethodEventPublisher : IMicroserviceMethodEventPublisher
{
    private readonly ITopicEventSender _eventSender;

    public MicroserviceMethodEventPublisher(ITopicEventSender eventSender)
    {
        _eventSender = eventSender;
    }

    public async Task PublishMicroserviceMethodCreatedAsync(MicroserviceMethodCreatedEvent methodEvent)
    {
        await _eventSender.SendAsync("MicroserviceMethodCreated", methodEvent);
    }

    public async Task PublishMicroserviceMethodUpdatedAsync(MicroserviceMethodUpdatedEvent methodEvent)
    {
        await _eventSender.SendAsync("MicroserviceMethodUpdated", methodEvent);
    }

    public async Task PublishMicroserviceMethodDeletedAsync(MicroserviceMethodDeletedEvent methodEvent)
    {
        await _eventSender.SendAsync("MicroserviceMethodDeleted", methodEvent);
    }
}
