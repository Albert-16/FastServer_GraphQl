using FastServer.Domain.Entities.Microservices;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Types.Microservices;

/// <summary>
/// Tipo GraphQL para User
/// </summary>
public class UserType : ObjectType<User>
{
    protected override void Configure(IObjectTypeDescriptor<User> descriptor)
    {
        descriptor.Name("User");
        descriptor.Description("Usuario del sistema");

        descriptor.Field(f => f.UserId)
            .Type<NonNullType<UuidType>>()
            .Description("ID único del usuario");

        descriptor.Field(f => f.UserPeoplesoft)
            .Type<StringType>()
            .Description("ID de PeopleSoft del usuario");

        descriptor.Field(f => f.UserActive)
            .Type<BooleanType>()
            .Description("Indica si el usuario está activo");

        descriptor.Field(f => f.UserName)
            .Type<StringType>()
            .Description("Nombre del usuario");

        descriptor.Field(f => f.UserEmail)
            .Type<StringType>()
            .Description("Email del usuario");

        descriptor.Field(f => f.CreateAt)
            .Type<DateTimeType>()
            .Description("Fecha de creación");

        descriptor.Field(f => f.ModifyAt)
            .Type<DateTimeType>()
            .Description("Fecha de última modificación");

        descriptor.Field(f => f.ActivityLogs)
            .Type<ListType<ActivityLogType>>()
            .Description("Logs de actividad del usuario");
    }
}
