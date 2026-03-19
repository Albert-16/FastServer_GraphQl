namespace FastServer.Application.DTOs.Microservices;

/// <summary>
/// DTO para MicroserviceMethod
/// </summary>
public class MicroserviceMethodDto
{
    public long MicroserviceMethodId { get; set; }
    public long MicroserviceId { get; set; }
    public long? MicroservicesClusterId { get; set; }
    public bool? MicroserviceMethodDelete { get; set; }
    public string? MicroserviceMethodName { get; set; }
    public string? MicroserviceMethodUrl { get; set; }
    public DateTime? CreateAt { get; set; }
    public DateTime? ModifyAt { get; set; }

    // Relaciones
    public MicroserviceRegisterDto? Microservice { get; set; }
    public MicroservicesClusterDto? Cluster { get; set; }
}
