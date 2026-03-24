namespace FastServer.Application.Events.MicroserviceMethodEvents;

/// <summary>
/// Evento publicado cuando se actualiza un método de microservicio
/// </summary>
public class MicroserviceMethodUpdatedEvent
{
    public Guid MicroserviceMethodId { get; set; }
    public Guid MicroserviceId { get; set; }
    public string? MicroserviceMethodName { get; set; }
    public string? MicroserviceMethodUrl { get; set; }
    public string? HttpMethod { get; set; }
    public bool? MicroserviceMethodDelete { get; set; }
    public DateTime UpdatedAt { get; set; }
}
