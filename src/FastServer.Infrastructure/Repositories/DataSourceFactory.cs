using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;
using FastServer.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FastServer.Infrastructure.Repositories;

/// <summary>
/// Fábrica para crear unidades de trabajo según el origen de datos
/// </summary>
public class DataSourceFactory : IDataSourceFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HashSet<DataSourceType> _availableDataSources;

    public DataSourceFactory(
        IServiceProvider serviceProvider,
        IEnumerable<DataSourceType> availableDataSources)
    {
        _serviceProvider = serviceProvider;
        _availableDataSources = new HashSet<DataSourceType>(availableDataSources);
    }

    public IUnitOfWork CreateUnitOfWork(DataSourceType dataSource)
    {
        if (!_availableDataSources.Contains(dataSource))
        {
            throw new InvalidOperationException(
                $"El origen de datos {dataSource} no está configurado o disponible.");
        }

        DbContext context = dataSource switch
        {
            DataSourceType.PostgreSQL => _serviceProvider.GetRequiredService<PostgreSqlDbContext>(),
            DataSourceType.SqlServer => _serviceProvider.GetRequiredService<SqlServerDbContext>(),
            _ => throw new ArgumentException($"Origen de datos no soportado: {dataSource}")
        };

        return new UnitOfWork(context);
    }

    public IEnumerable<DataSourceType> GetAvailableDataSources()
    {
        return _availableDataSources;
    }
}
