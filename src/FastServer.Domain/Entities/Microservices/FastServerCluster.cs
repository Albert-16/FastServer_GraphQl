namespace FastServer.Domain.Entities.Microservices;

/// <summary>
/// Cluster de FastServer — representa un nodo/instancia del servidor
/// Usa DateTimeOffset para manejo correcto de zonas horarias en entornos distribuidos
/// </summary>
public sealed class FastServerCluster
{
    /// <summary>
    /// ID único del cluster (GUID v7 time-ordered)
    /// </summary>
    public Guid FastServerClusterId { get; set; }

    /// <summary>
    /// Nombre identificador del cluster
    /// </summary>
    public string? FastServerClusterName { get; set; }

    /// <summary>
    /// URL base del cluster
    /// </summary>
    public string? FastServerClusterUrl { get; set; }

    /// <summary>
    /// Versión del software desplegada en el cluster
    /// </summary>
    public string? FastServerClusterVersion { get; set; }

    /// <summary>
    /// Nombre del servidor donde reside el cluster
    /// </summary>
    public string? FastServerClusterServerName { get; set; }

    /// <summary>
    /// Dirección IP del servidor
    /// </summary>
    public string? FastServerClusterServerIp { get; set; }

    /// <summary>
    /// Indica si el cluster está activo
    /// </summary>
    public bool? FastServerClusterActive { get; set; }

    /// <summary>
    /// Indica si el cluster está eliminado (soft delete)
    /// </summary>
    public bool? FastServerClusterDelete { get; set; }

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    public DateTimeOffset? CreateAt { get; set; }

    /// <summary>
    /// Fecha de última modificación
    /// </summary>
    public DateTimeOffset? ModifyAt { get; set; }

    /// <summary>
    /// Fecha de eliminación (soft delete)
    /// </summary>
    public DateTimeOffset? DeleteAt { get; set; }
}
