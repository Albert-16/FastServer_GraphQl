using FastServer.Application.DTOs.Microservices;

namespace FastServer.Application.Interfaces.Microservices;

/// <summary>
/// Interfaz para el servicio de credenciales de conectores del core
/// </summary>
public interface ICoreConnectorCredentialService
{
    Task<CoreConnectorCredentialDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<CoreConnectorCredentialDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CoreConnectorCredentialDto> CreateAsync(string? user, string? password, string? key, CancellationToken cancellationToken = default);
    Task<CoreConnectorCredentialDto?> UpdateAsync(Guid id, string? user, string? password, string? key, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
