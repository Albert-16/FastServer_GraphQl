namespace FastServer.Application.Events.CoreConnectorCredentialEvents;

/// <summary>
/// Evento publicado cuando se crea una nueva credencial de conector del core
/// </summary>
public class CoreConnectorCredentialCreatedEvent
{
    public long CoreConnectorCredentialId { get; set; }
    public string? CoreConnectorCredentialUser { get; set; }
    public string? CoreConnectorCredentialPass { get; set; }
    public string? CoreConnectorCredentialKey { get; set; }
    public bool? MicroserviceActive { get; set; }
    public bool? MicroserviceDeleted { get; set; }
    public DateTime? DeleteAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
