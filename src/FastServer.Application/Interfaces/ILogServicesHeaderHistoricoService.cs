using FastServer.Application.DTOs;

namespace FastServer.Application.Interfaces;

/// <summary>
/// Servicio de solo lectura para el histórico de LogServicesHeader
/// (tabla: FastServer_LogServices_Header_Historico) en PostgreSQL (BD: FastServer_Logs).
/// </summary>
public interface ILogServicesHeaderHistoricoService
{
    Task<LogServicesHeaderDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<LogServicesHeaderDto?> GetWithDetailsAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogServicesHeaderDto>> GetFailedLogsAsync(DateTime? fromDate = null, CancellationToken cancellationToken = default);
}
