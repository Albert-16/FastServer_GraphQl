using FastServer.Domain.Entities.Microservices;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Types.Microservices;

/// <summary>
/// Tipo GraphQL para EventType
/// </summary>
public class EventTypeType : ObjectType<EventType>
{
    protected override void Configure(IObjectTypeDescriptor<EventType> descriptor)
    {
        descriptor.Name("EventType");
        descriptor.Description("Tipo de evento para logs de actividad");

        descriptor.Field(f => f.EventTypeId)
            .Type<NonNullType<LongType>>()
            .Description("ID único del tipo de evento");

        descriptor.Field(f => f.EventTypeDescription)
            .Type<StringType>()
            .Description("Descripción del tipo de evento");

        descriptor.Field(f => f.CreateAt)
            .Type<DateTimeType>()
            .Description("Fecha de creación");

        descriptor.Field(f => f.ModifyAt)
            .Type<DateTimeType>()
            .Description("Fecha de última modificación");

        descriptor.Field(f => f.ActivityLogs)
            .Type<ListType<ActivityLogType>>()
            .Description("Logs de actividad de este tipo");
    }
}
