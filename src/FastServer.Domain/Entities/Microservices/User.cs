namespace FastServer.Domain.Entities.Microservices;

/// <summary>
/// Usuario del sistema
/// </summary>
public class User : BaseMicroserviceEntity
{
    /// <summary>
    /// ID único del usuario
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// ID de PeopleSoft del usuario
    /// </summary>
    public string? UserPeoplesoft { get; set; }

    /// <summary>
    /// Indica si el usuario está activo
    /// </summary>
    public bool? UserActive { get; set; }

    /// <summary>
    /// Nombre del usuario
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Email del usuario
    /// </summary>
    public string? UserEmail { get; set; }

    // Navegación
    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
}
