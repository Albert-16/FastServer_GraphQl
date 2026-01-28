namespace FastServer.Domain.Entities;

/// <summary>
/// Histórico de logs de microservicios - FastServer_LogMicroservice_Historico
/// </summary>
public class LogMicroserviceHistorico : BaseEntity
{
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
