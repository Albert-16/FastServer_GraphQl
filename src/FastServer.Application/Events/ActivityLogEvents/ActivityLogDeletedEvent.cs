namespace FastServer.Application.Events.ActivityLogEvents;

/// <summary>
/// Evento publicado cuando se elimina un log de actividad
/// </summary>
public class ActivityLogDeletedEvent
{
    public Guid ActivityLogId { get; set; }
    public string? ActivityLogDescription { get; set; }
    public DateTime DeletedAt { get; set; }
}
