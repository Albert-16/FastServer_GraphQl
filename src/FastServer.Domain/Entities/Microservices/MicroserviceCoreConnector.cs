namespace FastServer.Domain.Entities.Microservices;

/// <summary>
/// Conector entre microservicio y core
/// </summary>
public class MicroserviceCoreConnector : BaseMicroserviceEntity
{
    /// <summary>
    /// ID único del conector
    /// </summary>
    public long MicroserviceCoreConnectorId { get; set; }

    /// <summary>
    /// ID de la credencial asociada
    /// </summary>
    public long? CoreConnectorCredentialId { get; set; }

    /// <summary>
    /// ID del microservicio asociado
    /// </summary>
    public long? MicroserviceId { get; set; }

    // Navegación
    public virtual CoreConnectorCredential? CoreConnectorCredential { get; set; }
    public virtual MicroserviceRegister? MicroserviceRegister { get; set; }
}
