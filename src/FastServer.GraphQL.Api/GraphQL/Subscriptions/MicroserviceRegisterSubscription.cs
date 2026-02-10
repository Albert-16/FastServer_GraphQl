using FastServer.Application.Events.MicroserviceRegisterEvents;
using HotChocolate;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Subscriptions;

/// <summary>
/// Suscripciones GraphQL para MicroserviceRegister
/// </summary>
[ExtendObjectType("Subscription")]
public class MicroserviceRegisterSubscription
{
    /// <summary>
    /// Suscripción a eventos de creación de MicroserviceRegister
    /// </summary>
    [Subscribe]
    [Topic("MicroserviceRegisterCreated")]
    [GraphQLDescription("Se activa cuando se crea un nuevo MicroserviceRegister")]
    public MicroserviceRegisterCreatedEvent OnMicroserviceRegisterCreated(
        [EventMessage] MicroserviceRegisterCreatedEvent evt)
    {
        return evt;
    }

    /// <summary>
    /// Suscripción a eventos de actualización de MicroserviceRegister
    /// </summary>
    [Subscribe]
    [Topic("MicroserviceRegisterUpdated")]
    [GraphQLDescription("Se activa cuando se actualiza un MicroserviceRegister")]
    public MicroserviceRegisterUpdatedEvent OnMicroserviceRegisterUpdated(
        [EventMessage] MicroserviceRegisterUpdatedEvent evt)
    {
        return evt;
    }

    /// <summary>
    /// Suscripción a eventos de eliminación de MicroserviceRegister
    /// </summary>
    [Subscribe]
    [Topic("MicroserviceRegisterDeleted")]
    [GraphQLDescription("Se activa cuando se elimina un MicroserviceRegister")]
    public MicroserviceRegisterDeletedEvent OnMicroserviceRegisterDeleted(
        [EventMessage] MicroserviceRegisterDeletedEvent evt)
    {
        return evt;
    }
}
