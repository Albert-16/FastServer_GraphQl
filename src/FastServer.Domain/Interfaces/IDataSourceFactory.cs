using FastServer.Domain.Enums;

namespace FastServer.Domain.Interfaces;

/// <summary>
/// Fábrica para crear unidades de trabajo según el origen de datos
/// </summary>
public interface IDataSourceFactory
{
    /// <summary>
    /// Crea una unidad de trabajo para el origen de datos especificado
    /// </summary>
    IUnitOfWork CreateUnitOfWork(DataSourceType dataSource);

    /// <summary>
    /// Obtiene los orígenes de datos disponibles
    /// </summary>
    IEnumerable<DataSourceType> GetAvailableDataSources();
}
