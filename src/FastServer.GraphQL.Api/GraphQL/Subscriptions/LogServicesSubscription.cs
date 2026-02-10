using FastServer.Application.Events.LogEvents;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Subscriptions;

/// <summary>
/// Suscripciones para eventos de logs
/// </summary>
[ExtendObjectType("Subscription")]
public class LogServicesSubscription
{
    /// <summary>
    /// Suscripción a eventos de creación de logs
    /// </summary>
    [Subscribe]
    [Topic("LogCreated")]
    [GraphQLDescription("Se emite cuando se crea un nuevo log")]
    public LogCreatedEvent OnLogCreated(
        [EventMessage] LogCreatedEvent logEvent)
    {
        return logEvent;
    }

    /// <summary>
    /// Suscripción a eventos de actualización de logs
    /// </summary>
    [Subscribe]
    [Topic("LogUpdated")]
    [GraphQLDescription("Se emite cuando se actualiza un log")]
    public LogUpdatedEvent OnLogUpdated(
        [EventMessage] LogUpdatedEvent logEvent)
    {
        return logEvent;
    }

    /// <summary>
    /// Suscripción a eventos de eliminación de logs
    /// </summary>
    [Subscribe]
    [Topic("LogDeleted")]
    [GraphQLDescription("Se emite cuando se elimina un log")]
    public LogDeletedEvent OnLogDeleted(
        [EventMessage] LogDeletedEvent logEvent)
    {
        return logEvent;
    }
}
