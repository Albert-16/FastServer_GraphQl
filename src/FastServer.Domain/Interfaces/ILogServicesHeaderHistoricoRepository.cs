using FastServer.Domain.Entities;
using FastServer.Domain.Enums;

namespace FastServer.Domain.Interfaces;

/// <summary>
/// Repositorio específico para LogServicesHeaderHistorico
/// </summary>
public interface ILogServicesHeaderHistoricoRepository : IRepository<LogServicesHeaderHistorico>
{
    Task<IEnumerable<LogServicesHeaderHistorico>> GetByDateRangeAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<LogServicesHeaderHistorico>> GetByStateAsync(
        LogState state,
        CancellationToken cancellationToken = default);

    Task<LogServicesHeaderHistorico?> GetWithDetailsAsync(
        long id,
        CancellationToken cancellationToken = default);
}
