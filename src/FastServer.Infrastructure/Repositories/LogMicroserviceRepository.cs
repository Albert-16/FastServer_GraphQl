using FastServer.Domain.Entities;
using FastServer.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de LogMicroservice
/// </summary>
public class LogMicroserviceRepository : Repository<LogMicroservice>, ILogMicroserviceRepository
{
    public LogMicroserviceRepository(DbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LogMicroservice>> GetByLogIdAsync(
        long logId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.LogId == logId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LogMicroservice>> SearchByTextAsync(
        string searchText,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.LogMicroserviceText != null && x.LogMicroserviceText.Contains(searchText))
            .ToListAsync(cancellationToken);
    }
}
