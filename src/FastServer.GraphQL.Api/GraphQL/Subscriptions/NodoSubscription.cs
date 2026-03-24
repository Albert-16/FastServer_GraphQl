using FastServer.Application.Events.NodoEvents;
using HotChocolate;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Subscriptions;

/// <summary>
/// Suscripciones GraphQL para Nodo
/// </summary>
[ExtendObjectType("Subscription")]
public class NodoSubscription
{
    [Subscribe]
    [Topic("NodoCreated")]
    [GraphQLDescription("Se activa cuando se crea un nuevo Nodo")]
    public NodoCreatedEvent OnNodoCreated(
        [EventMessage] NodoCreatedEvent evt)
    {
        return evt;
    }

    [Subscribe]
    [Topic("NodoUpdated")]
    [GraphQLDescription("Se activa cuando se actualiza un Nodo")]
    public NodoUpdatedEvent OnNodoUpdated(
        [EventMessage] NodoUpdatedEvent evt)
    {
        return evt;
    }

    [Subscribe]
    [Topic("NodoDeleted")]
    [GraphQLDescription("Se activa cuando se elimina un Nodo")]
    public NodoDeletedEvent OnNodoDeleted(
        [EventMessage] NodoDeletedEvent evt)
    {
        return evt;
    }
}
