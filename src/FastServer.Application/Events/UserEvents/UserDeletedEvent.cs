namespace FastServer.Application.Events.UserEvents;

/// <summary>
/// Evento publicado cuando se elimina un usuario
/// </summary>
public class UserDeletedEvent
{
    public Guid UserId { get; set; }
    public string? UserEmail { get; set; }
    public DateTime DeletedAt { get; set; }
}
