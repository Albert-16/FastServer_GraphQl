using FastServer.Application.Events.LogMicroserviceEvents;
using HotChocolate;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Subscriptions;

/// <summary>
/// Suscripciones GraphQL para LogMicroservice
/// </summary>
[ExtendObjectType("Subscription")]
public class LogMicroserviceSubscription
{
    /// <summary>
    /// Suscripción a eventos de creación de LogMicroservice
    /// </summary>
    [Subscribe]
    [Topic("LogMicroserviceCreated")]
    [GraphQLDescription("Se activa cuando se crea un nuevo LogMicroservice")]
    public LogMicroserviceCreatedEvent OnLogMicroserviceCreated(
        [EventMessage] LogMicroserviceCreatedEvent logEvent)
    {
        return logEvent;
    }

    /// <summary>
    /// Suscripción a eventos de actualización de LogMicroservice
    /// </summary>
    [Subscribe]
    [Topic("LogMicroserviceUpdated")]
    [GraphQLDescription("Se activa cuando se actualiza un LogMicroservice")]
    public LogMicroserviceUpdatedEvent OnLogMicroserviceUpdated(
        [EventMessage] LogMicroserviceUpdatedEvent logEvent)
    {
        return logEvent;
    }

    /// <summary>
    /// Suscripción a eventos de eliminación de LogMicroservice
    /// </summary>
    [Subscribe]
    [Topic("LogMicroserviceDeleted")]
    [GraphQLDescription("Se activa cuando se elimina un LogMicroservice")]
    public LogMicroserviceDeletedEvent OnLogMicroserviceDeleted(
        [EventMessage] LogMicroserviceDeletedEvent logEvent)
    {
        return logEvent;
    }
}
