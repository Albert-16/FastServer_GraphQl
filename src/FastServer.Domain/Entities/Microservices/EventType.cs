namespace FastServer.Domain.Entities.Microservices;

/// <summary>
/// Tipos de eventos para activity logs
/// </summary>
public class EventType : BaseMicroserviceEntity
{
    /// <summary>
    /// ID del tipo de evento
    /// </summary>
    public long EventTypeId { get; set; }

    /// <summary>
    /// Descripción del tipo de evento
    /// </summary>
    public string? EventTypeDescription { get; set; }

    // Navegación
    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
}
