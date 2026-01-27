using FastServer.Domain.Entities.Microservices;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Types.Microservices;

/// <summary>
/// Tipo GraphQL para ActivityLog
/// </summary>
public class ActivityLogType : ObjectType<ActivityLog>
{
    protected override void Configure(IObjectTypeDescriptor<ActivityLog> descriptor)
    {
        descriptor.Name("ActivityLog");
        descriptor.Description("Log de actividad del sistema");

        descriptor.Field(f => f.ActivityLogId)
            .Type<NonNullType<UuidType>>()
            .Description("ID único del log de actividad");

        descriptor.Field(f => f.EventTypeId)
            .Type<LongType>()
            .Description("ID del tipo de evento");

        descriptor.Field(f => f.ActivityLogEntityName)
            .Type<StringType>()
            .Description("Nombre de la entidad afectada");

        descriptor.Field(f => f.ActivityLogEntityId)
            .Type<UuidType>()
            .Description("ID de la entidad afectada");

        descriptor.Field(f => f.ActivityLogDescription)
            .Type<StringType>()
            .Description("Descripción de la actividad");

        descriptor.Field(f => f.UserId)
            .Type<UuidType>()
            .Description("ID del usuario que realizó la actividad");

        descriptor.Field(f => f.EventType)
            .Type<EventTypeType>()
            .Description("Tipo de evento asociado");

        descriptor.Field(f => f.User)
            .Type<UserType>()
            .Description("Usuario que realizó la actividad");

        descriptor.Field(f => f.CreateAt)
            .Type<DateTimeType>()
            .Description("Fecha de creación");

        descriptor.Field(f => f.ModifyAt)
            .Type<DateTimeType>()
            .Description("Fecha de última modificación");
    }
}
