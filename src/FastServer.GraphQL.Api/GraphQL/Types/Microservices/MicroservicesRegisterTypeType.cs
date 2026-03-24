using FastServer.Domain.Entities.Microservices;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Types.Microservices;

/// <summary>
/// Tipo GraphQL para MicroservicesRegisterType
/// </summary>
public class MicroservicesRegisterTypeType : ObjectType<MicroservicesRegisterType>
{
    protected override void Configure(IObjectTypeDescriptor<MicroservicesRegisterType> descriptor)
    {
        descriptor.Name("MicroservicesRegisterType");
        descriptor.Description("Tipo de registro de microservicio");

        descriptor.Field(f => f.MicroservicesRegisterTypeId)
            .Type<NonNullType<UuidType>>()
            .Description("ID único del tipo de registro");

        descriptor.Field(f => f.MicroservicesRegisterTypeName)
            .Type<StringType>()
            .Description("Nombre del tipo de registro");

        descriptor.Field(f => f.MicroservicesRegisterTypeDescription)
            .Type<StringType>()
            .Description("Descripción del tipo de registro");

        descriptor.Field(f => f.CreateAt)
            .Type<DateTimeType>()
            .Description("Fecha de creación");

        descriptor.Field(f => f.ModifyAt)
            .Type<DateTimeType>()
            .Description("Fecha de última modificación");

        descriptor.Field(f => f.MicroserviceRegisters)
            .Type<ListType<MicroserviceRegisterType>>()
            .Description("Microservicios de este tipo");
    }
}
