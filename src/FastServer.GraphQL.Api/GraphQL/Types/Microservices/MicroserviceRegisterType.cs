using FastServer.Domain.Entities.Microservices;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Types.Microservices;

/// <summary>
/// Tipo GraphQL para MicroserviceRegister
/// </summary>
public class MicroserviceRegisterType : ObjectType<MicroserviceRegister>
{
    protected override void Configure(IObjectTypeDescriptor<MicroserviceRegister> descriptor)
    {
        descriptor.Name("MicroserviceRegister");
        descriptor.Description("Registro de microservicio");

        descriptor.Field(f => f.MicroserviceId)
            .Type<NonNullType<LongType>>()
            .Description("ID único del microservicio");

        descriptor.Field(f => f.MicroserviceClusterId)
            .Type<LongType>()
            .Description("ID del cluster al que pertenece");

        descriptor.Field(f => f.MicroserviceName)
            .Type<StringType>()
            .Description("Nombre del microservicio");

        descriptor.Field(f => f.MicroserviceActive)
            .Type<BooleanType>()
            .Description("Indica si el microservicio está activo");

        descriptor.Field(f => f.MicroserviceDeleted)
            .Type<BooleanType>()
            .Description("Indica si el microservicio está eliminado (soft delete)");

        descriptor.Field(f => f.MicroserviceCoreConnection)
            .Type<BooleanType>()
            .Description("Indica si tiene conexión con el core");

        descriptor.Field(f => f.CreateAt)
            .Type<DateTimeType>()
            .Description("Fecha de creación");

        descriptor.Field(f => f.ModifyAt)
            .Type<DateTimeType>()
            .Description("Fecha de última modificación");

        descriptor.Field(f => f.DeleteAt)
            .Type<DateTimeType>()
            .Description("Fecha de eliminación");

        descriptor.Field(f => f.MicroserviceCluster)
            .Type<MicroservicesClusterType>()
            .Description("Cluster al que pertenece");

        descriptor.Field(f => f.MicroserviceCoreConnectors)
            .Type<ListType<MicroserviceCoreConnectorType>>()
            .Description("Conectores del core asociados");
    }
}
