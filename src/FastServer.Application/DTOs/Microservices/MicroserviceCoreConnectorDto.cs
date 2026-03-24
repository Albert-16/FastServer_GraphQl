namespace FastServer.Application.DTOs.Microservices;

/// <summary>
/// DTO para MicroserviceCoreConnector
/// </summary>
public class MicroserviceCoreConnectorDto
{
    public Guid MicroserviceCoreConnectorId { get; set; }
    public Guid? CoreConnectorCredentialId { get; set; }
    public Guid? MicroserviceId { get; set; }
    public DateTime? CreateAt { get; set; }
    public DateTime? ModifyAt { get; set; }

    // Relaciones
    public CoreConnectorCredentialDto? Credential { get; set; }
    public MicroserviceRegisterDto? Microservice { get; set; }
}
