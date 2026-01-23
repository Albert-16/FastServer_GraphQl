namespace FastServer.Domain.Entities;

/// <summary>
/// Contenido de logs de servicios - FastServer_LogServices_Content
/// </summary>
public class LogServicesContent : BaseEntity
{
    /// <summary>
    /// Texto del contenido del log
    /// </summary>
    public string? LogServicesContentText { get; set; }

    /// <summary>
    /// Número de secuencia del contenido
    /// </summary>
    public string? ContentNo { get; set; }

    // Relación de navegación
    public virtual LogServicesHeader? LogServicesHeader { get; set; }
}
