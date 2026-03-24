using FastServer.Application.DTOs.Microservices;

namespace FastServer.Application.Interfaces.Microservices;

/// <summary>
/// Interfaz para el servicio de tipos de eventos
/// </summary>
public interface IEventTypeService
{
    Task<EventTypeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<EventTypeDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<EventTypeDto> CreateAsync(string description, CancellationToken cancellationToken = default);
    Task<EventTypeDto?> UpdateAsync(Guid id, string description, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
