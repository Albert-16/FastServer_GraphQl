using FastServer.Application.DTOs;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities;
using HotChocolate.Data;

namespace FastServer.GraphQL.Api.GraphQL.Queries;

/// <summary>
/// Queries GraphQL para LogServicesHeaderHistorico (archivo histórico inmutable).
/// </summary>
[ExtendObjectType("Query")]
public class LogServicesHeaderHistoricoQuery
{
    /// <summary>
    /// Obtiene un log histórico por su ID.
    /// </summary>
    [GraphQLDescription("Obtiene un log histórico de servicios por su ID desde FastServer_LogServices_Header_Historico (PostgreSQL)")]
    public async Task<LogServicesHeaderDto?> GetHistoricoLogById(
        [Service] ILogServicesHeaderHistoricoService service,
        [GraphQLDescription("ID del log histórico")] long logId,
        CancellationToken cancellationToken = default)
    {
        return await service.GetByIdAsync(logId, cancellationToken);
    }

    /// <summary>
    /// Obtiene un log histórico con todos sus detalles.
    /// </summary>
    [GraphQLDescription("Obtiene un log histórico de servicios con todos sus detalles desde FastServer_LogServices_Header_Historico (PostgreSQL)")]
    public async Task<LogServicesHeaderDto?> GetHistoricoLogWithDetails(
        [Service] ILogServicesHeaderHistoricoService service,
        [GraphQLDescription("ID del log histórico")] long logId,
        CancellationToken cancellationToken = default)
    {
        return await service.GetWithDetailsAsync(logId, cancellationToken);
    }

    /// <summary>
    /// Obtiene todos los logs históricos con paginación, filtrado y ordenamiento.
    /// </summary>
    [GraphQLDescription("Obtiene todos los logs históricos de servicios con paginación, filtrado y ordenamiento desde FastServer_LogServices_Header_Historico (PostgreSQL)")]
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<LogServicesHeaderHistorico> GetAllHistoricoLogs(
        [Service] ILogsDbContext context)
    {
        return context.LogServicesHeadersHistorico;
    }

    /// <summary>
    /// Obtiene logs históricos con errores.
    /// </summary>
    [GraphQLDescription("Obtiene todos los logs históricos que tienen errores desde FastServer_LogServices_Header_Historico (PostgreSQL)")]
    public async Task<IEnumerable<LogServicesHeaderDto>> GetFailedHistoricoLogs(
        [Service] ILogServicesHeaderHistoricoService service,
        [GraphQLDescription("Fecha desde la cual buscar")] DateTime? fromDate = null,
        CancellationToken cancellationToken = default)
    {
        return await service.GetFailedLogsAsync(fromDate, cancellationToken);
    }
}

/// <summary>
/// Queries GraphQL para LogServicesContentHistorico (archivo histórico inmutable).
/// </summary>
[ExtendObjectType("Query")]
public class LogServicesContentHistoricoQuery
{
    /// <summary>
    /// Obtiene todos los contenidos históricos de log con paginación, filtrado y ordenamiento.
    /// </summary>
    [GraphQLDescription("Obtiene todos los contenidos históricos de log con paginación, filtrado y ordenamiento desde FastServer_LogServices_Content_Historico (PostgreSQL)")]
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<LogServicesContentHistorico> GetAllHistoricoLogContents(
        [Service] ILogsDbContext context)
    {
        return context.LogServicesContentsHistorico;
    }

    /// <summary>
    /// Obtiene contenidos históricos de log por ID de log.
    /// </summary>
    [GraphQLDescription("Obtiene los contenidos históricos de log asociados a un log específico desde FastServer_LogServices_Content_Historico (PostgreSQL)")]
    public async Task<IEnumerable<LogServicesContentDto>> GetHistoricoLogContentsByLogId(
        [Service] ILogServicesContentHistoricoService service,
        [GraphQLDescription("ID del log")] long logId,
        CancellationToken cancellationToken = default)
    {
        return await service.GetByLogIdAsync(logId, cancellationToken);
    }

    /// <summary>
    /// Busca contenidos históricos de log por texto.
    /// </summary>
    [GraphQLDescription("Busca contenidos históricos de log que contengan el texto especificado desde FastServer_LogServices_Content_Historico (PostgreSQL)")]
    public async Task<IEnumerable<LogServicesContentDto>> SearchHistoricoLogContents(
        [Service] ILogServicesContentHistoricoService service,
        [GraphQLDescription("Texto a buscar")] string searchText,
        CancellationToken cancellationToken = default)
    {
        return await service.SearchByContentAsync(searchText, cancellationToken);
    }
}
