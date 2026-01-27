namespace FastServer.Application.DTOs.Microservices;

/// <summary>
/// DTO para MicroservicesCluster
/// </summary>
public class MicroservicesClusterDto
{
    public long MicroservicesClusterId { get; set; }
    public string? MicroservicesClusterName { get; set; }
    public string? MicroservicesClusterServerName { get; set; }
    public string? MicroservicesClusterServerIp { get; set; }
    public bool? MicroservicesClusterActive { get; set; }
    public bool? MicroservicesClusterDeleted { get; set; }
    public DateTime? CreateAt { get; set; }
    public DateTime? ModifyAt { get; set; }
    public DateTime? DeleteAt { get; set; }

    // Relaciones
    public List<MicroserviceRegisterDto>? MicroserviceRegisters { get; set; }
}
