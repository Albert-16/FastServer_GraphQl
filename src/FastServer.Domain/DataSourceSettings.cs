using FastServer.Domain.Enums;

namespace FastServer.Domain;

/// <summary>
/// Configuración del origen de datos predeterminado.
/// Esta clase se registra como Singleton en DI y permite que todos los servicios
/// accedan al mismo origen de datos configurado en appsettings.json.
/// </summary>
/// <remarks>
/// El origen de datos puede ser PostgreSQL o SqlServer.
/// Esta configuración se inyecta en los servicios de aplicación para determinar
/// qué base de datos usar cuando no se especifica explícitamente.
/// </remarks>
public class DataSourceSettings
{
    /// <summary>
    /// Obtiene el tipo de origen de datos predeterminado configurado.
    /// </summary>
    public DataSourceType DefaultDataSource { get; }

    /// <summary>
    /// Inicializa una nueva instancia de DataSourceSettings.
    /// </summary>
    /// <param name="defaultDataSource">El origen de datos predeterminado (PostgreSQL o SqlServer)</param>
    public DataSourceSettings(DataSourceType defaultDataSource)
    {
        DefaultDataSource = defaultDataSource;
    }
}
