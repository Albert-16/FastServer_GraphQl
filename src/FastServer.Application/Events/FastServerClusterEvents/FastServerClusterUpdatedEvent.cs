namespace FastServer.Application.Events.FastServerClusterEvents;

/// <summary>
/// Evento publicado cuando se actualiza un cluster de FastServer
/// </summary>
public class FastServerClusterUpdatedEvent
{
    public Guid FastServerClusterId { get; set; }
    public string? FastServerClusterName { get; set; }
    public string? FastServerClusterUrl { get; set; }
    public string? FastServerClusterVersion { get; set; }
    public string? FastServerClusterServerName { get; set; }
    public string? FastServerClusterServerIp { get; set; }
    public bool? FastServerClusterActive { get; set; }
    public bool? FastServerClusterDelete { get; set; }
    public DateTimeOffset? DeleteAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
