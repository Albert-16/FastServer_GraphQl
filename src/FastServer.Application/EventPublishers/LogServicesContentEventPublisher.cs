using FastServer.Application.Events.LogServicesContentEvents;
using HotChocolate.Subscriptions;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Implementación del publisher de eventos de logs de contenido de servicio usando HotChocolate
/// </summary>
public class LogServicesContentEventPublisher : ILogServicesContentEventPublisher
{
    private readonly ITopicEventSender _eventSender;

    public LogServicesContentEventPublisher(ITopicEventSender eventSender)
    {
        _eventSender = eventSender;
    }

    public async Task PublishLogServicesContentCreatedAsync(LogServicesContentCreatedEvent logEvent)
    {
        await _eventSender.SendAsync("LogServicesContentCreated", logEvent);
    }

    public async Task PublishLogServicesContentUpdatedAsync(LogServicesContentUpdatedEvent logEvent)
    {
        await _eventSender.SendAsync("LogServicesContentUpdated", logEvent);
    }

    public async Task PublishLogServicesContentDeletedAsync(LogServicesContentDeletedEvent logEvent)
    {
        await _eventSender.SendAsync("LogServicesContentDeleted", logEvent);
    }
}
