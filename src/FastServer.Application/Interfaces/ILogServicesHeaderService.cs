using FastServer.Application.DTOs;

namespace FastServer.Application.Interfaces;

/// <summary>
/// Servicio para operaciones de LogServicesHeader usando PostgreSQL (BD: FastServer_Logs).
/// </summary>
public interface ILogServicesHeaderService
{
    Task<LogServicesHeaderDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<LogServicesHeaderDto?> GetWithDetailsAsync(long id, CancellationToken cancellationToken = default);
    Task<PaginatedResultDto<LogServicesHeaderDto>> GetAllAsync(PaginationParamsDto pagination, CancellationToken cancellationToken = default);
    Task<PaginatedResultDto<LogServicesHeaderDto>> GetByFilterAsync(LogFilterDto filter, PaginationParamsDto pagination, CancellationToken cancellationToken = default);
    Task<LogServicesHeaderDto> CreateAsync(CreateLogServicesHeaderDto dto, CancellationToken cancellationToken = default);
    Task<LogServicesHeaderDto> UpdateAsync(UpdateLogServicesHeaderDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogServicesHeaderDto>> GetFailedLogsAsync(DateTime? fromDate = null, CancellationToken cancellationToken = default);
}
