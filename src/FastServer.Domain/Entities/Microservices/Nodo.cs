namespace FastServer.Domain.Entities.Microservices;

/// <summary>
/// Tabla intermedia entre MicroserviceMethod y MicroservicesCluster (many-to-many)
/// </summary>
public class Nodo : BaseMicroserviceEntity
{
    /// <summary>
    /// ID único del nodo (GUID v7)
    /// </summary>
    public Guid NodoId { get; set; }

    /// <summary>
    /// ID del método de microservicio (FK)
    /// </summary>
    public Guid MicroserviceMethodId { get; set; }

    /// <summary>
    /// ID del cluster de microservicios (FK)
    /// </summary>
    public Guid MicroservicesClusterId { get; set; }

    // Navegación
    public virtual MicroserviceMethod? MicroserviceMethod { get; set; }
    public virtual MicroservicesCluster? MicroservicesCluster { get; set; }
}
