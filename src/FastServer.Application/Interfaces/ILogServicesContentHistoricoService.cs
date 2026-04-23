using FastServer.Application.DTOs;

namespace FastServer.Application.Interfaces;

/// <summary>
/// Servicio de solo lectura para el histórico de LogServicesContent
/// (tabla: FastServer_LogServices_Content_Historico) en PostgreSQL (BD: FastServer_Logs).
/// </summary>
public interface ILogServicesContentHistoricoService
{
    Task<IEnumerable<LogServicesContentDto>> GetByLogIdAsync(long logId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogServicesContentDto>> SearchByContentAsync(string searchText, CancellationToken cancellationToken = default);
}
