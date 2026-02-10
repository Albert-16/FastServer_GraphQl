using FastServer.Application.Events.CoreConnectorCredentialEvents;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Publisher de eventos de credenciales de conectores del core para suscripciones GraphQL
/// </summary>
public interface ICoreConnectorCredentialEventPublisher
{
    /// <summary>
    /// Publica un evento de creación de credencial de conector del core
    /// </summary>
    Task PublishCoreConnectorCredentialCreatedAsync(CoreConnectorCredentialCreatedEvent credentialEvent);

    /// <summary>
    /// Publica un evento de actualización de credencial de conector del core
    /// </summary>
    Task PublishCoreConnectorCredentialUpdatedAsync(CoreConnectorCredentialUpdatedEvent credentialEvent);

    /// <summary>
    /// Publica un evento de eliminación de credencial de conector del core
    /// </summary>
    Task PublishCoreConnectorCredentialDeletedAsync(CoreConnectorCredentialDeletedEvent credentialEvent);
}
