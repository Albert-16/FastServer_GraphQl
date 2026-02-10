namespace FastServer.Application.Events.MicroservicesClusterEvents;

/// <summary>
/// Evento publicado cuando se crea un nuevo cluster de microservicios
/// </summary>
public class MicroservicesClusterCreatedEvent
{
    public long MicroservicesClusterId { get; set; }
    public string? MicroservicesClusterName { get; set; }
    public string? MicroservicesClusterServerName { get; set; }
    public string? MicroservicesClusterServerIp { get; set; }
    public bool? MicroservicesClusterActive { get; set; }
    public bool? MicroservicesClusterDeleted { get; set; }
    public DateTime? DeleteAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
