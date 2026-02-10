using FastServer.Application.Events.LogEvents;
using HotChocolate.Subscriptions;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Implementación del publisher de eventos de logs usando HotChocolate
/// </summary>
public class LogEventPublisher : ILogEventPublisher
{
    private readonly ITopicEventSender _eventSender;

    public LogEventPublisher(ITopicEventSender eventSender)
    {
        _eventSender = eventSender;
    }

    public async Task PublishLogCreatedAsync(LogCreatedEvent logEvent)
    {
        await _eventSender.SendAsync("LogCreated", logEvent);
    }

    public async Task PublishLogUpdatedAsync(LogUpdatedEvent logEvent)
    {
        await _eventSender.SendAsync("LogUpdated", logEvent);
    }

    public async Task PublishLogDeletedAsync(LogDeletedEvent logEvent)
    {
        await _eventSender.SendAsync("LogDeleted", logEvent);
    }
}
