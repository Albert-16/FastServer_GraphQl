using FastServer.Application.DTOs;
using FastServer.Application.DTOs.Microservices;

namespace FastServer.Application.Interfaces.Microservices;

/// <summary>
/// Interfaz para el servicio de registros de microservicios
/// </summary>
public interface IMicroserviceRegisterService
{
    Task<MicroserviceRegisterDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<MicroserviceRegisterDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<MicroserviceRegisterDto>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<PaginatedResultDto<MicroserviceRegisterDto>> GetAllPaginatedAsync(PaginationParamsDto pagination, CancellationToken ct = default);
    Task<MicroserviceRegisterDto> CreateAsync(string? name, bool active, bool coreConnection, string? soapBase, Guid? microserviceTypeId, CancellationToken cancellationToken = default);
    Task<MicroserviceRegisterDto?> UpdateAsync(Guid id, string? name, bool? active, bool? coreConnection, string? soapBase, Guid? microserviceTypeId, CancellationToken cancellationToken = default);
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> SetActiveAsync(Guid id, bool active, CancellationToken cancellationToken = default);
}
