namespace FastServer.Domain.Entities.Microservices;

/// <summary>
/// Tipo de registro de microservicio
/// </summary>
public class MicroservicesRegisterType : BaseMicroserviceEntity
{
    /// <summary>
    /// ID único del tipo de registro (GUID v7)
    /// </summary>
    public Guid MicroservicesRegisterTypeId { get; set; }

    /// <summary>
    /// Nombre del tipo de registro
    /// </summary>
    public string? MicroservicesRegisterTypeName { get; set; }

    /// <summary>
    /// Descripción del tipo de registro
    /// </summary>
    public string? MicroservicesRegisterTypeDescription { get; set; }

    // Navegación
    public virtual ICollection<MicroserviceRegister> MicroserviceRegisters { get; set; } = new List<MicroserviceRegister>();
}
