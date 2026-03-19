namespace FastServer.Application.Events.MicroserviceMethodEvents;

/// <summary>
/// Evento publicado cuando se crea un nuevo método de microservicio
/// </summary>
public class MicroserviceMethodCreatedEvent
{
    public long MicroserviceMethodId { get; set; }
    public long MicroserviceId { get; set; }
    public long? MicroservicesClusterId { get; set; }
    public string? MicroserviceMethodName { get; set; }
    public string? MicroserviceMethodUrl { get; set; }
    public bool? MicroserviceMethodDelete { get; set; }
    public DateTime CreatedAt { get; set; }
}
