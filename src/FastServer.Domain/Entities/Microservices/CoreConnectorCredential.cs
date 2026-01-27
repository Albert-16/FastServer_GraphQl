namespace FastServer.Domain.Entities.Microservices;

/// <summary>
/// Credenciales para conectores del core
/// </summary>
public class CoreConnectorCredential : BaseMicroserviceEntity
{
    /// <summary>
    /// ID único de la credencial
    /// </summary>
    public long CoreConnectorCredentialId { get; set; }

    /// <summary>
    /// Usuario de la credencial
    /// </summary>
    public string? CoreConnectorCredentialUser { get; set; }

    /// <summary>
    /// Contraseña de la credencial
    /// </summary>
    public string? CoreConnectorCredentialPass { get; set; }

    /// <summary>
    /// Llave de la credencial
    /// </summary>
    public string? CoreConnectorCredentialKey { get; set; }

    // Navegación
    public virtual ICollection<MicroserviceCoreConnector> MicroserviceCoreConnectors { get; set; } = new List<MicroserviceCoreConnector>();
}
