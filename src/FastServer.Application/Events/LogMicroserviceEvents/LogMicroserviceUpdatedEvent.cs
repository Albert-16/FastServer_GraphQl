namespace FastServer.Application.Events.LogMicroserviceEvents;

/// <summary>
/// Evento publicado cuando se actualiza un log de microservicio
/// </summary>
public class LogMicroserviceUpdatedEvent
{
    public long LogId { get; set; }
    public Guid LogMicroserviceId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public DateTime? LogDate { get; set; }
    public string? LogLevel { get; set; }
    public string? LogMicroserviceText { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifyAt { get; set; }
}
