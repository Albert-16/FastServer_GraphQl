using FastServer.Application.Events.CoreConnectorCredentialEvents;
using HotChocolate;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Subscriptions;

/// <summary>
/// Suscripciones GraphQL para CoreConnectorCredential
/// </summary>
[ExtendObjectType("Subscription")]
public class CoreConnectorCredentialSubscription
{
    [Subscribe]
    [Topic("CoreConnectorCredentialCreated")]
    [GraphQLDescription("Se activa cuando se crea un nuevo CoreConnectorCredential")]
    public CoreConnectorCredentialCreatedEvent OnCoreConnectorCredentialCreated(
        [EventMessage] CoreConnectorCredentialCreatedEvent evt)
    {
        return evt;
    }

    [Subscribe]
    [Topic("CoreConnectorCredentialUpdated")]
    [GraphQLDescription("Se activa cuando se actualiza un CoreConnectorCredential")]
    public CoreConnectorCredentialUpdatedEvent OnCoreConnectorCredentialUpdated(
        [EventMessage] CoreConnectorCredentialUpdatedEvent evt)
    {
        return evt;
    }

    [Subscribe]
    [Topic("CoreConnectorCredentialDeleted")]
    [GraphQLDescription("Se activa cuando se elimina un CoreConnectorCredential")]
    public CoreConnectorCredentialDeletedEvent OnCoreConnectorCredentialDeleted(
        [EventMessage] CoreConnectorCredentialDeletedEvent evt)
    {
        return evt;
    }
}
