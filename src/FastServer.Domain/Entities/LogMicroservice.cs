namespace FastServer.Domain.Entities;

/// <summary>
/// Logs de microservicios - FastServer_LogMicroservice
/// </summary>
public class LogMicroservice
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
    /// Identificador de la solicitud
    /// </summary>
    public long RequestId { get; set; }

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
    public string? LogMicroserviceText { get; set; }
}
