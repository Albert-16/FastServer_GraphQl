namespace FastServer.Application.DTOs.Microservices;

/// <summary>
/// DTO para MicroserviceMethod
/// </summary>
public class MicroserviceMethodDto
{
    public Guid MicroserviceMethodId { get; set; }
    public Guid MicroserviceId { get; set; }
    public bool? MicroserviceMethodDelete { get; set; }
    public string? MicroserviceMethodName { get; set; }
    public string? MicroserviceMethodUrl { get; set; }
    public string? HttpMethod { get; set; }
    public DateTime? CreateAt { get; set; }
    public DateTime? ModifyAt { get; set; }

    // Relaciones
    public MicroserviceRegisterDto? Microservice { get; set; }
    public List<NodoDto>? Nodos { get; set; }
}
