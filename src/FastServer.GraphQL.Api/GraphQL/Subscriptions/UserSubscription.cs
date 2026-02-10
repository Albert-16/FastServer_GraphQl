using FastServer.Application.Events.UserEvents;
using HotChocolate;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Subscriptions;

/// <summary>
/// Suscripciones GraphQL para User
/// </summary>
[ExtendObjectType("Subscription")]
public class UserSubscription
{
    [Subscribe]
    [Topic("UserCreated")]
    [GraphQLDescription("Se activa cuando se crea un nuevo User")]
    public UserCreatedEvent OnUserCreated(
        [EventMessage] UserCreatedEvent evt)
    {
        return evt;
    }

    [Subscribe]
    [Topic("UserUpdated")]
    [GraphQLDescription("Se activa cuando se actualiza un User")]
    public UserUpdatedEvent OnUserUpdated(
        [EventMessage] UserUpdatedEvent evt)
    {
        return evt;
    }

    [Subscribe]
    [Topic("UserDeleted")]
    [GraphQLDescription("Se activa cuando se elimina un User")]
    public UserDeletedEvent OnUserDeleted(
        [EventMessage] UserDeletedEvent evt)
    {
        return evt;
    }
}
