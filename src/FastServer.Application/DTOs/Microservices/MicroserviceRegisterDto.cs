namespace FastServer.Application.DTOs.Microservices;

/// <summary>
/// DTO para MicroserviceRegister
/// </summary>
public class MicroserviceRegisterDto
{
    public Guid MicroserviceId { get; set; }
    public string? MicroserviceName { get; set; }
    public bool? MicroserviceActive { get; set; }
    public bool? MicroserviceDeleted { get; set; }
    public bool? MicroserviceCoreConnection { get; set; }
    public string? SoapBase { get; set; }
    public Guid? MicroserviceTypeId { get; set; }
    public DateTime? CreateAt { get; set; }
    public DateTime? ModifyAt { get; set; }
    public DateTime? DeleteAt { get; set; }

    // Relaciones
    public MicroservicesRegisterTypeDto? MicroserviceType { get; set; }
    public List<MicroserviceCoreConnectorDto>? CoreConnectors { get; set; }
}
