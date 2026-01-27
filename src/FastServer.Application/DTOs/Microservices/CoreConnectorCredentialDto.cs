namespace FastServer.Application.DTOs.Microservices;

/// <summary>
/// DTO para CoreConnectorCredential
/// </summary>
public class CoreConnectorCredentialDto
{
    public long CoreConnectorCredentialId { get; set; }
    public string? CoreConnectorCredentialUser { get; set; }
    // NOTA: Por seguridad, el password no se expone en el DTO de lectura
    public string? CoreConnectorCredentialKey { get; set; }
    public DateTime? CreateAt { get; set; }
    public DateTime? ModifyAt { get; set; }
}
