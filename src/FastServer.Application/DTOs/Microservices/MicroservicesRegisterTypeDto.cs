namespace FastServer.Application.DTOs.Microservices;

/// <summary>
/// DTO para MicroservicesRegisterType
/// </summary>
public class MicroservicesRegisterTypeDto
{
    public Guid MicroservicesRegisterTypeId { get; set; }
    public string? MicroservicesRegisterTypeName { get; set; }
    public string? MicroservicesRegisterTypeDescription { get; set; }
    public DateTime? CreateAt { get; set; }
    public DateTime? ModifyAt { get; set; }
}
