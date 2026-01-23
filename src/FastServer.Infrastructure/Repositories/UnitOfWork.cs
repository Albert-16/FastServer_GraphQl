using FastServer.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FastServer.Infrastructure.Repositories;

/// <summary>
/// Implementación de la unidad de trabajo
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private IDbContextTransaction? _transaction;

    private ILogServicesHeaderRepository? _logServicesHeaders;
    private ILogMicroserviceRepository? _logMicroservices;
    private ILogServicesContentRepository? _logServicesContents;

    public UnitOfWork(DbContext context)
    {
        _context = context;
    }

    public ILogServicesHeaderRepository LogServicesHeaders =>
        _logServicesHeaders ??= new LogServicesHeaderRepository(_context);

    public ILogMicroserviceRepository LogMicroservices =>
        _logMicroservices ??= new LogMicroserviceRepository(_context);

    public ILogServicesContentRepository LogServicesContents =>
        _logServicesContents ??= new LogServicesContentRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
