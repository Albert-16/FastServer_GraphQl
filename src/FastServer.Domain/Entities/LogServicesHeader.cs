using FastServer.Domain.Enums;

namespace FastServer.Domain.Entities;

/// <summary>
/// Cabecera de logs de servicios - FastServer_LogServices_Header
/// </summary>
public class LogServicesHeader : BaseEntity
{
    /// <summary>
    /// Fecha y hora de entrada del log
    /// </summary>
    public DateTime LogDateIn { get; set; }

    /// <summary>
    /// Fecha y hora de salida del log
    /// </summary>
    public DateTime LogDateOut { get; set; }

    /// <summary>
    /// Estado del log
    /// </summary>
    public LogState LogState { get; set; }

    /// <summary>
    /// URL del método invocado
    /// </summary>
    public string LogMethodUrl { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del método invocado
    /// </summary>
    public string? LogMethodName { get; set; }

    /// <summary>
    /// ID de FastServer
    /// </summary>
    public long? LogFsId { get; set; }

    /// <summary>
    /// Descripción del método
    /// </summary>
    public string? MethodDescription { get; set; }

    /// <summary>
    /// IP y puerto TCI
    /// </summary>
    public string? TciIpPort { get; set; }

    /// <summary>
    /// Código de error
    /// </summary>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Descripción del error
    /// </summary>
    public string? ErrorDescription { get; set; }

    /// <summary>
    /// IP del FastServer
    /// </summary>
    public string? IpFs { get; set; }

    /// <summary>
    /// Tipo de proceso
    /// </summary>
    public string? TypeProcess { get; set; }

    /// <summary>
    /// Nodo del log
    /// </summary>
    public string? LogNodo { get; set; }

    /// <summary>
    /// Método HTTP (GET, POST, PUT, DELETE, etc.)
    /// </summary>
    public string? HttpMethod { get; set; }

    /// <summary>
    /// Nombre del microservicio
    /// </summary>
    public string? MicroserviceName { get; set; }

    /// <summary>
    /// Duración de la solicitud en milisegundos
    /// </summary>
    public long? RequestDuration { get; set; }

    /// <summary>
    /// ID de la transacción
    /// </summary>
    public string? TransactionId { get; set; }

    /// <summary>
    /// ID del usuario
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// ID de la sesión
    /// </summary>
    public string? SessionId { get; set; }

    /// <summary>
    /// ID de la solicitud
    /// </summary>
    public long? RequestId { get; set; }
}
