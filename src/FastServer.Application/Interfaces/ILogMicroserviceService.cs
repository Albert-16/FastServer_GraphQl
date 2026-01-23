using FastServer.Application.DTOs;
using FastServer.Domain.Enums;

namespace FastServer.Application.Interfaces;

/// <summary>
/// Servicio para operaciones de LogMicroservice
/// </summary>
public interface ILogMicroserviceService
{
    Task<LogMicroserviceDto?> GetByIdAsync(long id, DataSourceType? dataSource = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogMicroserviceDto>> GetByLogIdAsync(long logId, DataSourceType? dataSource = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogMicroserviceDto>> SearchByTextAsync(string searchText, DataSourceType? dataSource = null, CancellationToken cancellationToken = default);
    Task<LogMicroserviceDto> CreateAsync(CreateLogMicroserviceDto dto, DataSourceType? dataSource = null, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, DataSourceType? dataSource = null, CancellationToken cancellationToken = default);
}
