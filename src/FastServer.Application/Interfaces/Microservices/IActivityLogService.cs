using FastServer.Application.DTOs.Microservices;

namespace FastServer.Application.Interfaces.Microservices;

/// <summary>
/// Interfaz para el servicio de logs de actividad
/// </summary>
public interface IActivityLogService
{
    Task<ActivityLogDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<ActivityLogDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<List<ActivityLogDto>> GetByEntityAsync(string entityName, Guid? entityId, CancellationToken cancellationToken = default);
    Task<ActivityLogDto> CreateAsync(Guid? eventTypeId, string? entityName, Guid? entityId, string? description, Guid? userId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
