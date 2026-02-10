using FastServer.Application.Events.ActivityLogEvents;
using HotChocolate;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Subscriptions;

/// <summary>
/// Suscripciones GraphQL para ActivityLog
/// </summary>
[ExtendObjectType("Subscription")]
public class ActivityLogSubscription
{
    [Subscribe]
    [Topic("ActivityLogCreated")]
    [GraphQLDescription("Se activa cuando se crea un nuevo ActivityLog")]
    public ActivityLogCreatedEvent OnActivityLogCreated(
        [EventMessage] ActivityLogCreatedEvent evt)
    {
        return evt;
    }

    [Subscribe]
    [Topic("ActivityLogUpdated")]
    [GraphQLDescription("Se activa cuando se actualiza un ActivityLog")]
    public ActivityLogUpdatedEvent OnActivityLogUpdated(
        [EventMessage] ActivityLogUpdatedEvent evt)
    {
        return evt;
    }

    [Subscribe]
    [Topic("ActivityLogDeleted")]
    [GraphQLDescription("Se activa cuando se elimina un ActivityLog")]
    public ActivityLogDeletedEvent OnActivityLogDeleted(
        [EventMessage] ActivityLogDeletedEvent evt)
    {
        return evt;
    }
}
