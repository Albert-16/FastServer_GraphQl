using FastServer.Application.DTOs;
using FastServer.Application.Interfaces;
using FastServer.Domain.Enums;

namespace FastServer.GraphQL.Api.GraphQL.Types;

/// <summary>
/// Tipo GraphQL para LogServicesHeader
/// </summary>
public class LogServicesHeaderType : ObjectType<LogServicesHeaderDto>
{
    protected override void Configure(IObjectTypeDescriptor<LogServicesHeaderDto> descriptor)
    {
        descriptor.Name("LogServicesHeader");
        descriptor.Description("Cabecera de logs de servicios FastServer");

        descriptor.Field(x => x.LogId)
            .Name("logId")
            .Description("Identificador único del log");

        descriptor.Field(x => x.LogDateIn)
            .Name("logDateIn")
            .Description("Fecha y hora de entrada del log");

        descriptor.Field(x => x.LogDateOut)
            .Name("logDateOut")
            .Description("Fecha y hora de salida del log");

        descriptor.Field(x => x.LogState)
            .Name("logState")
            .Description("Estado del log");

        descriptor.Field(x => x.LogMethodUrl)
            .Name("logMethodUrl")
            .Description("URL del método invocado");

        descriptor.Field(x => x.LogMethodName)
            .Name("logMethodName")
            .Description("Nombre del método invocado");

        descriptor.Field(x => x.LogFsId)
            .Name("logFsId")
            .Description("ID de FastServer");

        descriptor.Field(x => x.MethodDescription)
            .Name("methodDescription")
            .Description("Descripción del método");

        descriptor.Field(x => x.TciIpPort)
            .Name("tciIpPort")
            .Description("IP y puerto TCI");

        descriptor.Field(x => x.ErrorCode)
            .Name("errorCode")
            .Description("Código de error");

        descriptor.Field(x => x.ErrorDescription)
            .Name("errorDescription")
            .Description("Descripción del error");

        descriptor.Field(x => x.IpFs)
            .Name("ipFs")
            .Description("IP del FastServer");

        descriptor.Field(x => x.TypeProcess)
            .Name("typeProcess")
            .Description("Tipo de proceso");

        descriptor.Field(x => x.LogNodo)
            .Name("logNodo")
            .Description("Nodo del log");

        descriptor.Field(x => x.HttpMethod)
            .Name("httpMethod")
            .Description("Método HTTP");

        descriptor.Field(x => x.MicroserviceName)
            .Name("microserviceName")
            .Description("Nombre del microservicio");

        descriptor.Field(x => x.RequestDuration)
            .Name("requestDuration")
            .Description("Duración de la solicitud en milisegundos");

        descriptor.Field(x => x.TransactionId)
            .Name("transactionId")
            .Description("ID de la transacción");

        descriptor.Field(x => x.UserId)
            .Name("userId")
            .Description("ID del usuario");

        descriptor.Field(x => x.SessionId)
            .Name("sessionId")
            .Description("ID de la sesión");

        descriptor.Field(x => x.RequestId)
            .Name("requestId")
            .Description("ID de la solicitud");
    }
}

/// <summary>
/// Tipo GraphQL para LogMicroservice
/// </summary>
public class LogMicroserviceType : ObjectType<LogMicroserviceDto>
{
    protected override void Configure(IObjectTypeDescriptor<LogMicroserviceDto> descriptor)
    {
        descriptor.Name("LogMicroservice");
        descriptor.Description("Log de microservicio");

        descriptor.Field(x => x.LogId)
            .Name("logId")
            .Description("Identificador del log");

        descriptor.Field(x => x.LogMicroserviceText)
            .Name("logMicroserviceText")
            .Description("Texto del log del microservicio");
    }
}

/// <summary>
/// Tipo GraphQL para LogServicesContent
/// </summary>
public class LogServicesContentType : ObjectType<LogServicesContentDto>
{
    protected override void Configure(IObjectTypeDescriptor<LogServicesContentDto> descriptor)
    {
        descriptor.Name("LogServicesContent");
        descriptor.Description("Contenido de log de servicios");

        descriptor.Field(x => x.LogId)
            .Name("logId")
            .Description("Identificador del log");

        descriptor.Field(x => x.LogServicesDate)
            .Name("logServicesDate")
            .Description("Fecha del log de servicio");

        descriptor.Field(x => x.LogServicesLogLevel)
            .Name("logServicesLogLevel")
            .Description("Nivel del log");

        descriptor.Field(x => x.LogServicesState)
            .Name("logServicesState")
            .Description("Estado del log de servicio");

        descriptor.Field(x => x.LogServicesContentText)
            .Name("logServicesContentText")
            .Description("Texto del contenido del log");
    }
}

/// <summary>
/// Tipo GraphQL para resultados paginados
/// </summary>
public class PaginatedLogServicesHeaderType : ObjectType<PaginatedResultDto<LogServicesHeaderDto>>
{
    protected override void Configure(IObjectTypeDescriptor<PaginatedResultDto<LogServicesHeaderDto>> descriptor)
    {
        descriptor.Name("PaginatedLogServicesHeader");
        descriptor.Description("Resultado paginado de logs de servicios");

        descriptor.Field(x => x.Items)
            .Name("items")
            .Description("Lista de logs");

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
