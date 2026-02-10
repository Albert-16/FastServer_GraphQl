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

    /// <summary>
    /// Hash de la contraseña del usuario (BCrypt)
    /// </summary>
    public string? PasswordHash { get; set; }

    /// <summary>
    /// Fecha del último login
    /// </summary>
    public DateTime? LastLogin { get; set; }

    /// <summary>
    /// Fecha del último cambio de contraseña
    /// </summary>
    public DateTime? PasswordChangedAt { get; set; }

    /// <summary>
    /// Indica si el email ha sido confirmado
    /// </summary>
    public bool EmailConfirmed { get; set; } = false;

    /// <summary>
    /// Refresh token activo para JWT
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Fecha de expiración del refresh token
    /// </summary>
    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Navegación
    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
}
