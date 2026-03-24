using FastServer.Application.DTOs.Microservices;

namespace FastServer.Application.Interfaces.Microservices;

/// <summary>
/// Interfaz para el servicio de métodos de microservicios
/// </summary>
public interface IMicroserviceMethodService
{
    Task<MicroserviceMethodDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<MicroserviceMethodDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<MicroserviceMethodDto>> GetByMicroserviceIdAsync(Guid microserviceId, CancellationToken cancellationToken = default);
    Task<MicroserviceMethodDto> CreateAsync(Guid microserviceId, string? name, string? url, string? httpMethod, CancellationToken cancellationToken = default);
    Task<MicroserviceMethodDto?> UpdateAsync(Guid id, Guid? microserviceId, string? name, string? url, string? httpMethod, CancellationToken cancellationToken = default);
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
