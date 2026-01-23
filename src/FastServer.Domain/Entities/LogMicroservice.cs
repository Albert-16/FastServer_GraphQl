namespace FastServer.Domain.Entities;

/// <summary>
/// Logs de microservicios - FastServer_LogMicroservice
/// </summary>
public class LogMicroservice : BaseEntity
{
    /// <summary>
    /// Texto del log del microservicio
    /// </summary>
    public string? LogMicroserviceText { get; set; }

    // Relación de navegación
    public virtual LogServicesHeader? LogServicesHeader { get; set; }
}
