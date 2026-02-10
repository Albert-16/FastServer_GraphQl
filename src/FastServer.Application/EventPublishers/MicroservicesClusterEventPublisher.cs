using FastServer.Application.Events.MicroservicesClusterEvents;
using HotChocolate.Subscriptions;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Implementación del publisher de eventos de clusters de microservicios usando HotChocolate
/// </summary>
public class MicroservicesClusterEventPublisher : IMicroservicesClusterEventPublisher
{
    private readonly ITopicEventSender _eventSender;

    public MicroservicesClusterEventPublisher(ITopicEventSender eventSender)
    {
        _eventSender = eventSender;
    }

    public async Task PublishMicroservicesClusterCreatedAsync(MicroservicesClusterCreatedEvent clusterEvent)
    {
        await _eventSender.SendAsync("MicroservicesClusterCreated", clusterEvent);
    }

    public async Task PublishMicroservicesClusterUpdatedAsync(MicroservicesClusterUpdatedEvent clusterEvent)
    {
        await _eventSender.SendAsync("MicroservicesClusterUpdated", clusterEvent);
    }

    public async Task PublishMicroservicesClusterDeletedAsync(MicroservicesClusterDeletedEvent clusterEvent)
    {
        await _eventSender.SendAsync("MicroservicesClusterDeleted", clusterEvent);
    }
}
