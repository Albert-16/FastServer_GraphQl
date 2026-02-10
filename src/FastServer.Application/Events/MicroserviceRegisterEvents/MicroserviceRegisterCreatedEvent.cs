namespace FastServer.Application.Events.MicroserviceRegisterEvents;

/// <summary>
/// Evento publicado cuando se crea un nuevo registro de microservicio
/// </summary>
public class MicroserviceRegisterCreatedEvent
{
    public long MicroserviceId { get; set; }
    public long? MicroserviceClusterId { get; set; }
    public string? MicroserviceName { get; set; }
    public bool? MicroserviceActive { get; set; }
    public bool? MicroserviceDeleted { get; set; }
    public bool? MicroserviceCoreConnection { get; set; }
    public DateTime? DeleteAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
