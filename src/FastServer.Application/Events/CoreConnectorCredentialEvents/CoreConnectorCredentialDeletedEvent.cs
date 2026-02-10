namespace FastServer.Application.Events.CoreConnectorCredentialEvents;

/// <summary>
/// Evento publicado cuando se elimina una credencial de conector del core
/// </summary>
public class CoreConnectorCredentialDeletedEvent
{
    public long CoreConnectorCredentialId { get; set; }
    public string? CoreConnectorCredentialUser { get; set; }
    public DateTime DeletedAt { get; set; }
}
