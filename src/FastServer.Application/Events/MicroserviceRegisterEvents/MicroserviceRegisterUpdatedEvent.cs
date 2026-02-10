namespace FastServer.Application.Events.MicroserviceRegisterEvents;

/// <summary>
/// Evento publicado cuando se actualiza un registro de microservicio
/// </summary>
public class MicroserviceRegisterUpdatedEvent
{
    public long MicroserviceId { get; set; }
    public long? MicroserviceClusterId { get; set; }
    public string? MicroserviceName { get; set; }
    public bool? MicroserviceActive { get; set; }
    public bool? MicroserviceDeleted { get; set; }
    public bool? MicroserviceCoreConnection { get; set; }
    public DateTime? DeleteAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
