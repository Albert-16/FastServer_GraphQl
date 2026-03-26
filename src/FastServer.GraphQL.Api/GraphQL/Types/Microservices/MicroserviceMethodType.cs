using FastServer.Domain.Entities.Microservices;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Types.Microservices;

/// <summary>
/// Tipo GraphQL para MicroserviceMethod
/// </summary>
public class MicroserviceMethodType : ObjectType<MicroserviceMethod>
{
    protected override void Configure(IObjectTypeDescriptor<MicroserviceMethod> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Name("MicroserviceMethod");
        descriptor.Description("Método/endpoint de un microservicio");

        descriptor.Field(f => f.MicroserviceMethodId)
            .Type<NonNullType<UuidType>>()
            .Description("ID único del método");

        descriptor.Field(f => f.MicroserviceId)
            .Type<NonNullType<UuidType>>()
            .Description("ID del microservicio al que pertenece");

        descriptor.Field(f => f.MicroserviceMethodDelete)
            .Type<BooleanType>()
            .Description("Indica si el método está eliminado (soft delete)");

        descriptor.Field(f => f.MicroserviceMethodName)
            .Type<StringType>()
            .Description("Nombre del método/endpoint");

        descriptor.Field(f => f.MicroserviceMethodUrl)
            .Type<StringType>()
            .Description("URL del método/endpoint");

        descriptor.Field(f => f.HttpMethod)
            .Type<StringType>()
            .Description("Método HTTP (GET, POST, PUT, DELETE, etc.)");

        descriptor.Field(f => f.CreateAt)
            .Type<DateTimeType>()
            .Description("Fecha de creación");

        descriptor.Field(f => f.ModifyAt)
            .Type<DateTimeType>()
            .Description("Fecha de última modificación");

        descriptor.Field(f => f.Nodos)
            .Type<ListType<NodoType>>()
            .Description("Nodos (relaciones con clusters)");
    }
}
