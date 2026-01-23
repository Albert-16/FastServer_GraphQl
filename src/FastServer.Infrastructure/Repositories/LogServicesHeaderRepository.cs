using FastServer.Domain.Entities;
using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de LogServicesHeader
/// </summary>
public class LogServicesHeaderRepository : Repository<LogServicesHeader>, ILogServicesHeaderRepository
{
    public LogServicesHeaderRepository(DbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LogServicesHeader>> GetByDateRangeAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.LogDateIn >= startDate && x.LogDateIn <= endDate)
            .OrderByDescending(x => x.LogDateIn)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LogServicesHeader>> GetByStateAsync(
        LogState state,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.LogState == state)
            .OrderByDescending(x => x.LogDateIn)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LogServicesHeader>> GetByMicroserviceNameAsync(
        string microserviceName,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.MicroserviceName != null && x.MicroserviceName.Contains(microserviceName))
            .OrderByDescending(x => x.LogDateIn)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LogServicesHeader>> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.LogDateIn)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LogServicesHeader>> GetByTransactionIdAsync(
        string transactionId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.TransactionId == transactionId)
            .OrderByDescending(x => x.LogDateIn)
            .ToListAsync(cancellationToken);
    }

    public async Task<LogServicesHeader?> GetWithDetailsAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.LogMicroservices)
            .Include(x => x.LogServicesContents)
            .FirstOrDefaultAsync(x => x.LogId == id, cancellationToken);
    }

    public async Task<IEnumerable<LogServicesHeader>> GetFailedLogsAsync(
        DateTime? fromDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.LogState == LogState.Failed || x.ErrorCode != null);

        if (fromDate.HasValue)
            query = query.Where(x => x.LogDateIn >= fromDate.Value);

        return await query
            .OrderByDescending(x => x.LogDateIn)
            .ToListAsync(cancellationToken);
    }
}
