namespace FastServer.Domain.Entities;

/// <summary>
/// Histórico de logs de microservicios - FastServer_LogMicroservice_Historico
/// </summary>
public class LogMicroserviceHistorico
{
    /// <summary>
    /// Identificador único del registro (PK, GUID v7)
    /// </summary>
    public Guid LogMicroserviceId { get; set; }

    /// <summary>
    /// Identificador del log (ya no es PK, permite duplicados)
    /// </summary>
    public long LogId { get; set; }

    /// <summary>
    /// Nombre del evento
    /// </summary>
    public string EventName { get; set; } = string.Empty;

    /// <summary>
    /// Fecha del log
    /// </summary>
    public DateTime? LogDate { get; set; }

    /// <summary>
    /// Nivel del log (INFO, WARN, ERROR, etc.)
    /// </summary>
    public string? LogLevel { get; set; }

    /// <summary>
    /// Texto del log del microservicio
    /// </summary>
    public string LogMicroserviceText { get; set; } = string.Empty;
}
