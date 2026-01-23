using FastServer.Domain.Entities;

namespace FastServer.Domain.Interfaces;

/// <summary>
/// Repositorio específico para LogMicroservice
/// </summary>
public interface ILogMicroserviceRepository : IRepository<LogMicroservice>
{
    Task<IEnumerable<LogMicroservice>> GetByLogIdAsync(
        long logId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<LogMicroservice>> SearchByTextAsync(
        string searchText,
        CancellationToken cancellationToken = default);
}
