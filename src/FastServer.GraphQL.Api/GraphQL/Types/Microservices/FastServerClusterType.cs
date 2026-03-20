using FastServer.Domain.Entities.Microservices;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Types.Microservices;

/// <summary>
/// Tipo GraphQL para FastServerCluster
/// </summary>
public class FastServerClusterType : ObjectType<FastServerCluster>
{
    protected override void Configure(IObjectTypeDescriptor<FastServerCluster> descriptor)
    {
        descriptor.Name("FastServerCluster");
        descriptor.Description("Cluster de FastServer — representa un nodo/instancia del servidor");

        descriptor.Field(f => f.FastServerClusterId)
            .Type<NonNullType<UuidType>>()
            .Description("ID único del cluster (GUID v7)");

        descriptor.Field(f => f.FastServerClusterName)
            .Type<StringType>()
            .Description("Nombre identificador del cluster");

        descriptor.Field(f => f.FastServerClusterUrl)
            .Type<StringType>()
            .Description("URL base del cluster");

        descriptor.Field(f => f.FastServerClusterVersion)
            .Type<StringType>()
            .Description("Versión del software desplegada en el cluster");

        descriptor.Field(f => f.FastServerClusterServerName)
            .Type<StringType>()
            .Description("Nombre del servidor donde reside el cluster");

        descriptor.Field(f => f.FastServerClusterServerIp)
            .Type<StringType>()
            .Description("Dirección IP del servidor");

        descriptor.Field(f => f.FastServerClusterActive)
            .Type<BooleanType>()
            .Description("Indica si el cluster está activo");

        descriptor.Field(f => f.FastServerClusterDelete)
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
    }
}
