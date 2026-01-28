namespace FastServer.Domain.Entities;

/// <summary>
/// Histórico de contenido de logs de servicios - FastServer_LogServices_Content_Historico
/// </summary>
public class LogServicesContentHistorico : BaseEntity
{
    /// <summary>
    /// Fecha del log de servicio
    /// </summary>
    public string? LogServicesDate { get; set; }

    /// <summary>
    /// Nivel del log (INFO, WARN, ERROR, etc.)
    /// </summary>
    public string? LogServicesLogLevel { get; set; }

    /// <summary>
    /// Estado del log de servicio
    /// </summary>
    public string? LogServicesState { get; set; }

    /// <summary>
    /// Texto del contenido del log
    /// </summary>
    public string? LogServicesContentText { get; set; }
}
