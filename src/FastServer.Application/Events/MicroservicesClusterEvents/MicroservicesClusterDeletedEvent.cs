namespace FastServer.Application.Events.MicroservicesClusterEvents;

/// <summary>
/// Evento publicado cuando se elimina un cluster de microservicios
/// </summary>
public class MicroservicesClusterDeletedEvent
{
    public Guid MicroservicesClusterId { get; set; }
    public string? MicroservicesClusterName { get; set; }
    public DateTime DeletedAt { get; set; }
}
