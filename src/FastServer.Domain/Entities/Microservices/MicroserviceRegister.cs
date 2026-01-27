namespace FastServer.Domain.Entities.Microservices;

/// <summary>
/// Registro de microservicio
/// </summary>
public class MicroserviceRegister : BaseMicroserviceEntity
{
    /// <summary>
    /// ID único del microservicio
    /// </summary>
    public long MicroserviceId { get; set; }

    /// <summary>
    /// ID del cluster al que pertenece
    /// </summary>
    public long? MicroserviceClusterId { get; set; }

    /// <summary>
    /// Nombre del microservicio
    /// </summary>
    public string? MicroserviceName { get; set; }

    /// <summary>
    /// Indica si el microservicio está activo
    /// </summary>
    public bool? MicroserviceActive { get; set; }

    /// <summary>
    /// Indica si el microservicio está eliminado (soft delete)
    /// </summary>
    public bool? MicroserviceDeleted { get; set; }

    /// <summary>
    /// Indica si tiene conexión con el core
    /// </summary>
    public bool? MicroserviceCoreConnection { get; set; }

    /// <summary>
    /// Fecha de eliminación (soft delete)
    /// </summary>
    public DateTime? DeleteAt { get; set; }

    // Navegación
    public virtual MicroservicesCluster? MicroserviceCluster { get; set; }
    public virtual ICollection<MicroserviceCoreConnector> MicroserviceCoreConnectors { get; set; } = new List<MicroserviceCoreConnector>();
}
