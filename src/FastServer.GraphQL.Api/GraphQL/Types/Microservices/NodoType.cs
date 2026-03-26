using FastServer.Domain.Entities.Microservices;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Types.Microservices;

/// <summary>
/// Tipo GraphQL para Nodo (tabla intermedia Method-Cluster)
/// </summary>
public class NodoType : ObjectType<Nodo>
{
    protected override void Configure(IObjectTypeDescriptor<Nodo> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Name("Nodo");
        descriptor.Description("Nodo de relación entre método y cluster");

        descriptor.Field(f => f.NodoId)
            .Type<NonNullType<UuidType>>()
            .Description("ID único del nodo");

        descriptor.Field(f => f.MicroserviceMethodId)
            .Type<NonNullType<UuidType>>()
            .Description("ID del método de microservicio");

        descriptor.Field(f => f.MicroservicesClusterId)
            .Type<NonNullType<UuidType>>()
            .Description("ID del cluster de microservicios");

        descriptor.Field(f => f.CreateAt)
            .Type<DateTimeType>()
            .Description("Fecha de creación");

        descriptor.Field(f => f.ModifyAt)
            .Type<DateTimeType>()
            .Description("Fecha de última modificación");

        descriptor.Field(f => f.MicroservicesCluster)
            .Type<MicroservicesClusterType>()
            .Description("Cluster de microservicios asociado");
    }
}
