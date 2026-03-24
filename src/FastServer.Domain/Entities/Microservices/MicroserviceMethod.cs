namespace FastServer.Domain.Entities.Microservices;

/// <summary>
/// Métodos/endpoints de microservicios - microservice_methods
/// </summary>
public class MicroserviceMethod : BaseMicroserviceEntity
{
    /// <summary>
    /// ID único del método (GUID v7)
    /// </summary>
    public Guid MicroserviceMethodId { get; set; }

    /// <summary>
    /// ID del microservicio al que pertenece
    /// </summary>
    public Guid MicroserviceId { get; set; }

    /// <summary>
    /// Indica si el método está eliminado (soft delete)
    /// </summary>
    public bool? MicroserviceMethodDelete { get; set; }

    /// <summary>
    /// Nombre del método/endpoint
    /// </summary>
    public string? MicroserviceMethodName { get; set; }

    /// <summary>
    /// URL del método/endpoint
    /// </summary>
    public string? MicroserviceMethodUrl { get; set; }

    /// <summary>
    /// Método HTTP (GET, POST, PUT, DELETE, PATCH, WebSocket, etc.)
    /// </summary>
    public string? HttpMethod { get; set; }

    // Navegación
    public virtual MicroserviceRegister? MicroserviceRegister { get; set; }
    public virtual ICollection<Nodo> Nodos { get; set; } = new List<Nodo>();
}
