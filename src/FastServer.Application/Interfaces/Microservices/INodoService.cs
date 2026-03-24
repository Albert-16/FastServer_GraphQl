using FastServer.Application.DTOs.Microservices;

namespace FastServer.Application.Interfaces.Microservices;

/// <summary>
/// Interfaz para el servicio de nodos (tabla intermedia Method-Cluster)
/// </summary>
public interface INodoService
{
    Task<NodoDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<NodoDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<NodoDto>> GetByMethodIdAsync(Guid methodId, CancellationToken cancellationToken = default);
    Task<IEnumerable<NodoDto>> GetByClusterIdAsync(Guid clusterId, CancellationToken cancellationToken = default);
    Task<NodoDto> CreateAsync(Guid methodId, Guid clusterId, CancellationToken cancellationToken = default);
    Task<NodoDto?> UpdateAsync(Guid id, Guid? methodId, Guid? clusterId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
