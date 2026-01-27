using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;
using FastServer.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FastServer.Infrastructure.Repositories;

/// <summary>
/// Implementación del patrón Factory para crear unidades de trabajo (Unit of Work)
/// específicas para cada origen de datos configurado.
/// </summary>
/// <remarks>
/// Esta fábrica permite trabajar con múltiples bases de datos de forma transparente.
/// Cada UnitOfWork creado tiene acceso a todos los repositorios necesarios para
/// interactuar con la base de datos seleccionada.
///
/// Flujo de uso:
/// 1. El servicio solicita un UnitOfWork para un DataSourceType específico
/// 2. La fábrica valida que ese origen de datos esté disponible
/// 3. Crea el DbContext correspondiente (PostgreSQL o SqlServer)
/// 4. Retorna un UnitOfWork que encapsula ese contexto
/// </remarks>
public class DataSourceFactory : IDataSourceFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HashSet<DataSourceType> _availableDataSources;

    /// <summary>
    /// Inicializa una nueva instancia de DataSourceFactory.
    /// </summary>
    /// <param name="serviceProvider">Proveedor de servicios para resolver dependencias</param>
    /// <param name="availableDataSources">Lista de orígenes de datos configurados y disponibles</param>
    public DataSourceFactory(
        IServiceProvider serviceProvider,
        IEnumerable<DataSourceType> availableDataSources)
    {
        _serviceProvider = serviceProvider;
        _availableDataSources = new HashSet<DataSourceType>(availableDataSources);
    }

    /// <summary>
    /// Crea una unidad de trabajo para el origen de datos especificado.
    /// </summary>
    /// <param name="dataSource">El tipo de origen de datos (PostgreSQL o SqlServer)</param>
    /// <returns>Una unidad de trabajo lista para ejecutar operaciones de base de datos</returns>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si el origen de datos no está configurado o disponible
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Se lanza si el tipo de origen de datos no es soportado
    /// </exception>
    public IUnitOfWork CreateUnitOfWork(DataSourceType dataSource)
    {
        // Validar que el origen de datos esté configurado
        if (!_availableDataSources.Contains(dataSource))
        {
            throw new InvalidOperationException(
                $"El origen de datos {dataSource} no está configurado o disponible.");
        }

        // Resolver el DbContext correspondiente desde DI
        DbContext context = dataSource switch
        {
            DataSourceType.PostgreSQL => _serviceProvider.GetRequiredService<PostgreSqlDbContext>(),
            DataSourceType.SqlServer => _serviceProvider.GetRequiredService<SqlServerDbContext>(),
            _ => throw new ArgumentException($"Origen de datos no soportado: {dataSource}")
        };

        // Crear y retornar el UnitOfWork con el contexto apropiado
        return new UnitOfWork(context);
    }

    /// <summary>
    /// Obtiene la lista de orígenes de datos que están configurados y disponibles.
    /// </summary>
    /// <returns>Colección de tipos de orígenes de datos disponibles</returns>
    public IEnumerable<DataSourceType> GetAvailableDataSources()
    {
        return _availableDataSources;
    }
}
