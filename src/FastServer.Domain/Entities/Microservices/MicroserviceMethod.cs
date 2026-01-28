namespace FastServer.Domain.Entities.Microservices;

/// <summary>
/// Métodos/endpoints de microservicios - microservice_methods
/// </summary>
public class MicroserviceMethod : BaseMicroserviceEntity
{
    /// <summary>
    /// ID único del método
    /// </summary>
    public long MicroserviceMethodId { get; set; }

    /// <summary>
    /// ID del microservicio al que pertenece
    /// </summary>
    public long MicroserviceId { get; set; }

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

    // Navegación
    public virtual MicroserviceRegister? MicroserviceRegister { get; set; }
}
