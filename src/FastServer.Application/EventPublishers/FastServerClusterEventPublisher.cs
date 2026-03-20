using FastServer.Application.Events.FastServerClusterEvents;
using HotChocolate.Subscriptions;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Implementación del publisher de eventos de clusters de FastServer usando HotChocolate
/// </summary>
public class FastServerClusterEventPublisher : IFastServerClusterEventPublisher
{
    private readonly ITopicEventSender _eventSender;

    public FastServerClusterEventPublisher(ITopicEventSender eventSender)
    {
        _eventSender = eventSender;
    }

    public async Task PublishFastServerClusterCreatedAsync(FastServerClusterCreatedEvent clusterEvent)
    {
        await _eventSender.SendAsync("FastServerClusterCreated", clusterEvent);
    }

    public async Task PublishFastServerClusterUpdatedAsync(FastServerClusterUpdatedEvent clusterEvent)
    {
        await _eventSender.SendAsync("FastServerClusterUpdated", clusterEvent);
    }

    public async Task PublishFastServerClusterDeletedAsync(FastServerClusterDeletedEvent clusterEvent)
    {
        await _eventSender.SendAsync("FastServerClusterDeleted", clusterEvent);
    }
}
