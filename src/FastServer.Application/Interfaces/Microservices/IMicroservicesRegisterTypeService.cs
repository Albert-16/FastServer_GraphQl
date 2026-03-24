using FastServer.Application.DTOs.Microservices;

namespace FastServer.Application.Interfaces.Microservices;

/// <summary>
/// Interfaz para el servicio de tipos de registro de microservicios
/// </summary>
public interface IMicroservicesRegisterTypeService
{
    Task<MicroservicesRegisterTypeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MicroservicesRegisterTypeDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<MicroservicesRegisterTypeDto> CreateAsync(string? name, string? description, CancellationToken cancellationToken = default);
    Task<MicroservicesRegisterTypeDto?> UpdateAsync(Guid id, string? name = null, string? description = null, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
