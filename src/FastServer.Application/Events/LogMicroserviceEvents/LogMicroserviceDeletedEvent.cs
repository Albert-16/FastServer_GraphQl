namespace FastServer.Application.Events.LogMicroserviceEvents;

/// <summary>
/// Evento publicado cuando se elimina un log de microservicio
/// </summary>
public class LogMicroserviceDeletedEvent
{
    public long LogId { get; set; }
    public string? LogLevel { get; set; }
    public string? LogMicroserviceText { get; set; }
    public DateTime DeletedAt { get; set; }
}
