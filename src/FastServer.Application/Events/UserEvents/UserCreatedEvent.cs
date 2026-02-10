namespace FastServer.Application.Events.UserEvents;

/// <summary>
/// Evento publicado cuando se crea un nuevo usuario
/// </summary>
public class UserCreatedEvent
{
    public Guid UserId { get; set; }
    public string? UserPeoplesoft { get; set; }
    public bool? UserActive { get; set; }
    public string? UserName { get; set; }
    public string? UserEmail { get; set; }
    public DateTime? LastLogin { get; set; }
    public DateTime? PasswordChangedAt { get; set; }
    public bool EmailConfirmed { get; set; }
    public DateTime CreatedAt { get; set; }
}
