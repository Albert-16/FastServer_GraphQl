namespace FastServer.Domain.Entities;

/// <summary>
/// Clase base para todas las entidades del dominio
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Identificador único del log
    /// </summary>
    public long LogId { get; set; }
}
