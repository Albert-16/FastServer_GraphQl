using FastServer.Application.DTOs;
using FastServer.Application.Interfaces;
using FastServer.GraphQL.Api.GraphQL.Types;

namespace FastServer.GraphQL.Api.GraphQL.Queries;

/// <summary>
/// Queries GraphQL para LogServices
/// </summary>
[ExtendObjectType("Query")]
public class LogServicesQuery
{
    /// <summary>
    /// Obtiene un log por su ID
    /// </summary>
    [GraphQLDescription("Obtiene un log de servicios por su ID desde FastServer_Logs (PostgreSQL)")]
    public async Task<LogServicesHeaderDto?> GetLogById(
        [Service] ILogServicesHeaderService service,
        [GraphQLDescription("ID del log")] long logId,
        CancellationToken cancellationToken = default)
    {
        return await service.GetByIdAsync(logId, cancellationToken);
    }

    /// <summary>
    /// Obtiene un log con todos sus detalles
    /// </summary>
    [GraphQLDescription("Obtiene un log de servicios con todos sus detalles (microservicios y contenidos) desde FastServer_Logs (PostgreSQL)")]
    public async Task<LogServicesHeaderDto?> GetLogWithDetails(
        [Service] ILogServicesHeaderService service,
        [GraphQLDescription("ID del log")] long logId,
        CancellationToken cancellationToken = default)
    {
        return await service.GetWithDetailsAsync(logId, cancellationToken);
    }

    /// <summary>
    /// Obtiene todos los logs paginados
    /// </summary>
    [GraphQLDescription("Obtiene todos los logs de servicios con paginación desde FastServer_Logs (PostgreSQL)")]
    public async Task<PaginatedResultDto<LogServicesHeaderDto>> GetAllLogs(
        [Service] ILogServicesHeaderService service,
        [GraphQLDescription("Parámetros de paginación")] PaginationInput? pagination = null,
        CancellationToken cancellationToken = default)
    {
        var paginationParams = new PaginationParamsDto
        {
            PageNumber = pagination?.PageNumber ?? 1,
            PageSize = pagination?.PageSize ?? 10
        };

        return await service.GetAllAsync(paginationParams, cancellationToken);
    }

    /// <summary>
    /// Obtiene logs filtrados
    /// </summary>
    [GraphQLDescription("Obtiene logs de servicios filtrados con paginación desde FastServer_Logs (PostgreSQL)")]
    public async Task<PaginatedResultDto<LogServicesHeaderDto>> GetLogsByFilter(
        [Service] ILogServicesHeaderService service,
        [GraphQLDescription("Filtros a aplicar")] LogFilterInput filter,
        [GraphQLDescription("Parámetros de paginación")] PaginationInput? pagination = null,
        CancellationToken cancellationToken = default)
    {
        var filterDto = new LogFilterDto
        {
            StartDate = filter.StartDate,
            EndDate = filter.EndDate,
            State = filter.State,
            MicroserviceName = filter.MicroserviceName,
            UserId = filter.UserId,
            TransactionId = filter.TransactionId,
            HttpMethod = filter.HttpMethod,
            HasErrors = filter.HasErrors
        };

        var paginationParams = new PaginationParamsDto
        {
            PageNumber = pagination?.PageNumber ?? 1,
            PageSize = pagination?.PageSize ?? 10
        };

        return await service.GetByFilterAsync(filterDto, paginationParams, cancellationToken);
    }

    /// <summary>
    /// Obtiene logs con errores
    /// </summary>
    [GraphQLDescription("Obtiene todos los logs que tienen errores desde FastServer_Logs (PostgreSQL)")]
    public async Task<IEnumerable<LogServicesHeaderDto>> GetFailedLogs(
        [Service] ILogServicesHeaderService service,
        [GraphQLDescription("Fecha desde la cual buscar")] DateTime? fromDate = null,
        CancellationToken cancellationToken = default)
    {
        return await service.GetFailedLogsAsync(fromDate, cancellationToken);
    }
}

/// <summary>
/// Queries GraphQL para LogMicroservice
/// </summary>
[ExtendObjectType("Query")]
public class LogMicroserviceQuery
{
    /// <summary>
    /// Obtiene logs de microservicio por ID de log
    /// </summary>
    [GraphQLDescription("Obtiene los logs de microservicio asociados a un log específico desde FastServer_Logs (PostgreSQL)")]
    public async Task<IEnumerable<LogMicroserviceDto>> GetLogMicroservicesByLogId(
        [Service] ILogMicroserviceService service,
        [GraphQLDescription("ID del log")] long logId,
        CancellationToken cancellationToken = default)
    {
        return await service.GetByLogIdAsync(logId, cancellationToken);
    }

    /// <summary>
    /// Busca logs de microservicio por texto
    /// </summary>
    [GraphQLDescription("Busca logs de microservicio que contengan el texto especificado desde FastServer_Logs (PostgreSQL)")]
    public async Task<IEnumerable<LogMicroserviceDto>> SearchLogMicroservices(
        [Service] ILogMicroserviceService service,
        [GraphQLDescription("Texto a buscar")] string searchText,
        CancellationToken cancellationToken = default)
    {
        return await service.SearchByTextAsync(searchText, cancellationToken);
    }
}

/// <summary>
/// Queries GraphQL para LogServicesContent
/// </summary>
[ExtendObjectType("Query")]
public class LogServicesContentQuery
{
    /// <summary>
    /// Obtiene contenidos de log por ID de log
    /// </summary>
    [GraphQLDescription("Obtiene los contenidos de log asociados a un log específico desde FastServer_Logs (PostgreSQL)")]
    public async Task<IEnumerable<LogServicesContentDto>> GetLogContentsByLogId(
        [Service] ILogServicesContentService service,
        [GraphQLDescription("ID del log")] long logId,
        CancellationToken cancellationToken = default)
    {
        return await service.GetByLogIdAsync(logId, cancellationToken);
    }

    /// <summary>
    /// Busca contenidos de log por texto
    /// </summary>
    [GraphQLDescription("Busca contenidos de log que contengan el texto especificado desde FastServer_Logs (PostgreSQL)")]
    public async Task<IEnumerable<LogServicesContentDto>> SearchLogContents(
        [Service] ILogServicesContentService service,
        [GraphQLDescription("Texto a buscar")] string searchText,
        CancellationToken cancellationToken = default)
    {
        return await service.SearchByContentAsync(searchText, cancellationToken);
    }
}
