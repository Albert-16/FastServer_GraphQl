namespace FastServer.Application.Events.LogServicesContentEvents;

/// <summary>
/// Evento publicado cuando se elimina un log de contenido de servicio
/// </summary>
public class LogServicesContentDeletedEvent
{
    public long LogId { get; set; }
    public string? LogServicesLogLevel { get; set; }
    public string? LogServicesState { get; set; }
    public DateTime DeletedAt { get; set; }
}
