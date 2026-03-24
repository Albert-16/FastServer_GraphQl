using FastServer.Application.Events.MicroservicesRegisterTypeEvents;
using HotChocolate;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Subscriptions;

/// <summary>
/// Suscripciones GraphQL para MicroservicesRegisterType
/// </summary>
[ExtendObjectType("Subscription")]
public class MicroservicesRegisterTypeSubscription
{
    [Subscribe]
    [Topic("MicroservicesRegisterTypeCreated")]
    [GraphQLDescription("Se activa cuando se crea un nuevo MicroservicesRegisterType")]
    public MicroservicesRegisterTypeCreatedEvent OnMicroservicesRegisterTypeCreated(
        [EventMessage] MicroservicesRegisterTypeCreatedEvent evt)
    {
        return evt;
    }

    [Subscribe]
    [Topic("MicroservicesRegisterTypeUpdated")]
    [GraphQLDescription("Se activa cuando se actualiza un MicroservicesRegisterType")]
    public MicroservicesRegisterTypeUpdatedEvent OnMicroservicesRegisterTypeUpdated(
        [EventMessage] MicroservicesRegisterTypeUpdatedEvent evt)
    {
        return evt;
    }

    [Subscribe]
    [Topic("MicroservicesRegisterTypeDeleted")]
    [GraphQLDescription("Se activa cuando se elimina un MicroservicesRegisterType")]
    public MicroservicesRegisterTypeDeletedEvent OnMicroservicesRegisterTypeDeleted(
        [EventMessage] MicroservicesRegisterTypeDeletedEvent evt)
    {
        return evt;
    }
}
