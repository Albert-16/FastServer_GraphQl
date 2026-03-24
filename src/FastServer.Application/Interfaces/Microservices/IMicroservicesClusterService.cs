using FastServer.Application.DTOs.Microservices;

namespace FastServer.Application.Interfaces.Microservices;

/// <summary>
/// Interfaz para el servicio de clusters de microservicios
/// </summary>
public interface IMicroservicesClusterService
{
    Task<MicroservicesClusterDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<MicroservicesClusterDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<MicroservicesClusterDto>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<MicroservicesClusterDto> CreateAsync(string? name, string? serverName, string? serverIp, string? protocol, bool active, CancellationToken cancellationToken = default);
    Task<MicroservicesClusterDto?> UpdateAsync(Guid id, string? name, string? serverName, string? serverIp, string? protocol, bool? active, CancellationToken cancellationToken = default);
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> SetActiveAsync(Guid id, bool active, CancellationToken cancellationToken = default);
}
