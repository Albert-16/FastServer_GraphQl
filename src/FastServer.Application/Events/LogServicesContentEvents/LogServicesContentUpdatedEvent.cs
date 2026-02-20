namespace FastServer.Application.Events.LogServicesContentEvents;

/// <summary>
/// Evento publicado cuando se actualiza un log de contenido de servicio
/// </summary>
public class LogServicesContentUpdatedEvent
{
    public Guid LogServicesContentId { get; set; }
    public long LogId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public DateTime? LogServicesDate { get; set; }
    public string? LogServicesLogLevel { get; set; }
    public string? LogServicesState { get; set; }
    public string? LogServicesContentText { get; set; }
    public DateTime UpdatedAt { get; set; }
}
