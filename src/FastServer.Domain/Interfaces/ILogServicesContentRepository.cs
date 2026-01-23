using FastServer.Domain.Entities;

namespace FastServer.Domain.Interfaces;

/// <summary>
/// Repositorio específico para LogServicesContent
/// </summary>
public interface ILogServicesContentRepository : IRepository<LogServicesContent>
{
    Task<IEnumerable<LogServicesContent>> GetByLogIdAsync(
        long logId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<LogServicesContent>> SearchByContentTextAsync(
        string searchText,
        CancellationToken cancellationToken = default);
}
