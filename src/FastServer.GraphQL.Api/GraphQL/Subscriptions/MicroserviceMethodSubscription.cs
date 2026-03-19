using FastServer.Application.Events.MicroserviceMethodEvents;
using HotChocolate;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Subscriptions;

/// <summary>
/// Suscripciones GraphQL para MicroserviceMethod
/// </summary>
[ExtendObjectType("Subscription")]
public class MicroserviceMethodSubscription
{
    [Subscribe]
    [Topic("MicroserviceMethodCreated")]
    [GraphQLDescription("Se activa cuando se crea un nuevo MicroserviceMethod")]
    public MicroserviceMethodCreatedEvent OnMicroserviceMethodCreated(
        [EventMessage] MicroserviceMethodCreatedEvent evt)
    {
        return evt;
    }

    [Subscribe]
    [Topic("MicroserviceMethodUpdated")]
    [GraphQLDescription("Se activa cuando se actualiza un MicroserviceMethod")]
    public MicroserviceMethodUpdatedEvent OnMicroserviceMethodUpdated(
        [EventMessage] MicroserviceMethodUpdatedEvent evt)
    {
        return evt;
    }

    [Subscribe]
    [Topic("MicroserviceMethodDeleted")]
    [GraphQLDescription("Se activa cuando se elimina un MicroserviceMethod")]
    public MicroserviceMethodDeletedEvent OnMicroserviceMethodDeleted(
        [EventMessage] MicroserviceMethodDeletedEvent evt)
    {
        return evt;
    }
}
