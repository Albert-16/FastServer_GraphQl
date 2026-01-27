namespace FastServer.Application.DTOs.Microservices;

/// <summary>
/// DTO para MicroserviceCoreConnector
/// </summary>
public class MicroserviceCoreConnectorDto
{
    public long MicroserviceCoreConnectorId { get; set; }
    public long? CoreConnectorCredentialId { get; set; }
    public long? MicroserviceId { get; set; }
    public DateTime? CreateAt { get; set; }
    public DateTime? ModifyAt { get; set; }

    // Relaciones
    public CoreConnectorCredentialDto? Credential { get; set; }
    public MicroserviceRegisterDto? Microservice { get; set; }
}
