namespace FastServer.Domain.Entities.Microservices;

/// <summary>
/// Cluster de microservicios
/// </summary>
public class MicroservicesCluster : BaseMicroserviceEntity
{
    /// <summary>
    /// ID único del cluster
    /// </summary>
    public long MicroservicesClusterId { get; set; }

    /// <summary>
    /// Nombre del cluster
    /// </summary>
    public string? MicroservicesClusterName { get; set; }

    /// <summary>
    /// Nombre del servidor
    /// </summary>
    public string? MicroservicesClusterServerName { get; set; }

    /// <summary>
    /// IP del servidor
    /// </summary>
    public string? MicroservicesClusterServerIp { get; set; }

    /// <summary>
    /// Indica si el cluster está activo
    /// </summary>
    public bool? MicroservicesClusterActive { get; set; }

    /// <summary>
    /// Indica si el cluster está eliminado (soft delete)
    /// </summary>
    public bool? MicroservicesClusterDeleted { get; set; }

    /// <summary>
    /// Fecha de eliminación (soft delete)
    /// </summary>
    public DateTime? DeleteAt { get; set; }

    // Navegación
    public virtual ICollection<MicroserviceRegister> MicroserviceRegisters { get; set; } = new List<MicroserviceRegister>();
}
