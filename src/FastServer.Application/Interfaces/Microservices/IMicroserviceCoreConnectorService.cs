using FastServer.Application.DTOs.Microservices;

namespace FastServer.Application.Interfaces.Microservices;

/// <summary>
/// Interfaz para el servicio de conectores entre microservicios y el core
/// </summary>
public interface IMicroserviceCoreConnectorService
{
    Task<MicroserviceCoreConnectorDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<MicroserviceCoreConnectorDto>> GetByMicroserviceIdAsync(Guid microserviceId, CancellationToken cancellationToken = default);
    Task<MicroserviceCoreConnectorDto> CreateAsync(Guid? credentialId, Guid? microserviceId, CancellationToken cancellationToken = default);
    Task<MicroserviceCoreConnectorDto?> UpdateAsync(Guid id, Guid? credentialId, Guid? microserviceId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
