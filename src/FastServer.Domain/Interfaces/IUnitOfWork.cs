namespace FastServer.Domain.Interfaces;

/// <summary>
/// Unidad de trabajo para coordinar transacciones
/// </summary>
public interface IUnitOfWork : IDisposable
{
    ILogServicesHeaderRepository LogServicesHeaders { get; }
    ILogMicroserviceRepository LogMicroservices { get; }
    ILogServicesContentRepository LogServicesContents { get; }

    /// <summary>
    /// Obtiene un repositorio genérico para cualquier entidad
    /// </summary>
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
