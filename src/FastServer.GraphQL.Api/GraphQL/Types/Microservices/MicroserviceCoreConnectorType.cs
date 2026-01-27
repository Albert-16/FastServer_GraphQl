using FastServer.Domain.Entities.Microservices;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Types.Microservices;

/// <summary>
/// Tipo GraphQL para MicroserviceCoreConnector
/// </summary>
public class MicroserviceCoreConnectorType : ObjectType<MicroserviceCoreConnector>
{
    protected override void Configure(IObjectTypeDescriptor<MicroserviceCoreConnector> descriptor)
    {
        descriptor.Name("MicroserviceCoreConnector");
        descriptor.Description("Conector entre microservicio y core");

        descriptor.Field(f => f.MicroserviceCoreConnectorId)
            .Type<NonNullType<LongType>>()
            .Description("ID único del conector");

        descriptor.Field(f => f.CoreConnectorCredentialId)
            .Type<LongType>()
            .Description("ID de la credencial asociada");

        descriptor.Field(f => f.MicroserviceId)
            .Type<LongType>()
            .Description("ID del microservicio asociado");

        descriptor.Field(f => f.CoreConnectorCredential)
            .Type<CoreConnectorCredentialType>()
            .Description("Credencial asociada");

        descriptor.Field(f => f.MicroserviceRegister)
            .Type<MicroserviceRegisterType>()
            .Description("Microservicio asociado");

        descriptor.Field(f => f.CreateAt)
            .Type<DateTimeType>()
            .Description("Fecha de creación");

        descriptor.Field(f => f.ModifyAt)
            .Type<DateTimeType>()
            .Description("Fecha de última modificación");
    }
}
