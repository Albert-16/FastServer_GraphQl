namespace FastServer.Application.Events.LogMicroserviceEvents;

/// <summary>
/// Evento publicado cuando se crea un nuevo log de microservicio
/// </summary>
public class LogMicroserviceCreatedEvent
{
    public long LogId { get; set; }
    public DateTime? LogDate { get; set; }
    public string? LogLevel { get; set; }
    public string? LogMicroserviceText { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifyAt { get; set; }
}
