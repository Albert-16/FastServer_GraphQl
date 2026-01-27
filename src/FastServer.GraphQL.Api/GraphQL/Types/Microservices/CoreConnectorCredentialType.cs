using FastServer.Domain.Entities.Microservices;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Types.Microservices;

/// <summary>
/// Tipo GraphQL para CoreConnectorCredential
/// </summary>
public class CoreConnectorCredentialType : ObjectType<CoreConnectorCredential>
{
    protected override void Configure(IObjectTypeDescriptor<CoreConnectorCredential> descriptor)
    {
        descriptor.Name("CoreConnectorCredential");
        descriptor.Description("Credenciales para conectores del core");

        descriptor.Field(f => f.CoreConnectorCredentialId)
            .Type<NonNullType<LongType>>()
            .Description("ID único de la credencial");

        descriptor.Field(f => f.CoreConnectorCredentialUser)
            .Type<StringType>()
            .Description("Usuario de la credencial");

        // NOTA: Por seguridad, passwords y keys NO se exponen en GraphQL
        descriptor.Ignore(f => f.CoreConnectorCredentialPass);
        descriptor.Ignore(f => f.CoreConnectorCredentialKey);

        descriptor.Field(f => f.CreateAt)
            .Type<DateTimeType>()
            .Description("Fecha de creación");

        descriptor.Field(f => f.ModifyAt)
            .Type<DateTimeType>()
            .Description("Fecha de última modificación");

        descriptor.Field(f => f.MicroserviceCoreConnectors)
            .Type<ListType<MicroserviceCoreConnectorType>>()
            .Description("Conectores que usan esta credencial");
    }
}
