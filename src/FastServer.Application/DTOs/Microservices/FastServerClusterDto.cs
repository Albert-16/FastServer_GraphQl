namespace FastServer.Application.DTOs.Microservices;

/// <summary>
/// DTO para FastServerCluster
/// </summary>
public class FastServerClusterDto
{
    public Guid FastServerClusterId { get; set; }
    public string? FastServerClusterName { get; set; }
    public string? FastServerClusterUrl { get; set; }
    public string? FastServerClusterVersion { get; set; }
    public string? FastServerClusterServerName { get; set; }
    public string? FastServerClusterServerIp { get; set; }
    public bool? FastServerClusterActive { get; set; }
    public bool? FastServerClusterDelete { get; set; }
    public DateTimeOffset? CreateAt { get; set; }
    public DateTimeOffset? ModifyAt { get; set; }
    public DateTimeOffset? DeleteAt { get; set; }
}
