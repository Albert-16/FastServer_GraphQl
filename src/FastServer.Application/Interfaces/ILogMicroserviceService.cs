using FastServer.Application.DTOs;

namespace FastServer.Application.Interfaces;

/// <summary>
/// Servicio para operaciones de LogMicroservice usando PostgreSQL (BD: FastServer_Logs).
/// </summary>
public interface ILogMicroserviceService
{
    Task<LogMicroserviceDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogMicroserviceDto>> GetByLogIdAsync(long logId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogMicroserviceDto>> SearchByTextAsync(string searchText, CancellationToken cancellationToken = default);
    Task<LogMicroserviceDto> CreateAsync(CreateLogMicroserviceDto dto, CancellationToken cancellationToken = default);
    Task<BulkInsertResultDto<LogMicroserviceDto>> CreateBulkAsync(IEnumerable<CreateLogMicroserviceDto> dtos, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
