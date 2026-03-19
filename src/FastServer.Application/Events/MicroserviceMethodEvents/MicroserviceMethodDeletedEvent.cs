namespace FastServer.Application.Events.MicroserviceMethodEvents;

/// <summary>
/// Evento publicado cuando se elimina un método de microservicio
/// </summary>
public class MicroserviceMethodDeletedEvent
{
    public long MicroserviceMethodId { get; set; }
    public string? MicroserviceMethodName { get; set; }
    public DateTime DeletedAt { get; set; }
}
