namespace FastServer.Domain.Entities.Microservices;

/// <summary>
/// Entidad base para todas las entidades de microservicios
/// </summary>
public abstract class BaseMicroserviceEntity
{
    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    public DateTime? CreateAt { get; set; }

    /// <summary>
    /// Fecha de última modificación del registro
    /// </summary>
    public DateTime? ModifyAt { get; set; }
}
