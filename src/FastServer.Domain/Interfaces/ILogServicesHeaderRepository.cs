using FastServer.Domain.Entities;
using FastServer.Domain.Enums;

namespace FastServer.Domain.Interfaces;

/// <summary>
/// Repositorio específico para LogServicesHeader
/// </summary>
public interface ILogServicesHeaderRepository : IRepository<LogServicesHeader>
{
    Task<IEnumerable<LogServicesHeader>> GetByDateRangeAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<LogServicesHeader>> GetByStateAsync(
        LogState state,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<LogServicesHeader>> GetByMicroserviceNameAsync(
        string microserviceName,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<LogServicesHeader>> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<LogServicesHeader>> GetByTransactionIdAsync(
        string transactionId,
        CancellationToken cancellationToken = default);

    Task<LogServicesHeader?> GetWithDetailsAsync(
        long id,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<LogServicesHeader>> GetFailedLogsAsync(
        DateTime? fromDate = null,
        CancellationToken cancellationToken = default);
}
