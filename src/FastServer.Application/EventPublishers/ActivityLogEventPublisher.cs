using FastServer.Application.Events.ActivityLogEvents;
using HotChocolate.Subscriptions;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Implementación del publisher de eventos de logs de actividad usando HotChocolate
/// </summary>
public class ActivityLogEventPublisher : IActivityLogEventPublisher
{
    private readonly ITopicEventSender _eventSender;

    public ActivityLogEventPublisher(ITopicEventSender eventSender)
    {
        _eventSender = eventSender;
    }

    public async Task PublishActivityLogCreatedAsync(ActivityLogCreatedEvent activityLogEvent)
    {
        await _eventSender.SendAsync("ActivityLogCreated", activityLogEvent);
    }

    public async Task PublishActivityLogUpdatedAsync(ActivityLogUpdatedEvent activityLogEvent)
    {
        await _eventSender.SendAsync("ActivityLogUpdated", activityLogEvent);
    }

    public async Task PublishActivityLogDeletedAsync(ActivityLogDeletedEvent activityLogEvent)
    {
        await _eventSender.SendAsync("ActivityLogDeleted", activityLogEvent);
    }
}
