using FastServer.Application.Events.LogMicroserviceEvents;
using HotChocolate.Subscriptions;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Implementación del publisher de eventos de logs de microservicios usando HotChocolate
/// </summary>
public class LogMicroserviceEventPublisher : ILogMicroserviceEventPublisher
{
    private readonly ITopicEventSender _eventSender;

    public LogMicroserviceEventPublisher(ITopicEventSender eventSender)
    {
        _eventSender = eventSender;
    }

    public async Task PublishLogMicroserviceCreatedAsync(LogMicroserviceCreatedEvent logEvent)
    {
        await _eventSender.SendAsync("LogMicroserviceCreated", logEvent);
    }

    public async Task PublishLogMicroserviceUpdatedAsync(LogMicroserviceUpdatedEvent logEvent)
    {
        await _eventSender.SendAsync("LogMicroserviceUpdated", logEvent);
    }

    public async Task PublishLogMicroserviceDeletedAsync(LogMicroserviceDeletedEvent logEvent)
    {
        await _eventSender.SendAsync("LogMicroserviceDeleted", logEvent);
    }
}
