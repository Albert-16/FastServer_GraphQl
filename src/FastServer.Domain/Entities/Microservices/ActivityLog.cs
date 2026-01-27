namespace FastServer.Domain.Entities.Microservices;

/// <summary>
/// Log de actividad del sistema
/// </summary>
public class ActivityLog : BaseMicroserviceEntity
{
    /// <summary>
    /// ID único del log de actividad
    /// </summary>
    public Guid ActivityLogId { get; set; }

    /// <summary>
    /// ID del tipo de evento
    /// </summary>
    public long? EventTypeId { get; set; }

    /// <summary>
    /// Nombre de la entidad afectada
    /// </summary>
    public string? ActivityLogEntityName { get; set; }

    /// <summary>
    /// ID de la entidad afectada
    /// </summary>
    public Guid? ActivityLogEntityId { get; set; }

    /// <summary>
    /// Descripción de la actividad
    /// </summary>
    public string? ActivityLogDescription { get; set; }

    /// <summary>
    /// ID del usuario que realizó la actividad
    /// </summary>
    public Guid? UserId { get; set; }

    // Navegación
    public virtual EventType? EventType { get; set; }
    public virtual User? User { get; set; }
}
