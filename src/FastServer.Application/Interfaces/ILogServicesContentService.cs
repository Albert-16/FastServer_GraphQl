using FastServer.Application.DTOs;
using FastServer.Domain.Enums;

namespace FastServer.Application.Interfaces;

/// <summary>
/// Servicio para operaciones de LogServicesContent
/// </summary>
public interface ILogServicesContentService
{
    Task<LogServicesContentDto?> GetByIdAsync(long id, DataSourceType? dataSource = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogServicesContentDto>> GetByLogIdAsync(long logId, DataSourceType? dataSource = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogServicesContentDto>> SearchByContentAsync(string searchText, DataSourceType? dataSource = null, CancellationToken cancellationToken = default);
    Task<LogServicesContentDto> CreateAsync(CreateLogServicesContentDto dto, DataSourceType? dataSource = null, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, DataSourceType? dataSource = null, CancellationToken cancellationToken = default);
}
