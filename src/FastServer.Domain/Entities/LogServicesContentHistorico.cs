namespace FastServer.Domain.Entities;

/// <summary>
/// Histórico de contenido de logs de servicios - FastServer_LogServices_Content_Historico
/// </summary>
public class LogServicesContentHistorico
{
    /// <summary>
    /// Identificador único del registro (PK, GUID v7)
    /// </summary>
    public Guid LogServicesContentId { get; set; }

    /// <summary>
    /// Identificador del log (ya no es PK, permite duplicados)
    /// </summary>
    public long LogId { get; set; }

    /// <summary>
    /// Nombre del evento
    /// </summary>
    public string EventName { get; set; } = string.Empty;

    /// <summary>
    /// Fecha del log de servicio
    /// </summary>
    public DateTime? LogServicesDate { get; set; }

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
