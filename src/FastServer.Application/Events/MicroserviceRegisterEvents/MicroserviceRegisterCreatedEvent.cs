namespace FastServer.Application.Events.MicroserviceRegisterEvents;

/// <summary>
/// Evento publicado cuando se crea un nuevo registro de microservicio
/// </summary>
public class MicroserviceRegisterCreatedEvent
{
    public Guid MicroserviceId { get; set; }
    public string? MicroserviceName { get; set; }
    public bool? MicroserviceActive { get; set; }
    public bool? MicroserviceDeleted { get; set; }
    public bool? MicroserviceCoreConnection { get; set; }
    public string? SoapBase { get; set; }
    public Guid? MicroserviceTypeId { get; set; }
    public Guid? FastServerUserId { get; set; }
    public DateTime? DeleteAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
