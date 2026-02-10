namespace FastServer.Application.Events.LogEvents;

/// <summary>
/// Evento publicado cuando se elimina un log
/// </summary>
public class LogDeletedEvent
{
    public long LogId { get; set; }
    public string? MicroserviceName { get; set; }
    public string? UserId { get; set; }
    public DateTime DeletedAt { get; set; }
}
