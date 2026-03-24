using FastServer.Application.Events.NodoEvents;
using HotChocolate.Subscriptions;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Implementacion del publisher de eventos de nodos usando HotChocolate
/// </summary>
public class NodoEventPublisher : INodoEventPublisher
{
    private readonly ITopicEventSender _eventSender;

    public NodoEventPublisher(ITopicEventSender eventSender)
    {
        _eventSender = eventSender;
    }

    public async Task PublishNodoCreatedAsync(NodoCreatedEvent evt, CancellationToken ct = default)
    {
        await _eventSender.SendAsync("NodoCreated", evt, ct);
    }

    public async Task PublishNodoUpdatedAsync(NodoUpdatedEvent evt, CancellationToken ct = default)
    {
        await _eventSender.SendAsync("NodoUpdated", evt, ct);
    }

    public async Task PublishNodoDeletedAsync(NodoDeletedEvent evt, CancellationToken ct = default)
    {
        await _eventSender.SendAsync("NodoDeleted", evt, ct);
    }
}
