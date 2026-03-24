using FastServer.Application.Events.MicroservicesRegisterTypeEvents;
using HotChocolate.Subscriptions;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Implementacion del publisher de eventos de tipos de registro de microservicios usando HotChocolate
/// </summary>
public class MicroservicesRegisterTypeEventPublisher : IMicroservicesRegisterTypeEventPublisher
{
    private readonly ITopicEventSender _eventSender;

    public MicroservicesRegisterTypeEventPublisher(ITopicEventSender eventSender)
    {
        _eventSender = eventSender;
    }

    public async Task PublishMicroservicesRegisterTypeCreatedAsync(MicroservicesRegisterTypeCreatedEvent evt, CancellationToken ct = default)
    {
        await _eventSender.SendAsync("MicroservicesRegisterTypeCreated", evt, ct);
    }

    public async Task PublishMicroservicesRegisterTypeUpdatedAsync(MicroservicesRegisterTypeUpdatedEvent evt, CancellationToken ct = default)
    {
        await _eventSender.SendAsync("MicroservicesRegisterTypeUpdated", evt, ct);
    }

    public async Task PublishMicroservicesRegisterTypeDeletedAsync(MicroservicesRegisterTypeDeletedEvent evt, CancellationToken ct = default)
    {
        await _eventSender.SendAsync("MicroservicesRegisterTypeDeleted", evt, ct);
    }
}
