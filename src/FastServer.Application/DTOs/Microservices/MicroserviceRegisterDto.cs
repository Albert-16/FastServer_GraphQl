namespace FastServer.Application.DTOs.Microservices;

/// <summary>
/// DTO para MicroserviceRegister
/// </summary>
public class MicroserviceRegisterDto
{
    public long MicroserviceId { get; set; }
    public long? MicroserviceClusterId { get; set; }
    public string? MicroserviceName { get; set; }
    public bool? MicroserviceActive { get; set; }
    public bool? MicroserviceDeleted { get; set; }
    public bool? MicroserviceCoreConnection { get; set; }
    public DateTime? CreateAt { get; set; }
    public DateTime? ModifyAt { get; set; }
    public DateTime? DeleteAt { get; set; }

    // Relaciones
    public MicroservicesClusterDto? Cluster { get; set; }
    public List<MicroserviceCoreConnectorDto>? CoreConnectors { get; set; }
}
