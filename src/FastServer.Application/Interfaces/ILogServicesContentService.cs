using FastServer.Application.DTOs;

namespace FastServer.Application.Interfaces;

/// <summary>
/// Servicio para operaciones de LogServicesContent usando PostgreSQL (BD: FastServer_Logs).
/// </summary>
public interface ILogServicesContentService
{
    Task<LogServicesContentDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogServicesContentDto>> GetByLogIdAsync(long logId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogServicesContentDto>> SearchByContentAsync(string searchText, CancellationToken cancellationToken = default);
    Task<LogServicesContentDto> CreateAsync(CreateLogServicesContentDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
