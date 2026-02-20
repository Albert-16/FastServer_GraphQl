namespace FastServer.Application.Events.LogServicesContentEvents;

/// <summary>
/// Evento publicado cuando se crea un nuevo log de contenido de servicio
/// </summary>
public class LogServicesContentCreatedEvent
{
    public Guid LogServicesContentId { get; set; }
    public long LogId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public DateTime? LogServicesDate { get; set; }
    public string? LogServicesLogLevel { get; set; }
    public string? LogServicesState { get; set; }
    public string? LogServicesContentText { get; set; }
    public DateTime CreatedAt { get; set; }
}
