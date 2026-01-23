using FastServer.Domain.Entities;
using FastServer.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de LogServicesContent
/// </summary>
public class LogServicesContentRepository : Repository<LogServicesContent>, ILogServicesContentRepository
{
    public LogServicesContentRepository(DbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LogServicesContent>> GetByLogIdAsync(
        long logId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.LogId == logId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LogServicesContent>> SearchByContentTextAsync(
        string searchText,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.LogServicesContentText != null && x.LogServicesContentText.Contains(searchText))
            .ToListAsync(cancellationToken);
    }
}
