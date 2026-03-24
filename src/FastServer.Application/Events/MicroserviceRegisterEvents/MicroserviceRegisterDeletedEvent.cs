namespace FastServer.Application.Events.MicroserviceRegisterEvents;

/// <summary>
/// Evento publicado cuando se elimina un registro de microservicio
/// </summary>
public class MicroserviceRegisterDeletedEvent
{
    public Guid MicroserviceId { get; set; }
    public string? MicroserviceName { get; set; }
    public DateTime DeletedAt { get; set; }
}
