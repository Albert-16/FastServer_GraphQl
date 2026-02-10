namespace FastServer.Application.Events.ActivityLogEvents;

/// <summary>
/// Evento publicado cuando se crea un nuevo log de actividad
/// </summary>
public class ActivityLogCreatedEvent
{
    public Guid ActivityLogId { get; set; }
    public long? EventTypeId { get; set; }
    public string? ActivityLogEntityName { get; set; }
    public Guid? ActivityLogEntityId { get; set; }
    public string? ActivityLogDescription { get; set; }
    public Guid? UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
