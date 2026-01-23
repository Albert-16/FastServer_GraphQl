using FastServer.Application.DTOs;
using FastServer.Domain.Enums;

namespace FastServer.Application.Interfaces;

/// <summary>
/// Servicio para operaciones de LogServicesHeader
/// </summary>
public interface ILogServicesHeaderService
{
    Task<LogServicesHeaderDto?> GetByIdAsync(long id, DataSourceType? dataSource = null, CancellationToken cancellationToken = default);
    Task<LogServicesHeaderDto?> GetWithDetailsAsync(long id, DataSourceType? dataSource = null, CancellationToken cancellationToken = default);
    Task<PaginatedResultDto<LogServicesHeaderDto>> GetAllAsync(PaginationParamsDto pagination, DataSourceType? dataSource = null, CancellationToken cancellationToken = default);
    Task<PaginatedResultDto<LogServicesHeaderDto>> GetByFilterAsync(LogFilterDto filter, PaginationParamsDto pagination, CancellationToken cancellationToken = default);
    Task<LogServicesHeaderDto> CreateAsync(CreateLogServicesHeaderDto dto, DataSourceType? dataSource = null, CancellationToken cancellationToken = default);
    Task<LogServicesHeaderDto> UpdateAsync(UpdateLogServicesHeaderDto dto, DataSourceType? dataSource = null, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, DataSourceType? dataSource = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogServicesHeaderDto>> GetFailedLogsAsync(DateTime? fromDate = null, DataSourceType? dataSource = null, CancellationToken cancellationToken = default);
}
