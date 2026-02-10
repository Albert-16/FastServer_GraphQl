using FastServer.Application.Events.LogServicesContentEvents;
using HotChocolate;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Subscriptions;

/// <summary>
/// Suscripciones GraphQL para LogServicesContent
/// </summary>
[ExtendObjectType("Subscription")]
public class LogServicesContentSubscription
{
    /// <summary>
    /// Suscripción a eventos de creación de LogServicesContent
    /// </summary>
    [Subscribe]
    [Topic("LogServicesContentCreated")]
    [GraphQLDescription("Se activa cuando se crea un nuevo LogServicesContent")]
    public LogServicesContentCreatedEvent OnLogServicesContentCreated(
        [EventMessage] LogServicesContentCreatedEvent logEvent)
    {
        return logEvent;
    }

    /// <summary>
    /// Suscripción a eventos de actualización de LogServicesContent
    /// </summary>
    [Subscribe]
    [Topic("LogServicesContentUpdated")]
    [GraphQLDescription("Se activa cuando se actualiza un LogServicesContent")]
    public LogServicesContentUpdatedEvent OnLogServicesContentUpdated(
        [EventMessage] LogServicesContentUpdatedEvent logEvent)
    {
        return logEvent;
    }

    /// <summary>
    /// Suscripción a eventos de eliminación de LogServicesContent
    /// </summary>
    [Subscribe]
    [Topic("LogServicesContentDeleted")]
    [GraphQLDescription("Se activa cuando se elimina un LogServicesContent")]
    public LogServicesContentDeletedEvent OnLogServicesContentDeleted(
        [EventMessage] LogServicesContentDeletedEvent logEvent)
    {
        return logEvent;
    }
}
