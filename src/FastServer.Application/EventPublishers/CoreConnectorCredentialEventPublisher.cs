using FastServer.Application.Events.CoreConnectorCredentialEvents;
using HotChocolate.Subscriptions;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Implementación del publisher de eventos de credenciales de conectores del core usando HotChocolate
/// </summary>
public class CoreConnectorCredentialEventPublisher : ICoreConnectorCredentialEventPublisher
{
    private readonly ITopicEventSender _eventSender;

    public CoreConnectorCredentialEventPublisher(ITopicEventSender eventSender)
    {
        _eventSender = eventSender;
    }

    public async Task PublishCoreConnectorCredentialCreatedAsync(CoreConnectorCredentialCreatedEvent credentialEvent)
    {
        await _eventSender.SendAsync("CoreConnectorCredentialCreated", credentialEvent);
    }

    public async Task PublishCoreConnectorCredentialUpdatedAsync(CoreConnectorCredentialUpdatedEvent credentialEvent)
    {
        await _eventSender.SendAsync("CoreConnectorCredentialUpdated", credentialEvent);
    }

    public async Task PublishCoreConnectorCredentialDeletedAsync(CoreConnectorCredentialDeletedEvent credentialEvent)
    {
        await _eventSender.SendAsync("CoreConnectorCredentialDeleted", credentialEvent);
    }
}
