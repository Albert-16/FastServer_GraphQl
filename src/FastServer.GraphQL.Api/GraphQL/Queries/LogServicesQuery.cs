using FastServer.Application.DTOs;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities;
using HotChocolate.Data;

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
    /// Obtiene todos los logs con paginación, filtrado y ordenamiento
    /// </summary>
    [GraphQLDescription("Obtiene todos los logs de servicios con paginación, filtrado y ordenamiento desde FastServer_Logs (PostgreSQL)")]
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<LogServicesHeader> GetAllLogs(
        [Service] ILogsDbContext context)
    {
        return context.LogServicesHeaders;
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
    /// Obtiene todos los contenidos de log con paginación, filtrado y ordenamiento
    /// </summary>
    [GraphQLDescription("Obtiene todos los contenidos de log con paginación, filtrado y ordenamiento desde FastServer_Logs (PostgreSQL)")]
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<LogServicesContent> GetAllLogContents(
        [Service] ILogsDbContext context)
    {
        return context.LogServicesContents;
    }

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
