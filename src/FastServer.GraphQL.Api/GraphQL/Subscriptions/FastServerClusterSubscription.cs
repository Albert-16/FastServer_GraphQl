using FastServer.Application.Events.FastServerClusterEvents;
using HotChocolate;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Subscriptions;

/// <summary>
/// Suscripciones GraphQL para FastServerCluster
/// </summary>
[ExtendObjectType("Subscription")]
public class FastServerClusterSubscription
{
    [Subscribe]
    [Topic("FastServerClusterCreated")]
    [GraphQLDescription("Se activa cuando se crea un nuevo FastServerCluster")]
    public FastServerClusterCreatedEvent OnFastServerClusterCreated(
        [EventMessage] FastServerClusterCreatedEvent evt)
    {
        return evt;
    }

    [Subscribe]
    [Topic("FastServerClusterUpdated")]
    [GraphQLDescription("Se activa cuando se actualiza un FastServerCluster")]
    public FastServerClusterUpdatedEvent OnFastServerClusterUpdated(
        [EventMessage] FastServerClusterUpdatedEvent evt)
    {
        return evt;
    }

    [Subscribe]
    [Topic("FastServerClusterDeleted")]
    [GraphQLDescription("Se activa cuando se elimina un FastServerCluster")]
    public FastServerClusterDeletedEvent OnFastServerClusterDeleted(
        [EventMessage] FastServerClusterDeletedEvent evt)
    {
        return evt;
    }
}
