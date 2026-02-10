using FastServer.Application.Events.MicroservicesClusterEvents;
using HotChocolate;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Subscriptions;

/// <summary>
/// Suscripciones GraphQL para MicroservicesCluster
/// </summary>
[ExtendObjectType("Subscription")]
public class MicroservicesClusterSubscription
{
    [Subscribe]
    [Topic("MicroservicesClusterCreated")]
    [GraphQLDescription("Se activa cuando se crea un nuevo MicroservicesCluster")]
    public MicroservicesClusterCreatedEvent OnMicroservicesClusterCreated(
        [EventMessage] MicroservicesClusterCreatedEvent evt)
    {
        return evt;
    }

    [Subscribe]
    [Topic("MicroservicesClusterUpdated")]
    [GraphQLDescription("Se activa cuando se actualiza un MicroservicesCluster")]
    public MicroservicesClusterUpdatedEvent OnMicroservicesClusterUpdated(
        [EventMessage] MicroservicesClusterUpdatedEvent evt)
    {
        return evt;
    }

    [Subscribe]
    [Topic("MicroservicesClusterDeleted")]
    [GraphQLDescription("Se activa cuando se elimina un MicroservicesCluster")]
    public MicroservicesClusterDeletedEvent OnMicroservicesClusterDeleted(
        [EventMessage] MicroservicesClusterDeletedEvent evt)
    {
        return evt;
    }
}
