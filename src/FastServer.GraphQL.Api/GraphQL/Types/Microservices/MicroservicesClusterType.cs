using FastServer.Domain.Entities.Microservices;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Types.Microservices;

/// <summary>
/// Tipo GraphQL para MicroservicesCluster
/// </summary>
public class MicroservicesClusterType : ObjectType<MicroservicesCluster>
{
    protected override void Configure(IObjectTypeDescriptor<MicroservicesCluster> descriptor)
    {
        descriptor.Name("MicroservicesCluster");
        descriptor.Description("Cluster de microservicios");

        descriptor.Field(f => f.MicroservicesClusterId)
            .Type<NonNullType<LongType>>()
            .Description("ID único del cluster");

        descriptor.Field(f => f.MicroservicesClusterName)
            .Type<StringType>()
            .Description("Nombre del cluster");

        descriptor.Field(f => f.MicroservicesClusterServerName)
            .Type<StringType>()
            .Description("Nombre del servidor");

        descriptor.Field(f => f.MicroservicesClusterServerIp)
            .Type<StringType>()
            .Description("IP del servidor");

        descriptor.Field(f => f.MicroservicesClusterActive)
            .Type<BooleanType>()
            .Description("Indica si el cluster está activo");

        descriptor.Field(f => f.MicroservicesClusterDeleted)
            .Type<BooleanType>()
            .Description("Indica si el cluster está eliminado (soft delete)");

        descriptor.Field(f => f.CreateAt)
            .Type<DateTimeType>()
            .Description("Fecha de creación");

        descriptor.Field(f => f.ModifyAt)
            .Type<DateTimeType>()
            .Description("Fecha de última modificación");

        descriptor.Field(f => f.DeleteAt)
            .Type<DateTimeType>()
            .Description("Fecha de eliminación");

        descriptor.Field(f => f.MicroserviceRegisters)
            .Type<ListType<MicroserviceRegisterType>>()
            .Description("Microservicios del cluster");
    }
}
