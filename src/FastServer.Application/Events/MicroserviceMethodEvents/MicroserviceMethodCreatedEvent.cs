namespace FastServer.Application.Events.MicroserviceMethodEvents;

/// <summary>
/// Evento publicado cuando se crea un nuevo método de microservicio
/// </summary>
public class MicroserviceMethodCreatedEvent
{
    public Guid MicroserviceMethodId { get; set; }
    public Guid MicroserviceId { get; set; }
    public string? MicroserviceMethodName { get; set; }
    public string? MicroserviceMethodUrl { get; set; }
    public string? HttpMethod { get; set; }
    public bool? MicroserviceMethodDelete { get; set; }
    public DateTime CreatedAt { get; set; }
}
