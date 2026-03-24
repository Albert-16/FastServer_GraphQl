using FastServer.Application.DTOs;
using FastServer.Application.DTOs.Microservices;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Types.Microservices;

/// <summary>
/// Tipo GraphQL para resultados paginados de MicroserviceRegister
/// </summary>
public class PaginatedMicroserviceRegisterType : ObjectType<PaginatedResultDto<MicroserviceRegisterDto>>
{
    protected override void Configure(IObjectTypeDescriptor<PaginatedResultDto<MicroserviceRegisterDto>> descriptor)
    {
        descriptor.Name("PaginatedMicroserviceRegister");
        descriptor.Description("Resultado paginado de registros de microservicios");

        descriptor.Field(x => x.Items)
            .Name("items")
            .Description("Lista de microservicios");

        descriptor.Field(x => x.TotalCount)
            .Name("totalCount")
            .Description("Total de registros");

        descriptor.Field(x => x.PageNumber)
            .Name("pageNumber")
            .Description("Número de página actual");

        descriptor.Field(x => x.PageSize)
            .Name("pageSize")
            .Description("Tamaño de página");

        descriptor.Field(x => x.TotalPages)
            .Name("totalPages")
            .Description("Total de páginas");

        descriptor.Field(x => x.HasPreviousPage)
            .Name("hasPreviousPage")
            .Description("Indica si hay página anterior");

        descriptor.Field(x => x.HasNextPage)
            .Name("hasNextPage")
            .Description("Indica si hay página siguiente");
    }
}
