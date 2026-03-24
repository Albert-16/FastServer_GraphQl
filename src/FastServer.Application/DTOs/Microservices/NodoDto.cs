namespace FastServer.Application.DTOs.Microservices;

/// <summary>
/// DTO para Nodo (tabla intermedia Method-Cluster)
/// </summary>
public class NodoDto
{
    public Guid NodoId { get; set; }
    public Guid MicroserviceMethodId { get; set; }
    public Guid MicroservicesClusterId { get; set; }
    public DateTime? CreateAt { get; set; }
    public DateTime? ModifyAt { get; set; }

    // Relaciones
    public MicroserviceMethodDto? MicroserviceMethod { get; set; }
    public MicroservicesClusterDto? MicroservicesCluster { get; set; }
}
