namespace FastServer.Application.Events.FastServerClusterEvents;

/// <summary>
/// Evento publicado cuando se elimina un cluster de FastServer
/// </summary>
public class FastServerClusterDeletedEvent
{
    public Guid FastServerClusterId { get; set; }
    public string? FastServerClusterName { get; set; }
    public DateTimeOffset DeletedAt { get; set; }
}
